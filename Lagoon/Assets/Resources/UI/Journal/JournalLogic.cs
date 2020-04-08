

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JournalLogic : MonoBehaviour
{
    TweenManager.TweenPathBundle showTween;
    TweenAnimator.Animation showAnimation;

    [SerializeField] GameObject pagePrefab;
    [SerializeField] int pooledPageObjectCount = 1;



    [SerializeField] GameObject journal;
    [SerializeField] GameObject frontCover;
    [SerializeField] GameObject backCover;
    [SerializeField] GameObject leftPageReference;
    [SerializeField] GameObject rightPageReference;

    [SerializeField] PageContentSlot FirstPageContentSlot;
    [SerializeField] PageContentSlot LastPageContentSlot;

    static float TIME_BOOK_SHOW = 1.0f;
    static float TIME_TURN_PAGE = 1.0f;


    [SerializeField] PageManager pageManager;
    [SerializeField] BasePagePair pausePage;

    bool isShowing = false;



    List<PageObject> availablePageObjects = new List<PageObject>();
    Queue<PageObject> usedPageObjects = new Queue<PageObject>();



    TweenManager.TweenPathBundle turnPageLeftToRightTween;

    void Start()
    {

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


        for (int i = 0; i < pooledPageObjectCount; i ++)
        {
            GameObject new_object = Instantiate<GameObject>(pagePrefab,transform);
            new_object.SetActive(false);
            availablePageObjects.Add(new_object.GetComponent<PageObject>());
            availablePageObjects[i].Init(turnPageLeftToRightTween, leftPageReference.transform, rightPageReference.transform , FirstPageContentSlot , LastPageContentSlot);
        }



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





        journal.SetActive(false);
    }




    PageManager.PairToPairInfo pairToPairInfo;
    List<BasePagePair> pairsRemaining = new List<BasePagePair>();
    BasePagePair startingPair;
    BasePagePair currentPair;
    BasePagePair futurePair;
    ActionTimer multiplePageIterator = new ActionTimer();


    BasePagePair previousPair;


    void requestedInfoOnPreviousPage(BasePagePair.InfoRequest_CameraFromPage_Args args)
    {
        if (previousPair != null)
            args.pageType = previousPair.GetType();
    }

    void requestedGoBackToPreviousPage()
    {
        requestedToChangePage(new BasePagePair.RequestToChangePage(previousPair));
    }



    public void RequestShowPage(BasePagePair pagePair)
    {
        requestedToChangePage(new BasePagePair.RequestToChangePage(pagePair));
    }
    void requestedToChangePage(BasePagePair.RequestToChangePage args)
    {

        if (!journal.activeSelf)
        {
            journal.SetActive(true);
        }


        if (pairsRemaining.Count == 0 && availablePageObjects.Count == pooledPageObjectCount)
        {

            pairsRemaining.Clear();
            pairToPairInfo = pageManager.GetInfoFromPairToPair(currentPair, args.changeToPagePair);


            startingPair = currentPair;
            futurePair = args.changeToPagePair;

            if (startingPair != null)
            {
                startingPair.EventRequest_ChangePage -= requestedToChangePage;
                startingPair.EventRequest_GoToPreviousPage -= requestedGoBackToPreviousPage;

                startingPair.BegunExitingPage();
                startingPair.InfoRequest_CameFromPage -= requestedInfoOnPreviousPage;
            }

            previousPair = currentPair;

            if (futurePair != null)
            {
                futurePair.gameObject.SetActive(true);
                futurePair.transform.GetChild(0).gameObject.SetActive(true);
                futurePair.transform.GetChild(1).gameObject.SetActive(true);

                futurePair.InfoRequest_CameFromPage += requestedInfoOnPreviousPage;
                futurePair.BegunEnteringPage();
            }

            switch (pairToPairInfo.direction)
            {
                case PageManager.PairToPairInfo.DIRECTION.GOING_LEFT:
                    {
                        pairsRemaining.AddRange(pairToPairInfo.basePagePairs);
                        for (int i = 0; i < pairToPairInfo.basePagePairs.Count; i++)
                        {
                            pairToPairInfo.basePagePairs[i].gameObject.SetActive(true);
                        }


                        multiplePageIterator.Clear();
                        float timePerPage = TIME_TURN_PAGE / ((float)pooledPageObjectCount);
                        int iterator = 0;
                        for (int i = pairToPairInfo.basePagePairs.Count - 1; i >= 0; i--)
                        {
                            multiplePageIterator.AddAction(ToChangePageIteration, timePerPage * ((float)iterator));
                            iterator++;
                        }

                        GM_.Instance.update_events.UpdateEvent += multiplePageIterator.Update;
                        multiplePageIterator.Start();


                    break;
                    }
                case PageManager.PairToPairInfo.DIRECTION.GOING_RIGHT:
                    {

                        pairsRemaining.AddRange(pairToPairInfo.basePagePairs);

                        for (int i = 0; i < pairToPairInfo.basePagePairs.Count; i++)
                        {
                            pairToPairInfo.basePagePairs[i].gameObject.SetActive(true);
                        }

                        multiplePageIterator.Clear();
                        float timePerPage = TIME_TURN_PAGE / ((float)pooledPageObjectCount);
                        int iterator = 0;
                        for (int i = pairToPairInfo.basePagePairs.Count - 1; i >= 0; i--)
                        {
                            multiplePageIterator.AddAction(ToChangePageIteration, timePerPage * ((float)iterator));
                            iterator++;
                        }

                        GM_.Instance.update_events.UpdateEvent += multiplePageIterator.Update;
                        multiplePageIterator.Start();

                        break;
                    }
                case PageManager.PairToPairInfo.DIRECTION.ON_TARGET:
                    {
                        if (futurePair == null)
                        {
                            isShowing = false;
 
                            currentPair = futurePair;

                            showAnimation.PlayAnimation(startingDirection_: TweenManager.DIRECTION.END_TO_START, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, animationCompleteDelegate_: completedHideJournal);
                        }
                        else if (currentPair == null)
                        {
                            GM_.Instance.ui.gameObject.SetActive(false);
                            GM_.Instance.pause.Pause();
                            isShowing = true;

                            currentPair = futurePair;

                            FirstPageContentSlot.DetachContent();
                            LastPageContentSlot.DetachContent();

                            FirstPageContentSlot.AttachContent(currentPair.LeftPage);
                            LastPageContentSlot.AttachContent(currentPair.RightPage);


                            showAnimation.PlayAnimation(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, animationCompleteDelegate_: pagePairFinalize);
                        }

                        break;
                    }     
            }

        }
    }
    void ToChangePageIteration()
    {
        PageObject used_page = availablePageObjects[availablePageObjects.Count - 1];
        availablePageObjects.RemoveAt(availablePageObjects.Count - 1);

        switch (pairToPairInfo.direction)
        {
            case PageManager.PairToPairInfo.DIRECTION.GOING_LEFT:
                {

                    if (pairsRemaining[pairsRemaining.Count - 1] != futurePair)
                        pairsRemaining[pairsRemaining.Count - 1].PassingBy();

                    FirstPageContentSlot.AttachContent(pairsRemaining[pairsRemaining.Count - 1].LeftPage);
                    used_page.TurnPageRight(pageCompletedTurn, pairsRemaining[pairsRemaining.Count - 1].RightPage, currentPair.LeftPage);

                    currentPair = pairsRemaining[pairsRemaining.Count - 1];
                    pairsRemaining.RemoveAt(pairsRemaining.Count - 1);
                    break;
                }
            case PageManager.PairToPairInfo.DIRECTION.GOING_RIGHT:
                {
                    if (pairsRemaining[pairsRemaining.Count - 1] != futurePair)
                        pairsRemaining[pairsRemaining.Count - 1].PassingBy();

                    LastPageContentSlot.AttachContent(pairsRemaining[pairsRemaining.Count - 1].RightPage);
                    used_page.TurnPageLeft(pageCompletedTurn, currentPair.RightPage, pairsRemaining[pairsRemaining.Count - 1].LeftPage);
                    currentPair = pairsRemaining[pairsRemaining.Count - 1];


                    pairsRemaining.RemoveAt(pairsRemaining.Count - 1);

                    break;
                }
        }
    }

    void pageCompletedTurn(PageObject sender)
    {
        availablePageObjects.Add(sender);

        if (pairsRemaining.Count == 0 && availablePageObjects.Count == pooledPageObjectCount) // completed transition
        {
            GM_.Instance.update_events.UpdateEvent -= multiplePageIterator.Update;
            pagePairFinalize();
        }
    }
    void pagePairFinalize()
    {
        for (int i = 0; i < pairToPairInfo.basePagePairs.Count; i++)
        {
            if (pairToPairInfo.basePagePairs[i] != futurePair)
            {
                pairToPairInfo.basePagePairs[i].gameObject.SetActive(false);
            }

        }

        if (startingPair != null)
        {
            startingPair.FinishedExitingPage();

            if (startingPair != futurePair)
                startingPair.gameObject.SetActive(false);
        }

        if (currentPair != futurePair)
        {
            Debug.LogError("Error! Something went wrong, the Finalizing page but the currentPare is not the same as the future pair!");
        }

        if (currentPair != null)
        {
            currentPair.EventRequest_ChangePage += requestedToChangePage;
            currentPair.EventRequest_GoToPreviousPage += requestedGoBackToPreviousPage;
            currentPair.FinishedEnteringPage();
        }
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
                    requestedToChangePage(new BasePagePair.RequestToChangePage(pausePage));
                    GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
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
                    requestedToChangePage(new BasePagePair.RequestToChangePage(null));

                    GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                 
                    isShowing = false;
                }
            }
        }
    }


    void completedHideJournal()
    {
        FirstPageContentSlot.DetachContent();
        LastPageContentSlot.DetachContent();

        pagePairFinalize();

        GM_.Instance.ui.gameObject.SetActive(true);
        GM_.Instance.pause.UnPause();
    }


}
