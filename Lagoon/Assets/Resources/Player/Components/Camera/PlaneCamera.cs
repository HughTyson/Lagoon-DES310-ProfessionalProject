using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{


    //Public Variable

    [HideInInspector] public PlaneSegments.SegmentType active_segment;
    [HideInInspector] public bool disable_input;

    //Private variable

    bool init_setup;

    Vector3 base_pos = new Vector3(0, 0, 0);
    private Vector3 cam_velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        disable_input = true;

    }

    private void OnEnable()
    {
        disable_input = true;
        init_setup = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(init_setup)
        {
            SetInitPos();
        }
        else if(!init_setup)
        {

        }


    }

    void SetInitPos()
    {

        transform.position = Vector3.SmoothDamp(transform.position, base_pos, ref cam_velocity, Time.deltaTime);

        init_setup = false;
    }
}
