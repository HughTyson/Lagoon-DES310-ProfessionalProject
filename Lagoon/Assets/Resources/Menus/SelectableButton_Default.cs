using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectableButton_Default : SelectableButton_
{
    private void Awake()
    {
        Init();
    }

    [SerializeField] Image image;
    [SerializeField] Sprite spriteSelected;
    [SerializeField] Sprite spriteHovering;
    [SerializeField] Sprite spriteNotSelected; 





    static readonly TweenManager.TweenPathBundle defaultHideButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.5f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );





    protected sealed override void ThisInit_Layer3()
    {
    }



    protected override void ApplyDefaults()
    {
        TweenManager.TweenPathBundle defaultShowButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 1.0f, 0.0f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0.0f, 1.0f, 0.2f, TweenManager.CURVE_PRESET.EASE_IN)
            ),
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(-15.0f, 0.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
            )
        );

        TweenManager.TweenPathBundle defaultHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 1.5f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultUnHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.5f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultSelectButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 0.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.5f, 1.25f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                 new TweenManager.TweenPart_Continue(1.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );



        TweenAnimator.Animation showAnimation = new TweenAnimator.Animation(
            defaultShowButtonTween,
            new TweenAnimator.Generic_(
                resetbaseColour,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.Generic_(
                setSpriteToNotSelectedOrHovering,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.TransfRect_(
                image.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.TransfRect_.Rotation_(2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );



        TweenAnimator.Animation hideAnimation = new TweenAnimator.Animation(
            defaultHideButtonTween,
            new TweenAnimator.Image_(
                image,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );



        TweenAnimator.Animation hoverAnimation = new TweenAnimator.Animation(
            defaultHoverOverButtonTween,
                new TweenAnimator.TransfRect_(
                    image.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
                new TweenAnimator.Generic_(
                    setSpriteToHover,
                    new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                )
            );




        TweenAnimator.Animation unHoverAnimation = new TweenAnimator.Animation(
            defaultUnHoverOverButtonTween,
                new TweenAnimator.TransfRect_(
                    image.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
                 new TweenAnimator.Generic_(
                    setSpriteToNotSelectedOrHovering,
                    new TweenAnimator.Generic_.Trigger_(0, 0.9f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                )
            );


        TweenAnimator.Animation selectAnimation = new TweenAnimator.Animation(
            defaultSelectButtonTween,
                new TweenAnimator.TransfRect_(
                    image.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 2, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
                new TweenAnimator.Image_(
                    image,
                    color: new TweenAnimator.Image_.Color_(true, 1, true, 1, true, 1, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
                 new TweenAnimator.Generic_(
                    setSpriteToSelect,
                    new TweenAnimator.Generic_.Trigger_(0, 0.5f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                )
            );

        OverrideShowAnimation(showAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideHideAnimation(hideAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginHoverOverAnimation(hoverAnimation, false, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginUnHoverOverAnimation(unHoverAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelectAnimation(selectAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
    }

    void resetbaseColour()
    {
        image.color = new Color(1, 1, 1, 1);
    }


    void setSpriteToHover()
    {
        image.sprite = spriteHovering;
    }
    void setSpriteToSelect()
    {
        image.sprite = spriteSelected;
    }
    void setSpriteToNotSelectedOrHovering()
    {
        image.sprite = spriteNotSelected;
    }

}
