using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExploreState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera camera_;

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
            StateManager.ChangeState(PlayerScriptManager.STATE.FISHING);
        }

        if (Input.GetButtonDown("PlayerX"))
        {
            StateManager.ChangeState(PlayerScriptManager.STATE.CONVERSATION);
        }
    }



}