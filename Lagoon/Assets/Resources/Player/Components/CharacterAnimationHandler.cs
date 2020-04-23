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






    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        


        DebugGUI.SetGraphProperties("LeftFootGrounded", "Left Foot Grounded:", -0.1f, 1.1f, 0, Color.green, true);
        DebugGUI.SetGraphProperties("RightFootGrounded", "Right Foot Grounded:", -0.1f, 1.1f, 1, Color.yellow, true);


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
        float toeLength = initLeftToeDirection.magnitude;
        initLeftToeDirection = initLeftToeDirection.normalized;



        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + (Vector3.up* characterController.stepOffset), Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 groundedFootPosition = hit.point;
            groundedFootPosition.y += heightOfGround;

            if (animator.GetFloat("LeftFootGrounded") < 0.9f)
            {
                DebugGUI.Graph("LeftFootGrounded", 0);

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
                DebugGUI.Graph("LeftFootGrounded", 1);
                desiredLeftFootIk = groundedFootPosition;
            }

            //float bodyYChange = Vector3.Distance(groundedFootPosition, animator.bodyPosition) - legLength;
            //if (bodyYChange > 0)
            //{
            //    animator.bodyPosition += (groundedFootPosition - animator.bodyPosition).normalized * bodyYChange;
            //}
        }



        CharacterController test;
        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + (Vector3.up * characterController.stepOffset), Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 groundedFootPosition = hit.point;
            groundedFootPosition.y += heightOfGround;



            if (animator.GetFloat("RightFootGrounded") < 0.9f)
            {
                DebugGUI.Graph("RightFootGrounded", 0);

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
                DebugGUI.Graph("RightFootGrounded", 1);

                desiredRightFootIk = groundedFootPosition;
            }

            //float bodyYChange = Vector3.Distance(groundedFootPosition, animator.bodyPosition) - legLength;
            //if (bodyYChange > 0)
            //{
            //    animator.bodyPosition += (groundedFootPosition - animator.bodyPosition).normalized * bodyYChange;
            //}
        }


        //Vector3 fromBodyPositionToLeftFoot = realLeftFootIk - animator.bodyPosition;
        //Vector3 fromBodyPositionToRightFoot = realRightFootIk - animator.bodyPosition;


        //float currentDistance_LeftFoot_To_Body = fromBodyPositionToLeftFoot.magnitude;
        //float currentDistance_RightFoot_To_Body = fromBodyPositionToRightFoot.magnitude;

        float leftFootYOffset = desiredLeftFootIk.y - animator.GetIKPosition(AvatarIKGoal.LeftFoot).y;
        float rightFootYOffset = desiredRightFootIk.y - animator.GetIKPosition(AvatarIKGoal.RightFoot).y;

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




        if (firstOnAnimatorIK)
        {
            realLeftFootIk = desiredLeftFootIk;
            realRightFootIk = desiredRightFootIk;
            realBodyIk = desiredBodyIk;
            firstOnAnimatorIK = false;

        }
        else
        {
            realLeftFootIk = Vector3.SmoothDamp(realLeftFootIk, desiredLeftFootIk, ref refLeftFootIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);
            realRightFootIk = Vector3.SmoothDamp(realRightFootIk, desiredRightFootIk, ref refRightFootIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);
            realBodyIk = Vector3.SmoothDamp(realBodyIk, desiredBodyIk, ref refBodyIkVelocity, smoothnessTime, maxSmoothnessVelocity, Time.deltaTime);

        }



        animator.bodyPosition = realBodyIk;
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, realLeftFootIk);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, realRightFootIk);


    }

        //private void LateUpdate()
        //{

        //   // animator.GetBoneTransform(HumanBodyBones.Chest).position = new Vector3(50, 0, 0);
        //}
    }
