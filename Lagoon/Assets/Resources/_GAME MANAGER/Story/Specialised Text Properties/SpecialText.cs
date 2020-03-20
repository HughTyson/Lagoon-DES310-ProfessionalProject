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
        private Coroutine coroutine;
        // Start is called before the first frame update
        void Start()
        {
        }

        System.Action TextCompleted;
        public void Begin(SpecialTextData specialTextData_, System.Action textCompleted_ = null)
        {
            specialTextManager.Begin(specialTextData_, text);
            TextCompleted = textCompleted_;
            if (coroutine != null)
                 StopCoroutine(coroutine);
            coroutine = StartCoroutine(UpdateText());
        }
        public bool AreAllCompleted()
        {
            return specialTextManager.AreAllCompleted();
        }

        private void Update()
        {
        }
        IEnumerator UpdateText()
        {
            while (true)
            {
                specialTextManager.Update();
                if (specialTextManager.AreAllCompleted())
                {
                    StopCoroutine(coroutine);
                    TextCompleted?.Invoke();
                }
                yield return null;
            }

        }

    }
}