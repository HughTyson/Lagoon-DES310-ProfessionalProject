using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConvoUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Sprite sprLeftCharacter;
    [SerializeField] Sprite sprRightCharacter;
    [SerializeField] Sprite sprLeftOption;
    [SerializeField] Sprite sprRightOption;
    [SerializeField] Sprite sprDialog;
    [SerializeField] Text txtDialog;
    [SerializeField] Text txtLeftOption;
    [SerializeField] Text txtRightOption;


    struct AnimHash
    {
        public int Default;
        public int startupDialog;
        public int startupBranch;
    }

    ConversationCharacter characterLeft;
    ConversationCharacter characterRight;

    DialogStruct.Talking whosTalking;
    enum STATE
    { 
    NOT_ACTIVE,
    ANIMATING,
    FINISHED // used multiple times to other classes that a transition has finished
    }

    STATE current_state;

    AnimHash animHash = new AnimHash();
    private void Start()
    {
        animHash.Default = Animator.StringToHash("Default");
        animHash.startupDialog = Animator.StringToHash("Startup_Dialog");
        animHash.startupBranch = Animator.StringToHash("Startup_Branch");
    }

    public void StartupAnimation(BaseNodeType node)
    {


        switch (node.GetNodeType())
        {
            case BaseNodeType.NODE_TYPE.BRANCH:
                {
                    characterLeft = ((BranchingNode)node).leftCharacter;
                    characterRight = ((BranchingNode)node).rightCharacter;

                    txtLeftOption.text = ((BranchingNode)node).LeftDecision;
                    txtRightOption.text = ((BranchingNode)node).RightDecision;

                    sprLeftCharacter = characterLeft.characterIcon;
                    sprRightCharacter = characterRight.characterIcon;

                    current_state = STATE.ANIMATING;
                    GetComponent<Animator>().Play(animHash.startupBranch, 0);
                    break;
                }
            case BaseNodeType.NODE_TYPE.DIALOG:
                {
                    characterLeft = ((DialogNode)node).leftCharacter;
                    characterRight = ((DialogNode)node).rightCharacter;

                    txtDialog.text = ((DialogNode)node).Dialog[0].dialog;
                    whosTalking = ((DialogNode)node).Dialog[0].whoIsTalking;

                    sprLeftCharacter = characterLeft.characterIcon;
                    sprRightCharacter = characterRight.characterIcon;

                    current_state = STATE.ANIMATING;
                    GetComponent<Animator>().Play(animHash.startupDialog,0);
                    break;
                }
            default:
                {
                    Debug.LogError("Convo UI Manager was given an unknown node type for the start up animation");
                    break;
                }
        }

    }



    public bool IsAnimationFinished()
    {
        return current_state == STATE.FINISHED;
    }

    private void OnEnable()
    {
        current_state = STATE.NOT_ACTIVE;
        GetComponent<Animator>().Play(animHash.Default);
    }

    private void OnDisable()
    {
        current_state = STATE.NOT_ACTIVE;
        GetComponent<Animator>().Play(animHash.Default);
    }



    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            current_state = STATE.FINISHED;
        }
    }
}
