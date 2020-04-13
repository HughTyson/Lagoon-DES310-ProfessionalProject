using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObjectiveHandler
{
    List<BaseObjective> objectives = new List<BaseObjective>();

    public event System.Action Event_BarrierObjectiveComplete;


    public StoryObjectiveHandler()
    {
        GM_.Instance.story.Event_BarrierStart += BarrierStart;
    }

    void BarrierStart(StoryManager.BarrierStartArgs args)
    {
        GM_.Instance.update_events.UpdateEvent += BarrierBlockingUpdate;

        GM_.Instance.DayNightCycle.SetBaseTime(1.0f);

        objectives.Clear();
        for (int i = 0; i < args.Barriers.Count; i++)
        {
            switch (args.Barriers[i])
            {
                case BarrierNode.BARRIER_STATE.CATCH_A_FISH:
                    {
                        objectives.Add(new Objective_CatchFish());
                        break;
                    }
                case BarrierNode.BARRIER_STATE.NEXT_DAY:
                    {
                        objectives.Add(new Objective_NextDay());
                        break;
                    }
                case BarrierNode.BARRIER_STATE.END:
                    {
                        objectives.Add(new Objective_End());
                        break;
                    }
            }
        }
    }


    public void BarrierBlockingUpdate()
    {
        objectives.RemoveAll(y => y.ObjectiveComplete());
        if (objectives.Count == 0)
        {
            GM_.Instance.update_events.UpdateEvent -= BarrierBlockingUpdate;
            Event_BarrierObjectiveComplete?.Invoke();
        }
    }



    abstract class BaseObjective
    {
        public abstract bool ObjectiveComplete();
    }

    class Objective_CatchFish : BaseObjective
    {
        int initFishCaught = GM_.Instance.stats.fishCaught;

        public override bool ObjectiveComplete()
        {
            return true;
            //return GM_.Instance.stats.fishCaught > initFishCaught;
        }

    }
    class Objective_NextDay : BaseObjective
    {
        int initDayNumber = GM_.Instance.stats.dayNumber;

        public override bool ObjectiveComplete()
        {
            //return true;
            return GM_.Instance.stats.dayNumber > initDayNumber;
        }
    }

    class Objective_End : BaseObjective
    {
        public override bool ObjectiveComplete()
        {
            return false;
        }
    }

}
