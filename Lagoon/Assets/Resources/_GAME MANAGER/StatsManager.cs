using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    public struct FishStats_
    {
        public float size;
        public string type;
        public string satisfaction;
    };


    public struct PlaneStats_
    {
        public bool complete;
        public PlaneSegments.SegmentType type;
        public string segment_name;
    }

    public int fishCaught = 0;
    public FishStats_ bigestFishStats = new FishStats_();
    public FishStats_ last_fish_stats = new FishStats_();
    public int dayNumber = 0;

    public List<PlaneStats_> plane_segments_stats = new List<PlaneStats_>();
    PlaneStats_ temp;


    public void DayCountIncrease()
    {
        dayNumber += 1;
        Debug.Log(dayNumber);
    }

    public void IncreaseFishCount()
    {
        fishCaught += 1;
    }

    public void NewBiggest(FishLogic.VarsFromFishGenerator vars)
    {
        if(vars.size > bigestFishStats.size)
        {
            bigestFishStats.size = vars.size;

            bigestFishStats.type = vars.fishTypeName;

            switch (vars.teir)
            {
                case FishType.FISH_TEIR.T1:
                    {
                        bigestFishStats.satisfaction = "D - Could catch better";
                        break;
                    }
                case FishType.FISH_TEIR.T2:
                    {
                        bigestFishStats.satisfaction = "C - A nice but unspectacular fish";
                        break;
                    }
                case FishType.FISH_TEIR.T3:
                    {
                        bigestFishStats.satisfaction = "B - This will fill me up";
                        break;
                    }
                case FishType.FISH_TEIR.T4:
                    {
                        bigestFishStats.satisfaction = "A - I don't ever need to eat again after having this for dinner";
                        break;
                    }
            }
        }
    }

    public void UpdateLastFish(FishLogic.VarsFromFishGenerator vars)
    {
        last_fish_stats.size = vars.size;
        last_fish_stats.type = vars.fishTypeName;

        switch (vars.teir)
        {
            case FishType.FISH_TEIR.T1:
                {
                    last_fish_stats.satisfaction = "D - Could catch better";
                    break;
                }
            case FishType.FISH_TEIR.T2:
                {
                    last_fish_stats.satisfaction = "C - A nice but unspectacular fish";
                    break;
                }
            case FishType.FISH_TEIR.T3:
                {
                    last_fish_stats.satisfaction = "B - This will fill me up";
                    break;
                }
            case FishType.FISH_TEIR.T4:
                {
                    last_fish_stats.satisfaction = "A - I don't ever need to eat again after having this for dinner";
                    break;
                }
        }

    }

    public void ResetStats()
    {
        bigestFishStats.size = 0;
        bigestFishStats.type = "";
        bigestFishStats.satisfaction = "I need to catch a fish first";


        last_fish_stats.size = 0;
        last_fish_stats.type = "";
        last_fish_stats.satisfaction = "I need to catch a fish first";
    }

    public void AddPlaneSegment(bool section_completed_, PlaneSegments.SegmentType type_, string segment_name)
    {

        temp = new PlaneStats_();

        temp.complete = section_completed_;
        temp.type = type_;
        temp.segment_name = segment_name;

        plane_segments_stats.Add(temp);

    }

    public void UpdateSegment(bool section_completed_, PlaneSegments.SegmentType type_, string segment_name, int i)
    {

        temp = new PlaneStats_();

        temp.complete = section_completed_;
        temp.type = type_;
        temp.segment_name = segment_name;

        plane_segments_stats[i] = temp;

    }



}
