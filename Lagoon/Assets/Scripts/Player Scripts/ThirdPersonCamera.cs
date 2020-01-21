using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    //minimum and maximum angles for the Y axis
    [SerializeField] float ANGLE_MIN = -10.0f;
    [SerializeField] float ANGLE_MAX = 80.0f;

    [SerializeField] Transform target; //what the camera looks at



    [SerializeField] float camera_rotation_speed = 1.0f;


    private float distance = 10.0f; //distance between player and the camera
    private float currentX = 0.0f;  //used to calculate the rotation in the X
    private float currentY = 0.0f;  //used to calculate the rotation in the Y

    


    private void Update()
    {
        HandleInput();
    }
    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distance); //set how far away the camera is from the player
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = Vector3.Lerp(transform.position, target.position + rotation * direction, Time.deltaTime); 

        transform.LookAt(target.position);
    }

    private void HandleInput()
    {
        currentX += Input.GetAxisRaw("PlayerRH") *camera_rotation_speed * Time.deltaTime;
        currentY += Input.GetAxisRaw("PlayerRV");

        currentY = Mathf.Clamp(currentY, ANGLE_MIN, ANGLE_MAX);
    }



}
