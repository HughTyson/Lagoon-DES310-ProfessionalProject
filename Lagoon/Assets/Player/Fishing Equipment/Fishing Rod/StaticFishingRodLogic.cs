using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFishingRodLogic : MonoBehaviour
{


    [SerializeField] Vector2 DefaultRotationXZ = new Vector2(17.76f,-8.18f);

    [SerializeField] Vector3 FishFightingLeftRotation = new Vector3(12,0,28);
    [SerializeField] Vector3 FishFightingMiddleRotation = new Vector3(-5, 0, -8.12f);
    [SerializeField] Vector3 FishFightingRightRotation = new Vector3(-17, 0, -40);

    //[SerializeField] float RotationXMaxUp;
    //[SerializeField] float RotationXMaxDown;

    //[SerializeField] float RotationZMaxLeft;
    //[SerializeField] float RotationZMaxRight;

    //[SerializeField] float analogMovementRodSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(DefaultRotationXZ.x, 0, DefaultRotationXZ.y);
    }

    public enum STATE
    { 
    GO_TO_DEFAULT_POSITION,
    FREEZE
    };

    public enum FISH_FIGHTING_STATE
    { 
    MIDDLE,
    RIGHT,
    LEFT
    }



    STATE current_state = STATE.GO_TO_DEFAULT_POSITION;

    FISH_FIGHTING_STATE fish_fighting_state = FISH_FIGHTING_STATE.MIDDLE;
    public void SetState(STATE state)
    {
        current_state = state;
    }

    public void SetFishFightingState(FISH_FIGHTING_STATE state)
    {
        fish_fighting_state = state;
        current_state = STATE.FREEZE;

        switch(fish_fighting_state)
        {
            case FISH_FIGHTING_STATE.LEFT:
                {
                    transform.localRotation = Quaternion.Euler(FishFightingLeftRotation);
                    break;
                }
            case FISH_FIGHTING_STATE.MIDDLE:
                {
                    transform.localRotation = Quaternion.Euler(FishFightingMiddleRotation);
                    break;
                }
            case FISH_FIGHTING_STATE.RIGHT:
                {
                    transform.localRotation = Quaternion.Euler(FishFightingRightRotation);
                    break;
                }
           
        }
    }
    public FISH_FIGHTING_STATE GetFishFightingState()
    {
        return fish_fighting_state;
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
        switch(current_state)
        {
            case STATE.GO_TO_DEFAULT_POSITION:
                {
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(DefaultRotationXZ.x, 0, DefaultRotationXZ.y), Time.deltaTime);
                    break;
                }
            //case STATE.ANALOG_CONTROL:
            //    {
            //        Vector2 analog = new Vector2(Input.GetAxis("PlayerLH"), Input.GetAxis("PlayerLV")); // normalized to change square into circle
            //        Vector3 eulerRotations = Vector3.zero;
            //        if (analog.x < 0)
            //        {
            //            eulerRotations.z = Mathf.Lerp(RotationZMaxLeft, DefaultRotationXZ.y, analog.x + 1.0f);
            //        }
            //        else
            //        {
            //            eulerRotations.z = Mathf.Lerp(DefaultRotationXZ.y, RotationZMaxRight, analog.x);
            //        }

            //        if (analog.y < 0)
            //        {
            //            eulerRotations.x = Mathf.Lerp(RotationXMaxDown, DefaultRotationXZ.x, analog.y + 1.0f);
            //        }
            //        else
            //        {
            //            eulerRotations.x = Mathf.Lerp(DefaultRotationXZ.x,RotationXMaxUp, analog.y);
            //        }
            //        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(eulerRotations), Time.deltaTime* analogMovementRodSpeed);
            //        break;
            //    }
            case STATE.FREEZE:
                {

                    break;
                }
            //case STATE.ANALOG_CONTROL_H_ONLY:
            //    {
            //        Vector2 analog = new Vector2(Input.GetAxis("PlayerLH"), Input.GetAxis("PlayerLV")); // normalized to change square into circle
            //        Vector3 eulerRotations = transform.localRotation.eulerAngles;
            //        if (analog.x < 0)
            //        {
            //            eulerRotations.z = Mathf.Lerp(RotationZMaxLeft, DefaultRotationXZ.y, analog.x + 1.0f);
            //        }
            //        else
            //        {
            //            eulerRotations.z = Mathf.Lerp(DefaultRotationXZ.y, RotationZMaxRight, analog.x);
            //        }

            //        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(eulerRotations), Time.deltaTime * analogMovementRodSpeed);
            //        break;
            //    }            

        }
    }



}
