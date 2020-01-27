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

    // ==========================================
    //              Hidden Variables
    //===========================================

    private CharacterController controller;
    Vector2 movement_input = Vector2.zero;
    private bool moving = false;
    Vector3 gravity;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        moving = isMoving();

        Rotation();
        controller.Move(transform.forward * movement_input.magnitude * Time.deltaTime);
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

        if (moving && target_direction != Vector3.zero)
        {
            Quaternion target_rotation = Quaternion.LookRotation(target_direction);
            Quaternion new_rotation = Quaternion.Slerp(transform.rotation, target_rotation, player_rotation_speed);

            transform.rotation = new_rotation;
        }


    }

    private bool isMoving()
    {
        return (movement_input.x != 0) || (movement_input.y != 0);
    }
}
