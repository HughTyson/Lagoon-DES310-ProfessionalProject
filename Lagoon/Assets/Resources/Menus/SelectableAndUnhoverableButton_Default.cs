using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectableAndUnhoverableButton_Default : SelectableAndUnhoverableButton
{
    
    [SerializeField] InputManager.BUTTON[] buttonsToCheckFor;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite unselectedSprite;

    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI text;


    Color default_colour;
    protected override void ThisInit_Layer3()
    {
        
    }
    protected override void ApplyDefaults()
    {
        SetButtonsToCheckForPress(buttonsToCheckFor);
        default_colour = text.color;

        TweenManager.TweenPathBundle tweenHide = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 0.5f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );
        TweenAnimator.Animation animHide = new TweenAnimator.Animation(
            tweenHide,
            new TweenAnimator.Image_
            (
                image,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.TMPText_
            (
                text,
                color: new TweenAnimator.TMPText_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
               )            
            );

        TweenManager.TweenPathBundle tweenShow = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
        TweenAnimator.Animation animShow = new TweenAnimator.Animation(
            tweenShow,
             new TweenAnimator.Image_
                (
                image,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
             new TweenAnimator.TMPText_
            (
                text,
                color: new TweenAnimator.TMPText_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
               )
            );

        TweenManager.TweenPathBundle tweenSelect = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );
        TweenAnimator.Animation animSelect = new TweenAnimator.Animation(
            tweenSelect,
             new TweenAnimator.Generic_
            (
                setToPressed,
                new TweenAnimator.Generic_.Trigger_(0,0, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
            ),
             new TweenAnimator.Generic_
            (
                setToNonPressed,
                new TweenAnimator.Generic_.Trigger_(0, 0.5f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
            )
            );


        OverrideHideAnimation(animHide, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideShowAnimation(animShow, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelectAnimation(animSelect, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
    }

   
    void setToPressed()
    {
        image.sprite = selectedSprite;
    }
    void setToNonPressed()
    {
        image.sprite = unselectedSprite;
    }
}
