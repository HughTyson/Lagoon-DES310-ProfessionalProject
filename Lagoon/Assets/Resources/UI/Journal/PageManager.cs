using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{





    //public enum BOOK_PAGE
    //{ 
    //    BOOK_LEFT,
    //    BOOK_RIGHT    
    //}


    //[SerializeField] Transform bookLeftPage;
    //[SerializeField] Transform pageLeftPage;
    //[SerializeField] Transform pageRightPage;
    //[SerializeField] Transform bookRightPage;


    //BasePagePair current_pair = null;

    //bool isDirty = false;

    //PagePositionerManager pagePositionerManager;


    List<BasePagePair> AllPairs = new List<BasePagePair>();

    // Start is called before the first frame update
    void Start()
    {
        Quaternion no_rotation = Quaternion.Euler(0, 0, 0);
        // Setup distance between the pairs
        for (int i = 0; i < transform.childCount; i++)
        {
            BasePagePair new_pair = transform.GetChild(i).GetComponent<BasePagePair>();
            if (new_pair == null)
            {
                Debug.LogError("Error, children of PageManager Object must have BasePagePairs!");
                Debug.Break();
            }
            new_pair.transform.position = transform.position + new Vector3(2 * i, 0, 0);
            new_pair.transform.rotation = no_rotation;
            new_pair.gameObject.SetActive(false);
            new_pair.SetupPageInfo();
            AllPairs.Add(new_pair);
        }        
    }


    public class PairToPairInfo
    {
        public enum DIRECTION
        { 
        GOING_RIGHT,
        GOING_LEFT,
        ON_TARGET
        }
        public DIRECTION direction;
        public List<BasePagePair> basePagePairs = new List<BasePagePair>(); // note. pairs are stored from closest to use at the end of the list
    
    }

    public PairToPairInfo GetInfoFromPairToPair(BasePagePair from_pair, BasePagePair to_pair)
    {
        PairToPairInfo pairToPair = new PairToPairInfo();

        if (from_pair == null || to_pair == null)
        {
            pairToPair.direction = PairToPairInfo.DIRECTION.ON_TARGET;
            return pairToPair;
        }


        int from_pair_index = -1;
        int to_pair_index = -1;
        for (int i = 0; i < AllPairs.Count; i++)
        {
            if (AllPairs[i] == from_pair)
                from_pair_index = i;

            if (AllPairs[i] == to_pair)
                to_pair_index = i;

            if (from_pair_index != -1 && to_pair_index != -1)
                break;
        }


        if (from_pair_index < to_pair_index)
        {
            pairToPair.direction = PairToPairInfo.DIRECTION.GOING_RIGHT;
            for (int i = to_pair_index; i > from_pair_index; i--)
                pairToPair.basePagePairs.Add(AllPairs[i]);

        }
        else if (from_pair_index > to_pair_index)
        {
            pairToPair.direction = PairToPairInfo.DIRECTION.GOING_LEFT;
            for (int i = to_pair_index; i < from_pair_index; i++)
                pairToPair.basePagePairs.Add(AllPairs[i]);
        }
        else
            pairToPair.direction = PairToPairInfo.DIRECTION.ON_TARGET;

        return pairToPair;

    }
    

    //public event System.Action<BasePagePair.RequestToChangePage> EventRequest_CurrentPage_WantsToChange;
    //public event System.Action EventRequest_CurrentPage_WantsToCloseJournal;



    //public void Init(List<PageObject> pageObjects)
    //{
    //    for (int i = 0; i < pageObjects.Count; i++)
    //    {
    //        pagePositionerManager.AddPagePosition(pageObjects[i].LeftSide, pageObjects[i].LeftSide);
    //        pagePositionerManager.AddPagePosition(pageObjects[i].RightSide, pageObjects[i].RightSide);
    //    }
    //}


    //struct UnappliedPage
    //{
    //    public UnappliedPage(BasePagePair pagePair_, PageObject pageObjectSide_)
    //    {
    //        pagePair = pagePair_;
    //        pageObject = pageObjectSide_;
    //    }

    //    public BasePagePair pagePair;
    //    public PageObject pageObject;

    //}

    //BasePagePair finalPagePair = null;
    //List<UnappliedPage> unappliedPages = new List<UnappliedPage>();


    //int pageChanging_PagesLeft  = 0;
    //public void StartPageChangingMode(BasePagePair finalPagePair_)
    //{
    //    if (isDirty)
    //    {
    //        Debug.LogError("Error! tried to start page changing mode while already in that mode");
    //        Debug.Break();
    //    }
    //    finalPagePair = finalPagePair_;
    //    pageChanging_PagesLeft = CompareCurrentPairToPair(finalPagePair);
    //    isDirty = true;



    //    if (current_pair != null)
    //    {
    //        current_pair.EventRequest_ChangePage -= EventRequest_CurrentPage_WantsToChange;
    //        current_pair.EventRequest_CloseJournal -= EventRequest_CurrentPage_WantsToCloseJournal;
    //    }

    //    if (current_pair != null)
    //        current_pair.BegunExitingPage();

    //}

    //BasePagePair new_pair = null;
    //public void PrepareNewPair(PageObject pageObjectReference)
    //{
    //    if (pageChanging_PagesLeft != 0)
    //    {
    //        new_pair = AllPairs[finalPagePair.transform.GetSiblingIndex() + pageChanging_PagesLeft];

    //        unappliedPages.Add(new UnappliedPage(new_pair, pageObjectReference));

    //        new_pair.gameObject.SetActive(true);
    //        new_pair.BegunEnteringPage();
    //        isDirty = true;

    //    }
    //    else
    //    {
    //        Debug.LogError("Error, PerpareNewPair was called, but the iterated pair is the final pair!");
    //    }


    //    if (pageChanging_PagesLeft < 0)
    //    {
    //        pageChanging_PagesLeft++;
    //    }
    //    else if (pageChanging_PagesLeft > 0)
    //    {
    //        pageChanging_PagesLeft--;
    //    }




    //}
    //public void ApplyNewPair()
    //{
    //    unappliedPages[0].pagePair.FinishedExitingPage();
    //    unappliedPages.RemoveAt(0);
    //}

    //public void FinalizeNewPair()
    //{
    //    if (unappliedPages.Count > 0)
    //    {
    //        Debug.LogError("Error! FinalizeNewPair occurred, but there are still unapplied pages!");
    //        Debug.Break();
    //    }
    //    isDirty = false;
    //    if (current_pair != null)
    //    {
    //        current_pair.FinishedExitingPage();
    //    }

    //    setPageBasePair(finalPagePair);
    //    if (current_pair != null)
    //    {
    //        current_pair.EventRequest_ChangePage += EventRequest_CurrentPage_WantsToChange;
    //        current_pair.EventRequest_CloseJournal += EventRequest_CurrentPage_WantsToCloseJournal;
    //        current_pair.FinishedEnteringPage();
    //    }

    //    finalPagePair = null;
    //}


    //void setPageBasePair(BasePagePair pair)
    //{
    //    if (isDirty)
    //    {
    //        Debug.LogError("Error, page manager attempt to change pages but pages where transitioning!");
    //        Debug.Break();
    //    }
    //    isDirty = false;
    //    if (current_pair != null)
    //        current_pair.gameObject.SetActive(false);

    //    current_pair = pair;
    //    pagePositionerManager.DetachAllPages();

    //    if (current_pair != null)
    //    {
    //        current_pair.gameObject.SetActive(true);

    //        pagePositionerManager.AttachPage(BOOK_PAGE.BOOK_LEFT, current_pair.LeftPage);
    //        pagePositionerManager.AttachPage(BOOK_PAGE.BOOK_RIGHT, current_pair.RightPage);
    //    }
    //}

    //public int CompareCurrentPairToPair(BasePagePair pair)
    //{
    //    if (current_pair == null || pair == null)
    //        return 0;
    //    else
    //        return pair.transform.GetSiblingIndex() - current_pair.transform.GetSiblingIndex();
    //}

    //public void HidePages()
    //{
    //    if (isDirty)
    //    {
    //        Debug.LogError("Error, page manager attempt to hide pages but pages where transitioning!");
    //        Debug.Break();
    //    }

    //    if (current_pair != null)
    //    {
    //        current_pair.gameObject.SetActive(false);
    //        current_pair = null;
    //    }
    //    pagePositionerManager.DetachAllPages();
    //}

    //public void SwapAttachedPages(object from, object to )
    //{
    //    isDirty = true;        
    //    pagePositionerManager.SwapAttachedPages(to, from);
    //}
    //public void HidePage(object page)
    //{
    //    isDirty = true;
    //    pagePositionerManager.HidePage(page);
    //}
    //public void ShowPage(object page)
    //{
    //    isDirty = true;
    //    pagePositionerManager.ShowPage(page);

    //}
    //public void SetPage(object page, PageContent pageInfo)
    //{
    //    isDirty = true;
    //    pagePositionerManager.AttachPage(page, pageInfo);
    //}
    //public void Update()
    //{
    //    pagePositionerManager.Update();
    //}
}
