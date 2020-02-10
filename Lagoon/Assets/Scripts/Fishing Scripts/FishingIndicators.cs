using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingIndicators : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ANIMATION_STATE
    { 
    NOT_ACTIVE,
    FISH_BITE,
    FISH_TEST,
    FISH_LEFT,
    FISH_RIGHT    
    };

    int EnumState;
    int ResetAnim;

    int FishTest;
    int FishBite;
    Transform LookAt;

    void Awake()
    {
        EnumState = Animator.StringToHash("EnumState");
        ResetAnim = Animator.StringToHash("ResetAnim");
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
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Animator>().SetTrigger(ResetAnim);
                    break;
                }
            case ANIMATION_STATE.FISH_BITE:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<Animator>().SetTrigger(ResetAnim);
                    break;
                }
            case ANIMATION_STATE.FISH_TEST:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    GetComponent<Animator>().SetTrigger(ResetAnim);
                    break;
                }
            case ANIMATION_STATE.FISH_LEFT:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = false;
                    break;
                }
            case ANIMATION_STATE.FISH_RIGHT:
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<SpriteRenderer>().flipX = true;
                    break;
                }
        }

        GetComponent<Animator>().SetInteger(EnumState, (int)state);
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
