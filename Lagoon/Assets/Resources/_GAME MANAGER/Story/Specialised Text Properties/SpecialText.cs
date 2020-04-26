using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public class SpecialText : MonoBehaviour
    {

        [SerializeField]
        TMPro.TextMeshProUGUI text;

        SpecialTexManager specialTextManager = new SpecialTexManager();
        // Start is called before the first frame update

        public event System.Action Event_NewCharacterShown;
        bool isOn = false;
        System.Action TextCompleted;

        bool isTimeUnscaled;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="specialTextData_"></param>
        /// <param name="textCompleted_"> Optional Param: Called when Text is finished being shown</param>
        public void Begin(SpecialTextData specialTextData_, System.Action textCompleted_ = null)
        {
            specialTextManager.Event_NewCharacterShown -= Event_NewCharacterShown;
            specialTextManager.Event_NewCharacterShown += Event_NewCharacterShown;

            textCompletedEventCalled = false;
            text.enabled = true;
            Revert();
            specialTextManager.Begin(specialTextData_, text);
            TextCompleted = textCompleted_;
            isOn = true;
            Update();
        }
        public bool AreAllCompleted()
        {
            return specialTextManager.AreAllCompleted();
        }

        public void End()
        {
            isOn = false;
        }
        public void ForceAll()
        {
            specialTextManager.ForceAll();
            Update();
        }

        bool textCompletedEventCalled = false;
        public void Revert()
        {
            if (isOn)
            {
                isOn = false;
                specialTextManager.Revert();
            }
        }

        public void Hide()
        {
            text.enabled = false;
            isOn = false; 
        }


        private void Update()
        {
            if (isOn)
            {
                specialTextManager.Update();
                if (!textCompletedEventCalled)
                {
                    if (specialTextManager.AreAllCompleted())
                    {
                        textCompletedEventCalled = true;
                        TextCompleted?.Invoke();
                    }
                }
            }
        }

    }
}