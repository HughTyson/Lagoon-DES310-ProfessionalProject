using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAME_UI : MonoBehaviour
{
    [Header("Self Pointers")]
    [SerializeField] UIHelperButtons helperButtons;
    [SerializeField] UITransition transition;
    [SerializeField] UIStateFishVictory state_fishVictory;
    [SerializeField] UIStateConversation state_conversation;
    [SerializeField] UIStateRepair state_repair;
    // Start is called before the first frame update
    
    
    public GAME_UI()
    {

    }

    static GAME_UI instance_ = null;
    Members members = null;

    public static Members Instance
    { 
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<GAME_UI>();
                if (instance_ == null)
                {
                    GameObject test = Instantiate(Resources.Load<GameObject>("_GAME MANAGER/GAME_UI"));
                    test.name = "GAME UI";
                    instance_ = test.GetComponent<GAME_UI>();
                }
            }
            return instance_.members;
        }   
    }

    public void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this;
            Members mem = new Members(this);
            mem.helperButtons = helperButtons;
            mem.transition = transition;
            mem.state_fishVictory = state_fishVictory;
            mem.state_conversation = state_conversation;
            mem.state_repair = state_repair;
            instance_.members = mem;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public class Members
    {
        GAME_UI parent;
        public Members(GAME_UI parent_)
        {
            parent = parent_;
        }

        public UIHelperButtons helperButtons;
        public UITransition transition;
        public UIStateFishVictory state_fishVictory;
        public UIStateConversation state_conversation;
        public UIStateRepair state_repair;

        public void EnableAll()
        {
            parent.gameObject.SetActive(true);
        }
        public void DisableAll()
        {
            parent.gameObject.SetActive(false);
        }

    }



    private void OnDestroy()
    {
        if (instance_ == this)
        {
            instance_ = null;
            Destroy(helperButtons);
            Destroy(transition);
            Destroy(state_fishVictory);
            Destroy(state_conversation);
            Destroy(state_repair);
        }
    }
}
