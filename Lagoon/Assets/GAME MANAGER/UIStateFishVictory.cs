using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class UIStateFishVictory : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<GameObject> staticObjects;

    [SerializeField] GameObject objectFishType;
    [SerializeField] GameObject objectFishGrade;
    [SerializeField] GameObject objectFishSize;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        for (int i = 0; i < staticObjects.Count; i++)
        {
            staticObjects[i].SetActive(true);
        }

        objectFishType.SetActive(true);
        objectFishGrade.SetActive(true);
        objectFishSize.SetActive(true);
    }

    public void Hide()
    {
        for (int i = 0; i < staticObjects.Count; i++)
        {
            staticObjects[i].SetActive(false);
        }

        objectFishType.SetActive(false);
        objectFishGrade.SetActive(false);
        objectFishSize.SetActive(false);
    
    }


    public void SetVictoryStats(string fish_type, FishType.FISH_TEIR fish_grade, float fish_size)
    {
        objectFishType.GetComponent<TextMeshProUGUI>().text = fish_type;

        switch(fish_grade)
        {
            case FishType.FISH_TEIR.T1:
                {
                    objectFishGrade.GetComponent<TextMeshProUGUI>().text = "D";
                    break;
                }
            case FishType.FISH_TEIR.T2:
                {
                    objectFishGrade.GetComponent<TextMeshProUGUI>().text = "C";
                    break;
                }
            case FishType.FISH_TEIR.T3:
                {
                    objectFishGrade.GetComponent<TextMeshProUGUI>().text = "B";
                    break;
                }
            case FishType.FISH_TEIR.T4:
                {
                    objectFishGrade.GetComponent<TextMeshProUGUI>().text = "A";
                    break;
                }
        }

        objectFishSize.GetComponent<TextMeshProUGUI>().text = fish_size.ToString("#.##") + " m";

    }

}
