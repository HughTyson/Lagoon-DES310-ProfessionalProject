using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    //Movement direction indicated by the left analogue stick
    Vector3 MovementDirection = new Vector2();

    //Speed that the player moves at

    [SerializeField] float MovementSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void HandleInput()
    {



    }
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("PlayerLH") * MovementSpeed * Time.deltaTime;
        float z = Input.GetAxisRaw("PlayerLV") * MovementSpeed * Time.deltaTime;

        MovementDirection = new Vector3(x, 0, z);   

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(MovementDirection * MovementSpeed, ForceMode.VelocityChange);

        
    }
}
