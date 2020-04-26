using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepUiSignal : MonoBehaviour
{

    [SerializeField] Transform look_at;
    [SerializeField] HDRP_SpriteSheet ui_signal_spritesheet;

    HDRP_Unlit_ManualAnimator manual_sprite_animator;
    Material material;

    TweenAnimator.Animation sleep_signal_animation;
    TweenManager.TweenPathBundle sleep_signal_tween;

    public bool run_ui = false;

    private void Awake()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += Story_objective_Event_BarrierObjectiveComplete;
        GM_.Instance.story.Event_BarrierStart += Story_Event_BarrierStart;

        manual_sprite_animator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        sleep_signal_tween = new TweenManager.TweenPathBundle(
            // frame index
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, (ui_signal_spritesheet.Animations[0].FrameIndexes.Count - 1) + 0.01f, ui_signal_spritesheet.Animations[0].Duration, TweenManager.CURVE_PRESET.LINEAR)
                )

            );


        sleep_signal_animation = new TweenAnimator.Animation(
            sleep_signal_tween,
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
            Debug.Log("hello");
            transform.rotation = Quaternion.LookRotation(look_at.position - transform.position, Vector3.up);

            if (!sleep_signal_animation.IsPlaying)
            {
                manual_sprite_animator.SetAnimation(ui_signal_spritesheet.Animations[0]);
                sleep_signal_animation.PlayAnimation();
            }
        }
        else if (!run_ui)
        {
            sleep_signal_animation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE);
            manual_sprite_animator.Hide();
        }
    }

    private void Story_Event_BarrierStart(StoryManager.BarrierStartArgs args)
    {
        for(int i = 0; i < args.Barriers.Count; i++)
        {
            if(args.Barriers[i] == RootNode.BARRIER_STATE.NEXT_DAY)
            {
                run_ui = true;
            }
        }
    }

    private void Story_objective_Event_BarrierObjectiveComplete()
    {
        run_ui = false;
    }




}
