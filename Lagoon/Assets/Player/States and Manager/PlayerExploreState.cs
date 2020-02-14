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
        SLEEP
    }

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;

    INTERACTION_TYPE interaction_type;

    private void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FREE;
        camera_.look_at_target = transform;
    }

    private void OnDisable()
    {
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (Input.GetButtonDown("PlayerA"))
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
                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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
                        }
                        break;
                    case TriggerType.TRIGGER_TYPE.RADIO:
                        {
                            interaction_type = INTERACTION_TYPE.RADIO;
                        }
                        break;
                    case TriggerType.TRIGGER_TYPE.SLEEP:
                        {
                            interaction_type = INTERACTION_TYPE.SLEEP;
                        }
                        break;
                    default:
                        {
                            interaction_type = INTERACTION_TYPE.NONE;
                        }
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interaction_type = INTERACTION_TYPE.NONE;
    }

}