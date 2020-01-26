using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{

    

    [SerializeField] CharacterController controller;



    [SerializeField] float player_rotation_speed = 1.0f;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float gravityScale = 20.0f;
     
    //Movement direction indicated by the left analogue stick
    Vector3 direction = Vector3.zero;

    private float rs_x = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("PlayerLH");
        float v = Input.GetAxis("PlayerLV");

        //limit ot forward movement
        if(v < 0)
        {
            v = 0;
        }

        transform.Rotate(0, h * player_rotation_speed * Time.deltaTime, 0);

        if(controller.isGrounded)
        {

            direction = Vector3.forward * v;

            //this is where the animation call for walking would go

            direction = transform.TransformDirection(direction);
            direction *= speed;

        }

        direction.y -= gravityScale * Time.deltaTime;

        controller.Move(direction * Time.deltaTime);
        
    }

    private void HandleInput()
    {



    }
}
