using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [System.SerializableAttribute]
    public struct ButtonBundle
    {
       public GameObject text;
       public GameObject sprite;
        public BUTTON_TYPE button_type;
    }

    public enum BUTTON_TYPE
    { 
        A = 0,
        B = 1,
        X = 2,
        Y = 3 ,
        RT = 4
    };

    [SerializeField]
    List<ButtonBundle> buttonList;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisableAllButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].text.GetComponent<TextMeshProUGUI>().text = "";
            buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        }
    }

    public void DisableButton(BUTTON_TYPE button_type)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].button_type == button_type)
            {
                buttonList[i].text.GetComponent<TextMeshProUGUI>().text = "";
                buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
        }
    }

    public void EnableButton(BUTTON_TYPE button_type, string text)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].button_type == button_type)
            {
                buttonList[i].text.GetComponent<TextMeshProUGUI>().text = text;
                buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
            }
        }
    }

}
