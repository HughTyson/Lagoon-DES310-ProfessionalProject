using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class HDRP_Unlit_Animator : MonoBehaviour
{


    HDRP_SpriteSheet spriteSheet;

    Material myMaterial;
    MeshRenderer myRenderer;
    int meshTextureID = Shader.PropertyToID("_UnlitColorMap");

    Animation current_animation = new Animation();


    public bool Pause { set { isPlaying = value; } }

    // Start is called before the first frame update
    void Start()
    {



    }

    Material MyMaterial
    {
        get
        {
            if (myMaterial == null)
            {
                myMaterial = MyRenderer.material;

            }
            return myMaterial;
        }
    }
    MeshRenderer MyRenderer
    {
        get
        {
            if (myRenderer == null)
            {
                myRenderer = GetComponent<MeshRenderer>();

            }
            return myRenderer;
        }
    }



     void SetSpriteSheet(HDRP_SpriteSheet sprite_sheet)
    {
        spriteSheet = sprite_sheet;
        MyMaterial.SetTexture(meshTextureID, spriteSheet.SpriteSheetTexture);
        MyMaterial.SetTextureScale(meshTextureID, spriteSheet.Offset);
    }


    bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    public  void PlayAnimation(HDRP_SpriteSheet.AnimationFrames frames, float speed = 1, bool flipOnX = false, bool flipOnY = false, bool startFromEnd = false, System.Action animationCompletedDelegate = null, System.Action animationInterupted = null, TIME_FORMAT time_format_ = TIME_FORMAT.DELTA, ANIMATION_PLAYER anim_player = ANIMATION_PLAYER.ONCE)
    {
        if (frames.SpriteSheet != spriteSheet)
            SetSpriteSheet(frames.SpriteSheet);

        if (isPlaying)
        {
            current_animation.CallAnimationInterupted();
        }
        isPlaying = true;


        Vector2 startingOffset = Vector2.zero;

        // required alligment offset if a flip on an axis is performed
        if (flipOnX)
            startingOffset.x += 1;
        if (flipOnY)
            startingOffset.y += 1;     

        current_animation.Setup(frames.FrameIndexes, speed * frames.Duration, startingOffset, spriteSheet.Offset,  startFromEnd, time_format_, anim_player, animationCompletedDelegate, animationInterupted);
        current_animation.Update();
        MyMaterial.SetTextureOffset(meshTextureID, current_animation.CurrentOffset);
        Show();
    }

    public void Hide()
    {
        MyRenderer.enabled = false;
    }
    public void Show()
    {
        MyRenderer.enabled = true;
    }

    public void StopAnimation()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Hide();
            current_animation.CallAnimationInterupted();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            current_animation.Update();
            MyMaterial.SetTextureOffset(meshTextureID, current_animation.CurrentOffset);

            if (current_animation.IsFinished)
            {
                isPlaying = false;
                current_animation.CallAnimationCompleted();
            }
        }
    }


    class Animation
    {
        TIME_FORMAT time_format;
        ANIMATION_PLAYER animation_player;
        List<Vector2Int> frame_indexes;
        float duration;

        int current_index = 0;
        float current_time = 0;

        Vector2 startingOffset;
        Vector2 tileOffset;
        bool fromEndToStart;

        Vector2 currentTileOffset;

        System.Action animationCompletedDelegate;
        
        System.Action animationInteruptedDelegate;

        bool isFinished;
        public bool IsFinished => isFinished;
        public Vector2 CurrentOffset => currentTileOffset;

        public void CallAnimationCompleted()
        {
            animationCompletedDelegate?.Invoke();
        }
        public void CallAnimationInterupted()
        {
            animationInteruptedDelegate?.Invoke();
        }

        public void Setup(List<Vector2Int> frame_indexes_, float duration_, Vector2 startingOffset_, Vector2 tileOffset_, bool fromEndToStart_, TIME_FORMAT time_format_, ANIMATION_PLAYER animation_player_, System.Action animationCompletedDelegate_, System.Action animationInteruptedDelegate_)
        {
            isFinished = false;
            frame_indexes = frame_indexes_;
            duration = duration_;
            startingOffset = startingOffset_;
            tileOffset = tileOffset_;
            fromEndToStart = fromEndToStart_;
            time_format = time_format_;
            animation_player = animation_player_;
            animationCompletedDelegate = animationCompletedDelegate_;
            animationInteruptedDelegate = animationInteruptedDelegate_;

            current_index = 0;
            current_time = 0;
        }

        public void Update()
        {
            switch(time_format)
            {
                case TIME_FORMAT.DELTA: current_time += Time.deltaTime; break;
                case TIME_FORMAT.FIXED_DELTA: current_time += Time.fixedDeltaTime; break;
                case TIME_FORMAT.UNSCALED_DELTA: current_time += Time.unscaledDeltaTime; break;
                case TIME_FORMAT.FIXED_UNSCALED_DELTA: current_time += Time.fixedUnscaledDeltaTime; break;
            }

            current_index = Mathf.FloorToInt(((float)(frame_indexes.Count - 1) * (current_time / duration)));
            if (fromEndToStart)
                current_index = (frame_indexes.Count  - 1) - current_index;


            if (current_index < 0 || current_index > (frame_indexes.Count - 1))
            {
                switch (animation_player)
                {
                    case ANIMATION_PLAYER.ONCE:
                        {
                            current_index = Mathf.Clamp(current_index, 0, frame_indexes.Count - 1);
                            currentTileOffset = startingOffset + (tileOffset * (frame_indexes[current_index]));
                            isFinished = true;
                            break;
                        }
                    case ANIMATION_PLAYER.LOOP:
                        {
                            current_time = 0;
                            break;
                        }
                    case ANIMATION_PLAYER.PING_PONG:
                        {
                            fromEndToStart = !fromEndToStart;
                            current_time = 0;
                            break;
                        }                
                }


            }
            else
            {
                currentTileOffset = startingOffset + (tileOffset * (frame_indexes[current_index]));
            }

        }
    }


    public enum TIME_FORMAT
    { 
    DELTA,
    FIXED_DELTA,
    UNSCALED_DELTA,
    FIXED_UNSCALED_DELTA
    }
    public enum ANIMATION_PLAYER
    { 
    ONCE,
    PING_PONG,
    LOOP,
    }


}
