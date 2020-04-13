using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System.Linq;

public class PlayerExploreState : BaseState
{

    enum INTERACTION_TYPE
    {
        NONE,
        FISH,
        RADIO,
        SLEEP,
        REPAIR
    }

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_third_person;
    [SerializeField] CelebrationCamera camera_celebration;

    bool radioIsAvailable = false;

    INTERACTION_TYPE interaction_type;

    public void Start()
    {
        GM_.Instance.story.Event_BarrierOpened += ConversationStateAvailable;
        GM_.Instance.story.Event_ConvoEnter += ConversationStateUnavailalble;

    }

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        camera_third_person.current_state = ThirdPersonCamera.STATE.FREE;
        camera_celebration.enabled = false;
        camera_third_person.look_at_target = transform;

        GM_.Instance.DayNightCycle.SetTime();
    }

    public void OnDisable()
    {
    }

    void ConversationStateAvailable()
    {
        radioIsAvailable = true;
    }
    void ConversationStateUnavailalble()
    {
        radioIsAvailable = false;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            switch (interaction_type)
            {
                case INTERACTION_TYPE.NONE:
                    { }
                    break;
                case INTERACTION_TYPE.FISH:
                    { 
                        StateManager.ChangeState(PlayerScriptManager.STATE.FISHING); 
                      //  Debug.Log("FISHING"); 
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                        break;
                    }

                case INTERACTION_TYPE.RADIO:
                    {
                        if (GM_.Instance.story.RequestConversationEnter())
                        {
                            interaction_type = INTERACTION_TYPE.NONE;
                            GAME_UI.Instance.helperButtons.DisableAll();
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                            StateManager.ChangeState(PlayerScriptManager.STATE.CONVERSATION);
                        }
                        else
                        {
                            Debug.LogError("Error in PlayerExploreState. Request for ConversationEnter was denied!");
                            Debug.Break();
                        }
                        break;
                    }
                case INTERACTION_TYPE.SLEEP:
                    {
                        StateManager.ChangeState(PlayerScriptManager.STATE.SLEEP);

                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    }
                    break;
                case INTERACTION_TYPE.REPAIR:
                    { 

                        if(GM_.Instance.DayNightCycle.GetSolar() == TimeMovement.Solar.DAY)
                        {
                            StateManager.ChangeState(PlayerScriptManager.STATE.REPAIR); 
                            //  Debug.Log("REPIAR"); 
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED); 
                        }

                        break;
                    }
                default:
                    break;
            }
        }
    }


    

    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            if (other.GetComponent<TagsScript>() != null)
            {
                if (other.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.TRIGGER))        //check if the collider is a trigger
                {
                    switch (other.GetComponent<TriggerType>().GetTrigger())                         //if it is a trigger then get type of trigger
                    {
                        case TriggerType.TRIGGER_TYPE.FISHING:
                            {
                                interaction_type = INTERACTION_TYPE.FISH;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Start Fishing");
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.RADIO:
                            {
                                if (radioIsAvailable)
                                {
                                    interaction_type = INTERACTION_TYPE.RADIO;
                                    GAME_UI.Instance.helperButtons.DisableAll();
                                    GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Talk to Radio");
                                }
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.SLEEP:
                            {
                                interaction_type = INTERACTION_TYPE.SLEEP;

                                GAME_UI.Instance.helperButtons.DisableAll();
                                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Go to Sleep");
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.REPAIR:
                            {

                                if (GM_.Instance.DayNightCycle.GetSolar() == TimeMovement.Solar.DAY)
                                {
                                    interaction_type = INTERACTION_TYPE.REPAIR;

                                    GAME_UI.Instance.helperButtons.DisableAll();
                                    GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Plane Repair");
                                }
                                else
                                {
                                    GAME_UI.Instance.helperButtons.DisableAll();
                                }

                                break;
                            }
                        default:
                            {
                                interaction_type = INTERACTION_TYPE.NONE;
                                GAME_UI.Instance.helperButtons.DisableAll();

                            }
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interaction_type = INTERACTION_TYPE.NONE;
        GAME_UI.Instance.helperButtons.DisableAll();
    }

}