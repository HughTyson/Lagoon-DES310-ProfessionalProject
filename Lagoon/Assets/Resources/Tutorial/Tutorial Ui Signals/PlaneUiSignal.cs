using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneUiSignal : MonoBehaviour
{


    [SerializeField] Transform look_at;
    [SerializeField] HDRP_SpriteSheet ui_signal_spritesheet;

    HDRP_Unlit_ManualAnimator manual_sprite_animator;
    Material material;

    TweenAnimator.Animation repair_signal_animation;
    TweenManager.TweenPathBundle repair_signal_tween;

    public bool run_ui = false;

    private void Awake()
    {

        GM_.Instance.inventory.Event_CanFixPlane += Inventory_Event_CanFixPlane;
        GM_.Instance.inventory.Event_MissingParts += Inventory_Event_MissingParts;

        manual_sprite_animator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Inventory_Event_MissingParts()
    {
        run_ui = false;
    }

    private void Inventory_Event_CanFixPlane()
    {
        run_ui = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        repair_signal_tween = new TweenManager.TweenPathBundle(
            // frame index
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, (ui_signal_spritesheet.Animations[0].FrameIndexes.Count - 1) + 0.01f, ui_signal_spritesheet.Animations[0].Duration, TweenManager.CURVE_PRESET.LINEAR)
                )

            );


        repair_signal_animation = new TweenAnimator.Animation(
            repair_signal_tween,
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

            if (!repair_signal_animation.IsPlaying)
            {
                manual_sprite_animator.SetAnimation(ui_signal_spritesheet.Animations[0]);
                repair_signal_animation.PlayAnimation();
            }
        }
        else if(!run_ui)
        {
            repair_signal_animation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE);
            manual_sprite_animator.Hide();
        }
    }
}
