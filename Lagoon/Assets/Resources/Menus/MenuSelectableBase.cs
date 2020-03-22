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

    // Allow derived classes to call event
    protected void Invoke_EventSelected()
    {
        Event_Selected?.Invoke();
    }

    public virtual void HoveredOver() 
    { 
    
    }
    public virtual void Selected()
    {

    }
}
