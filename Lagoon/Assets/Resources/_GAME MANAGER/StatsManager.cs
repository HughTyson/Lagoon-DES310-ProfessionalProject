using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    public struct BiggestFishStats
    {
        float size;
    };

    public int fishCaught = 0;
    public BiggestFishStats bigestFishStats = new BiggestFishStats();
    public int dayNumber = 0;


    public void DayCountIncrease()
    {
        dayNumber += 1;
        Debug.Log(dayNumber);
    }

}
