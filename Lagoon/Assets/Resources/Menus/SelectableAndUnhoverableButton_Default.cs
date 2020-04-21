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
    protected override void ThisInit_Layer3()
    {
        
    }
    protected override void ApplyDefaults()
    {
        SetButtonsToCheckForPress(buttonsToCheckFor);


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
                )
            );

        TweenManager.TweenPathBundle tweenShow = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.5f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
        TweenAnimator.Animation animShow = new TweenAnimator.Animation(
            tweenShow,
            new TweenAnimator.Image_
            (
                image,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );

        TweenManager.TweenPathBundle tweenSelect = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(1, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );
        TweenAnimator.Animation animSelect = new TweenAnimator.Animation(
            tweenSelect,
            new TweenAnimator.Image_
            (
                image,
                color: new TweenAnimator.Image_.Color_(true, 0, true, 0, true, 0, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );


        OverrideHideAnimation(animHide, new TweenAnimator.Animation.PlayArgs());
        OverrideShowAnimation(animShow, new TweenAnimator.Animation.PlayArgs());
        OverrideBeginSelectAnimation(animSelect, new TweenAnimator.Animation.PlayArgs());
    }

   
}
