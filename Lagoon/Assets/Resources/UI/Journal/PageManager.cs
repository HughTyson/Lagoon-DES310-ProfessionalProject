using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{

    public enum PAGE
    { 
    BOOK_LEFT,
    PAGE_LEFT,
    PAGE_RIGHT,
    BOOK_RIGHT    
    }


    [SerializeField] Transform bookLeftPage;
    [SerializeField] Transform pageLeftPage;
    [SerializeField] Transform pageRightPage;
    [SerializeField] Transform bookRightPage;


    BasePagePair current_pair = null;
    BasePagePair new_pair = null;

    bool isDirty = false;

    PagePositionerManager pagePositionerManager;


    // Start is called before the first frame update
    void Start()
    {
        pagePositionerManager = new PagePositionerManager();
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
            new_pair.SetupPageInfo();
            new_pair.transform.position = transform.position + new Vector3(2*i, 0, 0);
            new_pair.transform.rotation = no_rotation;
            new_pair.gameObject.SetActive(false);
        }

        pagePositionerManager.AddPagePosition(PAGE.BOOK_LEFT, bookLeftPage);
        pagePositionerManager.AddPagePosition(PAGE.PAGE_LEFT, pageLeftPage);
        pagePositionerManager.AddPagePosition(PAGE.PAGE_RIGHT, pageRightPage);
        pagePositionerManager.AddPagePosition(PAGE.BOOK_RIGHT, bookRightPage);
    }

    public event System.Action<BasePagePair.RequestToChangePage> EventRequest_CurrentPage_WantsToChange;
    public event System.Action EventRequest_CurrentPage_WantsToCloseJournal;

    public void PrepareNewPair(BasePagePair new_pair_)
    {
        isDirty = true;
        new_pair = new_pair_;

        if (current_pair != null)
        {
            current_pair.EventRequest_ChangePage -= EventRequest_CurrentPage_WantsToChange;
            current_pair.EventRequest_CloseJournal -= EventRequest_CurrentPage_WantsToCloseJournal;
        }

        if (new_pair != null)
            new_pair.gameObject.SetActive(true);
        if (current_pair != null)
            current_pair.BegunExitingPage();
        if (new_pair != null)
            new_pair.BegunEnteringPage();
    }
    public void ApplyNewPair()
    {
        isDirty = false;
        if (current_pair != null)
        {
            current_pair.FinishedExitingPage();
        }

        setPageBasePair(new_pair);
        if (current_pair != null)
        {
            current_pair.EventRequest_ChangePage += EventRequest_CurrentPage_WantsToChange;
            current_pair.EventRequest_CloseJournal += EventRequest_CurrentPage_WantsToCloseJournal;
            current_pair.FinishedEnteringPage();
        }

        new_pair = null;

    }

    
    void setPageBasePair(BasePagePair pair)
    {
        if (isDirty)
        {
            Debug.LogError("Error, page manager attempt to change pages but pages where transitioning!");
            Debug.Break();
        }
        isDirty = false;
        if (current_pair != null)
            current_pair.gameObject.SetActive(false);

        current_pair = pair;
        pagePositionerManager.DetachAllPages();

        if (current_pair != null)
        {
            current_pair.gameObject.SetActive(true);

            pagePositionerManager.AttachPage(PAGE.BOOK_LEFT, current_pair.LeftPage);
            pagePositionerManager.AttachPage(PAGE.BOOK_RIGHT, current_pair.RightPage);
        }
    }

    public int CompareCurrentPairToPair(BasePagePair pair)
    {
        if (current_pair == null || pair == null)
            return 0;
        else
            return pair.transform.GetSiblingIndex() - current_pair.transform.GetSiblingIndex();
    }

    public void HidePages()
    {
        if (isDirty)
        {
            Debug.LogError("Error, page manager attempt to hide pages but pages where transitioning!");
            Debug.Break();
        }

        if (current_pair != null)
        {
            current_pair.gameObject.SetActive(false);
            current_pair = null;
        }
        pagePositionerManager.DetachAllPages();
    }


    public void SwapAttachedPages(PAGE from, PAGE to )
    {
        isDirty = true;
        pagePositionerManager.SwapAttachedPages(to, from);
    }
    public void HidePage(PAGE page)
    {
        isDirty = true;
        pagePositionerManager.HidePage(page);
    }
    public void ShowPage(PAGE page)
    {
        isDirty = true;
        pagePositionerManager.ShowPage(page);

    }
    public void SetPage(PAGE page, PageContent pageInfo)
    {
        isDirty = true;
        pagePositionerManager.AttachPage(page, pageInfo);
    }

    public void Update()
    {
        pagePositionerManager.Update();
    }
}
