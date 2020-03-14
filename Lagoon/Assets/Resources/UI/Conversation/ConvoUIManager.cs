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



    bool isTransitioning = false;

    public enum ACTION
    { 
        ACTIVATE_DIALOG,
        ACTIVATE_BRANCH,
        ACTIVATE_EVENT,
        DEACTIVATE_DIALOG,
        DEACTIVATE_BRANCH,
        DEACTIVATE_EVENT,
        TRANSITION_TO_DIALOG,
        TRANSITION_TO_BRANCH,
        TRANSITION_TO_EVENT,
        BRANCH_SELECT_OTHER
    }

    private void Start()
    {
        
    }




    private void Update()
    {
        
    }

    class Action_ActivateDialog
    { 
        public void OnEnter()
        {

        }
        public void Update()
        {

        }
        public void OnExit()
        {

        }    
    }

}
