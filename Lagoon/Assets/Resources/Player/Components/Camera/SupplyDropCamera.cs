﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropCamera : MonoBehaviour
{

    Vector3 base_pos = new Vector3(12, 5, 0);
    Vector3 look_at = new Vector3(-31, 22, 0);


    TweenManager.TweenPathBundle init_movement;
    TypeRef<float> x = new TypeRef<float>();
    TypeRef<float> y = new TypeRef<float>();
    TypeRef<float> z = new TypeRef<float>();

    //[SerializeField] AnimationClip clip;
    //Animation animation;

    bool move = false;

    bool should_shake = false;

    TypeRef<float> shake_magnitude = new TypeRef<float>(0);

    float shake_time = 0.5f;

    TweenManager.TweenPathBundle magnitude;
    Quaternion rot;


    // Start is called before the first frame update
    void Start()
    {


        magnitude = new TweenManager.TweenPathBundle(
                            new TweenManager.TweenPath(
                                new TweenManager.TweenPart_Start(0, 0.15f, 1.0f, TweenManager.CURVE_PRESET.EASE_IN)
                            )
                        );


    }

    private void Awake()
    {
        GM_.Instance.story.Event_GameEventStart += SupplyStart;

        //GM_.Instance.story.EventRequest_GameEventContinue += SupplyFinish;

        GM_.Instance.story.Event_GameEventEnd += SupplyFinish;

    }

    private void OnEnable()
    {

        should_shake = false;

        shake_magnitude.value = 0f;

        if(move)
        {
            x.value = transform.position.x;
            y.value = transform.position.y;
            z.value = transform.position.z;

            init_movement = new TweenManager.TweenPathBundle(
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.x, base_pos.x, 4, TweenManager.CURVE_PRESET.LINEAR)
                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.y, base_pos.y, 4, TweenManager.CURVE_PRESET.LINEAR)

                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.z, base_pos.z, 4.0f, TweenManager.CURVE_PRESET.LINEAR)
                    )
            );

            GM_.Instance.tween_manager.StartTweenInstance(
                init_movement,
                new TypeRef<float>[] { x, y, z },
                tweenUpdatedDelegate_: PosUpdate
            );
        }

        move = true;

    }

    private void Update()
    {
        if (should_shake)
        {

            transform.rotation =  Quaternion.Lerp(transform.rotation, rot * Random.rotation /** shake_magnitude.value*/, Time.deltaTime * shake_magnitude.value);

        }
    }

    void PosUpdate()
    {
        transform.position = new Vector3(x.value, y.value, z.value);

        rot = Quaternion.LookRotation(look_at - transform.position );

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);
    }

    void SupplyStart(StoryManager.GameEventTriggeredArgs args)
    {
        if (args.event_type == EventNode.EVENT_TYPE.SUPPLY_DROP)
        {

            GM_.Instance.tween_manager.StartTweenInstance(
                magnitude,
                new TypeRef<float>[] { shake_magnitude }
            );

            should_shake = true;
        }
    }

    private void SupplyFinish()
    {

        GM_.Instance.tween_manager.StartTweenInstance(
            magnitude,
            new TypeRef<float>[] { shake_magnitude },
            tweenUpdatedDelegate_: UpdateFinish,
            tweenCompleteDelegate_: Finish
        );

        

        transform.position = base_pos;
    }

    void UpdateFinish()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look_at - transform.position), Time.deltaTime);
    }

    void Finish()
    {
        should_shake = false;
    }

}
