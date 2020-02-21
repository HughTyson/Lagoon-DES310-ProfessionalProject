using System.Collections;
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
    [SerializeField] float ANGLE_MIN_Y = -10.0f;
    [Tooltip("Maximum angles for the Y axis movement of the camera")]
    [SerializeField] float ANGLE_MAX_Y = 80.0f;

    //minimum and maximum angles for the Y axis
    [Tooltip("Minimum angles for the X axis movement of the camera")]
    [SerializeField] float ANGLE_MIN_X = 50.0f;
    [Tooltip("Maximum angles for the X axis movement of the camera")]
    [SerializeField] float ANGLE_MAX_X = 50.0f;

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

    private Vector3 cam_velocity = Vector3.zero;

    private float adjusted_distance = 0f;

    private Vector3 temp_eular;

    public enum STATE
    {
        FREE,                 //Camera has free rotation around target
        CLAMPED_LOOK_AT,      //Camera rotates around the player, but will look at the position of the fishing bob
        FIXED_LOOK_AT,
        TRANSITION
       
    }
    [SerializeField] public STATE current_state;

    public STATE transition_;

    void Start()
    {
        _camera = transform;

        target_pos = rot_target.position;
        destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * distance_from_target;
        destination += target_pos;

        //Collision

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjusted_cp_pos);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desired_cp_pos);

        if(shoulder_side)
        {
            target_offset = camera_offset_right;
        }
        else
        {
            target_offset = camera_offset_left;
        }

        current_state = STATE.FREE;
    }

    //update function

    //private void Update()
    //{
    //    HandleInput();

    //    //switch (current_state)
    //    //{
    //    //    case STATE.FREE:
    //    //        {
    //    //            camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN_Y, ANGLE_MAX_Y); //limit the y rotation
    //    //        }
    //    //        break;
    //    //    case STATE.CLAMPED_LOOK_AT:
    //    //        {
    //    //            camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN_Y, ANGLE_MAX_Y); //limit the y rotation
    //    //            camera_input.x = Mathf.Clamp(camera_input.x, rot_target.localRotation.eulerAngles.y - ANGLE_MIN_X, rot_target.localRotation.eulerAngles.y + ANGLE_MAX_X);
    //    //        }
    //    //        break;
    //    //    default:
    //    //        break;
    //    //}


    //    if (shoulder_side)
    //    {
    //        target_offset = camera_offset_right;
    //    }
    //    else
    //    {
    //        target_offset = camera_offset_left;
    //    }
    //}

    private void Update()
    {
        HandleInput();

        camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN_Y, ANGLE_MAX_Y); //limit the y rotation

        switch (current_state)
        {
            case STATE.FREE:
                {

                    destinationUpdate();            //calculate the new poititon of the camera

                    setPosition();                  //set the position based on the new destination

                    Quaternion new_look = Quaternion.LookRotation(rot_target.position - _camera.position);

                    new_look.Normalize();

                    _camera.rotation = Quaternion.Slerp(transform.rotation, new_look, camera_rotation_speed * Time.deltaTime);

                    Debug.Log(camera_input);
                    Debug.Log(transform.rotation.eulerAngles);

                }
                break;
            case STATE.CLAMPED_LOOK_AT:
                {

                    destinationUpdate();            //calculate the new poititon of the camera

                    setPosition();                  //set the position based on the new destination#

                    Quaternion new_look = Quaternion.LookRotation(look_at_target.position - _camera.position, Vector3.up);

                    
                    _camera.rotation = Quaternion.Slerp(transform.rotation, new_look, camera_rotation_speed * Time.deltaTime);
                }
                break;
            case STATE.FIXED_LOOK_AT:
                {

                    destination = rot_target.position + Vector3.up * target_offset.y + Vector3.forward * target_offset.z + transform.TransformDirection(Vector3.right * target_offset.x);            //calculate the new poititon of the camera

                    setPosition();                                                                                                          //set the position based on the new destination#

                    Quaternion new_look = Quaternion.LookRotation(look_at_target.position - _camera.position, Vector3.up);

                    _camera.rotation = Quaternion.RotateTowards(_camera.rotation, new_look, 1);

                    break;
                }
            case STATE.TRANSITION:
                {
                    target_pos = rot_target.position + Vector3.up * target_offset.y + Vector3.forward * target_offset.z + transform.TransformDirection(Vector3.right * target_offset.x);
                    destination = Quaternion.Euler(rot_target.eulerAngles) * -Vector3.forward * distance_from_target;

                    destination += target_pos;

                    setPosition();

                    Quaternion new_look = Quaternion.LookRotation(rot_target.position - _camera.position);

                    _camera.rotation = Quaternion.Slerp(transform.rotation, new_look, camera_rotation_speed * Time.deltaTime);

                    //Debug.Log("Camera: " + _camera.rotation.eulerAngles + "    Player: " + rot_target.eulerAngles);

                    

                    if(transform.eulerAngles == rot_target.eulerAngles)
                    {
                        current_state = transition_;
                        camera_input.Set(rot_target.eulerAngles.y, 0.0f);
                    }

                    
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {

        //collisionUpdate();

        //draw the debug line

        for (int i = 0; i < 5; i++)
        {
            if (drawDesiredCollisionLines)
            {
                Debug.DrawLine(rot_target.position, collision.desired_cp_pos[i], Color.white);
            }

            if (drawAdjustedCollisionLines)
            {
                Debug.DrawLine(rot_target.position, collision.adjusted_cp_pos[i], Color.green);
            }
        }

        collision.CheckColliding(rot_target.position); //using raycasts here
        adjusted_distance = collision.GetAdjustedDistanceWithRay(rot_target.position);

    }

    //methods

    private void HandleInput()
    {
        camera_input += new Vector2(GM_.instance.input.GetAxis(InputManager.AXIS.RH) * camera_rotation_speed, GM_.instance.input.GetAxis(InputManager.AXIS.RV) * camera_rotation_speed) * Time.deltaTime; //get the input from the left stick
    }

    //updates the clip points for the collision
    private void collisionUpdate()
    {
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjusted_cp_pos); 
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desired_cp_pos);
    }

    private void destinationUpdate()
    {
        //set the target position based of the targets current position, the offset values,
        //the input from the controller and the distance that the player is from the camera

        target_pos = rot_target.position + rot_target.up * target_offset.y + rot_target.forward * target_offset.z + transform.TransformDirection(rot_target.right * target_offset.x); 
        destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * -Vector3.forward * distance_from_target;

        destination += target_pos;
    }

    private void setPosition()
    {
        if (collision.collision) //check if there has been a collision
        {
            //calculated an adjusted destination based on the collision
            adjusted_destination = Quaternion.Euler(camera_input.y, camera_input.x, 0) * (-Vector3.forward * adjusted_distance);
            adjusted_destination += rot_target.position;

            //linear interpolation between the camera's current position and its new destination
           
            _camera.position = Vector3.SmoothDamp(_camera.position, adjusted_destination, ref cam_velocity, camera_movement_speed * Time.deltaTime);
        }
        else
        {
            //linear interpolation between the camera's current position and its new destination]
            _camera.position = Vector3.SmoothDamp(_camera.position, destination, ref cam_velocity, camera_movement_speed * Time.deltaTime);
        }
    }
}

