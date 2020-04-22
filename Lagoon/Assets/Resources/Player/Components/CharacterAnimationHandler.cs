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

    
    [SerializeField]
    float defaultBodyYOffset = 1.15f;
    [SerializeField]
    float legLength = 1.0f;

    [Range(0,1)]
    [SerializeField] float ikWeighting = 1;


    [SerializeField] bool TestOn = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       // animator.SetFloat(idParam_MovementSpeed, GM_.Instance.input.GetAxis( InputManager.AXIS.RT), 0.1f, Time.deltaTime);

       
        animator.SetFloat(idParam_MovementSpeed, characterControllerMovement.CurrentNormalizedVelocity, 0.1f, Time.deltaTime);
       // Debug.Log("Test: " + animator.GetFloat(idParam_MovementSpeed));


    }


    //private void OnAnimatorMove()
    //{

    //}
    private void OnAnimatorIK(int layerIndex)
    {

        //Debug.Log(animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        if (!TestOn)
            return;


        //    Vector3 test = animator.bodyPosition;
        // test.y = testY;
        animator.bodyPosition = new Vector3(0, defaultBodyYOffset, 0) + transform.position;
        // animator.SetBoneLocalRotation(HumanBodyBones)
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeighting);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikWeighting);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeighting);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikWeighting);

        RaycastHit hit;
        Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += heightOfGround;
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal)); // do this differently, base it on a ray at the toes aswell

            float bodyYChange = Vector3.Distance(footPosition, animator.bodyPosition) - legLength;
            if (bodyYChange > 0)
            {
                animator.bodyPosition += (footPosition - animator.bodyPosition).normalized * bodyYChange;
            }
        }


        ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, heightOfGround + 3.0f, layerMask))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += heightOfGround;
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal)); // do this differently, base it on a ray at the toes aswell

            float bodyYChange = Vector3.Distance(footPosition, animator.bodyPosition) - legLength;
            if (bodyYChange > 0)
            {
                animator.bodyPosition += (footPosition - animator.bodyPosition).normalized * bodyYChange;
            }
        }


        }
        //private void LateUpdate()
        //{

        //   // animator.GetBoneTransform(HumanBodyBones.Chest).position = new Vector3(50, 0, 0);
        //}
    }
