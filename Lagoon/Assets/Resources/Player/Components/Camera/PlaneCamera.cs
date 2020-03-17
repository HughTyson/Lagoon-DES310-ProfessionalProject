using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCamera : MonoBehaviour
{


    //Public Variable

    [HideInInspector] public PlaneSegments.SegmentType active_segment_type;
    [HideInInspector] public Transform current_look_at;
    [HideInInspector] public Transform old_look_at;
    [HideInInspector] public bool disable_input;

    [SerializeField] Transform prop;
    [SerializeField] Transform engine_front;
    [SerializeField] Transform engine_mid;
    [SerializeField] Transform cockpit;
    [SerializeField] Transform left_wing;
    [SerializeField] Transform right_wing;
    //[SerializeField] Transform prop;
    //[SerializeField] Transform prop;
    //[SerializeField] Transform prop;
    //[SerializeField] Transform prop;
    //[SerializeField] Transform prop;

    //Private variable

    bool init_setup;
    PlaneSegments.SegmentType old_segment_type;

    TweenManager.TweenPathBundle prop_to_engine;

    TweenManager.TweenPathBundle look_at;

    [SerializeField] float player_step = 150;

    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();
    TypeRef<float> positionValZ = new TypeRef<float>();

    TypeRef<float> look_at_x = new TypeRef<float>();
    TypeRef<float> look_at_y = new TypeRef<float>();
    TypeRef<float> look_at_z = new TypeRef<float>();

    Vector3 c_look = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        disable_input = true;
        active_segment_type = PlaneSegments.SegmentType.PROPELLER;
        old_segment_type = PlaneSegments.SegmentType.PROPELLER;

        prop_to_engine = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(prop.position.x, engine_front.position.x, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(prop.position.y, engine_front.position.y + 2.0f, 0.3f, TweenManager.CURVE_PRESET.EASE_IN),
                new TweenManager.TweenPart_Continue(engine_front.position.y, 1.0f, TweenManager.CURVE_PRESET.EASE_OUT)
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(prop.position.z, engine_front.position.z, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
            )
        );


    }

    private void OnEnable()
    {
        disable_input = true;
        init_setup = true;

        positionValX.value = prop.position.x;
        positionValY.value = prop.position.y;
        positionValZ.value = prop.position.z;

        look_at_x.value = old_look_at.position.x;
        look_at_y.value = old_look_at.position.y;
        look_at_z.value = old_look_at.position.z;

        c_look = new Vector3(look_at_x.value, look_at_y.value, look_at_z.value);

        active_segment_type = PlaneSegments.SegmentType.PROPELLER;
        old_segment_type = PlaneSegments.SegmentType.PROPELLER;
    }

    // Update is called once per frame
    void Update()
    {

        if (init_setup)
        {
            SetInitPos();
        }
        else if (!init_setup)
        {
            if (active_segment_type != old_segment_type)
            {
                Debug.Log(old_segment_type);
                Movement();
                old_segment_type = active_segment_type;


                //look_at = new TweenManager.TweenPathBundle(
                //    new TweenManager.TweenPath(
                //        new TweenManager.TweenPart_Start(old_look_at.position.x, current_look_at.position.x, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                //    ),
                //    new TweenManager.TweenPath(
                //        new TweenManager.TweenPart_Start(old_look_at.position.y, current_look_at.position.y, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                //    ),
                //    new TweenManager.TweenPath(
                //        new TweenManager.TweenPart_Start(old_look_at.position.z, current_look_at.position.z, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                //    )

                //);

                TweenManager.TweenPathBundle lerp_t = new TweenManager.TweenPathBundle(
                    new TweenManager.TweenPath(
                        new TweenManager.TweenPart_Start(0, 1, 5.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                    )
                    );

                TypeRef<float> test = new TypeRef<float>(0);

                GM_.Instance.tween_manager.StartTweenInstance(
                    lerp_t,
                    new TypeRef<float>[] { test}
                    );

                Mathf.Lerp(2, 2, test.value);

                Debug.Log(old_look_at.position + "   " + current_look_at.position);

                old_look_at = current_look_at;

                //GM_.Instance.tween_manager.StartTweenInstance(
                //    look_at,
                //    new TypeRef<float>[] { look_at_x, look_at_y, look_at_z }
                //    );

            }

            transform.position = new Vector3(positionValX.value, positionValY.value, positionValZ.value);

        }


        c_look = new Vector3(look_at_x.value, look_at_y.value, look_at_z.value);

        Quaternion new_look = Quaternion.LookRotation(c_look - transform.position);                                     //create a new look at rotation based on the position of the camera and the position of the target

        transform.rotation = Quaternion.RotateTowards(transform.rotation, new_look, player_step * Time.deltaTime);             //use unity rotate twoards to rotate the camera from the current rotation to the new rotation
    }

    void Movement()
    {
        switch (old_segment_type)
        {
            case PlaneSegments.SegmentType.NONE:
                { }
                break;
            case PlaneSegments.SegmentType.PROPELLER:
                {

                    if (active_segment_type == PlaneSegments.SegmentType.ENGINE_FRONT)
                    {
                        GM_.Instance.tween_manager.StartTweenInstance(
                            prop_to_engine,
                            new TypeRef<float>[] { positionValX, positionValY, positionValZ },
                            tweenCompleteDelegate_: EnableInputs,
                            tweenUpdatedDelegate_: PositionUpdate
                            );
                    }
                    else
                    {

                    }


                }
                break;
            case PlaneSegments.SegmentType.ENGINE_FRONT:
                {

                    if (active_segment_type == PlaneSegments.SegmentType.PROPELLER)
                    {
                        GM_.Instance.tween_manager.StartTweenInstance(
                            prop_to_engine,
                            new TypeRef<float>[] { positionValX, positionValY, positionValZ },
                            tweenCompleteDelegate_: EnableInputs,
                            tweenUpdatedDelegate_: PositionUpdate,
                            startingDirection_: TweenManager.DIRECTION.END_TO_START
                            );
                    }



                }
                break;
            case PlaneSegments.SegmentType.ENGINE_MID:
                { }
                break;
            case PlaneSegments.SegmentType.COCKPIT:
                { }
                break;
            case PlaneSegments.SegmentType.LEFTWING:
                { }
                break;
            case PlaneSegments.SegmentType.RIGHTWING:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_MID:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_BACK:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_BACK:
                { }
                break;
            default:
                break;
        }
    }

    void SetInitPos()
    {
        transform.position = Vector3.Lerp(transform.position, prop.position, Time.deltaTime);

        if (Vector3.Distance(transform.position, prop.position) < 0.04)
        {
            init_setup = false;
            disable_input = false;
        }
    }

    void EnableInputs()
    {
        disable_input = false;
    }

    void PositionUpdate()
    {

        disable_input = true;
    }
}
