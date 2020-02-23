using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] ThirdPersonCamera camera_;



    INTERACTION_TYPE interaction_type;

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FREE;
        camera_.look_at_target = transform;
    }

    public void OnDisable()
    {
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (GM_.instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            switch (interaction_type)
            {
                case INTERACTION_TYPE.NONE:
                    { }
                    break;
                case INTERACTION_TYPE.FISH:
                    { StateManager.ChangeState(PlayerScriptManager.STATE.FISHING); Debug.Log("FISHING"); }
                    break;
                case INTERACTION_TYPE.RADIO:
                    { StateManager.ChangeState(PlayerScriptManager.STATE.CONVERSATION); Debug.Log("RADIO"); }
                    break;
                case INTERACTION_TYPE.SLEEP:
                    { }
                    break;
                case INTERACTION_TYPE.REPAIR:
                    { StateManager.ChangeState(PlayerScriptManager.STATE.REPAIR); Debug.Log("REPIAR"); }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(enabled)
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

                                GM_.instance.ui.helperButtons.DisableAllButtons();
                                GM_.instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Start Fishing");
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.RADIO:
                            {
                                interaction_type = INTERACTION_TYPE.RADIO;

                                GM_.instance.ui.helperButtons.DisableAllButtons();
                                GM_.instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Talk to Radio");
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.SLEEP:
                            {
                                interaction_type = INTERACTION_TYPE.SLEEP;

                                GM_.instance.ui.helperButtons.DisableAllButtons();
                                GM_.instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Go to Sleep");
                            }
                            break;
                        case TriggerType.TRIGGER_TYPE.REPAIR:
                            {
                                interaction_type = INTERACTION_TYPE.REPAIR;

                                GM_.instance.ui.helperButtons.DisableAllButtons();
                                GM_.instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Plane Repair");

                                break;
                            }
                        default:
                            {
                                interaction_type = INTERACTION_TYPE.NONE;
                                GM_.instance.ui.helperButtons.DisableAllButtons();

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
        GM_.instance.ui.helperButtons.DisableAllButtons();
    }

}