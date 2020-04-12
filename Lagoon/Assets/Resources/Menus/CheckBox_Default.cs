using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheckBox_Default : Checkbox_
{
    [SerializeField] Image imageCheck;
    [SerializeField] Image imageBox;

    [SerializeField]
    RectTransform mainParentRectTransfrom;


    protected override void ThisInit_Layer3()
    {
        InternalEvent_ToggleSet += valueSet;
    }

    protected override void ApplyDefaults()
    {
        TweenManager.TweenPathBundle showToggleOn_Tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 1.0f, 0.0f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.0f, 1.0f, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(-15.0f, 0.0f, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.0f, 0.0f, 0.05f, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
                )
            );

        TweenAnimator.Animation showToggleOn_Anim = new TweenAnimator.Animation(
            showToggleOn_Tween,
            new TweenAnimator.Generic_(
                correctImageCheckSetting,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.TransfRect_.Rotation_(2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
           new TweenAnimator.TransfRect_(
                imageCheck.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 3, true, 3, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );

        TweenManager.TweenPathBundle showToggleOff_Tween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 1.0f, 0.0f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.0f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(-15.0f, 0.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
                )
            );

        TweenAnimator.Animation showToggleOff_Anim = new TweenAnimator.Animation(
            showToggleOn_Tween,
            new TweenAnimator.Generic_(
                correctImageCheckSetting,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.TransfRect_.Rotation_(2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );



        TweenManager.TweenPathBundle hideTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );

        TweenAnimator.Animation hideAnim = new TweenAnimator.Animation(
            hideTween,
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );


        TweenManager.TweenPathBundle hoverOver_Tween  = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 1.5f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
            )
        );
        TweenAnimator.Animation hoverOver_anim = new TweenAnimator.Animation(
            hoverOver_Tween,
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );

        TweenManager.TweenPathBundle unhoverOver_Tween = new TweenManager.TweenPathBundle(
         new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.5f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
            )
        );
        TweenAnimator.Animation unhoverOver_anim = new TweenAnimator.Animation(
            unhoverOver_Tween,
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );

        TweenManager.TweenPathBundle selectToggleOn_Tween = new TweenManager.TweenPathBundle(
         new TweenManager.TweenPath(
             new TweenManager.TweenPart_Start(1,1,0, TweenManager.CURVE_PRESET.LINEAR)
             ),
         new TweenManager.TweenPath( // Parent Scale
            new TweenManager.TweenPart_Start(1.5f, 1.25f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
         new TweenManager.TweenPath( // RGB
            new TweenManager.TweenPart_Start(1.0f, 0.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        new TweenManager.TweenPath( // Check Scale
            new TweenManager.TweenPart_Start(0.0f, 0.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
            )
        );
        TweenAnimator.Animation selectToggleOn_anim = new TweenAnimator.Animation(
            selectToggleOn_Tween,
            new TweenAnimator.Generic_(correctImageCheckSetting, new TweenAnimator.Generic_.Trigger_(0, 0, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)),
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Image_(
                imageBox,
                color: new TweenAnimator.Image_.Color_(true, 2, true, 2, true, 2, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.TransfRect_(
                imageCheck.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 3, true, 3, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Image_(
                imageCheck,
                color: new TweenAnimator.Image_.Color_(true, 2, true, 2, true, 2, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );


        TweenManager.TweenPathBundle selectToggleOff_Tween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
             new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
             ),
         new TweenManager.TweenPath( // Scale
            new TweenManager.TweenPart_Start(1.5f, 1.25f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
         new TweenManager.TweenPath( // RGB
            new TweenManager.TweenPart_Start(1.0f, 0.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
         new TweenManager.TweenPath( // Child Scale
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
        TweenAnimator.Animation selectToggleOff_anim = new TweenAnimator.Animation(
            selectToggleOff_Tween,
            new TweenAnimator.Generic_(correctImageCheckSetting, new TweenAnimator.Generic_.Trigger_(0, 0.9999f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)),
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Image_(
                imageBox,
                color: new TweenAnimator.Image_.Color_(true, 2, true, 2, true, 2, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
            new TweenAnimator.TransfRect_(
                imageCheck.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 3, true, 3, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Image_(
                imageCheck,
                color: new TweenAnimator.Image_.Color_(true, 2, true, 2, true, 2, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           );


        OverrideBeginShow_ToggledOn(showToggleOn_Anim, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginShow_ToggledOff(showToggleOff_Anim, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideHideAnimation(hideAnim, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginHoverOverAnimation(hoverOver_anim, false, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginUnHoverOverAnimation(unhoverOver_anim, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelect_ToggledOn(selectToggleOn_anim, true, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelect_ToggledOff(selectToggleOff_anim,true,  new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));

    }

    void correctImageCheckSetting()
    {
        imageCheck.enabled = ToggledOn;
    }

    void valueSet(EventArgs_ValueChanged args)
    {
        imageCheck.enabled = args.newValue;
    }




   


}
