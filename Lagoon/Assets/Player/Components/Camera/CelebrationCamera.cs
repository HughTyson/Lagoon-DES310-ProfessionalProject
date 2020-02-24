using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationCamera : MonoBehaviour
{


    [Tooltip("The game object that the camera rotates around")]
    [SerializeField] Transform target;

    [Tooltip("The offset of the celebration camera")]
    [SerializeField] Vector3 target_offset;

    [Tooltip("Distance from the celebration target")]
    [SerializeField] float distance = 5.0f;

    [Tooltip("Rotation speed of the camera")]
    [SerializeField] float camera_rotation_speed;

    [Tooltip("Step Size for the look at angle")]
    [SerializeField] float camera_step_size;

    float rotate;
    Vector3 target_pos;
    Vector3 destination;
    private Vector3 cam_velocity = Vector3.zero;

    void OnEnable()
    {
        Vector3 target_dir = target.transform.forward;

        transform.position = (target.position + target_offset) + target_dir * distance;

        transform.LookAt(target.position + target_offset);

        rotate = 180;
    }

    private void Update()
    {
        rotate += camera_rotation_speed * Time.deltaTime;

        destinationUpdate();

        // transform.position = Vector3.SmoothDamp(transform.position, destination, ref cam_velocity, camera_rotation_speed * Time.deltaTime);

        transform.position = destination;
        Quaternion new_look = Quaternion.LookRotation((target.position + target_offset) - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, new_look, camera_step_size * Time.deltaTime);


        // Tomas Test: 
        transform.rotation = new_look;
    }

    private void destinationUpdate()
    {
        //set the target position based of the targets current position, the offset values,
        //the input from the controller and the distance that the player is from the camera

        
        target_pos = target.position + target.up * target_offset.y + target.forward * target_offset.z + transform.TransformDirection(target.right * target_offset.x);
        destination = Quaternion.Euler(0, rotate, 0) * -target.forward * distance;

        destination += target_pos;
    }

}
