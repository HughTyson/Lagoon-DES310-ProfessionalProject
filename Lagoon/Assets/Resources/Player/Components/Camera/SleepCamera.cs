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


    // Start is called before the first frame update
    void Start()
    {
        target_position = new Vector3(0, 2, 0);
        look_at = new Vector3(-10, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target_position, camera_speed * Time.deltaTime);

        transform.LookAt(look_at);
    }
}
