using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFishingRodLogic : MonoBehaviour
{


    [SerializeField] Vector2 DefaultRotationXZ = new Vector2(17.76f,-8.18f);

    [SerializeField] Vector3 FishFightingLeftRotation = new Vector3(12,0,28);
    [SerializeField] Vector3 FishFightingMiddleRotation = new Vector3(-5, 0, -8.12f);
    [SerializeField] Vector3 FishFightingRightRotation = new Vector3(-17, 0, -40);

    [SerializeField] Animator characterAnimator;

    //[SerializeField] float RotationXMaxUp;
    //[SerializeField] float RotationXMaxDown;

    //[SerializeField] float RotationZMaxLeft;
    //[SerializeField] float RotationZMaxRight;

    //[SerializeField] float analogMovementRodSpeed = 10;
    // Start is called before the first frame update

    private void Awake()
    {
   //     transform.position = characterAnimator.GetBoneTransform(HumanBodyBones.LeftHand).position;
   //     transform.rotation = characterAnimator.GetBoneTransform(HumanBodyBones.LeftHand).rotation;
    }
    void Start()
    {
        //transform.localRotation = Quaternion.Euler(DefaultRotationXZ.x, 0, DefaultRotationXZ.y);
    }

    public enum STATE
    { 
    GO_TO_DEFAULT_POSITION,
    FREEZE
    };



    STATE current_state = STATE.GO_TO_DEFAULT_POSITION;
    public void SetState(STATE state)
    {
        current_state = state;
    }

    public void SetFishFightingState(float analog_value)
    {

    //    current_state = STATE.FREEZE;

    //    Vector3 eulerRotations;

    //    if (analog_value < 0)
    //    {
    //        eulerRotations = Vector3.Lerp(FishFightingLeftRotation, FishFightingMiddleRotation, analog_value + 1.0f);
    //    }
    //    else
    //    {
    //        eulerRotations = Vector3.Lerp(FishFightingMiddleRotation, FishFightingRightRotation, analog_value);
    //    }

    //    transform.localRotation = Quaternion.Euler(eulerRotations);
    }

    public void SetRodRotation(float percentageX, float percentageZ)
    {
        //Vector3 eulerRotations = Vector3.zero;
        //if (percentageX < 0)
        //{
        //    eulerRotations.z = Mathf.Lerp(RotationZMaxLeft, DefaultRotationXZ.y, percentageX + 1.0f);
        //}
        //else
        //{
        //    eulerRotations.z = Mathf.Lerp(DefaultRotationXZ.y, RotationZMaxRight, percentageX);
        //}

        //if (percentageZ < 0)
        //{
        //    eulerRotations.x = Mathf.Lerp(RotationXMaxDown, DefaultRotationXZ.x, percentageZ + 1.0f);
        //}
        //else
        //{
        //    eulerRotations.x = Mathf.Lerp(DefaultRotationXZ.x, RotationXMaxUp, percentageZ);
        //}
        //transform.localRotation = Quaternion.Euler(eulerRotations);
    }
    // Update is called once per frame
    void Update()
    {

        transform.localRotation = Quaternion.Euler(rotationOffset);
        transform.localPosition = positionOffset;

        //switch(current_state)
        //{
        //    case STATE.GO_TO_DEFAULT_POSITION:
        //        {
        //            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(DefaultRotationXZ.x, 0, DefaultRotationXZ.y), 1);
        //            break;
        //        }

        //}
    }


    [SerializeField] Vector3 rotationOffset;
    [SerializeField] Vector3 positionOffset;


    bool firstLateUpate = true;


    private void LateUpdate()
    {


   //     transform.position = handTransform.TransformPoint(positionOffset);
   //     transform.rotation = initRotaiton * (handTransform.rotation * Quaternion.Inverse(initHandRotation));// * Quaternion.Euler(rotationOffset) ;



    }


    //public void OnDrawGizmos()
    //{
    //    Transform handTransform = characterAnimator.GetBoneTransform(HumanBodyBones.LeftHand);


    //    Gizmos.DrawWireSphere(handTransform.TransformPoint(positionOffset), 0.1f);
    //}

}
