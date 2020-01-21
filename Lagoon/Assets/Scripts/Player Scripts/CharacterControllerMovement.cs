using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{

    

    [SerializeField] CharacterController controller;

    //Movement direction indicated by the left analogue stick
    Vector3 direction = new Vector3();
    private float rs_x = 0.0f;

    [SerializeField] float player_rotation_speed = 1.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float gravityScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        HandleInput();

        //direction.y = direction.y + (Physics.gravity.y * gravityScale);

        controller.Move(direction * Time.deltaTime);

        
    }

    private void HandleInput()
    {

        direction = new Vector3(Input.GetAxis("PlayerLH") * speed, 0, Input.GetAxis("PlayerLV") * speed);

        rs_x += Input.GetAxisRaw("PlayerRH");

    }
}
