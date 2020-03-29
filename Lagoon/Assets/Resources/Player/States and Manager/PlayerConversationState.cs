using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConversationState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;
  //  [SerializeField] ConvoUIManager convoUIManager;
   // [SerializeField] ConvoManager convoManager;

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
        GM_.Instance.story.Event_ConvoExit += ExitConversation;
    }

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FREE;
        state = ConversationState.CONVERSATION;
        GM_.Instance.ui.helperButtons.HideButtons();
    }

    void ExitConversation()
    {
        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
        GM_.Instance.ui.helperButtons.ShowButtons();
    }

    public void OnDisable()
    {
        GM_.Instance.ui.helperButtons.ShowButtons();
    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        switch (state)
        {
            case ConversationState.CONVERSATION:
                {
                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
                    {
                        GM_.Instance.story.RequestButtonPressA();
                    }
                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        GM_.Instance.story.RequestButtonPressB();
                    }
                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.X))
                    {
                        GM_.Instance.story.RequestButtonPressX();
                    }

                }
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