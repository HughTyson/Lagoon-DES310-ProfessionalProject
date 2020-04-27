using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Foot Audio Trigger")]
public class CharacterFootAudioTrigger : ScriptableObject
{
    [SerializeField] AudioSFX audioSFX;


    [Header("Play Sound Conditions")]
    [SerializeField] bool characterAnimationIsGrounded = true;
    [Range(0,1)]
    [SerializeField] float characterAnimationIsGroundedVal = 0.5f;
    [SerializeField] bool touchingGround = true;
    [SerializeField] float minVelocity;

    [Range(0,255)]
    [SerializeField] int defaultMultipleTriggeredPriority = 128;

    public AudioSFX audio_SFX => audioSFX;
    public bool CharacterAnimationIsGrounded => characterAnimationIsGrounded;
    public float CharacterAnimationIsGroundedVal => characterAnimationIsGroundedVal;
    public bool TouchingGround => touchingGround;
    public float MinVelocity => minVelocity;

    public int DefaultMultipleTriggeredPriority => defaultMultipleTriggeredPriority;

}
