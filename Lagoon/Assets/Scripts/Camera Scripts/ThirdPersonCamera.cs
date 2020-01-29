using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    // ==========================================
    //              Visible Variables
    //===========================================


    [SerializeField] Transform target; //what the camera looks at

    [SerializeField] Vector3 base_position = new Vector3(0.4f ,0.5f, -2.0f);
    [SerializeField] Vector3 base_pivot = new Vector3(0.0f, 1.0f,  0.0f);

    //minimum and maximum angles for the Y axis
    [SerializeField] float ANGLE_MIN = -10.0f;
    [SerializeField] float ANGLE_MAX = 80.0f;

    [SerializeField] float camera_rotation_speed = 1f;


    [Header("Varibles used for Camera Collisions")]

    [SerializeField] bool drawDesiredCollisionLines = true;
    [SerializeField] bool drawAdjustedCollisionLines = true;

    // ==========================================
    //              Hidden Variables
    //===========================================

    Camera camera;
    CameraCollision collision = new CameraCollision();

    private Transform _camera;

    private Vector2 camera_input = Vector2.zero;

    private Vector3 current_cam_pos;
    private float current_cam_distance;

    private Vector3 position_offset;
    private Vector3 pivot_offset;

    private Vector3 destination;

    float adjustment_distance = 0;

    void Start()
    {
        _camera = transform;

        //set the default position and rotation of the camera
        _camera.position = target.position + Quaternion.identity * base_pivot + Quaternion.identity * base_position;
        _camera.rotation = Quaternion.identity;

        current_cam_pos = _camera.position - target.position;
        current_cam_distance = current_cam_pos.magnitude;

        position_offset = base_position;
        pivot_offset = base_pivot;

        //Collision

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(current_cam_pos, transform.rotation, ref collision.desiredCameraClipPoints);

    }

    //update function

    private void LateUpdate()
    {

        HandleInput();

        Quaternion rot_y = Quaternion.Euler(0, camera_input.x, 0);
        Quaternion rot_aim = Quaternion.Euler(-camera_input.y, camera_input.x, 0);
        _camera.rotation = rot_aim;

      //  Debug.Log(adjustment_distance);

        if (collision.collision)
        {
            destination = target.position + ((rot_y * pivot_offset) + (rot_aim * position_offset));
        }
        else
        {
            destination = target.position + (rot_y * pivot_offset) + (rot_aim * position_offset);
        }

        _camera.position = destination;

    }

    private void FixedUpdate()
    {
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(_camera.position, transform.rotation, ref collision.desiredCameraClipPoints);

        for (int i = 0; i < 5; i++)
        {
            if (drawDesiredCollisionLines)
            {
                Debug.DrawLine(target.position, collision.desiredCameraClipPoints[i], Color.white);
            }

            if (drawAdjustedCollisionLines)
            {
                Debug.DrawLine(target.position, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(target.position); //using raycasts here
        adjustment_distance = collision.GetAdjustedDistanceWithRay(target.position);
    }

    //methods

    private void HandleInput()
    {

        camera_input += new Vector2(Input.GetAxisRaw("PlayerRH") * camera_rotation_speed, Input.GetAxisRaw("PlayerRV") * camera_rotation_speed) * Time.deltaTime;

        camera_input.y = Mathf.Clamp(camera_input.y, ANGLE_MIN, ANGLE_MAX);
    }
}