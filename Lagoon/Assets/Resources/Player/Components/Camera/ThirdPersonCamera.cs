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
    [Tooltip("The transform of the player")]
    [SerializeField] Transform characters_transform;
    [Tooltip("The offset from the origin of the target")]
    [SerializeField] Vector3 target_offset = new Vector3(0.0f ,3.5f, 0.0f);
    
    
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

    [Header("TEST")]

    [SerializeField] float player_step = 1.0f;
    [SerializeField] float bob_step = 1.0f;

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
    

    private Vector3 cam_velocity = Vector3.zero;

    private float adjusted_distance = 0f;

    private Vector3 temp_eular;

    int x_invert = 1;
    int y_invert = -1;

    public enum STATE
    {
        FREE,                 //Camera has free rotation around target
        CLAMPED_LOOK_AT,      //Camera rotates around the player, but will look at the position of the fishing bob
        NO_MOVEMENT
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

        current_state = STATE.FREE;
    }

    //update function
    private void LateUpdate()
    {

        HandleInput();


        switch (current_state)
        {
            case STATE.FREE:
                {
                    camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN_Y, ANGLE_MAX_Y); //limit the y rotation
                }
                break;
            case STATE.CLAMPED_LOOK_AT:
                {
                    camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN_Y, ANGLE_MAX_Y); //limit the y rotation
                    camera_input.x = Mathf.Clamp(camera_input.x, characters_transform.localRotation.eulerAngles.y - ANGLE_MIN_X, characters_transform.localRotation.eulerAngles.y + ANGLE_MAX_X);
                }
                break;
            case STATE.NO_MOVEMENT:
                {
                }
                break;
            default:
                break;
        }

        collisionUpdate();

        //draw the debug line

        for (int i = 0; i < 4; i++)
        {
            if (drawDesiredCollisionLines)
            {
                Debug.DrawLine((rot_target.position + target_offset), collision.desired_cp_pos[i], Color.white);
            }

            if (drawAdjustedCollisionLines)
            {
                Debug.DrawLine((rot_target.position + target_offset), collision.adjusted_cp_pos[i], Color.green);
            }
        }

        collision.CheckColliding((rot_target.position + target_offset));
        adjusted_distance = collision.GetAdjustedDistanceWithRay((rot_target.position + target_offset));

        switch (current_state)
        {
            case STATE.FREE:
                {

                    destinationUpdate();                                 //calculate the new poititon of the camera

                    setPosition();                                      //set the position based on the new destination

                    SetLookAt((rot_target.position + target_offset));   //set the direction that the camera will face
                }
                break;
            case STATE.CLAMPED_LOOK_AT:
                {

                    destinationUpdate();                //calculate the new poititon of the camera

                    setPosition();                      //set the position based on the new destination

                    SetLookAt(look_at_target.position); //set the direction that the camera will face

                }
                break;
            default:
                break;
        }
    }

    //methods

    private void HandleInput()
    {


        if(GM_.Instance.settings.IsXInverted)
        {
            x_invert = -1;
        }
        else
        {
            x_invert = 1;
        }

        if(GM_.Instance.settings.IsYInverted)
        {
            y_invert = 1;
        }
        else
        {
            y_invert = -1;
        }

        camera_input += new Vector2(GM_.Instance.input.GetAxis(InputManager.AXIS.RH) * (((GM_.Instance.settings.XSensitivity * 10) + 30) * x_invert) /*camera_rotation_speed*/, GM_.Instance.input.GetAxis(InputManager.AXIS.RV) * (((GM_.Instance.settings.YSensitivity * 10) + 30) * y_invert) /*camera_rotation_speed*/) * Time.deltaTime; //get the input from the left stick

        //limit the camera between 0 and 360 - this is to stop the camera jumping when fixing
        //use .01 to combat floating point problems that may arise
        if(camera_input.x > 360.01)
        {
            camera_input.x -= 360;
        }
        
        if(camera_input.x < 0.01)
        {
            camera_input.x += 360;
        }
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
            adjusted_destination += (rot_target.position + target_offset);

            //linear interpolation between the camera's current position and its new destination
           
            _camera.position = Vector3.SmoothDamp(_camera.position, adjusted_destination, ref cam_velocity, camera_movement_speed * Time.deltaTime);
        }
        else
        {
            //linear interpolation between the camera's current position and its new destination]
            _camera.position = Vector3.SmoothDamp(_camera.position, destination, ref cam_velocity, camera_movement_speed * Time.deltaTime);
        }
    }

    private void SetLookAt(Vector3 look_target)
    {
        Quaternion new_look = Quaternion.LookRotation(look_target - _camera.position);                                     //create a new look at rotation based on the position of the camera and the position of the target

        _camera.rotation = Quaternion.RotateTowards(_camera.rotation, new_look, player_step * Time.deltaTime);             //use unity rotate twoards to rotate the camera from the current rotation to the new rotation
    }
}

