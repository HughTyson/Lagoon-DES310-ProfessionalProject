using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class UIStateRepair : MonoBehaviour
{

    [SerializeField] GameObject games_complete_text;

    [SerializeField] SpecialText.SpecialText special_text;
    SpecialText.SpecialTextData special_text_data_fixed = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData special_text_data_not_fixed = new SpecialText.SpecialTextData();

    // Start is called before the first frame update
    void Start()
    {

        special_text_data_fixed.CreateCharacterData(games_complete_text.GetComponent<TextMeshProUGUI>().text);
        special_text_data_fixed.AddPropertyToText(
            new List<SpecialText.TextProperties.Base>()
            {
                new SpecialText.TextProperties.Colour(0,255,0),
                new SpecialText.TextProperties.StaticAppear(),
                new SpecialText.TextProperties.WaveScaled(1,2,5)
            },
            0,
            5
            );


        special_text_data_not_fixed.CreateCharacterData("NEEDS FIXED");
        special_text_data_not_fixed.AddPropertyToText(
            new List<SpecialText.TextProperties.Base>()
            {
                new SpecialText.TextProperties.Colour(255,0,0),
                new SpecialText.TextProperties.StaticAppear(),
                new SpecialText.TextProperties.WaveScaled(1,2,5)
            },
            0,
            11
            );

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
            special_text.Begin(special_text_data_fixed);
        }
        else
        {
            games_complete_text.GetComponent<TextMeshProUGUI>().text = "NEEDS FIXED";
            special_text.Begin(special_text_data_not_fixed);
        }

    }
}
