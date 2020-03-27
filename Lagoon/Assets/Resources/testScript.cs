using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{

    static public readonly TweenManager.TweenPathBundle animationTestTween = new TweenManager.TweenPathBundle(
        //  X Pos
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0 , 3, 2, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        //  Y Pos
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 2, 1, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(0, 1, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        //  Z Pos
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 1, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        //  X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 180, 2, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        //  Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 90, 2, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // X Scale
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 3, 1, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(1, 1, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

    public TweenAnimator.Animation animation;


    // Start is called before the first frame update
    void Start()
    {
        animation = new TweenAnimator.Animation(
        animationTestTween,
        new TweenAnimator.Transf_(
            transform,
            position: new TweenAnimator.Transf_.Position_(true,0, true,1, true,2, TweenAnimator.MOD_TYPE.OFFSET),
            rotation: new TweenAnimator.Transf_.Rotation_(true,3, true,3, true,4, TweenAnimator.MOD_TYPE.OFFSET),
            local_scale: new TweenAnimator.Transf_.LocalScale_(true,5, false,0, false,0, TweenAnimator.MOD_TYPE.ABSOLUTE)
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            if (!animation.IsPlaying)
            {
                animation.PlayAnimation();
            }
        }

    }
}
