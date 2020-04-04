using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageContentSlot : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (attachedContent != null)
        {
            attachedContent.transform.position = transform.position;
            attachedContent.transform.rotation = transform.rotation;
        }
    }

    PageContent attachedContent = null;
    bool hidingContent = false;

    int forcingToHideCount = 0;

    public void AttachContent(PageContent content)
    {

        DetachContent();
        attachedContent = content;
    }

    public void DetachContent()
    {
        if (attachedContent != null)
        {
            attachedContent.transform.gameObject.SetActive(false);
            attachedContent.transform.position = attachedContent.UnactivePosition;
            attachedContent.transform.rotation = attachedContent.UnactiveRotation;
            attachedContent = null;
        }
    }

    public void ResetHideCount()
    {
        forcingToHideCount = 0;
    }
    public void HideContent()
    {
        forcingToHideCount++;
        if (attachedContent != null)
        {
            attachedContent.transform.gameObject.SetActive(false);
            hidingContent = true;
        }
    }
    public void ShowContent()
    {
        forcingToHideCount = Mathf.Max(0, forcingToHideCount - 1);

        if (forcingToHideCount == 0)
        {
            if (attachedContent != null)
            {
                attachedContent.transform.gameObject.SetActive(true);
                attachedContent.transform.position = transform.position;
                attachedContent.transform.rotation = transform.rotation;
                hidingContent = false;
            }
        }

    }
}
