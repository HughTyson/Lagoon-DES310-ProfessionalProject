using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Transitions
{

    public enum TransitionType
    {
        PROP_ENGFRONT,
        ENGFRONT_FUSELF,
        FUSELF_LEFTWING,
        LEFTWING_COCKPIT,
        COCKPIT_FUSELM,
        FUSELM_ENGMID,
        ENGMID_TAIL,
        TAIL_PROP
    }

    public TransitionType type;

    [Header("Transforms")]
    [Tooltip("The transform of the start object")]
    [SerializeField] Transform start_pos;
    [Tooltip("The transform of the end object")]
    [SerializeField] Transform end_pos;

    [Header("The time for position to change")]

    [Tooltip("The time taken for the X position to change")]
    [SerializeField] float x_time;
    [Tooltip("The time taken for the Y position to change. Use continue if the bool 'use_continue' is true")]
    [SerializeField] float y_time_start;

    [Tooltip("The time takne for the Z position ot change")]
    [SerializeField] float z_time;

    [Header("Curves Used")]

    [SerializeField] TweenManager.CURVE_PRESET x_curve;
    [SerializeField] TweenManager.CURVE_PRESET y_curve1;
    [SerializeField] TweenManager.CURVE_PRESET z_curve;

    [Header("Use if a continue is needed - In Y")]

    [SerializeField] bool use_continue = false;
    [Tooltip("Use if the 'use_condition' bool is true. The rest of the time taken for the Y position to change")]
    [SerializeField] float y_time_continue;
    [SerializeField] float y_end_mid;
    [SerializeField] TweenManager.CURVE_PRESET y_curve2;



    private TweenManager.TweenPathBundle tween;

    public void Init()
    {

        if (use_continue)
        {
            tween = new TweenManager.TweenPathBundle(
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.x, end_pos.position.x, x_time, x_curve)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.y, end_pos.position.y + y_end_mid, y_time_start, y_curve1),
                           new TweenManager.TweenPart_Continue(end_pos.position.y, y_time_continue, y_curve2)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.z, end_pos.position.z, 1.0f, z_curve)
                       )
                   );
        }
        else if (!use_continue)
        {
            tween = new TweenManager.TweenPathBundle(
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.x, end_pos.position.x, x_time, x_curve)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.y, end_pos.position.y, y_time_start, y_curve1)
                       ),
                       new TweenManager.TweenPath(
                           new TweenManager.TweenPart_Start(start_pos.position.z, end_pos.position.z, 1.0f, z_curve)
                       )
                    );
        }


    }

    public void Start(ref TypeRef<float> x, ref TypeRef<float> y, ref TypeRef<float> z, TweenManager.DIRECTION direction, System.Action enable, System.Action disable)
    {

        GM_.Instance.tween_manager.StartTweenInstance(
            tween,
            new TypeRef<float>[] { x, y, z },
            tweenCompleteDelegate_: enable,
            tweenUpdatedDelegate_: disable,
            startingDirection_: direction
        );

    }

}

public class PlaneCamera : MonoBehaviour
{

    enum PlaneCameraStates
    {
        INIT,
        SELECTION,
        SEGMENT
    }


    PlaneCameraStates current_state;


    //Public Variable

    [HideInInspector] public PlaneSegments.SegmentType active_segment_type;
    [HideInInspector] public Transform current_look_at;
    [HideInInspector] public Transform old_look_at;
    [HideInInspector] public bool disable_input;

    [SerializeField] Transform prop;
    [SerializeField] Transform engine_front;


    [SerializeField] List<Transitions> transition = new List<Transitions>();

    //Private variable

    bool init_setup;
    PlaneSegments.SegmentType old_segment_type;

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


        for(int i = 0; i < transition.Count; i++)
        { 
            transition[i].Init();
        }

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

