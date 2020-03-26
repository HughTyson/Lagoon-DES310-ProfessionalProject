using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] HDRP_SpriteSheet spriteSheet_FishBite;
    [SerializeField] HDRP_SpriteSheet spriteSheet_FishTest;

    Dictionary<ANIMATION_STATE, int> Hashes = new Dictionary<ANIMATION_STATE, int>();
    public enum ANIMATION_STATE
    { 
    NOT_ACTIVE,
    FISH_BITE,
    FISH_TEST,
    FISH_LEFT, // unused
    FISH_RIGHT // unused
    };

    Transform LookAt;



    HDRP_Unlit_Animator animator;
    void Awake()
    {
        animator = GetComponent<HDRP_Unlit_Animator>();
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
                    animator.StopAnimation();
                    break;
                }
            case ANIMATION_STATE.FISH_BITE:
                {
                    Vector3 scale = transform.localScale;
                    scale.y = scale.x * spriteSheet_FishBite.SpriteToHeightAspectRation;
                    transform.localScale = scale;
                    animator.PlayAnimation(spriteSheet_FishBite.Animations[0]);
                    break;
                }
            case ANIMATION_STATE.FISH_TEST:
                {
                    Vector3 scale = transform.localScale;
                    scale.y = scale.x * spriteSheet_FishTest.SpriteToHeightAspectRation;
                    transform.localScale = scale;
                    animator.PlayAnimation(spriteSheet_FishTest.Animations[0]);
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
