using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestIK : MonoBehaviour
{

        protected Animator animator;

        public bool ikActive = false;
        public Transform rightHandObj = null;
        public Transform lookObj = null;

    [Range(0,1)]
    public float ikWeighing = 0;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(ikWeighing);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeighing);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeighing);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                   // animator.SetIKPosition(AvatarMaskBodyPart.);

       
 
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
