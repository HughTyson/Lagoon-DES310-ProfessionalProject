using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSignal : MonoBehaviour
{

    [SerializeField] Transform look_at;
    [SerializeField] HDRP_SpriteSheet ui_signal_spritesheet;
    public bool run_ui;

    TweenAnimator.Animation incoming_signal_animation;
    TweenManager.TweenPathBundle radio_signal_tween;


    HDRP_Unlit_ManualAnimator manual_sprite_animator;
    Material material;

    private void Awake()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += Story_objective_Event_BarrierObjectiveComplete;
        GM_.Instance.story.Event_BarrierStart += BarrierStart;


        manual_sprite_animator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void BarrierStart(StoryManager.BarrierStartArgs obj)
    {
        run_ui = false;
        GM_.Instance.DayNightCycle.SetBaseTime(1.0f);
        GM_.Instance.DayNightCycle.SetTime();
    }

    private void Story_objective_Event_BarrierObjectiveComplete()
    {
        run_ui = true;

        GM_.Instance.DayNightCycle.SetBaseTime(0.0f);

    }


    // Start is called before the first frame update
    void Start()
    {

        SetPosition();

        radio_signal_tween = new TweenManager.TweenPathBundle(
            // frame index
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, (ui_signal_spritesheet.Animations[0].FrameIndexes.Count - 1) + 0.01f, ui_signal_spritesheet.Animations[0].Duration, TweenManager.CURVE_PRESET.LINEAR)
                )

            );


        incoming_signal_animation = new TweenAnimator.Animation(
            radio_signal_tween,
            new TweenAnimator.HDRP_Animator_(
                manual_sprite_animator,
               new TweenAnimator.HDRP_Animator_.Frame_(
                0,
                TweenAnimator.Base.IntProperty.INT_SELECTION_METHOD.FLOOR,
                TweenAnimator.MOD_TYPE.ABSOLUTE
                )
                )
                );
    }

    // Update is called once per frame
    void Update()
    {

        if(run_ui)
        {

            transform.rotation = Quaternion.LookRotation(look_at.position - transform.position , Vector3.up);


            if (!incoming_signal_animation.IsPlaying)
            {
                manual_sprite_animator.SetAnimation(ui_signal_spritesheet.Animations[0]);
                incoming_signal_animation.PlayAnimation();
            }


        }
        else if(!run_ui)
        {
            incoming_signal_animation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE);
            manual_sprite_animator.Hide();
        }


    }


    void SetPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
    }

    private void OnDestroy()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete -= Story_objective_Event_BarrierObjectiveComplete;
        GM_.Instance.story.Event_BarrierStart -= BarrierStart;
    }

}
