using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{

    Animator animator;

    [SerializeField] float heightOfGround = 0.05f;
    [SerializeField] LayerMask layerMask;

    [SerializeField] CharacterControllerMovement characterControllerMovement;
    int idParam_MovementSpeed = Animator.StringToHash("Movement Speed");

    [SerializeField] float smoothnessTime = 0.2f;
    [SerializeField] float maxSmoothnessVelocity = 1.0f;

    [SerializeField]
    float fromBodyToFootLength = 1.15f;
    [SerializeField]
    float legLength = 1.0f;

    [Range(0,1)]
    [SerializeField] float ikWeighting = 1;

    CharacterController characterController;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;


    [SerializeField] float footLength = 0.12f;

    [SerializeField] GameObject CharacterRoot;
    [Header("Modular Peices")]
    [SerializeField] GameObject Hair_MPeice;
    [SerializeField] GameObject Beard_MPeice;
    [SerializeField] GameObject Brows_MPeice;
    [SerializeField] GameObject Shirt_MPeice;
    [SerializeField] GameObject Trousers_MPeice;
    CharacterModularPieceManager modularPeiceManager;


    [SerializeField] float TestFootVelValue;
    private void Awake()
    {
        audio_sandStep = GM_.Instance.audio.GetSFX("WalkingOnSand");
    }

    AudioSFX audio_sandStep;
    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        modularPeiceManager = new CharacterModularPieceManager(CharacterRoot, "_jnt");
        modularPeiceManager.AddNewModularPeice("HAIR", Hair_MPeice, "_jnt", true);
        modularPeiceManager.AddNewModularPeice("BEARD", Beard_MPeice, "_jnt", true);
        modularPeiceManager.AddNewModularPeice("BROWS", Brows_MPeice, "_jnt", true);
        modularPeiceManager.AddNewModularPeice("SHIRT", Shirt_MPeice, "_jnt", true);
        modularPeiceManager.AddNewModularPeice("TROUSERS", Trousers_MPeice, "_jnt", true);
        //DebugGUI.SetGraphProperties("LeftFootGrounded", "Left Foot Grounded:", -0.1f, 1.1f, 0, Color.green, true);
        //DebugGUI.SetGraphProperties("RightFootGrounded", "Right Foot Grounded:", -0.1f, 1.1f, 1, Color.yellow, true);



    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat(idParam_MovementSpeed, GM_.Instance.input.GetAxis( InputManager.AXIS.RT) + GM_.Instance.input.GetAxis(InputManager.AXIS.LT), 0.1f, Time.deltaTime);


         animator.SetFloat(idParam_MovementSpeed, characterControllerMovement.CurrentNormalizedVelocity, 0.1f, Time.deltaTime);
       // animator.SetFloat(idParam_MovementSpeed, characterControllerMovement.CurrentNormalizedVelocity);

        // Debug.Log("Test: " + animator.GetFloat(idParam_MovementSpeed));
    }

    Vector3 desiredLeftFootIk;
    Vector3 desiredRightFootIk;
    Vector3 desiredBodyIk;


    Vector3 realLeftFootIk;
    Vector3 realRightFootIk;
    Vector3 realBodyIk;


    Vector3 refLeftFootIkVelocity = Vector3.zero;
    Vector3 refRightFootIkVelocity = Vector3.zero;
    Vector3 refBodyIkVelocity = Vector3.zero;


    Vector3 desiredLookAtDir = Vector3.zero;
    Vector3 realLookAtDir = Vector3.zero;
    Vector3 refLookAtDirVelocity = Vector3.zero;


    bool firstOnAnimatorIK = true;

    bool firstFrameLeftStep = false;
    bool firstFrameRightStep = false;
    float rightFootVelocity = 0;
    float leftFootVelocity = 0;

    private void OnAnimatorIK(int layerIndex)
    {



        animator.SetLookAtWeight(0.5f);
        desiredLookAtDir = (animator.GetBoneTransform(HumanBodyBones.Head).position - thirdPersonCamera.transform.position).normalized;
        realLookAtDir = Vector3.SmoothDamp(realLookAtDir, desiredLookAtDir, ref refLookAtDirVelocity, 0.1f, 2.0f, Time.deltaTime);
        animator.SetLookAtPosition(animator.GetBoneTransform(HumanBodyBones.Head).position + desiredLookAtDir);


        animator.bodyPosition += new Vector3(0,heightOfGround,0);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeighting);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikWeighting);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeighting);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikWeighting);



        Vector3 initLeftToeDirection = animator.GetBoneTransform(HumanBodyBones.LeftFoot).position - animator.GetBoneTransform(HumanBodyBones.LeftToes).position;


        float heightDifference_LeftToesAndFeet = Mathf.Max(0, animator.GetBoneTransform(HumanBodyBones.LeftFoot).position.y - animator.GetBoneTransform(HumanBodyBones.LeftToes).position.y);

        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + (Vector3.up* characterController.stepOffset), Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 groundedFootPosition = hit.point;
            groundedFootPosition.y += heightOfGround;


            if (animator.GetFloat("LeftFootGrounded") < 0.5f)
            {


                float heightfromRayOriginToGround = Mathf.Abs(Mathf.Abs(ray.origin.y) - Mathf.Abs(groundedFootPosition.y));
                float heightfromTayOriginToFoot = Mathf.Abs(Mathf.Abs(ray.origin.y) - Mathf.Abs(realLeftFootIk.y));

                if (heightfromRayOriginToGround < heightfromTayOriginToFoot)
                {
                    desiredLeftFootIk = groundedFootPosition;
                }
                else
                {
                    desiredLeftFootIk = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                }
                //animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal)); // do this differently, base it on a ray at the toes aswell
            }
            else
            {


                //          DebugGUI.Graph("LeftFootGrounded", 1);
                desiredLeftFootIk = groundedFootPosition;

            }

            desiredLeftFootIk.y += heightDifference_LeftToesAndFeet;


            //float bodyYChange = Vector3.Distance(groundedFootPosition, animator.bodyPosition) - legLength;
            //if (bodyYChange > 0)
            //{
            //    animator.bodyPosition += (groundedFootPosition - animator.bodyPosition).normalized * bodyYChange;
            //}
        }
        else
        {
                desiredLeftFootIk = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        }


        float heightDifference_RightToesAndFeet = Mathf.Max(0, animator.GetBoneTransform(HumanBodyBones.RightFoot).position.y - animator.GetBoneTransform(HumanBodyBones.RightToes).position.y);

        CharacterController test;
        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + (Vector3.up * characterController.stepOffset), Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 groundedFootPosition = hit.point;
            groundedFootPosition.y += heightOfGround;

            if (animator.GetFloat("RightFootGrounded") < 0.5f)
            {

                //           DebugGUI.Graph("RightFootGrounded", 0);

                float heightfromRayOriginToGround = Mathf.Abs(Mathf.Abs(ray.origin.y) - Mathf.Abs(groundedFootPosition.y));
                float heightfromTayOriginToFoot = Mathf.Abs(Mathf.Abs(ray.origin.y) - Mathf.Abs(realRightFootIk.y));

                if (heightfromRayOriginToGround < heightfromTayOriginToFoot)
                {
                    desiredRightFootIk = groundedFootPosition;
                }
                else
                {
                    desiredRightFootIk = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                }
                // animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal)); // do this differently, base it on a ray at the toes aswell
            }
            else
            {


                //        DebugGUI.Graph("RightFootGrounded", 1);

                desiredRightFootIk = groundedFootPosition;

            }

            desiredRightFootIk.y += heightDifference_RightToesAndFeet;


            //float bodyYChange = Vector3.Distance(groundedFootPosition, animator.bodyPosition) - legLength;
            //if (bodyYChange > 0)
            //{
            //    animator.bodyPosition += (groundedFootPosition - animator.bodyPosition).normalized * bodyYChange;
            //}
        }
        else
        {
            desiredRightFootIk = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        }

        //Vector3 fromBodyPositionToLeftFoot = realLeftFootIk - animator.bodyPosition;
        //Vector3 fromBodyPositionToRightFoot = realRightFootIk - animator.bodyPosition;


        //float currentDistance_LeftFoot_To_Body = fromBodyPositionToLeftFoot.magnitude;
        //float currentDistance_RightFoot_To_Body = fromBodyPositionToRightFoot.magnitude;

        float leftFootYOffset = desiredLeftFootIk.y - animator.GetIKPosition(AvatarIKGoal.LeftFoot).y;
      //  leftFootYOffset -= heightDifference_LeftToesAndFeet;
        float rightFootYOffset = desiredRightFootIk.y - animator.GetIKPosition(AvatarIKGoal.RightFoot).y;
       // rightFootYOffset -= heightDifference_RightToesAndFeet;
        desiredBodyIk = animator.bodyPosition;
        desiredBodyIk.y += Mathf.Max(leftFootYOffset, rightFootYOffset); // move body up based on the higher y value of the legs, keeping the body moving correctly with the animation


        // move body down if one of the feet ik's are too far to reach

        float bodyToLeftFoot = desiredLeftFootIk.y - desiredBodyIk.y;
        float bodyToRightFoot = desiredRightFootIk.y - desiredBodyIk.y;

        float cantReachLeftFootIKAmmount = Mathf.Abs(bodyToLeftFoot) - fromBodyToFootLength;
        float cantReachRightFootIKAmmount = Mathf.Abs(bodyToRightFoot) - fromBodyToFootLength;
        if (cantReachLeftFootIKAmmount > cantReachRightFootIKAmmount)
        {
            desiredBodyIk.y += cantReachLeftFootIKAmmount* Mathf.Sign(bodyToLeftFoot);
        }
        else 
        {
            desiredBodyIk.y += cantReachRightFootIKAmmount * Mathf.Sign(bodyToRightFoot);
        }




        Vector3 prevLeftFootIk;
        Vector3 prevRightFootIk;

        if (firstOnAnimatorIK)
        {

            realLeftFootIk = desiredLeftFootIk;
            realRightFootIk = desiredRightFootIk;
            realBodyIk = desiredBodyIk;
            firstOnAnimatorIK = false;

            prevLeftFootIk = realLeftFootIk;
            prevRightFootIk = realRightFootIk;
        }
        else
        {
            prevLeftFootIk = realLeftFootIk;
            prevRightFootIk = realRightFootIk;

            realLeftFootIk = Vector3.SmoothDamp(realLeftFootIk, desiredLeftFootIk, ref refLeftFootIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);
            realRightFootIk = Vector3.SmoothDamp(realRightFootIk, desiredRightFootIk, ref refRightFootIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);
            realBodyIk = Vector3.SmoothDamp(realBodyIk, desiredBodyIk, ref refBodyIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);



            //CODE FOR TESTING
            //realLeftFootIk = desiredLeftFootIk;
            //realRightFootIk = desiredRightFootIk;
            //realBodyIk = desiredBodyIk;
        }
        leftFootVelocity = (realLeftFootIk.magnitude - prevLeftFootIk.magnitude) / Time.deltaTime;
        rightFootVelocity = (realRightFootIk.magnitude - prevRightFootIk.magnitude) / Time.deltaTime;


        animator.bodyPosition = realBodyIk;
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, realLeftFootIk);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, realRightFootIk);





        audioCalcs();

    }



    void audioCalcs()
    {
        float normalizedVelocity = characterControllerMovement.CurrentNormalizedVelocity;

        if (animator.GetFloat("LeftFootGrounded") < 0.99f)
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
                        GM_.Instance.audio.PlaySFX(audio_sandStep, animator.GetBoneTransform(HumanBodyBones.LeftFoot), settingPitch: footPitch);
                    }


                firstFrameLeftStep = false;
            }
        }
        if (animator.GetFloat("RightFootGrounded") < 0.99f)
        {
            firstFrameRightStep = true;
        }
        else
        {
            if (firstFrameRightStep)
            {

                    if (normalizedVelocity > 0.4f)
                    {
                        AudioSettings.AnyFloatSetting.Constant footPitch = new AudioSettings.AnyFloatSetting.Constant(Mathf.Lerp(0.85f, 1.10f, ((normalizedVelocity - 0.4f) / 0.8f)));
                        GM_.Instance.audio.PlaySFX(audio_sandStep, animator.GetBoneTransform(HumanBodyBones.RightFoot), settingPitch: footPitch);
                    }

                firstFrameRightStep = false;
            }
        }
    }

    private void LateUpdate()
    {
        modularPeiceManager.LateUpdate();
    }
}
