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
    public float CurrentVelocity => current_velocity;
    public float CurrentNormalizedVelocity => current_velocity / max_velocity;

    SupplyDrop test_drop;

    float g;

    bool journal_open = false;

    AudioSFX sfx_walking;
    [HideInInspector] public AudioManager.SFXInstanceInterface walking;

    float time_passed;

    public enum STATE
    {
        FREE_MOVEMENT,       //Player can move any way they want
        ROT_ONLY,            //Player can only rotate
        ROT_LIMITS,          //Player can only rotate - between limits
        NO_MOVEMENT,         //Player can not move
        ROT_CAMERA           //Rotates to the cameras rotation in real time
    }


    CharacterAnimationHandler animationHandler;
    private void Awake()
    {
        animationHandler = GetComponent<CharacterAnimationHandler>();
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        current_state = STATE.FREE_MOVEMENT;

        test_drop = GetComponent<SupplyDrop>();


        sfx_walking = GM_.Instance.audio.GetSFX("WalkingOnSand");

    }

    float desired_velocity = 0;
    // Update is called once per frame


    float refVelocity_velocity;




    void FixedUpdate()
    {
        switch (current_state)
        {
            case STATE.FREE_MOVEMENT:
                {
                    // when -0.5 = min
                    // when -1 = max
                    float normalizedUnderwaterModified = 1.0f - Mathf.Clamp01(((transform.position.y - (-0.2f)) / (-1.5f - (-0.2f))));
                    normalizedUnderwaterModified = Mathf.Max(normalizedUnderwaterModified, 0.6f);
                    desired_velocity = max_velocity * Mathf.Min(normalizedUnderwaterModified, (new Vector2(GM_.Instance.input.GetAxis(InputManager.AXIS.LH), GM_.Instance.input.GetAxis(InputManager.AXIS.LV)).magnitude));

                    HandleInput();
                    Rotation();

                    //if(!journal_open)
                    //{
                    //    if (Vector3.Magnitude(move_direction) > 1)
                    //    {
                    //        if(walking == null)
                    //        {
                    //            walking = GM_.Instance.audio.PlaySFX(sfx_walking, null);
                    //        }
                    //        else if(walking != null)
                    //        {
                    //            walking.Loop = true;
                    //        }

                    //        time_passed = 0;
                    //    }
                    //    else if(Vector3.Magnitude(move_direction) < 1)
                    //    {
                    //        if(walking != null)
                    //        {
                    //            if (walking.Loop)
                    //            {
                    //                walking.Loop = false;
                    //            }
                    //            time_passed += Time.deltaTime;
                    //        }

                    //        if(time_passed > 0.2)
                    //        {
                    //            time_passed = 0;
                    //            walking = null;
                    //        }
                    //    }
                    //}



                    controller.Move(new Vector3(0,move_direction.y,0)* Time.fixedDeltaTime);
                    //  controller.Move(move_direction * Time.fixedDeltaTime);

                }
                break;
            case STATE.ROT_ONLY:
                {
                    desired_velocity = 0;
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
                    desired_velocity = 0;
                    movement_input = new Vector2(0, 0);
                }
                break;
            case STATE.ROT_CAMERA:
                {
                    desired_velocity = 0;
                    //rotates to face the camera in real time
                    movement_input = new Vector2(0, 0);
                    transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, _camera.localEulerAngles.y, transform.localEulerAngles.z);
                }
                break;
            default:
                break;
        }


        current_velocity = Mathf.SmoothDamp(current_velocity, desired_velocity, ref refVelocity_velocity, acceleration);
        move_direction = transform.forward * current_velocity;
        move_direction.y += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
    }


    
    //methods
    private void HandleInput()
    {
        movement_input = new Vector2(GM_.Instance.input.GetAxis(InputManager.AXIS.LH) * joyStick_sensitivity, GM_.Instance.input.GetAxis(InputManager.AXIS.LV) * joyStick_sensitivity);

        //if(GM_.Instance.input.GetButton(InputManager.BUTTON.Y) && !journal_open)
        //{
        //    journal_open = true;
        //    walking.Stop();
        //    walking = null;
        //}

        //if(GM_.Instance.input.GetButton(InputManager.BUTTON.B))
        //{
        //    journal_open = false;
        //}
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
