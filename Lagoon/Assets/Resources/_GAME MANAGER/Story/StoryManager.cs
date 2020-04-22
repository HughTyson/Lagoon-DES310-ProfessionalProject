using System.Collections.Generic;
public class StoryManager
{

     ConvoGraph convoGraph;

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



    // -- GameEvent Events -- //
    public event System.Action<GameEventTriggeredArgs> Event_GameEventStart; // Called upon entering an event node. Holds info about the event that is to be triggered
    public event System.Action<EventRequestArgs> EventRequest_GameEventContinue; // Called when the StoryManager has been requested to continue and end the game event. Alls subscibers are able to block the request, as they are not finished
    public event System.Action Event_GameEventEnd; // Called after a successfull EventRequest of the GameEventContinue
                                                   // -- //
    
    public event System.Action<EventRequestArgs> EventRequest_BarrierStart;


    public event System.Action<ButtonPressRequestArgs> EventRequest_BlockingButtonPress;


    SpecialText.Parser specialTextParser;


    public class ButtonPressRequestArgs
    {
        bool isBlocked = false;
        InputManager.BUTTON button;
        public InputManager.BUTTON RequestedButton => button;
        public bool IsBlocked => isBlocked;
        public void Block()
        {
            isBlocked = true;
        }
        public ButtonPressRequestArgs(InputManager.BUTTON requestedButton)
        {
            button = requestedButton;
        }

    }
    public class EventRequestArgs
    {
        bool isBlocked = false;
        public bool IsBlocked => isBlocked;
        public void Block()
        {
            isBlocked = true;
        }
    }

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

    public class GameEventTriggeredArgs
    {
        public readonly EventNode.EVENT_TYPE event_type;
        public readonly List<AdditionalInfoNode_CertainItemsInNextSupplyDrop.ItemData> certainItemDrops = new List<AdditionalInfoNode_CertainItemsInNextSupplyDrop.ItemData>();

        public GameEventTriggeredArgs(EventNode.EVENT_TYPE event_type_, List<BaseAdditionalInfoNode.AdditionalInfo> usedAdditionalInfoOfEvents)
        {
            event_type = event_type_;

            for (int i = 0; i < usedAdditionalInfoOfEvents.Count; i++)
            {
                if (typeof(AdditionalInfoNode_CertainItemsInNextSupplyDrop.CertainNextSupplyDropItems) == usedAdditionalInfoOfEvents[i].GetType())
                {
                    certainItemDrops.AddRange(((AdditionalInfoNode_CertainItemsInNextSupplyDrop.CertainNextSupplyDropItems)usedAdditionalInfoOfEvents[i]).items);
                }
            }

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
    public StoryManager(BaseNodeType current_node_,ConvoGraph convoGraph_ )
    {
        convoGraph = convoGraph_;
        current_node = current_node_;
        
    }


    // called after other objects had a chance to setup actions
    public void Init()
    {
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += BarrierObjectivesComplete;
        Event_BarrierOpened += BarrierOpened;

       

        specialTextParser = new SpecialText.Parser(((ConvoGraph)current_node.graph).GlobalProperties);

    }
    public void StartOrReset(float delay = 5.0f)
    {
        startDelay = delay;
        GM_.Instance.update_events.LateUpdateEvent += resetDelay;


    }
    float startDelay = 0;
    void resetDelay()
    {
        startDelay -= UnityEngine.Time.unscaledDeltaTime;
        if (startDelay <= 0)
        {
            GM_.Instance.update_events.LateUpdateEvent -= resetDelay;
            current_node = convoGraph.Root;
            Event_BarrierStart?.Invoke(new BarrierStartArgs(((RootNode)current_node).barriers));
        }
        
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
        ButtonPressRequestArgs requestArgs = new ButtonPressRequestArgs(InputManager.BUTTON.A);
        EventRequest_BlockingButtonPress?.Invoke(requestArgs);

        if (!requestArgs.IsBlocked)
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.DIALOG)
            {
                    GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
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
            else if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.EVENT)
            {
                RequestGameEventContinue();
            }
        }
    }
    public void RequestButtonPressX()
    {
        ButtonPressRequestArgs requestArgs = new ButtonPressRequestArgs(InputManager.BUTTON.X);
        EventRequest_BlockingButtonPress?.Invoke(requestArgs);

        if (!requestArgs.IsBlocked)
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.BRANCH)
            {
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                Event_BranchChoiceMade?.Invoke(new BranchChoiceMadeArgs(BranchingNode.CHOICE.LEFT));
                ((BranchingNode)current_node).SetChoice(BranchingNode.CHOICE.LEFT);
                current_node = ((BranchingNode)current_node).NextNode();
                EnteredNewNode();
            }
        }
    }
    public void RequestButtonPressB()
    {
        ButtonPressRequestArgs requestArgs = new ButtonPressRequestArgs(InputManager.BUTTON.B);
        EventRequest_BlockingButtonPress?.Invoke(requestArgs);

        if (!requestArgs.IsBlocked)
        {
            if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.BRANCH)
            {
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                Event_BranchChoiceMade?.Invoke(new BranchChoiceMadeArgs(BranchingNode.CHOICE.RIGHT));
                ((BranchingNode)current_node).SetChoice(BranchingNode.CHOICE.RIGHT);
               current_node = ((BranchingNode)current_node).NextNode();
                EnteredNewNode();
            }
        }
    }
    public void RequestGameEventContinue()
    {
        if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.EVENT)
        {
            EventRequestArgs requestArgs = new EventRequestArgs();
            EventRequest_GameEventContinue?.Invoke(requestArgs);

            if (!requestArgs.IsBlocked)
            {
                Event_GameEventEnd?.Invoke();
                current_node = ((EventNode)current_node).NextNode();
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
            barrierIsOpen = false;
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
                    EventNode node = ((EventNode)current_node);

                    Event_GameEventStart?.Invoke(new GameEventTriggeredArgs(node.event_occured, node.TakeUsedAdditionalInfo()));
                    break;
                }
            case BaseNodeType.NODE_TYPE.BARRIER:
                {               
                    Event_ConvoExit?.Invoke();
                    BarrierNode node = ((BarrierNode)current_node);

                    RequestBarrierStart();                  
                    break;
                }
        }
    }


    public void RequestBarrierStart()
    {
        if (current_node.GetNodeType() == BaseNodeType.NODE_TYPE.BARRIER)
        {
            EventRequestArgs args = new EventRequestArgs();
            EventRequest_BarrierStart?.Invoke(args);

            if (!args.IsBlocked)
            {
                barrierIsOpen = false;
                Event_BarrierStart?.Invoke(new BarrierStartArgs(((BarrierNode)current_node).barriers));
            }
        }
    }
}