        current_state = PlaneCameraStates.INIT;
    }

    // Update is called once per frame
    void Update()
    {


        switch (current_state)
        {
            case PlaneCameraStates.INIT:
                {
                    SetInitPos();
                }
                break;
            case PlaneCameraStates.SELECTION:
                {
                    if (active_segment_type != old_segment_type)
                    {
                        Debug.Log(old_segment_type);
                        Movement();
                        old_segment_type = active_segment_type;


                        look_at = new TweenManager.TweenPathBundle(
                            new TweenManager.TweenPath(
                                new TweenManager.TweenPart_Start(old_look_at.position.x, current_look_at.position.x, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                            ),
                            new TweenManager.TweenPath(
                                new TweenManager.TweenPart_Start(old_look_at.position.y, current_look_at.position.y, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                            ),
                            new TweenManager.TweenPath(
                                new TweenManager.TweenPart_Start(old_look_at.position.z, current_look_at.position.z, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                            )

                        );

                        Debug.Log(old_look_at.position + "   " + current_look_at.position);

                        old_look_at = current_look_at;

                        GM_.Instance.tween_manager.StartTweenInstance(
                            look_at,
                            new TypeRef<float>[] { look_at_x, look_at_y, look_at_z }
                            );

                    }

                    transform.position = new Vector3(positionValX.value, positionValY.value, positionValZ.value);

                }
                break;
            case PlaneCameraStates.SEGMENT:
                break;
            default:
                break;
        }

        c_look = new Vector3(look_at_x.value, look_at_y.value, look_at_z.value);

        Quaternion new_look = Quaternion.LookRotation(c_look - transform.position);                                     //create a new look at rotation based on the position of the camera and the position of the target

        transform.rotation = Quaternion.RotateTowards(transform.rotation, new_look, player_step * Time.deltaTime);             //use unity rotate twoards to rotate the camera from the current rotation to the new rotation

        //if (init_setup)
        //{
        //    SetInitPos();
        //}
        //else if (!init_setup)
        //{
        //    if (active_segment_type != old_segment_type)
        //    {
        //        Debug.Log(old_segment_type);
        //        Movement();
        //        old_segment_type = active_segment_type;


        //        look_at = new TweenManager.TweenPathBundle(
        //            new TweenManager.TweenPath(
        //                new TweenManager.TweenPart_Start(old_look_at.position.x, current_look_at.position.x, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        //            ),
        //            new TweenManager.TweenPath(
        //                new TweenManager.TweenPart_Start(old_look_at.position.y, current_look_at.position.y, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        //            ),
        //            new TweenManager.TweenPath(
        //                new TweenManager.TweenPart_Start(old_look_at.position.z, current_look_at.position.z, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        //            )

        //        );

        //        Debug.Log(old_look_at.position + "   " + current_look_at.position);

        //        old_look_at = current_look_at;

        //        GM_.Instance.tween_manager.StartTweenInstance(
        //            look_at,
        //            new TypeRef<float>[] { look_at_x, look_at_y, look_at_z }
        //            );

        //    }

        //    transform.position = new Vector3(positionValX.value, positionValY.value, positionValZ.value);

        //}


        //c_look = new Vector3(look_at_x.value, look_at_y.value, look_at_z.value);

        //Quaternion new_look = Quaternion.LookRotation(c_look - transform.position);                                     //create a new look at rotation based on the position of the camera and the position of the target

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, new_look, player_step * Time.deltaTime);             //use unity rotate twoards to rotate the camera from the current rotation to the new rotation
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
                        Transition(Transitions.TransitionType.PROP_ENGFRONT, TweenManager.DIRECTION.START_TO_END);
                    }
                    else if (active_segment_type == PlaneSegments.SegmentType.TAIL)
                    {
                        Transition(Transitions.TransitionType.TAIL_PROP, TweenManager.DIRECTION.END_TO_START);
                    }
                }
                break;
            case PlaneSegments.SegmentType.ENGINE_FRONT:
                {
                    if(active_segment_type == PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT)
                    {
                        Transition(Transitions.TransitionType.ENGFRONT_FUSELF, TweenManager.DIRECTION.START_TO_END);
                    }
                    else if (active_segment_type == PlaneSegments.SegmentType.PROPELLER)
                    {
                        Transition(Transitions.TransitionType.PROP_ENGFRONT, TweenManager.DIRECTION.END_TO_START);
                    }
                }
                break;
            case PlaneSegments.SegmentType.ENGINE_MID:
                {
                    if (active_segment_type == PlaneSegments.SegmentType.TAIL)
                    {
                        Transition(Transitions.TransitionType.ENGMID_TAIL, TweenManager.DIRECTION.START_TO_END);
                    }
                    if (active_segment_type == PlaneSegments.SegmentType.FUSELAGE_LEFT_MID)
                    {
                        Transition(Transitions.TransitionType.FUSELM_ENGMID, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.COCKPIT:
                {

                    if(active_segment_type == PlaneSegments.SegmentType.FUSELAGE_LEFT_MID)
                    {
                        Transition(Transitions.TransitionType.COCKPIT_FUSELM, TweenManager.DIRECTION.START_TO_END);
                    }
                    else if(active_segment_type == PlaneSegments.SegmentType.LEFTWING)
                    {
                        Transition(Transitions.TransitionType.LEFTWING_COCKPIT, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.LEFTWING:
                {

                    if(active_segment_type == PlaneSegments.SegmentType.COCKPIT)
                    {
                        Transition(Transitions.TransitionType.LEFTWING_COCKPIT, TweenManager.DIRECTION.START_TO_END);
                    }
                    else if(active_segment_type == PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT)
                    {
                        Transition(Transitions.TransitionType.FUSELF_LEFTWING, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.RIGHTWING:
                { }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_FRONT:
                {

                    if(active_segment_type == PlaneSegments.SegmentType.LEFTWING)
                    {
                        Transition(Transitions.TransitionType.FUSELF_LEFTWING, TweenManager.DIRECTION.START_TO_END);
                    }
                    if (active_segment_type == PlaneSegments.SegmentType.ENGINE_FRONT)
                    {
                        Transition(Transitions.TransitionType.ENGFRONT_FUSELF, TweenManager.DIRECTION.END_TO_START);
                    }
                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_LEFT_MID:
                {

                    if (active_segment_type == PlaneSegments.SegmentType.ENGINE_MID)
                    {
                        Transition(Transitions.TransitionType.FUSELM_ENGMID, TweenManager.DIRECTION.START_TO_END);
                    }
                    if (active_segment_type == PlaneSegments.SegmentType.COCKPIT)
                    {
                        Transition(Transitions.TransitionType.COCKPIT_FUSELM, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.TAIL:
                {

                    if (active_segment_type == PlaneSegments.SegmentType.PROPELLER)
                    {
                        Transition(Transitions.TransitionType.TAIL_PROP, TweenManager.DIRECTION.START_TO_END);
                    }
                    if (active_segment_type == PlaneSegments.SegmentType.ENGINE_MID)
                    {
                        Transition(Transitions.TransitionType.ENGMID_TAIL, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_FRONT:
                {

                    if (active_segment_type == PlaneSegments.SegmentType.ENGINE_MID)
                    {
                        Transition(Transitions.TransitionType.FUSELM_ENGMID, TweenManager.DIRECTION.START_TO_END);
                    }
                    if (active_segment_type == PlaneSegments.SegmentType.COCKPIT)
                    {
                        Transition(Transitions.TransitionType.COCKPIT_FUSELM, TweenManager.DIRECTION.END_TO_START);
                    }

                }
                break;
            case PlaneSegments.SegmentType.FUSELAGE_RIGHT_MID:
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
            current_state = PlaneCameraStates.SELECTION;
            disable_input = false;
        }
    }

    void EnableInputs()
    {
        disable_input = false;
    }

    void DisableInputs()
    {
        disable_input = true;
    }

    void Transition(Transitions.TransitionType type, TweenManager.DIRECTION direction)
    {
        for (int i = 0; i < transition.Count; i++)
        {
            if (transition[i].type == type)
            {
                transition[i].Start(ref positionValX, ref positionValY, ref positionValZ, direction, EnableInputs, DisableInputs);
            }
        }
    }
}
