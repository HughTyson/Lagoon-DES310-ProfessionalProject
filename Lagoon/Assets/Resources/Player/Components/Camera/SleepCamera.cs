using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCamera : MonoBehaviour
{

    [Tooltip("The look at for the camera")]
    [SerializeField] Transform look_at;

    [Tooltip("The position of the camera")]
    [SerializeField] Transform target_position;

    [Tooltip("The speed of the camera when moving")]
    [SerializeField] float camera_speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target_position.position, camera_speed * Time.deltaTime);

        transform.LookAt(look_at);
    }
}
