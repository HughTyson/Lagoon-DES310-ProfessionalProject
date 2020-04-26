using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSteppingAudioHandler : MonoBehaviour
{

    CharacterAnimationHandler animHandler;
    CharacterControllerMovement characterControllerMovement;
    Animator animator;

    [SerializeField] LayerMask layerMask_StepAudio;



    FootTrigger leftFoot;
    FootTrigger rightFoot;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animHandler = GetComponent<CharacterAnimationHandler>();
        characterControllerMovement = GetComponent<CharacterControllerMovement>();

        leftFoot = new FootTrigger(animator, "LeftFootGrounded",animator.GetBoneTransform(HumanBodyBones.LeftFoot), animHandler, characterControllerMovement);
        rightFoot = new FootTrigger(animator, "RightFootGrounded", animator.GetBoneTransform(HumanBodyBones.RightFoot), animHandler, characterControllerMovement);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // occurs after the AnimatorIK has been called
    void LateUpdate()
    {
        leftFoot.ApplyFootAudioTrigger(GetCharacterFootAudioTrigger(animator.GetBoneTransform(HumanBodyBones.LeftFoot).position));
        rightFoot.ApplyFootAudioTrigger(GetCharacterFootAudioTrigger(animator.GetBoneTransform(HumanBodyBones.RightFoot).position));


        leftFoot.Update();
        rightFoot.Update();
    }


    CharacterFootAudioTrigger GetCharacterFootAudioTrigger(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.01f, layerMask_StepAudio);

        if (colliders.Length != 0)
        {
            return colliders[0].GetComponent<CharacterFootAudioTriggerLogic>().AudioTrigger;
        }
        return null;
    }


    class FootTrigger
    {
        Animator animator;
        CharacterAnimationHandler animHandler;
        CharacterControllerMovement characterControllerMovement;
        CharacterFootAudioTrigger audioTrigger = null;
        Transform boneTransform;
        string animatorGroundedParameter;


        bool firstFrameLeftStep = false;
        bool firstFrameRightStep = false;
        float rightFootVelocity = 0;
        float leftFootVelocity = 0;

        public FootTrigger(Animator animator_, string animatorGroundedParameter_, Transform boneTransform_, CharacterAnimationHandler animHandler_, CharacterControllerMovement characterControllerMovement_)
        {
            characterControllerMovement = characterControllerMovement_;
            animatorGroundedParameter = animatorGroundedParameter_;
            animHandler = animHandler_;
            animator = animator_;
            boneTransform = boneTransform_;
        }

        public void ApplyFootAudioTrigger(CharacterFootAudioTrigger audioTrigger_)
        {
            audioTrigger = audioTrigger_;
        }

        public void Update()
        {
            if (audioTrigger == null)
                return;

            float normalizedVelocity = characterControllerMovement.CurrentNormalizedVelocity;

            if (animator.GetFloat(animatorGroundedParameter) < 0.99f)
            {
                firstFrameLeftStep = true;
            }
            else
            {
                if (firstFrameLeftStep)
                {
                    if (normalizedVelocity > 0.4f)
                    {
                        AudioSettings.AnyFloatSetting.Constant footPitch = new AudioSettings.AnyFloatSetting.Constant(Mathf.Lerp(0.85f, 1.10f, ((normalizedVelocity - 0.4f) / 0.8f)));
                        GM_.Instance.audio.PlaySFX(audioTrigger.audio_SFX, boneTransform, settingPitch: footPitch);
                    }
                    firstFrameLeftStep = false;
                }
            }
        }
    
    }
}
