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

        bool started = false;

        public void Begin(SpecialTextData specialTextData_)
        {
            specialTextManager.Begin(specialTextData_, text);
            started = true;

           // coroutine = StartCoroutine(UpdateText());

        }

        private void Update()
        {
            if (started)
                specialTextManager.Update();
        }
        //IEnumerator UpdateText()
        //{



        //    yield return new WaitForSeconds(Time.deltaTime);
        //}

    }
}