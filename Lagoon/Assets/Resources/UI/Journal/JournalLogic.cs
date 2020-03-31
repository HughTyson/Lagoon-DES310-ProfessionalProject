using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalLogic : MonoBehaviour
{
    TweenManager.TweenPathBundle showTween;
    TweenAnimator.Animation showAnimation;

    TweenManager.TweenPathBundle turnPageLeftToRightTween;
    TweenAnimator.Animation turnPageLeftToRightAnimation;

    [SerializeField] GameObject journal;
    [SerializeField] GameObject page;
    [SerializeField] GameObject frontCover;
    [SerializeField] GameObject backCover;
    [SerializeField] GameObject leftPageReference;
    [SerializeField] GameObject rightPageReference;

    static float TIME_BOOK_SHOW = 1.0f;
    static float TIME_TURN_PAGE = 1.0f;


    bool IsActive = false;
    void Start()
    {
        showTween = new TweenManager.TweenPathBundle(
            // Journal X Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Journal Y Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(-0.356f, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Journal Z Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.217f, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Journal X Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 60, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Journal Y Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(180, 180, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Journal Z Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),

            // Front Cover X Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.72f, 0.72f, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
           // Front Cover Y Rotation
           new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
           // Front Cover Z Rotation
           new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(175.0f, -1.07f, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_INOUT)
                    )
            );

        showAnimation = new TweenAnimator.Animation(
            showTween,
            new TweenAnimator.Transf_(
                journal.transform,
                local_position: new TweenAnimator.Transf_.LocalPosition_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                local_rotation: new TweenAnimator.Transf_.LocalRotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Transf_(
                frontCover.transform,
                local_rotation: new TweenAnimator.Transf_.LocalRotation_(true, 6, true, 7, true, 8, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );


        turnPageLeftToRightTween = new TweenManager.TweenPathBundle(
            // Page X Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.x, rightPageReference.transform.position.x, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page Y Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.y, rightPageReference.transform.position.y, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page Z Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.z, rightPageReference.transform.position.z, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
            //    new TweenManager.TweenPart_Continue(rightPageReference.transform.position.z, TIME_TURN_PAGE/ 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page X Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.x, rightPageReference.transform.rotation.eulerAngles.x, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page Y Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.y, rightPageReference.transform.rotation.eulerAngles.y, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page Z Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.z, rightPageReference.transform.rotation.eulerAngles.z + 360.0f, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.EASE_INOUT)
                    )
            );

        turnPageLeftToRightAnimation = new TweenAnimator.Animation(
            turnPageLeftToRightTween,
            new TweenAnimator.Transf_(
                page.transform,
                position: new TweenAnimator.Transf_.Position_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.Transf_.Rotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );
    }


    // Update is called once per frame
    void Update()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.START))
        {
            if (!showAnimation.IsPlaying)
            {
                showAnimation.PlayAnimation();
            }
        }
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        {
            turnPageLeftToRightAnimation.PlayAnimation(startingDirection_: TweenManager.DIRECTION.END_TO_START);
        }
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.RB))
        {
            turnPageLeftToRightAnimation.PlayAnimation();
        }
        //if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        //{
        //    animator.Play(hideBookAnimID, 0);
        //}
    }


    public void Show()
    {

    }

    public void Hide()
    {

    }
}
