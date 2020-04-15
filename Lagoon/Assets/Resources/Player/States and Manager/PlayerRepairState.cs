using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairState : BaseState
{

    float transition_time = 0.2f;
    [SerializeField] List<PlaneSegments> plane_segments;

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera third_person_camera;
    [SerializeField] PlaneCamera plane_camera;

    PlaneSegments.SegmentType part;

    // varibles used to control player in plane
    int selected_part;

    float counter;

    bool text = false;

    enum RepairState
    {
        FULLPLANE,
        SEGMENT
    }

    RepairState state;

    bool update_stats = false;




    public void OnEnable()
    {

        state = RepairState.FULLPLANE;

        selected_part = 0;

        counter = 0;

        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        third_person_camera.enabled = false;
          
        for(int i = 0; i < plane_segments.Count; i++)
        {
            if(plane_segments[selected_part].type == PlaneSegments.SegmentType.PROPELLER)
            {
                plane_camera.old_look_at = plane_segments[selected_part].transform;
            }
        }

        plane_camera.enabled = true;

        GAME_UI.Instance.state_repair.Show();

        if(text)
        {
            GAME_UI.Instance.state_repair.Complete(plane_segments[selected_part].segment_complete);
        }

        text = true;

        GM_.Instance.DayNightCycle.SetTime(0.0f);

        if(update_stats)
        {
            UpdateStats();
        }
            
    }

    public void OnDisable()
    {
        plane_camera.enabled = false;
        third_person_camera.enabled = true;

        GAME_UI.Instance.state_repair.Hide();


        UpdateStats();
        

        
        update_stats = true;
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

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.PROPELLER;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Propeller");
                                
                            }
                            break;
                        case PlaneSegments.SegmentType.ENGINE_FRONT:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.ENGINE_FRONT;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Engine");
                            }
                            break;
                        case PlaneSegments.SegmentType.ENGINE_MID:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.ENGINE_MID;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Top Fuselage");
                            }
                            break;
                        case PlaneSegments.SegmentType.COCKPIT:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.COCKPIT;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Cockpit");
                            }
                            break;
                        case PlaneSegments.SegmentType.LEFTWING:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.LEFTWING;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "LEFTWING");
                            }
                            break;
                        case PlaneSegments.SegmentType.RIGHTWING:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.RIGHTWING;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Rightwing");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Front Fuselage");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_LEFT_MID:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.FUSELAGE_LEFT_MID;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;


                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Rear Manifold");
                            }
                            break;
                        case PlaneSegments.SegmentType.TAIL:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.TAIL;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Tail");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_RIGHT_FRONT");
                            }
                            break;
                        case PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID:
                            {

                                plane_camera.active_segment_type = PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID;
                                plane_camera.current_look_at = plane_segments[selected_part].transform;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "FUSELAGE_RIGHT_MID");
                            }
                            break;
                        default:
                            break;
                    }
                }

                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Exit");
                GAME_UI.Instance.helperButtons.EnableLeftStick(true, true, false, false, "Change Segment");

                break;
            case RepairState.SEGMENT:
                {

                    GAME_UI.Instance.helperButtons.DisableAll();
                    GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Exit");

                    HandelInput();

                    //update the segment that has been selected by the player
                    plane_segments[selected_part].SegmentUpdate();

                    //if this segment has already been completed then change back to the plane
                    if (plane_segments[selected_part].segment_complete)
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
        if (!plane_camera.disable_input)
        {
            if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A)) //if the a button is pressed then updaate the segment that is currently selected
            {
                if(state != RepairState.SEGMENT)
                {
                    if (!plane_segments[selected_part].segment_complete)
                    {

                        if(GM_.Instance.inventory.SearchFor(typeof(SwitchItem)))    //this owuld need to be changed so that every plane segement would have required objects ot be acceses. However for time's sake this was the best option
                        {

                            state = RepairState.SEGMENT;

                            plane_camera.current_state = PlaneCamera.PlaneCameraStates.SEGMENT;
                            plane_camera.zoom = true;

                        }
                    }
                }
            }

            if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B)) //if the B button is pressed then either change the state to the explore state, or if in a segment then move back to segment selector
            {
                if (state == RepairState.FULLPLANE)
                {
                    StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
                }

                if (!plane_segments[selected_part].segment_complete)
                {
                    if (plane_camera.current_state == PlaneCamera.PlaneCameraStates.SEGMENT)
                    {
                        plane_camera.un_zoom = true;
                        Debug.Log("UNZOOM");
                    }
                }

                state = RepairState.FULLPLANE;
            }

            if (state != RepairState.SEGMENT)
            {
                if (GM_.Instance.input.GetAxis(InputManager.AXIS.LH) > 0.2)
                {
                    if (counter >= transition_time)
                    {
                        counter = 0;
                        selected_part++;

                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);

                        if (selected_part > plane_segments.Count - 1)
                        {
                            selected_part = 0;
                            
                        }

                        GAME_UI.Instance.state_repair.Complete(plane_segments[selected_part].segment_complete);

                    }
                    else
                    {
                        counter += Time.deltaTime;
                    }

                    
                }
                else if (GM_.Instance.input.GetAxis(InputManager.AXIS.LH) < -0.2)
                {
                    if (counter >= transition_time)
                    {
                        counter = 0;
                        selected_part--;

                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);

                        if (selected_part < 0)
                        {
                            selected_part = plane_segments.Count - 1;
                            
                        }

                        GAME_UI.Instance.state_repair.Complete(plane_segments[selected_part].segment_complete);

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


    void AddStats()
    {

    }


    void UpdateStats()
    {

            for (int i = 0; i < plane_segments.Count; i++)
            {
                GM_.Instance.stats.UpdateSegment(plane_segments[i].segment_complete, plane_segments[i].type, plane_segments[i].segment_name, i);
            }

    }
}
