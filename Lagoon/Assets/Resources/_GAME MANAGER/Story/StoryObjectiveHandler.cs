using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObjectiveHandler
{
    List<BaseObjective> objectives = new List<BaseObjective>();

    public StoryObjectiveHandler()
    {
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BARRIER_OPENED, OnBarrierOpened);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BARRIER_BLOCKING_UPDATE, BarrierBlockingUpdate);
    }

    public void OnBarrierOpened()
    {
        objectives.Clear();
        for (int i = 0; i < ((BarrierNode)GM_.Instance.story.current_node).barriers.Count; i++)
        {
            switch (((BarrierNode)GM_.Instance.story.current_node).barriers[i])
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
            }
        }
    }
    public void BarrierBlockingUpdate()
    {
        objectives.RemoveAll(y => y.ObjectiveComplete());
        if (objectives.Count == 0)
        {
            GM_.Instance.story.actionHandler.InvokeActions(StoryManager.ACTION.BARRIER_EXIT);
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
            return GM_.Instance.stats.fishCaught > initFishCaught;
        }

    }
    class Objective_NextDay : BaseObjective
    {
        int initDayNumber = GM_.Instance.stats.dayNumber;

        public override bool ObjectiveComplete()
        {
            return GM_.Instance.stats.dayNumber > initDayNumber;
        }
    }
}
