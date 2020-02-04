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
    }

    private void OnDisable()
    {
    }

    private void Update()
    {
        if(Input.GetButtonDown("PlayerA"))
        {
            StateManager.ChangeState(PlayerScriptManager.STATE.FISHING);
        }
    }

    

}