using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleepState : BaseState
{

    [SerializeField] ThirdPersonCamera third_person_camera;
    [SerializeField] SleepCamera sleep_camera;
    [SerializeField] DayNightCycle cycle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
        third_person_camera.enabled = false;
        sleep_camera.enabled = true;

        cycle.secondsInFullDay = 60;
    }

    public void OnDisable()
    {
        sleep_camera.enabled = false;
        third_person_camera.enabled = true;

        cycle.secondsInFullDay = 120;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        
    }

}
