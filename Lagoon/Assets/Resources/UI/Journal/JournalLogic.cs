

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JournalLogic : MonoBehaviour
{
    TweenManager.TweenPathBundle showTween;
    TweenAnimator.Animation showAnimation;

    TweenManager.TweenPathBundle turnPageLeftToRightTween;
    TweenAnimator.Animation turnPageLeftToRightAnimation;
    TweenAnimator.Animation turnPageRightToLeftAnimation;


    [SerializeField] GameObject journal;
    [SerializeField] GameObject page;
    [SerializeField] GameObject frontCover;
    [SerializeField] GameObject backCover;
    [SerializeField] GameObject leftPageReference;
    [SerializeField] GameObject rightPageReference;

    static float TIME_BOOK_SHOW = 1.0f;
    static float TIME_TURN_PAGE = 1.0f;


    [SerializeField] PageManager pageManager;
    [SerializeField] BasePagePair pausePage;

    bool isShowing = false;




    void Start()
    {

        pageManager.EventRequest_CurrentPage_WantsToChange += requestedToChangePage;
        pageManager.EventRequest_CurrentPage_WantsToCloseJournal += requestedToCloseJournal;

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
                    ),

            // Normalized Time
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.LINEAR)
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
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.x, rightPageReference.transform.position.x, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                    ),
            // Page Y Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.y, rightPageReference.transform.position.y, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                    ),
            // Page Z Position
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.position.z, rightPageReference.transform.position.z, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
            //    new TweenManager.TweenPart_Continue(rightPageReference.transform.position.z, TIME_TURN_PAGE/ 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
                    ),
            // Page X Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.x, rightPageReference.transform.rotation.eulerAngles.x, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                    ),
            // Page Y Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.y, rightPageReference.transform.rotation.eulerAngles.y, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                    ),
            // Page Z Rotation
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(leftPageReference.transform.rotation.eulerAngles.z, rightPageReference.transform.rotation.eulerAngles.z + 360.0f, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                    ),
            // Normalized Time
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, TIME_TURN_PAGE, TweenManager.CURVE_PRESET.LINEAR)
                )
            );

        turnPageLeftToRightAnimation = new TweenAnimator.Animation(
            turnPageLeftToRightTween,
            new TweenAnimator.Transf_(
                page.transform,
                position: new TweenAnimator.Transf_.Position_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.Transf_.Rotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
           new TweenAnimator.Generic_(
               fireCloseToLeftToRightStart
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.1f)
               ),
           new TweenAnimator.Generic_(
               fireCloseToLeftToRightEnd
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.9f)
               )
            );



        // is inverse of tween
        turnPageRightToLeftAnimation = new TweenAnimator.Animation(
            turnPageLeftToRightTween,
            new TweenAnimator.Transf_(
                page.transform,
                position: new TweenAnimator.Transf_.Position_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.Transf_.Rotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
           new TweenAnimator.Generic_(
               fireCloseToRightToLeftStart
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.9f)
               ),
           new TweenAnimator.Generic_(
               fireCloseToRightToLeftEnd
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.1f)
               )
            );

        page.SetActive(false);
        journal.SetActive(false);
    }



    void requestedToChangePage(BasePagePair.RequestToChangePage args)
    {       
        if (!turnPageRightToLeftAnimation.IsPlaying && !turnPageRightToLeftAnimation.IsPlaying && !showAnimation.IsPlaying)
        {
            int comparison_to_current = pageManager.CompareCurrentPairToPair(args.changeToPagePair);

            // Right To Left
            if (comparison_to_current > 0)
            {
                page.SetActive(true);
                pageManager.PrepareNewPair(args.changeToPagePair);
                pageManager.SwapAttachedPages(PageManager.PAGE.BOOK_RIGHT, PageManager.PAGE.PAGE_LEFT);
                pageManager.SetPage(PageManager.PAGE.PAGE_RIGHT, args.changeToPagePair.LeftPage);
                pageManager.SetPage(PageManager.PAGE.BOOK_RIGHT, args.changeToPagePair.RightPage);
                pageManager.HidePage(PageManager.PAGE.BOOK_RIGHT);

                turnPageRightToLeftAnimation.PlayAnimation
                    (
                    animationCompleteDelegate_: rightToLeftCompleted,
                    startingDirection_: TweenManager.DIRECTION.END_TO_START,
                    TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
                    );

            }
            // Left To Right
            else if (comparison_to_current < 0)
            {
                page.SetActive(true);
                pageManager.PrepareNewPair(args.changeToPagePair);
                pageManager.SwapAttachedPages(PageManager.PAGE.BOOK_LEFT, PageManager.PAGE.PAGE_RIGHT);
                pageManager.SetPage(PageManager.PAGE.PAGE_LEFT, args.changeToPagePair.RightPage);
                pageManager.SetPage(PageManager.PAGE.BOOK_LEFT, args.changeToPagePair.LeftPage);
                pageManager.HidePage(PageManager.PAGE.BOOK_LEFT);

                turnPageLeftToRightAnimation.PlayAnimation
                    (
                    animationCompleteDelegate_: leftToRightCompleted,
                    TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
                    );
            }
            else
            {
                pageManager.PrepareNewPair(args.changeToPagePair);

                if (args.changeToPagePair != null)
                {
                    pageManager.SetPage(PageManager.PAGE.BOOK_RIGHT, args.changeToPagePair.RightPage);
                    pageManager.SetPage(PageManager.PAGE.BOOK_LEFT, args.changeToPagePair.LeftPage);
                }


            }
        }
    }

    void requestedToCloseJournal()
    {
        if (!turnPageRightToLeftAnimation.IsPlaying && !turnPageRightToLeftAnimation.IsPlaying && !showAnimation.IsPlaying)
        {
            if (turnPageLeftToRightAnimation.IsPlaying)
                turnPageLeftToRightAnimation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);

            requestedToChangePage(new BasePagePair.RequestToChangePage(null));
            showAnimation.PlayAnimation(startingDirection_: TweenManager.DIRECTION.END_TO_START, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, animationCompleteDelegate_: completedHideJournal);

            isShowing = false;
        }
    }
    void fireCloseToRightToLeftStart()
    {
        pageManager.ShowPage(PageManager.PAGE.BOOK_RIGHT);
    }
    void fireCloseToRightToLeftEnd()
    {
        pageManager.HidePage(PageManager.PAGE.BOOK_LEFT);
    }

    void leftToRightCompleted()
    {
        page.SetActive(false);
        pageManager.SwapAttachedPages(PageManager.PAGE.PAGE_LEFT, PageManager.PAGE.BOOK_RIGHT);
        pageManager.ShowPage(PageManager.PAGE.BOOK_RIGHT);
        pageManager.ApplyNewPair();
    }


    void fireCloseToLeftToRightStart()
    {
        pageManager.ShowPage(PageManager.PAGE.BOOK_LEFT);
    }
    void fireCloseToLeftToRightEnd()
    {

        pageManager.HidePage(PageManager.PAGE.BOOK_RIGHT);
    }
    void rightToLeftCompleted()
    {
        page.SetActive(false);
        pageManager.SwapAttachedPages(PageManager.PAGE.PAGE_RIGHT, PageManager.PAGE.BOOK_LEFT);
        pageManager.ShowPage(PageManager.PAGE.BOOK_LEFT);
        pageManager.ApplyNewPair();
    }

    // Update is called once per frame
    void Update()
    {
        // Show Journal
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.START) || GM_.Instance.input.GetButtonDown(InputManager.BUTTON.Y))
        {
            if (!isShowing)
            {
                if (!showAnimation.IsPlaying)
                {

                    if (!journal.activeSelf)
                    {
                        journal.SetActive(true);
                    }

                    requestedToChangePage(new BasePagePair.RequestToChangePage(pausePage));

                    GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    showAnimation.PlayAnimation(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, animationCompleteDelegate_: completedShowJournal);

                    GM_.Instance.ui.gameObject.SetActive(false);
                    GM_.Instance.pause.Pause();

                    isShowing = true;
                }

            }
        }

        // Hide Journal
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.START))
        {
            if (isShowing)
            {
                if (!showAnimation.IsPlaying)
                {

                    if (!journal.activeSelf)
                    {
                        journal.SetActive(true);
                    }

                    if (turnPageLeftToRightAnimation.IsPlaying)
                        turnPageLeftToRightAnimation.StopAnimation(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);

                    requestedToChangePage(new BasePagePair.RequestToChangePage(null));

                    GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    showAnimation.PlayAnimation(startingDirection_: TweenManager.DIRECTION.END_TO_START, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, animationCompleteDelegate_: completedHideJournal);

                    isShowing = false;
                }
            }
        }
    }

    void completedHideJournal()
    {
        pageManager.ApplyNewPair();
        GM_.Instance.ui.gameObject.SetActive(true);
        GM_.Instance.pause.UnPause();
    }

    void completedShowJournal()
    {
        pageManager.ApplyNewPair();
    }
}
