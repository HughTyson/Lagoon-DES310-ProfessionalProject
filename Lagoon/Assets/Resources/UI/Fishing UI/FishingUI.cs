using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] HDRP_SpriteSheet spriteSheet_FishBite;
    [SerializeField] HDRP_SpriteSheet spriteSheet_FishTest;


    public enum ANIMATION_STATE
    { 
    NOT_ACTIVE,
    FISH_BITE,
    FISH_TEST,
    FISH_LEFT, // unused
    FISH_RIGHT // unused
    };

    Transform LookAt;

    TweenManager.TweenPathBundle fishBiteIndicatorTween;
    TweenManager.TweenPathBundle fishTestIndicatorTween;

    TweenAnimator.Animation fishBiteIndicatorAnimation;
    TweenAnimator.Animation fishTestIndicatorAnimation;
    HDRP_Unlit_ManualAnimator manualSpriteAnimator;
    Material material;

    void Awake()
    {
        manualSpriteAnimator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        fishBiteIndicatorTween = new TweenManager.TweenPathBundle(
            // frame index
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, (spriteSheet_FishBite.Animations[0].FrameIndexes.Count - 1) + 0.01f, 0.25f, TweenManager.CURVE_PRESET.EASE_IN),
                new TweenManager.TweenPart_Delay(0.5f)
                ),
            // alpha
            new TweenManager.TweenPath(
                 new TweenManager.TweenPart_Start(0, 1, 0.25f, TweenManager.CURVE_PRESET.LINEAR),
                 new TweenManager.TweenPart_Delay(0.5f),
                 new TweenManager.TweenPart_Continue(0, 0.25f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            // X scale
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(transform.localScale.x * 2.0f, transform.localScale.x, 0.40f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                ),
            // Y scale
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start((transform.localScale.x * spriteSheet_FishBite.SpriteToHeightAspectRation)/2.0f, transform.localScale.x * spriteSheet_FishBite.SpriteToHeightAspectRation, 0.40f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )

            );

        fishBiteIndicatorAnimation = new TweenAnimator.Animation(
            fishBiteIndicatorTween,
            new TweenAnimator.Transf_(
                transform,
                local_scale: new TweenAnimator.Transf_.LocalScale_(true, 2, true, 3, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.HDRP_Animator_(
                manualSpriteAnimator,
               new TweenAnimator.HDRP_Animator_.Frame_(
                0,
                TweenAnimator.Base.IntProperty.INT_SELECTION_METHOD.FLOOR,
                TweenAnimator.MOD_TYPE.ABSOLUTE
                )
                ),
            new TweenAnimator.HDRP_Unlit_Material_(
                material,
                new TweenAnimator.HDRP_Unlit_Material_.Color_(
                    false, 0, false, 0, false, 0, true, 1,
                    TweenAnimator.MOD_TYPE.ABSOLUTE
                    )
                )
                );

        fishTestIndicatorTween = new TweenManager.TweenPathBundle(
            // frame index
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, (spriteSheet_FishTest.Animations[0].FrameIndexes.Count - 1) + 0.01f, 0.25f, TweenManager.CURVE_PRESET.EASE_IN),
                new TweenManager.TweenPart_Delay(0.5f)
                ),
            // alpha
            new TweenManager.TweenPath(
                 new TweenManager.TweenPart_Start(0, 1, 0.25f, TweenManager.CURVE_PRESET.LINEAR),
                 new TweenManager.TweenPart_Delay(0.5f),
                 new TweenManager.TweenPart_Continue(0, 0.25f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            // X scale
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(transform.localScale.x * 2.0f, transform.localScale.x, 0.40f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                ),
            // Y scale
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start((transform.localScale.x * spriteSheet_FishTest.SpriteToHeightAspectRation) / 2.0f, transform.localScale.x * spriteSheet_FishTest.SpriteToHeightAspectRation, 0.40f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )

            );

        fishTestIndicatorAnimation = new TweenAnimator.Animation(
            fishTestIndicatorTween,
            new TweenAnimator.Transf_(
                transform,
                local_scale: new TweenAnimator.Transf_.LocalScale_(true, 2, true, 3, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.HDRP_Animator_(
                manualSpriteAnimator,
               new TweenAnimator.HDRP_Animator_.Frame_(
                0,
                TweenAnimator.Base.IntProperty.INT_SELECTION_METHOD.FLOOR,
                TweenAnimator.MOD_TYPE.ABSOLUTE
                )
                ),
            new TweenAnimator.HDRP_Unlit_Material_(
                material,
                new TweenAnimator.HDRP_Unlit_Material_.Color_(
                    false, 0, false, 0, false, 0, true, 1,
                    TweenAnimator.MOD_TYPE.ABSOLUTE
                    )
                )
                );
    }

    private void OnEnable()
    {
        SetIndicator(ANIMATION_STATE.NOT_ACTIVE);
    }

    private void OnDisable()
    {

    }

    public void SetIndicator(ANIMATION_STATE state)
    {
        switch (state)
        {
            case ANIMATION_STATE.NOT_ACTIVE:
                {
                    fishBiteIndicatorAnimation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE);
                    fishTestIndicatorAnimation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE);
                    manualSpriteAnimator.Hide();
                    break;
                }
            case ANIMATION_STATE.FISH_BITE:
                {
                    manualSpriteAnimator.SetAnimation(spriteSheet_FishBite.Animations[0]);
                    fishBiteIndicatorAnimation.PlayAnimation();
                    break;
                }
            case ANIMATION_STATE.FISH_TEST:
                {
                    manualSpriteAnimator.SetAnimation(spriteSheet_FishTest.Animations[0]);
                    fishTestIndicatorAnimation.PlayAnimation();
                    break;
                }
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void AttachLookAtTransform(Transform lookat)
    {
        LookAt = lookat;
    }
    private void Update()
    {
       if (LookAt != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - LookAt.position, Vector3.up);
        }
    }

}
