using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public class SpecialText : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI text;

        List<SpecialTextData> specialTextDatas;

        // Start is called before the first frame update
        void Start()
        {

        }
        class SpecialTextGlobalData
        { }


        void Begin(List<SpecialTextData> specialTextDatas_, SpecialTextGlobalData specialTextGlobalData_)
        {
            specialTextDatas = specialTextDatas_;

            string full_text = "";
            for (int i = 0; i < specialTextDatas.Count; i++)
            {
                full_text += specialTextDatas[i].text;
            }

            text.text = full_text;
           // text.textInfo.characterInfo[0].
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}