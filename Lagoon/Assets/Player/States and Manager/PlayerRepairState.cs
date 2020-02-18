using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairState : BaseState
{

    List<PlaneSegments> plane_segments;

    PlaneSegments.SegmentType part;


    // varibles used to control player in plane
    int selected_part;
    bool selected = false;

    float counter;
    [SerializeField] float transition_time = 0.3f;

    bool just_disabled = false;
    public void OnEnable()
    {
        part = PlaneSegments.SegmentType.PROPELLER;
    }

    public void OnDisable()
    {
        
    }

    public override void StateUpdate()
    {


        HandelInput();



        switch (plane_segments[selected_part].type)
        {
            case PlaneSegments.SegmentType.PROPELLER:
                {
                    if(!selected)
                    {
                        //set the look at and the new cameras position
                    }
                    else if(selected)
                    {
                        //zoomed in the camera
                    }
                    //set the new camera's position and the new cameras look at
                    
                }
                break;
            case PlaneSegments.SegmentType.ENGINE_FRONT:
                {

                }
                break;
            case PlaneSegments.SegmentType.ENGINE_MID:
                {

                }
                break;
            case PlaneSegments.SegmentType.COCKPIT:
                {

                }
                break;
            case PlaneSegments.SegmentType.LEFTWING:
                {

                }
                break;
            case PlaneSegments.SegmentType.RIGHTWING:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_MID:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_BACK:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID:
                {

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_BACK:
                break;
            default:
                break;
        }


        //check if the segment has been selected by the player
        //if it has then update the segments

        if (selected)
        {
            plane_segments[selected_part].SegmentUpdate();
        }

        if(just_disabled)
        {
            plane_segments[selected_part].CleanUp();
        }
    }

    void HandelInput()
    {

        if(Input.GetButtonDown("PlayerA"))
        {
            selected = true;
        }

        if(Input.GetButtonDown("PlayerB"))
        {
            selected = false;
            just_disabled = true;
        }

        if(!selected)
        {
            if(Input.GetAxisRaw("PlayerLH") > 0.2)
            {
                if(counter > transition_time)
                {
                    counter = 0;
                    selected_part++;

                    if(selected_part > 12)
                    {
                        selected_part = 0;
                    }

                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = 0;
            }

            if (Input.GetAxisRaw("PlayerLH") < 0.2)
            {
                if (counter > transition_time)
                {
                    counter = 0;
                    selected_part--;

                    if (selected_part < 0)
                    {
                        selected_part = 12;
                    }
                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = 0;
            }
        }

    }

}
