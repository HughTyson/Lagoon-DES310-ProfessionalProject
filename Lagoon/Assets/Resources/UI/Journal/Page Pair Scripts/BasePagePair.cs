using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePagePair : MonoBehaviour
{

    public class RequestToChangePage
    {
        public readonly BasePagePair changeToPagePair;
        public RequestToChangePage(BasePagePair changeToPagePair_)
        {
            changeToPagePair = changeToPagePair_;
        }
    }
    //public class PageInfo
    //{
    //    Transform transform;
    //    public PageInfo(Transform transform_)
    //    {
    //        transform = transform_;
    //    }

    //    public Vector3 DefaultPosition => transform.position;
    //    public Quaternion DefaultRotation => transform.rotation;
    //}

    PageContent leftPage;
    PageContent rightPage;

    public PageContent LeftPage => leftPage;
    public PageContent RightPage => rightPage;

    public void SetupPageInfo()
    {
        // order from GetChild isn't determenistic
        if (transform.GetChild(0).name == "Left Page Canvas")
        {
            leftPage = transform.GetChild(0).gameObject.AddComponent<PageContent>();
            rightPage = transform.GetChild(1).gameObject.AddComponent<PageContent>();
        }
        else
        {
            leftPage = transform.GetChild(1).gameObject.AddComponent<PageContent>();
            rightPage = transform.GetChild(0).gameObject.AddComponent<PageContent>();
        }


        if (transform.childCount != 2)
        {
            Debug.LogError("Error. PagePair Has to have 2 children!");
            Debug.Break();
        }
    }

    public class InfoRequest_CameraFromPage_Args
    {
        public System.Type pageType;
    }



    public event System.Action<RequestToChangePage> EventRequest_ChangePage;
    public event System.Action EventRequest_GoToPreviousPage;
    public event System.Action<InfoRequest_CameraFromPage_Args> InfoRequest_CameFromPage;
    protected void Invoke_EventRequest_ChangePage(RequestToChangePage args)
    {
        EventRequest_ChangePage?.Invoke(args);
    }

    
    protected void Invoke_EventRequest_CloseJournal()
    {
        EventRequest_ChangePage?.Invoke(new RequestToChangePage(null));
    }
    protected InfoRequest_CameraFromPage_Args Invoke_InfoRequest_CameFromPage()
    {
        InfoRequest_CameraFromPage_Args args = new InfoRequest_CameraFromPage_Args();
        InfoRequest_CameFromPage?.Invoke(args);
        return args;
    }

    public virtual void BegunEnteringPage()
    {

    }
    public virtual void FinishedEnteringPage()
    {

    }


    public virtual void BegunExitingPage()
    {

    }
 
    public virtual void FinishedExitingPage()
    {

    }


    // Called when page is passed by when scimming through pages. It is visible, but not stopped at.
    public virtual void PassingBy()
    {

    }



}
