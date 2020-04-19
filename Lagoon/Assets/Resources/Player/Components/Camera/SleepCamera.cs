using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepCamera : MonoBehaviour
{

    [Tooltip("The look at for the camera")]
    [SerializeField] Transform look_at;

    [Tooltip("The position of the camera")]
    [SerializeField] Transform base_pos;

    [Tooltip("The speed of the camera when moving")]
    float camera_speed = 1f;

    TweenManager.TweenPathBundle init_movement;
    TypeRef<float> x = new TypeRef<float>();
    TypeRef<float> y = new TypeRef<float>();
    TypeRef<float> z = new TypeRef<float>();

    bool move = false;

    private Vector3 cam_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
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
                        new TweenManager.TweenPart_Start(transform.position.x, base_pos.position.x, 4, TweenManager.CURVE_PRESET.LINEAR)
                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.y, base_pos.position.y, 4, TweenManager.CURVE_PRESET.LINEAR)

                    ),
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(transform.position.z, base_pos.position.z, 4.0f, TweenManager.CURVE_PRESET.LINEAR)
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

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.LookRotation(look_at.position - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 1.5f);
    }

    void PosUpdate()
    {
        transform.position = new Vector3(x.value, y.value, z.value);
    }
}
