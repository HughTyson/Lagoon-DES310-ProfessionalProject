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

    AudioSFX sfx_radio;
    AudioSFX sfx_talking;

    AudioManager.SFXInstanceInterface incoming_transmission;
    AudioManager.SFXInstanceInterface talking;

    private void Awake()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += Story_objective_Event_BarrierObjectiveComplete;
        GM_.Instance.story.Event_BarrierStart += BarrierStart;

        GM_.Instance.story.Event_ConvoEnter += ConvoStart;

        GM_.Instance.story.Event_BranchStart += BranchStart;

        GM_.Instance.story.Event_ConvoExit += ConvoExit;

        manual_sprite_animator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;

        Application.quitting += Quitting;

        sfx_radio = GM_.Instance.audio.GetSFX("Radio_Calling");
        sfx_talking = GM_.Instance.audio.GetSFX("Radio_Talking");
    }

    private void ConvoExit()
    {
     
        talking?.Stop();
    }

    private void BranchStart(StoryManager.BranchEnterArgs args)
    {
        talking?.Stop();

        GM_.Instance.story.Event_DialogNewText += StartTalking;
    }

    private void StartTalking(StoryManager.DialogNewTextArgs obj)
    {
        talking = GM_.Instance.audio.PlaySFX(sfx_talking, null);
        GM_.Instance.story.Event_DialogNewText -= StartTalking;
    }

    private void ConvoStart()
    {
        incoming_transmission.Stop();

        talking = GM_.Instance.audio.PlaySFX(sfx_talking, null);

        GM_.Instance.story.RequestGameEventContinue();
        GM_.Instance.day_night_cycle.SetTime(0);
    }

    private void BarrierStart(StoryManager.BarrierStartArgs obj)
    {
        run_ui = false;

        if(incoming_transmission != null)
            incoming_transmission.Stop();

        GM_.Instance.day_night_cycle.SetBaseTime(1.0f);
        GM_.Instance.day_night_cycle.SetTime();
    }

    private void Story_objective_Event_BarrierObjectiveComplete()
    {
        run_ui = true;

        incoming_transmission =  GM_.Instance.audio.PlaySFX(sfx_radio, transform);

        GM_.Instance.day_night_cycle.SetBaseTime(0.0f);
       
    }


    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);

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

    private void OnDisable()
    {
        if (incoming_transmission != null)
            incoming_transmission.Stop();

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

    private void OnDestroy()
    {

        if(incoming_transmission != null)
        {
            incoming_transmission.Stop();
        }


        if (!quiting)
        {
            GM_.Instance.story_objective.Event_BarrierObjectiveComplete -= Story_objective_Event_BarrierObjectiveComplete;
            GM_.Instance.story.Event_BarrierStart -= BarrierStart;
            GM_.Instance.story.Event_ConvoEnter -= ConvoStart;
        }

        
    }

    bool quiting = false;
    void Quitting()
    {
        quiting = true;
    }

    
}
