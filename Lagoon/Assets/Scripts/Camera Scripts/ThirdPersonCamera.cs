﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    // ==========================================
    //              Visible Variables
    //===========================================

    [Header("Targets")]
    [Tooltip("The game object that the camera rotates around")]
    [SerializeField] Transform rot_target; //what the camera rotates around
    [Tooltip("The game object that the camera will look at as it rotates")]
    [SerializeField] public Transform look_at_target; // what the camera looks at

    [Header("Variables used to determine what shoulder camera looks over")]

    [SerializeField] Vector3 camera_offset_right = new Vector3(0.4f ,0.5f, -2.0f);
    [SerializeField] Vector3 camera_offset_left = new Vector3(0f, 0f, 0f);
    [Tooltip("False looks over left shoulder, true looks over right shoulder")]
    [SerializeField] bool shoulder_side = false; // false is for left   |  true is for right

    [Header("Misc")]
    //minimum and maximum angles for the Y axis
    [Tooltip("Minimum angles for the Y axis movement of the camera")]
    [SerializeField] float ANGLE_MIN = -10.0f;
    [Tooltip("Maximum angles for the Y axis movement of the camera")]
    [SerializeField] float ANGLE_MAX = 80.0f;

    [SerializeField] float camera_rotation_speed = 1f; //how fast the rotation of the camera is around the player
    [SerializeField] float camera_movement_speed = 0.3f; //how fast the camera moves to its new position

    [SerializeField] float distance_from_target = 5.0f; //how far away the camera is from the target

    [Header("Varibles used for Camera Collisions")]

    [SerializeField] bool drawDesiredCollisionLines = true;
    [SerializeField] bool drawAdjustedCollisionLines = true;

    // ==========================================
    //              Hidden Variables
    //===========================================

    CameraCollision collision = new CameraCollision();

    private Transform _camera;
    private Vector2 camera_input = Vector2.zero;
   

    //varibales used for calculating new position of the player
    private Vector3 destination;
    private Vector3 adjusted_destination;
    private Vector3 target_pos;
    private Vector3 target_offset;

    private float adjusted_distance = 0f;

    public enum STATE
    {
        NORMAL,       //Camera has free rotation around target
        FISHING       //Camera rotates around the player, but will look at the position of the fishing bob
       
    }
    [SerializeField] public STATE current_state;

    void Start()
    {
        _camera = transform;

        target_pos = rot_target.position;
        destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * distance_from_target;
        destination += target_pos;

        //Collision

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        if(shoulder_side)
        {
            target_offset = camera_offset_right;
        }
        else
        {
            target_offset = camera_offset_left;
        }

        current_state = STATE.NORMAL;
    }

    //update function

    private void Update()
    {
        HandleInput();

        switch (current_state)
        {
            case STATE.NORMAL:
                { }
                break;
            case STATE.FISHING:
                {
                    camera_input.x = Mathf.Clamp(camera_input.x, -50, 50);
                }
                break;
            default:
                break;
        }


        if (shoulder_side)
        {
            target_offset = camera_offset_right;
        }
        else
        {
            target_offset = camera_offset_left;
        }
    }

    private void LateUpdate()
    {

        switch (current_state)
        {
            case STATE.NORMAL:
                {
                    collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
                    collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

                    target_pos = rot_target.position + Vector3.up * target_offset.y + Vector3.forward * target_offset.z + transform.TransformDirection(Vector3.right * target_offset.x);
                    destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * distance_from_target;
                    destination += target_pos;

                    if(collision.collision)
                    {
                        adjusted_destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * adjusted_distance;
                        adjusted_destination += rot_target.position;
                        _camera.position = Vector3.Lerp(_camera.position, adjusted_destination, camera_movement_speed * Time.deltaTime);
                    }
                    else
                    {
                        _camera.position = Vector3.Lerp(_camera.position, destination, camera_movement_speed * Time.deltaTime); ;
                    }

                    _camera.LookAt(target_pos);

                }
                break;
            case STATE.FISHING:
                {
                    collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
                    collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

                    target_pos = rot_target.position + Vector3.up * target_offset.y + Vector3.forward * target_offset.z + transform.TransformDirection(Vector3.right * target_offset.x);
                    destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * distance_from_target;
                    destination += target_pos;

                    if (collision.collision)
                    {
                        adjusted_destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * adjusted_distance;
                        adjusted_destination += rot_target.position;
                        _camera.position = Vector3.Lerp(_camera.position, adjusted_destination, camera_movement_speed * Time.deltaTime);
                    }
                    else
                    {
                        _camera.position = Vector3.Lerp(_camera.position, destination, camera_movement_speed * Time.deltaTime); ;
                    }

                    _camera.LookAt(look_at_target);
                }
                break;
            default:
                break;
        }





    }

    private void FixedUpdate()
    {

        for (int i = 0; i < 5; i++)
        {
            if (drawDesiredCollisionLines)
            {
                Debug.DrawLine(rot_target.position, collision.desiredCameraClipPoints[i], Color.white);
            }

            if (drawAdjustedCollisionLines)
            {
                Debug.DrawLine(rot_target.position, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(rot_target.position); //using raycasts here
        adjusted_distance = collision.GetAdjustedDistanceWithRay(rot_target.position);

    }

    //methods

    private void HandleInput()
    {

        camera_input += new Vector2(Input.GetAxisRaw("PlayerRH") * camera_rotation_speed, Input.GetAxisRaw("PlayerRV") * camera_rotation_speed) * Time.deltaTime;

        camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN, ANGLE_MAX);
    }
}