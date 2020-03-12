using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairState : BaseState
{

    float transition_time = 0.2f;
    [SerializeField] List<PlaneSegments> plane_segments;

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;

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

        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
     
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

                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Access Propeller");

                                //set the camera's new position and look at based on the segment that has been selected

                            }
                            break;
                        case PlaneSegments.SegmentType.ENGINE_FRONT:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Engine Front");
                            }
                            break;
                        case PlaneSegments.SegmentType.ENGINE_MID:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "ENGINE_MID");
                            }
                            break;
                        case PlaneSegments.SegmentType.COCKPIT:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "COCKPIT");
                            }
                            break;
                        case PlaneSegments.SegmentType.LEFTWING:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "LEFTWING");
                            }
                            break;
                        case PlaneSegments.SegmentType.RIGHTWING:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "RIGHTWING");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_LEFT_FRONT");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_LEFT_MID:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_LEFT_MID");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_LEFT_BACK:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_LEFT_BACK");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_RIGHT_FRONT");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID:
                            {
                                GM_.Instance.ui.helperButtons.DisableAll();
                                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_RIGHT_MID");
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

                    //update the segment that has been selected by the player
                    plane_segments[selected_part].SegmentUpdate();

                    //if this segment has already been completed then change back to the plane
                    if(plane_segments[selected_part].segment_complete)
                    {
                        state = RepairState.FULLPLANE;
                    }
                    
                }
                break;
            default:
                break;
        }



    }

    void HandelInput()
    {

        if(GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A)) //if the a button is pressed then updaate the segment that is currently selected
        {
            if(!plane_segments[selected_part].segment_complete)
            {
                state = RepairState.SEGMENT;
            }
        }

        if(GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B)) //if the B button is pressed then either change the state to the explore state, or if in a segment then move back to segment selector
        {
            if (state == RepairState.FULLPLANE)
            {
                StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
            }

            state = RepairState.FULLPLANE;
        }

        if(state != RepairState.SEGMENT)
        {
            if(GM_.Instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
            {
                if(counter >= transition_time)
                {
                    counter = 0;
                    selected_part++;

                    if(selected_part > 11 -1)
                    {
                        selected_part = 0;
                    }

                }
                else
                {
                    counter += Time.deltaTime;
                }
            } else if (GM_.Instance.input.GetAxis(InputManager.AXIS.LH) < -0.2)
            {
                if (counter >= transition_time)
                {
                    counter = 0;
                    selected_part--;

                    if (selected_part < 0)
                    {
                        selected_part = 11 -1;
                    }
                }
                else
                {
                    counter += Time.deltaTime;
                }
            }
            else
            {
                counter = transition_time;
            }
        }

    }

}
