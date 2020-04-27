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

    bool start_ = false;

    System.Action<TweenManager.STOP_COMMAND> tween;

    // Start is called before the first frame update
    void Start()
    {
        base_speed = GM_.Instance.day_night_cycle.GetTime();

        multiplyer.value = base_speed;

        speed_tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(base_speed, max_sleep_speed, 3.0f, TweenManager.CURVE_PRESET.EASE_IN)
            )
        );


    }

    public void OnEnable()
    {
        third_person_camera.enabled = false;
        sleep_camera.enabled = true;
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        GAME_UI.Instance.helperButtons.DisableAll();

        start_ = true;

        //change the time for the tween, to crete a better effect

        cycle.ChangeIntensityTweenTime(0.2f);

        cycle.ChangeAtmosphereTweenTime(2f);

    }

    public void OnDisable()
    {
        sleep_camera.enabled = false;
        third_person_camera.enabled = true;

        cycle.ChangeIntensityTweenTime(2f);

        cycle.ChangeAtmosphereTweenTime(6f);

        GM_.Instance.day_night_cycle.SetTime();

    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        if(start_)
        {
            GM_.Instance.tween_manager.StartTweenInstance(
                            speed_tween,
                            new TypeRef<float>[] { multiplyer },
                            tweenUpdatedDelegate_: time_update,
                            startingDirection_: TweenManager.DIRECTION.START_TO_END
             );

            start_ = false;
        }

        if(cycle.current_time > 0.45f && cycle.current_time < 0.46f)
        {
            GM_.Instance.tween_manager.StartTweenInstance(
                            speed_tween,
                            new TypeRef<float>[] { multiplyer },
                            tweenUpdatedDelegate_: time_update,
                            startingDirection_: TweenManager.DIRECTION.END_TO_START,
                            tweenCompleteDelegate_: sleep_done
                        );
        }
    }

    private void time_update()
    {
        GM_.Instance.day_night_cycle.SetTime(multiplyer.value);
    }

    private void sleep_done()
    {
        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
    }

}
