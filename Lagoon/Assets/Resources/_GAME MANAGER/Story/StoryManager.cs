using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;
public class StoryManager
{

    [SerializeField] ConvoGraph convoGraph;

    BaseNodeType current_node;


    public event System.Action<BarrierStartArgs> Event_BarrierStart;                        // 
    public event System.Action Event_BarrierOpened;                       //
                                                                          //
    public event System.Action Event_ConvoEnter;                          //
    public event System.Action Event_ConvoExit;                           //

    public event System.Action<ConvoCharactersShowArgs> Event_ConvoCharactersShow;
    public event System.Action<DialogEnterArgs> Event_DialogStart;                         //
    public event System.Action<DialogNewTextArgs> Event_DialogNewText;                       //                                                                          //
    public event System.Action<ConvoCharacterChangeArgs> Event_ConvoCharacterChange;                //
                                                                          //
    public event System.Action<BranchEnterArgs> Event_BranchStart;                         //
    public event System.Action Event_BranchChoiceMade;                    //

    public class BarrierStartArgs
    {      
        public IReadOnlyList<BarrierNode.BARRIER_STATE> Barriers {get {return barriers;} }
        List<BarrierNode.BARRIER_STATE> barriers;
        public BarrierStartArgs(List<BarrierNode.BARRIER_STATE> barriers_)
        {
            barriers = barriers_;
        }
    }


    public class DialogEnterArgs
    {
        public readonly DialogStruct dialogVars;
        public DialogEnterArgs(DialogStruct dialogVars_)
        {
            dialogVars = dialogVars_;
        }
    }
    public class DialogNewTextArgs
    {
        public readonly DialogStruct dialogVars;
        public DialogNewTextArgs(DialogStruct dialogVars_)
        {
            dialogVars = dialogVars_;
        }
    }
    public class ConvoCharacterChangeArgs
    {
        public enum WHO { LEFT, RIGHT};
        public readonly WHO who;
        public readonly  ConversationCharacter character;
        public ConvoCharacterChangeArgs(ConversationCharacter character_, WHO who_)
        {
            character = character_;
            who = who_;
        }
    }
    public class ConvoCharactersShowArgs
    {
        public readonly ConversationCharacter leftCharacter;
        public readonly ConversationCharacter rightCharacter;
        public ConvoCharactersShowArgs(ConversationCharacter leftCharacter_, ConversationCharacter rightCharacter_ )
        {
            leftCharacter = leftCharacter_;
            rightCharacter = rightCharacter_;
        }

    }

    public class BranchEnterArgs
    {
        public readonly string leftChoice;
        public readonly string rightChoice;
        public BranchEnterArgs(string leftChoice_, string rightChoice_)
        {
            leftChoice = leftChoice_;
            rightChoice = rightChoice_;
        }
    }


    enum STATES
    {
        BARRIER_NODE,
        WAITING_FOR_CONVO,
        DIALOG_NODE,
        BRANCH_NODE,
        EVENT_NODE,
    }
    public StoryManager(BaseNodeType current_node_)
    {
        current_node = current_node_;
    
    }

    // called after other objects had a chance to setup actions
    public void Begin()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += BarrierObjectivesComplete;
        Event_BarrierOpened += BarrierOpened;

        Event_BarrierStart?.Invoke(new BarrierStartArgs(((RootNode)current_node).barriers));
    }


    public bool RequestConversationEnter()
    {
        if (barrierIsOpen)
        {
            barrierIsOpen = false;

            Event_ConvoEnter?.Invoke();
            BaseNodeType.NODE_TYPE node_type = current_node.GetNodeType();

            switch (node_type)
            {
                case BaseNodeType.NODE_TYPE.DIALOG:
                    {
                        DialogNode node = (DialogNode)current_node;
                        Event_ConvoCharactersShow?.Invoke(new ConvoCharactersShowArgs(node.leftCharacter, node.rightCharacter));
                        node.ResetDialogIndex();
                        Event_DialogStart?.Invoke(new DialogEnterArgs(node.GetCurrentDialog()));
                        break;
                    }
                case BaseNodeType.NODE_TYPE.BRANCH:
                    {
                        BranchingNode node = (BranchingNode)current_node;
                        Event_ConvoCharactersShow?.Invoke(new ConvoCharactersShowArgs(node.leftCharacter, node.rightCharacter));
                        Event_BranchStart?.Invoke(new BranchEnterArgs(node.LeftDecision, node.RightDecision));
                        break;
                    }
            }

            return true;
        }
        return false;
    }

    bool barrierIsOpen = false;
    void BarrierOpened()
    {
        barrierIsOpen = true;
    }

    void BarrierObjectivesComplete()
    {
        current_node = ((RootNode)current_node).NextNode();
        if (current_node.GetNodeType() != BaseNodeType.NODE_TYPE.BARRIER)
        {
            Event_BarrierOpened?.Invoke();
        }
        else
        {
            Event_BarrierStart?.Invoke(new BarrierStartArgs(((BarrierNode)current_node).barriers));
        }

    }


}



