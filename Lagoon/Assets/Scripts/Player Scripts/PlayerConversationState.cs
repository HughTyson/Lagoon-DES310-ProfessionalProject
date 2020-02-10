using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConversationState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;

    SupplyDrop test_drop;

    enum ConversationState 
    {
        CONVERSATION,
        WATCH_DROP
    }
    ConversationState state;

    private void Start()
    {
        test_drop = GetComponent<SupplyDrop>();
    }

    private void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FIXED_LOOK_AT;
        state = ConversationState.WATCH_DROP;
    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        switch (state)
        {
            case ConversationState.CONVERSATION:
                { }
                break;
            case ConversationState.WATCH_DROP:
                {
                    
                    if(Input.GetButtonDown("PlayerA"))
                    {
                        Debug.Log("HELLO");

                        test_drop.Spawn();

                        camera_.look_at_target = ;
                    }
                    
                    if(Input.GetButtonDown("PlayerB"))
                    {
                        test_drop.DestroyBox();
                    }

                }




                break;
            default:
                break;
        }

    }
}