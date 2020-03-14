using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHelperButtons : MonoBehaviour
{
    // Start is called before the first frame update

    [System.SerializableAttribute]
    public struct ButtonBundle
    {
       public GameObject text;
       public GameObject sprite;
       public BUTTON_TYPE button_type;
    }

    [System.SerializableAttribute]
    public struct StickBundle
    {
        public GameObject text;
        public GameObject sprite;
        public GameObject leftArrow;
        public GameObject rightArrow;
        public GameObject upArrow;
        public GameObject downArrow;
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

    [SerializeField]
    StickBundle leftStick;

    public void HideButtons()
    {
        enabled = false;
    }

    public void ShowButtons()
    {
        enabled = true;
    }

    public void DisableAll()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].button_type == BUTTON_TYPE.RT)
            {
                buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.0f);
            }
            else
            {
                buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
            buttonList[i].text.GetComponent<TextMeshProUGUI>().text = "";
        }

        
        leftStick.sprite.SetActive(false);
        leftStick.leftArrow.SetActive(false);
        leftStick.rightArrow.SetActive(false);
        leftStick.upArrow.SetActive(false);
        leftStick.downArrow.SetActive(false);
        leftStick.text.GetComponent<TextMeshProUGUI>().text = "";

    }

    public void EnableLeftStick(bool leftArrow, bool rightArrow, bool upArrow, bool downArrow, string text)
    {
        leftStick.sprite.SetActive(true);
        leftStick.leftArrow.SetActive(leftArrow);
        leftStick.rightArrow.SetActive(rightArrow);
        leftStick.upArrow.SetActive(upArrow);
        leftStick.downArrow.SetActive(downArrow);
        leftStick.text.GetComponent<TextMeshProUGUI>().text = text;
    }
    public void DisableLeftStick()
    {
        leftStick.sprite.SetActive(false);
        leftStick.leftArrow.SetActive(false);
        leftStick.rightArrow.SetActive(false);
        leftStick.upArrow.SetActive(false);
        leftStick.downArrow.SetActive(false);
        leftStick.text.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void DisableButton(BUTTON_TYPE button_type)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i].button_type == button_type)
            {
                if (buttonList[i].button_type == BUTTON_TYPE.RT)
                {
                    buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.0f);
                }
                else
                {
                    buttonList[i].sprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                }
                buttonList[i].text.GetComponent<TextMeshProUGUI>().text = "";
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
