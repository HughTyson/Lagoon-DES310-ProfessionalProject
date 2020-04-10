using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConversationState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] SupplyDropCamera supply_cam;
    [SerializeField] ThirdPersonCamera camera_;

    private void Start()
    {
        GM_.Instance.story.Event_ConvoExit += ExitConversation;
    }

    public void OnEnable()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        camera_.enabled = false;
        supply_cam.enabled = true;
        GAME_UI.Instance.helperButtons.HideButtons();

    }

    void ExitConversation()
    {
        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
        GAME_UI.Instance.helperButtons.ShowButtons();
    }

    public void OnDisable()
    {
        GAME_UI.Instance.helperButtons.ShowButtons();
        supply_cam.enabled = false;
        camera_.enabled = true;
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