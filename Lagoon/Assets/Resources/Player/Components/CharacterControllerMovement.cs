﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{

    // ==========================================
    //              Visible Variables
    //===========================================


    [SerializeField] Transform _camera;                         //The cameras informtation

    [SerializeField] float joyStick_sensitivity = 6.0f;         //Value that the input can be multiplied by to increase sensitivity

    [Range(0.01f, 0.5f)]
    [SerializeField] float player_rotation_speed = 0.12f;       //Variable increases the speed of the rotation

    [SerializeField] float gravityScale = 1.0f;                 //1 = 9.8ms^2 - if increased gravity increases            

    
    [SerializeField] float X_ANGLE_LIMITS = 30f;                // limits of the players movement for certain states


    [SerializeField] public STATE current_state;                // the current state fo the character controller

    [SerializeField] float acceleration = 1.0f;                 //The acceleration valeue
    [SerializeField] float max_velocity = 5.0f;                 //The max velocity that the player can reach before being clamped

    // ==========================================
    //              Hidden Variables
    //===========================================

    private CharacterController controller;                     //Define the character contoller
    Vector2 movement_input = Vector2.zero;                      //A vector 2 to store the input taken in when moving the character

    Vector3 move_direction;                                     //stores the move information thats calculated in the 

    float current_velocity;                                     //keeps track of the players current_velocity

    SupplyDrop test_drop;

    float g;

    public enum STATE
    {
        FREE_MOVEMENT,       //Player can move any way they want
        ROT_ONLY,            //Player can only rotate
        ROT_LIMITS,          //Player can only rotate - between limits
        NO_MOVEMENT,         //Player can not move
        ROT_CAMERA           //Rotates to the cameras rotation in real time
    }
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        current_state = STATE.FREE_MOVEMENT;

        test_drop = GetComponent<SupplyDrop>();
        
    }

    // Update is called once per frame
    void Update()
    {
        current_velocity += acceleration * movement_input.magnitude;
        current_velocity = Mathf.Clamp(current_velocity, 0, max_velocity);

        if (movement_input.x == 0 && movement_input.y == 0)
        {
            if (current_velocity > 0)
                current_velocity -= acceleration;
        }

        move_direction = transform.forward * current_velocity * Time.deltaTime;

        move_direction.y = g;

        switch (current_state)
        {
            case STATE.FREE_MOVEMENT:
                {
                    HandleInput();
                    Rotation();

                    controller.Move(move_direction);

                }
                break;
            case STATE.ROT_ONLY:
                {
                    HandleInput();
                    Rotation();
                }
                break;
            case STATE.ROT_LIMITS:
                {
                    HandleInput();

                    movement_input.x = Mathf.Clamp(movement_input.x, -X_ANGLE_LIMITS, X_ANGLE_LIMITS);

                    transform.rotation = Quaternion.Euler(0, movement_input.x, 0);
                }
                break;
            case STATE.NO_MOVEMENT:
                {

                }
                break;
            case STATE.ROT_CAMERA:
                {
                    //rotates to face the camera in real time
 
                    transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, _camera.localEulerAngles.y, transform.localEulerAngles.z);
                }
                break;
            default:
                break;
        }
    }

     //this function is called every fixed framerate frame
    private void FixedUpdate()
    {

        move_direction.y += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
         g = move_direction.y;

    }

    //methods
    private void HandleInput()
    {
        movement_input = new Vector2(GM_.Instance.input.GetAxis(InputManager.AXIS.LH) * joyStick_sensitivity, GM_.Instance.input.GetAxis(InputManager.AXIS.LV) * joyStick_sensitivity);
    }

    private void Rotation()
    {
        //Gets the camera forward and right vector
        Vector3 forward = _camera.forward;
        Vector3 right = _camera.right;

        forward.y = 0.0f;
        forward = forward.normalized;

        right.y = 0.0f;
        right = right.normalized;

        //create target direction for the player to look
        Vector3 target_direction = (forward * movement_input.y) + (right * movement_input.x);

        if (target_direction != Vector3.zero)
        {
            Quaternion target_rotation = Quaternion.LookRotation(target_direction);
            Quaternion new_rotation = Quaternion.Slerp(transform.rotation, target_rotation, player_rotation_speed);

            transform.rotation = new_rotation;
        }
    }
}