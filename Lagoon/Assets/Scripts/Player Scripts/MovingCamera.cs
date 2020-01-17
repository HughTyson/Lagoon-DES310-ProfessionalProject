using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{

    private const float ANGLE_MIN = -10.0f;
    private const float ANGLE_MAX = 80.0f;

    [SerializeField] Transform lookAt;
    [SerializeField] Transform camTransform;
    [SerializeField] float distance = 5.0f;
    
    [SerializeField] float baseX = 0.0f;
    [SerializeField] float baseY = 30.0f;

    private Camera cam;

    private float changedX = 0.0f;
    private float changedY = 0.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        changedY = baseY;
        changedX = baseX;
    }

    void MoveCamera()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(changedY, changedX, 0);
        camTransform.position = lookAt.position + rotation * direction;
        camTransform.LookAt(lookAt.position);
    }

    void HandleInput()
    {

        changedX += Input.GetAxisRaw("PlayerRH");
        changedY += Input.GetAxisRaw("PlayerRV");

        changedY = Mathf.Clamp(changedY, ANGLE_MIN, ANGLE_MAX);

    }

    // Update is called once per frame - uses late update so it will be done after all the update functions are done
    public void Update()
    {

        HandleInput();

        MoveCamera();


    }
}
