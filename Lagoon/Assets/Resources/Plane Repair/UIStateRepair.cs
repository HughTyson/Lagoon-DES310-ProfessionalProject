using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class UIStateRepair : MonoBehaviour
{

    [SerializeField] GameObject games_complete_text;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Hide()
    {
        games_complete_text.SetActive(false);
    }

    public void Show()
    {
        games_complete_text.SetActive(true);
    }

    public void Complete(bool complete)
    {

        if(complete)
        {
            games_complete_text.GetComponent<TextMeshProUGUI>().text = "FIXED";
        }
        else
        {
            games_complete_text.GetComponent<TextMeshProUGUI>().text = "NEEDS FIXED";
        }

    }
}
