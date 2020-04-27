using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(menuName = "HDRP SpriteSheet")]
public class HDRP_SpriteSheet : ScriptableObject
{
    [SerializeField]
    Texture2D spriteSheet;

    [SerializeField]
    Vector2Int spriteAmmounts;

    [SerializeField]
    List<AnimationFrames> animations;

    Dictionary<int, AnimationFrames> animationFramesDictionary;


    bool init_called = false;
    void Init()
    {
        if (init_called)
            return;


        init_called = true;
        animationFramesDictionary = new Dictionary<int, AnimationFrames>();
        for (int i = 0; i < animations.Count; i++)
        {
            animations[i].SetParent(this);
            animationFramesDictionary.Add(animations[i].ID, animations[i]);
        }
    }
    private void OnEnable()
    {
        Init();
    }

    public AnimationFrames GetAnimationFrames(int ID)
    {
        AnimationFrames output;

        if (!animationFramesDictionary.TryGetValue(ID, out output))
        {
            Debug.LogError("Error, could not find Animation in SpriteSheet");
            Debug.Break();
        }
        return output;
    }

    public Texture2D SpriteSheetTexture => spriteSheet;
    public Vector2Int SpriteCount => spriteAmmounts;
    public Vector2 Offset => new Vector2(1.0f / spriteAmmounts.x, 1.0f / spriteAmmounts.y);

    public float SpriteToWidthAspectRation => ((float)(spriteSheet.width / spriteAmmounts.x)) / ((float)(spriteSheet.height/ spriteAmmounts.y));
    public float SpriteToHeightAspectRation => ((float)(spriteSheet.height / spriteAmmounts.y)) / ((float)(spriteSheet.width / spriteAmmounts.x));

    public IReadOnlyList<AnimationFrames> Animations => animations;

    [System.Serializable]
    public class AnimationFrames
    {
        [SerializeField]
        string nameID;
        [SerializeField]
        float duration;
        [SerializeField]
        List<Vector2Int> frameIndexes;

        HDRP_SpriteSheet spriteSheet;
        public void SetParent(HDRP_SpriteSheet spriteSheet_)
        {
            spriteSheet = spriteSheet_;
        }
        public HDRP_SpriteSheet SpriteSheet => spriteSheet;

        public string NameID => nameID;
        public int ID => nameID.GetHashCode();
        public float Duration => duration;
        public List<Vector2Int> FrameIndexes => frameIndexes;
    }
}
