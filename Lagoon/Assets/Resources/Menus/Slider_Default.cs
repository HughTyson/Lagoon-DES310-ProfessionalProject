using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slider_Default : Slider_
{
    private void Awake()
    {
        Init();
    }

    [SerializeField] RectTransform startPoint;
    [SerializeField] RectTransform endPoint;

    [SerializeField] Image startImage;
    [SerializeField] Image endImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image handleImage;

    [SerializeField] Color background_image_color = Color.white;

    [SerializeField]
    RectTransform mainParentRectTransfrom;

    [Tooltip("Don't set under 1")]
    [SerializeField] float hover_size_increase = 1.5f;



    protected override void ThisInit_Layer3()
    {
        InternalEvent_ValueChanged += valueChanged;
        InternalEvent_ValueSet += valueSet;

        backgroundImage.color = background_image_color;
    }

    protected override void ApplyDefaults()
    {
        TweenManager.TweenPathBundle defaultHideButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.5f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );

        TweenManager.TweenPathBundle defaultShowButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 1, 0.0f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
            ),
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0,1, 0.2f, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
            )
        );

        TweenManager.TweenPathBundle defaultHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, hover_size_increase, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0.75f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultUnHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(hover_size_increase, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.75f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultSelectButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.75f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );
        TweenManager.TweenPathBundle defaultUnSelectButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 0.75f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );




        TweenAnimator.Animation showAnimation = new TweenAnimator.Animation(
            defaultShowButtonTween,
            new TweenAnimator.Generic_(
                setupHandle,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.Generic_(
                resetValues,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(false, -1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                 ),
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(false, -1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );



        TweenAnimator.Animation hideAnimation = new TweenAnimator.Animation(
            defaultHideButtonTween,
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(false, -1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
           new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(false, -1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
           // new TweenAnimator.Image_(
           //     startImage,
           //     color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
           //     ),
           // new TweenAnimator.Image_(
           //     endImage,
           //     color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
           //     ),
           //new TweenAnimator.Image_(
           //     backgroundImage,
           //     color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
           //     )
            );



        TweenAnimator.Animation hoverAnimation = new TweenAnimator.Animation(
            defaultHoverOverButtonTween,
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );



        TweenAnimator.Animation unHoverAnimation = new TweenAnimator.Animation(
            defaultUnHoverOverButtonTween,
            new TweenAnimator.TransfRect_(
                mainParentRectTransfrom,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );


        TweenAnimator.Animation selectAnimation = new TweenAnimator.Animation(
            defaultSelectButtonTween,
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );

        TweenAnimator.Animation unSelectAnimation = new TweenAnimator.Animation(
            defaultUnSelectButtonTween,
            new TweenAnimator.TransfRect_(
                handleImage.rectTransform,
                scale: new TweenAnimator.TransfRect_.Scale_(true, 0, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );

        OverrideShowAnimation(showAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideHideAnimation(hideAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginHoverOverAnimation(hoverAnimation, false, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginUnHoverOverAnimation(unHoverAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelectAnimation(selectAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginUnSelectAnimation(unSelectAnimation, true, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));

        //OverrideUpdateSelectedAnimation();

    }

    void resetValues()
    {
        handleImage.rectTransform.localScale = new Vector3(1, 1, 1);
        handleImage.color = new Color(1, 1, 1, 1);
        mainParentRectTransfrom.localScale = new Vector3(1, 1, 1);
        startImage.color = new Color(1, 1, 1, 1);
        endImage.color = new Color(1, 1, 1, 1);
        backgroundImage.color = background_image_color;
    }


    void setupHandle()
    {
        Vector2 handlePosition = Vector2.Lerp(startPoint.anchoredPosition, endPoint.anchoredPosition, SliderWeighting);
        handleImage.rectTransform.anchoredPosition = handlePosition;
    }


    void valueChanged(EventArgs_ValueChanged args)
    {
        setupHandle();
    }
    void valueSet(EventArgs_ValueChanged args)
    {
        setupHandle();
    }

    private void OnDestroy()
    {
        InternalEvent_ValueChanged -= valueChanged;
        InternalEvent_ValueSet -= valueSet;
    }

}
