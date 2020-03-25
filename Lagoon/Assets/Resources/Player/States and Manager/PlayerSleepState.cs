using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleepState : BaseState
{

    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] ThirdPersonCamera third_person_camera;
    [SerializeField] SleepCamera sleep_camera;
    [SerializeField] DayNightCycle cycle;

    float base_speed;
    [SerializeField] float max_sleep_speed = 30;

    TypeRef<float> multiplyer = new TypeRef<float>();

    TweenManager.TweenPathBundle speed_tween;
    bool b = false;


    // Start is called before the first frame update
    void Start()
    {
        base_speed = cycle.time_multiplyer;

        multiplyer.value = base_speed;

        //atmosphere_tween = new TweenManager.TweenPathBundle(
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(1, 0.3f, 6.0f, TweenManager.CURVE_PRESET.LINEAR)
        //    )
        //);

        //speed_tween = new TweenManager.TweenPathBundle(
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(base_speed, max_sleep_speed, 3.0f, TweenManager.CURVE_PRESET.EASE_IN)
        //    )
        //);
    }

    public void OnEnable()
    {
        third_person_camera.enabled = false;
        sleep_camera.enabled = true;
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        if(b)
        {
            //GM_.Instance.tween_manager.StartTweenInstance(
            //                speed_tween,
            //                new TypeRef<float>[] { multiplyer },
            //                tweenUpdatedDelegate_: time_update
            // );
        }

        b = true;


    }

    public void OnDisable()
    {
        sleep_camera.enabled = false;
        third_person_camera.enabled = true;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        if(cycle.current_time > 0.3f && cycle.current_time < 0.31f)
        {
            //GM_.Instance.tween_manager.StartTweenInstance(
            //                speed_tween,
            //                new TypeRef<float>[] { multiplyer },
            //                tweenUpdatedDelegate_: time_update,
            //                startingDirection_: TweenManager.DIRECTION.END_TO_START
            //            );
        }


    }

    private void time_update()
    {
        cycle.time_multiplyer = multiplyer.value;
    }

}
