using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCamera : MonoBehaviour
{

    [Tooltip("The look at for the camera")]
    [SerializeField] Vector3 look_at;

    [Tooltip("The position of the camera")]
    [SerializeField] Vector3 target_position;

    [Tooltip("The speed of the camera when moving")]
    float camera_speed = 1f;

    private Vector3 cam_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        target_position = new Vector3(0, 2, 0);
        look_at = new Vector3(-10, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target_position, ref cam_velocity, camera_speed * Time.deltaTime);

        Quaternion rot = Quaternion.LookRotation(look_at - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);
    }
}
