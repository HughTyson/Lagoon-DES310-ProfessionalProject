using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuItem_Image : MenuItem_
{
    [SerializeField] Image image;


    [Range(0,10)]
    [SerializeField] float duration;

    private void Awake()
    {
        Init();
    }

    protected override void ApplyDefaults()
    {
        TweenManager.TweenPathBundle tweenHide = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1,0, duration, TweenManager.CURVE_PRESET.LINEAR)
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
            new TweenManager.TweenPart_Start(0, 1, duration, TweenManager.CURVE_PRESET.LINEAR)
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


        OverrideHideAnimation(animHide, new TweenAnimator.Animation.PlayArgs());
        OverrideShowAnimation(animShow, new TweenAnimator.Animation.PlayArgs());
    }



}
