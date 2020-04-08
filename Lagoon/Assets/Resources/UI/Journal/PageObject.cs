using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageObject : MonoBehaviour
{
    [SerializeField] PageContentSlot leftSide;
    [SerializeField] PageContentSlot rightSide;


    TweenAnimator.Animation turnPageLeftToRightAnimation;
    TweenAnimator.Animation turnPageRightToLeftAnimation;



    Transform leftBookReference;
    Transform rightBookReference;

    PageContentSlot firstPageContentSlot;
    PageContentSlot lastPageContentSlot;


    public void Init(TweenManager.TweenPathBundle leftToRightPageTurnTween, Transform leftBookReference_, Transform rightBookPageReference_, PageContentSlot firstPageContentSlot_, PageContentSlot lastPageContentSlot_)
    {



        leftBookReference = leftBookReference_;
        rightBookReference = rightBookPageReference_;
        firstPageContentSlot = firstPageContentSlot_;
        lastPageContentSlot = lastPageContentSlot_;

        turnPageLeftToRightAnimation = new TweenAnimator.Animation(
            leftToRightPageTurnTween,
            new TweenAnimator.Transf_(
                transform,
                position: new TweenAnimator.Transf_.Position_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                rotation: new TweenAnimator.Transf_.Rotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.Generic_(
               fireCloseToLeftToRightStart
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.2f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.FLIP_FLOP)
               ),
            new TweenAnimator.Generic_(
               fireCloseToLeftToRightEnd
               ,
               new TweenAnimator.Generic_.Trigger_(6, 0.8f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.FLIP_FLOP)
               )
            );



        // is inverse of tween
        turnPageRightToLeftAnimation = new TweenAnimator.Animation(
                leftToRightPageTurnTween,
                new TweenAnimator.Transf_(
                    transform,
                    position: new TweenAnimator.Transf_.Position_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
                    rotation: new TweenAnimator.Transf_.Rotation_(true, 3, true, 4, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
               new TweenAnimator.Generic_(
                   fireCloseToRightToLeftStart
                   ,
                   new TweenAnimator.Generic_.Trigger_(6, 0.8f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.FLIP_FLOP)
                   ),
               new TweenAnimator.Generic_(
                   fireCloseToRightToLeftEnd
                   ,
                   new TweenAnimator.Generic_.Trigger_(6, 0.2f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.FLIP_FLOP)
                   )
                );
    }

    void fireCloseToLeftToRightStart()
    {
        firstPageContentSlot.ShowContent();
    }
    void fireCloseToLeftToRightEnd()
    {

        lastPageContentSlot.HideContent();
    }
    void fireCloseToRightToLeftStart()
    {
        lastPageContentSlot.ShowContent();
    }
    void fireCloseToRightToLeftEnd()
    {

        firstPageContentSlot.HideContent();
    }

    System.Action<PageObject> completedTurn;
    PageContent leftPageContent;
    PageContent rightPageContent;
    public void TurnPageRight(System.Action<PageObject> completedTurn_, PageContent leftContentPage_, PageContent rightContentPage_)
    {
        gameObject.SetActive(true);

        completedTurn = completedTurn_;
        leftPageContent = leftContentPage_;
        rightPageContent = rightContentPage_;

        turnPageLeftToRightAnimation.PlayAnimation(
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA,
            animationCompleteDelegate_: toLeftAnimationCompleted
            );

        firstPageContentSlot.HideContent();
        leftSide.AttachContent(leftPageContent);
        leftSide.ShowContent();
        rightSide.AttachContent(rightPageContent);
        rightSide.ShowContent();

    }


    void toLeftAnimationCompleted()
    {
        leftSide.ResetHideCount();
        rightSide.ResetHideCount();
        leftSide.DetachContent();
        rightSide.DetachContent();

        lastPageContentSlot.AttachContent(leftPageContent);
        lastPageContentSlot.ShowContent();
        gameObject.SetActive(false);
        completedTurn(this);
    }

    public void TurnPageLeft(System.Action<PageObject> completedTurn_, PageContent leftPage_, PageContent rightPage_)
    {
        gameObject.SetActive(true);

        completedTurn = completedTurn_;
        leftPageContent = leftPage_;
        rightPageContent = rightPage_;

        turnPageRightToLeftAnimation.PlayAnimation(
            TimeFormat_:TweenManager.TIME_FORMAT.UNSCALE_DELTA,
            animationCompleteDelegate_:toRightAnimationCompleted,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );

        lastPageContentSlot.HideContent();
        leftSide.AttachContent(leftPageContent);
        leftSide.ShowContent();
        rightSide.AttachContent(rightPageContent);
        rightSide.ShowContent();
    }

    void toRightAnimationCompleted()
    {
        leftSide.ResetHideCount();
        rightSide.ResetHideCount();
        leftSide.DetachContent();
        rightSide.DetachContent();

        firstPageContentSlot.AttachContent(rightPageContent);
        firstPageContentSlot.ShowContent();
        gameObject.SetActive(false);

        completedTurn(this);
    }
}
