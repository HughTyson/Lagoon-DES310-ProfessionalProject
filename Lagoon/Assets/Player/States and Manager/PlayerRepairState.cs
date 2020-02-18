using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairState : BaseState
{

    [SerializeField] float transition_time = 0.3f;
    [SerializeField] List<PlaneSegments> plane_segments;

    [SerializeField] ButtonUIManager buttonUIManager;
    [SerializeField] CharacterControllerMovement movement_;
    PlaneSegments.SegmentType part;


    // varibles used to control player in plane
    int selected_part;
  

    float counter;


    enum RepairState
    {
        FULLPLANE,
        SEGMENT
    }

    RepairState state;

    bool just_disabled = false;
    public void OnEnable()
    {

        state = RepairState.FULLPLANE;

        selected_part = 0;

        counter = 0;

       
    }

    public void OnDisable()
    {
        
    }

    public override void StateUpdate()
    {


        

        switch (state)
        {
            case RepairState.FULLPLANE:
                {
                    HandelInput();

                    switch (plane_segments[selected_part].type)
                    {
                        case PlaneSegments.SegmentType.PROPELLER:
                            {

                                buttonUIManager.DisableAllButtons();
                                buttonUIManager.EnableButton(ButtonUIManager.BUTTON_TYPE.A, "Access Propeller");

                                //set the camera's new position and look at based on the segment that has been selected

                            }
                            break;
                        case PlaneSegments.SegmentType.ENGINE_FRONT:
                            {
                                buttonUIManager.DisableAllButtons();
                                buttonUIManager.EnableButton(ButtonUIManager.BUTTON_TYPE.A, "Engine Front");
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

                }
                break;
            case RepairState.SEGMENT:
                {
                    HandelInput();

                    //set the cameras new distance from the segment.

                    plane_segments[selected_part].SegmentUpdate();
                    
                }
                break;
            default:
                break;
        }

        


        //check if the segment has been selected by the player
        //if it has then update the segments




    }

    void HandelInput()
    {

        if(GM_.instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
           
            state = RepairState.SEGMENT;
        }

        if(GM_.instance.input.GetButtonDown(InputManager.BUTTON.B))
        {
            if (state == RepairState.FULLPLANE)
            {
                StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
            }

            state = RepairState.FULLPLANE;
        }

        if(state != RepairState.SEGMENT)
        {
            if(GM_.instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
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

            if (GM_.instance.input.GetAxis(InputManager.AXIS.LH) < 0.2)
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
