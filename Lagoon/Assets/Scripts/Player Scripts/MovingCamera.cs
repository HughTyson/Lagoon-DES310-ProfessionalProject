using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    //minimum and maximum angles for the Y axis
    [SerializeField] float ANGLE_MIN = -10.0f;
    [SerializeField] float ANGLE_MAX = 80.0f;

    [SerializeField] Transform player;
    [SerializeField] Transform cam;
    [SerializeField] float distance = 5.0f;

    //speed varibles for the camera
    [SerializeField] float left_stick_speed = 1.0f;
    [SerializeField] float rotation_speed = 1.0f;
    [SerializeField] float camera_speed = 1.0f;

    private float changedX = 0.0f;
    private float changedY = 0.0f;

    private void Start()
    {
        cam = transform;
 
    }

    void MoveCamera()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(changedY, changedX, 0);
       
        cam.position = Vector3.Lerp(cam.position, player.position + (rotation * direction), Time.deltaTime * camera_speed);

        transform.LookAt(player);

        if (player.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
        {
            player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0, changedX, 0), Time.deltaTime * rotation_speed);
        }

    }

    void HandleInput()
    {

        changedX += Input.GetAxisRaw("PlayerRH") * left_stick_speed;
        changedY += Input.GetAxisRaw("PlayerRV") * left_stick_speed;

        changedY = Mathf.Clamp(changedY, ANGLE_MIN, ANGLE_MAX);

    }

    // Update is called once per frame - uses late update so it will be done after all the update functions are done
    void LateUpdate()
    {

        HandleInput();

        MoveCamera();
    }
}
