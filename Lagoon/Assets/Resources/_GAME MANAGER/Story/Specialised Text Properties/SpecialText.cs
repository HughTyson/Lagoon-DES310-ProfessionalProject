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
        void Start()
        {
        }
        bool isOn = false;
        System.Action TextCompleted;
        public void Begin(SpecialTextData specialTextData_, System.Action textCompleted_ = null)
        {
            textCompletedEventCalled = false;
            text.enabled = true;
            Revert();
            specialTextManager.Begin(specialTextData_, text);
            TextCompleted = textCompleted_;
            isOn = true;
        }
        public bool AreAllCompleted()
        {
            return specialTextManager.AreAllCompleted();
        }

        public void End()
        {
            isOn = false;
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