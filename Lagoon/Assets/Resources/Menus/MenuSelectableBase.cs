using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuSelectableBase : MonoBehaviour
{

    [SerializeField] protected MenuSelectableBase siblingUp;
    [SerializeField] protected MenuSelectableBase siblingDown;
    [SerializeField] protected MenuSelectableBase siblingLeft;
    [SerializeField] protected MenuSelectableBase siblingRight;

    protected MenuScreenBase parentMenu;

    public event System.Action Event_Selected;
    public event System.Action Event_CancelledWhileHovering;
    public event System.Action Event_FinishedHide;
    public event System.Action Event_FinishedShow;

    // Allow derived classes to call event
    protected void Invoke_EventSelected()
    {
        Event_Selected?.Invoke();
    }
    protected void Invoke_CancelledWhileHovering()
    {
        Event_CancelledWhileHovering?.Invoke();
    }
    public virtual void HoveredOver() 
    { 
    
    }
    public virtual void Selected()
    {

    }
    public virtual void Hide()
    {

    }
    public virtual void Show()
    {

    }
    public virtual void InstantHide()
    {

    }
    public virtual void InstantShow()
    {

    }
}
