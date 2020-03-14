using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    // Start is called before the first frame update


    Dictionary<ANIMATION_STATE, int> Hashes = new Dictionary<ANIMATION_STATE, int>();
    public enum ANIMATION_STATE
    { 
    NOT_ACTIVE,
    FISH_BITE,
    FISH_TEST,
    FISH_LEFT,
    FISH_RIGHT,
    FISH_INTERACTING
    };

    int EnumState;
    int ResetAnim;

    Transform LookAt;

    void Awake()
    {
        Hashes.Add(ANIMATION_STATE.NOT_ACTIVE, Animator.StringToHash("NotActive"));
        Hashes.Add(ANIMATION_STATE.FISH_BITE, Animator.StringToHash("FishBite"));
        Hashes.Add(ANIMATION_STATE.FISH_TEST, Animator.StringToHash("FishTest"));
        Hashes.Add(ANIMATION_STATE.FISH_LEFT, Animator.StringToHash("FishDirection"));
        Hashes.Add(ANIMATION_STATE.FISH_RIGHT, Animator.StringToHash("FishDirection"));
        Hashes.Add(ANIMATION_STATE.FISH_INTERACTING, Animator.StringToHash("FishInteract"));
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

        AnimatorStateInfo current_anim = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        switch (state)
        {
            case ANIMATION_STATE.NOT_ACTIVE:
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Animator>().Play(Hashes[state],0,0);
                    break;
                }
            case ANIMATION_STATE.FISH_BITE:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<Animator>().Play(Hashes[state], 0, 0);
                    break;
                }
            case ANIMATION_STATE.FISH_TEST:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<Animator>().Play(Hashes[state], 0, 0);
                    break;
                }
            case ANIMATION_STATE.FISH_LEFT:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;

                    if (current_anim.shortNameHash != Hashes[state] || (current_anim.shortNameHash == Hashes[state] && current_anim.normalizedTime > 0.99f))
                    {
                        GetComponent<Animator>().Play(Hashes[state], 0, 0);
                    }

                    break;
                }
            case ANIMATION_STATE.FISH_RIGHT:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = true;

                    if (current_anim.shortNameHash != Hashes[state] || (current_anim.shortNameHash == Hashes[state] && current_anim.normalizedTime > 0.99f))
                    {
                        GetComponent<Animator>().Play(Hashes[state], 0, 0);
                    }
                    break;
                }
            case ANIMATION_STATE.FISH_INTERACTING:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<Animator>().Play(Hashes[state], 0, 0);

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
            transform.rotation = Quaternion.LookRotation(transform.position - LookAt.position,Vector3.up);

        }
    }

}
