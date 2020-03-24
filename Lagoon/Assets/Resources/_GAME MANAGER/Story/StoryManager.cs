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
                                                                          //
    public event System.Action<BranchEnterArgs> Event_BranchStart;                         //
    public event System.Action<BranchChoiceMadeArgs> Event_BranchChoiceMade;                    //

    public event System.Action Event_SkipTextCrawl;


    SpecialText.Parser specialTextParser;
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
        public readonly ConversationCharacter leftCharacter;
        public readonly ConversationCharacter rightCharacter;
        public readonly SpecialText.SpecialTextData specialTextData;
        public readonly DialogNode.DialogData.Talking whosTalking;
        public DialogEnterArgs(ConversationCharacter leftCharacter_, ConversationCharacter rightCharacter_ ,DialogNode.DialogData.Talking whosTalking_, SpecialText.SpecialTextData specialTextData_)
        {
            whosTalking = whosTalking_;
            leftCharacter = leftCharacter_;
            rightCharacter = rightCharacter_;
            specialTextData = specialTextData_;
        }
    }
    public class DialogNewTextArgs
    {
        public readonly DialogNode.DialogData.Talking whosTalking;
        public readonly SpecialText.SpecialTextData specialTextData;
        public DialogNewTextArgs(DialogNode.DialogData.Talking whosTalking_, SpecialText.SpecialTextData specialTextData_)
        {
            whosTalking = whosTalking_;
            specialTextData = specialTextData_;
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
        public readonly SpecialText.SpecialTextData leftChoice;
        public readonly SpecialText.SpecialTextData rightChoice;
        public BranchEnterArgs(SpecialText.SpecialTextData leftChoice_, SpecialText.SpecialTextData rightChoice_)
        {
            leftChoice = leftChoice_;
            rightChoice = rightChoice_;
        }
    }
    public class BranchChoiceMadeArgs
    {
        public readonly BranchingNode.CHOICE choice;
        public BranchChoiceMadeArgs(BranchingNode.CHOICE choice_)
        {
            choice = choice_;
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

        specialTextParser = new SpecialText.Parser(((ConvoGraph)current_node.graph).GlobalProperties);

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
                        DialogNode.DialogData dialogData = node.GetCurrentDialog();
                        Event_DialogStart?.Invoke(new DialogEnterArgs(node.leftCharacter,node.rightCharacter, dialogData.whoIsTalking, specialTextParser.ParseToSpecialTextData(dialogData.dialog_text)));
                        break;
                    }
            }

            return true;
        }
        return false;
    }

    public void RequestButtonPressA()
    {
        if (!GM_.Instance.ui.state_conversation.IsTransitioning())
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.DIALOG)
            {
                    if (((DialogNode)current_node).IsOnLastDialog())
                    {
                        current_node = ((DialogNode)current_node).NextNode();
                        EnteredNewNode();
                    }
                    else
                    {
                        DialogNode.DialogData dialog_data = ((DialogNode)current_node).IterateAndGetDialog();
                        DialogNewTextArgs args = new DialogNewTextArgs(dialog_data.whoIsTalking, specialTextParser.ParseToSpecialTextData(dialog_data.dialog_text));
                        Event_DialogNewText?.Invoke(args);
                    }
            }
        }
        else
        {
            Event_SkipTextCrawl?.Invoke();
        }
    }
    public void RequestButtonPressX()
    {
        if (!GM_.Instance.ui.state_conversation.IsTransitioning())
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.BRANCH)
            {
                Event_BranchChoiceMade?.Invoke(new BranchChoiceMadeArgs(BranchingNode.CHOICE.LEFT));
                current_node = ((BranchingNode)current_node).NextNode(BranchingNode.CHOICE.LEFT);
                EnteredNewNode();
            }
        }
    }
    public void RequestButtonPressB()
    {
        if (!GM_.Instance.ui.state_conversation.IsTransitioning())
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.BRANCH)
            {
                Event_BranchChoiceMade?.Invoke(new BranchChoiceMadeArgs(BranchingNode.CHOICE.RIGHT));
                current_node = ((BranchingNode)current_node).NextNode(BranchingNode.CHOICE.RIGHT);
                EnteredNewNode();
            }
        }
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


    void EnteredNewNode()
    {
        switch (current_node.GetNodeType())
        {
            case BaseNodeType.NODE_TYPE.DIALOG:
                {
                    DialogNode node = ((DialogNode)current_node);
                    node.ResetDialogIndex();
                    DialogNode.DialogData dialogData = node.GetCurrentDialog();
                    Event_DialogStart?.Invoke(new DialogEnterArgs(node.leftCharacter, node.rightCharacter, dialogData.whoIsTalking, specialTextParser.ParseToSpecialTextData(dialogData.dialog_text)));
                    break;
                }
            case BaseNodeType.NODE_TYPE.BRANCH:
                {
                    BranchingNode node = ((BranchingNode)current_node);
                    Event_BranchStart?.Invoke(new BranchEnterArgs(specialTextParser.ParseToSpecialTextData(node.LeftDecision), specialTextParser.ParseToSpecialTextData(node.RightDecision)));
                    break;
                }
            case BaseNodeType.NODE_TYPE.EVENT:
                {

                    break;
                }
            case BaseNodeType.NODE_TYPE.BARRIER:
                {
                    Event_ConvoExit?.Invoke();
                    BarrierNode node = ((BarrierNode)current_node);
                    Event_BarrierStart?.Invoke(new BarrierStartArgs(node.barriers));

                    break;
                }
        }
    }
}



