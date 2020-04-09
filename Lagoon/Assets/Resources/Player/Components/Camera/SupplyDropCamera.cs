using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropCamera : MonoBehaviour
{

    Vector3 base_pos = new Vector3(0, 2, 0);
    public Vector3 look_at = new Vector3(-10, 2, 0);


    TweenManager.TweenPathBundle init_movement;
    TypeRef<float> x = new TypeRef<float>();
    TypeRef<float> y = new TypeRef<float>();
    TypeRef<float> z = new TypeRef<float>();

    //[SerializeField] AnimationClip clip;
    //Animation animation;

    bool move = false;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
    }

    private void Awake()
    {
        GM_.Instance.story.Event_GameEventStart += SupplyStart;
    }

    private void OnEnable()
    {

        if(move)
        {
            x.value = transform.position.x;
            y.value = transform.position.y;
            z.value = transform.position.z;

            init_movement = new TweenManager.TweenPathBundle(
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.x, base_pos.x, 1, TweenManager.CURVE_PRESET.LINEAR)
                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.y, base_pos.y, 1, TweenManager.CURVE_PRESET.LINEAR)

                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.z, base_pos.z, 1.0f, TweenManager.CURVE_PRESET.LINEAR)
                    )
            );

            GM_.Instance.tween_manager.StartTweenInstance(
                init_movement,
                new TypeRef<float>[] { x, y, z },
                tweenCompleteDelegate_: AnimationStart,
                tweenUpdatedDelegate_: PosUpdate
            );
        }

        move = true;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position);
    }

    void AnimationStart()
    {

    }

    void PosUpdate()
    {
        transform.position = new Vector3(x.value, y.value, z.value);

        Quaternion rot = Quaternion.LookRotation(look_at - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);
    }

    void SupplyStart(StoryManager.GameEventTriggeredArgs args)
    {

        if (args.event_type == EventNode.EVENT_TYPE.SUPPLY_DROP)
        {

            //animation.Play(clip.name);

        }





    }
}
