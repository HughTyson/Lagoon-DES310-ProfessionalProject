using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{


    // ==========================================
    //              Visible Variables
    //===========================================


    [SerializeField] Transform _camera;

    [SerializeField] float move_speed = 6.0f;

    [Range(0.01f, 0.5f)]
    [SerializeField] float player_rotation_speed = 0.12f;

    [SerializeField] float gravityScale = 20.0f;

    
    [SerializeField] float X_ANGLE_LIMITS = 30f;


    [SerializeField] public STATE current_state;

    // ==========================================
    //              Hidden Variables
    //===========================================

    private CharacterController controller;
    Vector2 movement_input = Vector2.zero;
    private bool moving = false;
    Vector3 gravity;

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
        
    }

    // Update is called once per frame
    void Update()
    {

        switch (current_state)
        {
            case STATE.FREE_MOVEMENT:
                {
                    HandleInput();
                    Rotation();
                    controller.Move(transform.forward * movement_input.magnitude * Time.deltaTime);
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
                }
                break;
            default:
                break;
        }
    }

     //this function is called every fixed framerate frame
    private void FixedUpdate()
    {
        gravity.y = Physics.gravity.y * Time.deltaTime;

        controller.Move(gravity * gravityScale * Time.deltaTime);
    }

    //methods
    private void HandleInput()
    {
        movement_input = new Vector3(Input.GetAxisRaw("PlayerLH") * move_speed, Input.GetAxisRaw("PlayerLV") * move_speed);
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
