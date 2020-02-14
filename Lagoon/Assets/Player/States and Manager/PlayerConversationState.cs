using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConversationState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;
    [SerializeField] ConvoUIManager convoUIManager;
    [SerializeField] ConvoManager convoManager;

    SupplyDrop test_drop;

    enum ConversationState 
    {
        STARTUP,
        CONVERSATION,
        WATCH_DROP
    }
    ConversationState state;

    private void Start()
    {
        test_drop = GetComponent<SupplyDrop>();
    }

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FREE;
        state = ConversationState.WATCH_DROP;

        convoUIManager.enabled = true;

       
  //      convoUIManager.StartupAnimation(convoManager.GetCurrentNode());

    }

    public void OnDisable()
    {
        convoUIManager.enabled = false;

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
                    }
                    
                    if(Input.GetButtonDown("PlayerY"))
                    {
                        test_drop.DestroyBox();
                    }

                    if (Input.GetButtonDown("PlayerB"))
                    {
                        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
                    }

                }




                break;
            default:
                break;
        }

    }
}