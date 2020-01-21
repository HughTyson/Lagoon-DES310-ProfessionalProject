using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] Transform third_person_camera;

    //Movement direction indicated by the left analogue stick
    Vector3 direction = new Vector3();
    private float rs_x = 0.0f;

    //Speed that the player moves at
    [SerializeField] float speed = 1.0f;
    [SerializeField] float player_rotation_speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
    }

    void HandleInput()
    {

        direction = new Vector3(Input.GetAxisRaw("PlayerLH") * speed, 0, Input.GetAxisRaw("PlayerLV") * speed);

        rs_x += Input.GetAxisRaw("PlayerRH");

    }
    // Update is called once per frame
    void Update()
    {

        HandleInput();

    }

    private void FixedUpdate()
    {

        if (body.velocity.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,rs_x,0), Time.deltaTime * player_rotation_speed);

        }

        body.AddRelativeForce(direction * speed, ForceMode.VelocityChange); //Add force onto the player


    }
}
