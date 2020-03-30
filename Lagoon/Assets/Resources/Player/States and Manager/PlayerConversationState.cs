using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConversationState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;

    private void Start()
    {
        GM_.Instance.story.Event_ConvoExit += ExitConversation;
    }

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        camera_.current_state = ThirdPersonCamera.STATE.FREE;
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
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            GM_.Instance.story.RequestButtonPressA();
        }
        else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
        {
            GM_.Instance.story.RequestButtonPressB();
        }
        else if(GM_.Instance.input.GetButtonDown(InputManager.BUTTON.X))
        {
            GM_.Instance.story.RequestButtonPressX();
        }
    }
}