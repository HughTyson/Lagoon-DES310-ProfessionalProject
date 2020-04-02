using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePositionerManager
{




    class PagePositioner
    {
         Transform pageTransform;
        Content content = new Content();

        bool hasContent = false;
        class Content
        {
            public PageContent page_ref;
            public Transform pageContenttransfrom;
            public Vector3 unactivePageContentPosition;
            public Quaternion unactivePageContentRotation;
        }
        
        public PageContent ContentPage
        { 
            get
            {
                if (hasContent)
                {
                    return content.page_ref;
                }
                return null;
            }
        
        }

        public Vector3 PagePosition => pageTransform.position;
        public Quaternion PageRotation => pageTransform.rotation;
        public bool HasContent => hasContent;
        public PagePositioner(Transform pageTransform_)
        {
            pageTransform = pageTransform_;
        }


        public void SetContent(PageContent page)
        {

            if (page != null)
            {
                hasContent = true;
                content.page_ref = page;
                content.pageContenttransfrom = page.transform;
                content.unactivePageContentPosition = page.UnactivePosition;
                content.unactivePageContentRotation = page.UnactiveRotation;
            }
            else
            {
                hasContent = false;
            }

        }
        public void ClearContent()
        {
            
            if (hasContent)
            {
                content.pageContenttransfrom.position = content.unactivePageContentPosition;
                content.pageContenttransfrom.rotation = content.unactivePageContentRotation;
                hasContent = false;
            }

        }

        bool isHidden = false;
        public void HideContent()
        {
            isHidden = true;
            if (hasContent)
            {
                content.page_ref.gameObject.SetActive(false);
            }
        }
        public void ShowContent()
        {
            isHidden = false;
            if (hasContent)
            {
                content.page_ref.gameObject.SetActive(true);
            }
        }
        public void Update()
        { 
          if (hasContent && !isHidden)
            {
                content.pageContenttransfrom.position = PagePosition;
                content.pageContenttransfrom.rotation = PageRotation;
            }
        }

    }


    Dictionary<object, PagePositioner> pagePositions = new Dictionary<object, PagePositioner>();

    public void AddPagePosition(object key, Transform pageTransf)
    {
        pagePositions.Add(key, new PagePositioner(pageTransf));
    }



    public void AttachPage(object key, PageContent page)
    {
        pagePositions[key].SetContent(page);
    }
    public void DetachPage(object key)
    {
        pagePositions[key].ClearContent();
    }
    public void HidePage(object key)
    {
        pagePositions[key].HideContent();
    }
    public void ShowPage(object key)
    {
        pagePositions[key].ShowContent();
    }

    public void DetachAllPages()
    {
        foreach (KeyValuePair<object, PagePositioner> entry in pagePositions)
        {
            entry.Value.ClearContent();
        }
    }
    public void SwapAttachedPages(object from, object to)
    {
        PageContent temp_page = pagePositions[to].ContentPage;
        pagePositions[to].SetContent(pagePositions[from].ContentPage);
        pagePositions[from].SetContent(temp_page);

    }
    public void Update()
    {
        foreach (KeyValuePair<object, PagePositioner> entry in pagePositions)
        {
            entry.Value.Update();
        }
    }
}
