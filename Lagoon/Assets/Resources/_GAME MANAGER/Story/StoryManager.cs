using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;
public class StoryManager
{

    [SerializeField] ConvoGraph convoGraph;

    public BaseNodeType current_node;
    public enum ACTION
    { 
        BARRIER_START,
        BARRIER_EXIT,
        BARRIER_OPENED,
        BARRIER_OPEN_UPDATE,
        BARRIER_BLOCKING_UPDATE,
        DIALOG_STARTED,
        BRANCH_STARTED,
        BRANCH_CHOICE_CHANGE,
        BRANCH_SELECT,
        EVENT_STARTED,

        DIALOG_UPDATE,
        BRANCH_UPDATE,
        EVENT_UPDATE    
    }


    public ActionHandler<ACTION> actionHandler = new ActionHandler<ACTION>();

    enum STATES
    {
        BARRIER_NODE,
        DIALOG_NODE,
        BRANCH_NODE,
        EVENT_NODE,
    }
    public StoryManager(BaseNodeType current_node_)
    {
        current_node = current_node_;

        if (current_node.GetNodeType() != BaseNodeType.NODE_TYPE.BARRIER)
        {
            Debug.LogError("Error, ConvoGraph Node after 'Root Node' has to be 'Barrier Node'!");
            Debug.Break();
        }

        actionHandler.AddAction(ACTION.BARRIER_START, Barrier_Started);
        actionHandler.AddAction(ACTION.BARRIER_OPENED, Barrier_Opened);
    }

    // called after other objects had a chance to setup actions
    public void Begin()
    {
        actionHandler.InvokeActions(ACTION.BARRIER_START);
    }

    public void Update()
    {
        switch (current_node.GetNodeType())
        {
            case BaseNodeType.NODE_TYPE.BARRIER:
                {
                    if (barrierOpen)
                    {
                        actionHandler.InvokeActions(ACTION.BARRIER_OPEN_UPDATE);
                    }
                    else
                    {
                        actionHandler.InvokeActions(ACTION.BARRIER_BLOCKING_UPDATE);
                    }

                    break;
                }
            case BaseNodeType.NODE_TYPE.BRANCH:
                {
                    actionHandler.InvokeActions(ACTION.BRANCH_UPDATE);
                    break;
                }
            case BaseNodeType.NODE_TYPE.DIALOG:
                {
                    actionHandler.InvokeActions(ACTION.DIALOG_UPDATE);
                    break;
                }
            case BaseNodeType.NODE_TYPE.EVENT:
                {
                    actionHandler.InvokeActions(ACTION.EVENT_UPDATE);
                    break;
                }        
        }
    }


    bool barrierOpen = false;
    void Barrier_Started()
    {
        barrierOpen = false;
    }
    void Barrier_Opened()
    {
        barrierOpen = true;
    }

}



