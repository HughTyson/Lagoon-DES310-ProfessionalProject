using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    //Movement direction indicated by the left analogue stick
    Vector3 MovementDirection = new Vector2();

    //Speed that the player moves at

    [SerializeField] float MovementSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void HandleInput()
    {

        MovementDirection = new Vector3(Input.GetAxisRaw("PlayerLH") * MovementSpeed, 0, Input.GetAxisRaw("PlayerLV") * MovementSpeed);
        MovementDirection.Normalize();

    }
    // Update is called once per frame
    void Update()
    {

        HandleInput();

     

    }

    private void FixedUpdate()
    {
        //GetComponent<Rigidbody>().AddForce(MovementDirection * MovementSpeed, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddRelativeForce(MovementDirection * MovementSpeed, ForceMode.VelocityChange);
    }
}
