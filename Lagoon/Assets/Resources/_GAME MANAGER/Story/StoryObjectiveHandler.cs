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


    public void Reset()
    {
        objectives.Clear();
        GM_.Instance.update_events.UpdateEvent -= BarrierBlockingUpdate;
    }
    void BarrierStart(StoryManager.BarrierStartArgs args)
    {
        GM_.Instance.update_events.UpdateEvent += BarrierBlockingUpdate;

        GM_.Instance.day_night_cycle.SetBaseTime(1.0f);

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
                case BarrierNode.BARRIER_STATE.PLANE_SEGMENT_FINISHED:
                    {
                        objectives.Add(new Objective_PlaneSectionRepair());
                        break;
                    }
                case BarrierNode.BARRIER_STATE.NEW_SCENE_START:
                    {
                        objectives.Add(new Objective_SceneStart());
                        break;
                    }
                case BarrierNode.BARRIER_STATE.CURRENT_SCENE_END:
                    {
                        objectives.Add(new Objective_SceneEnd());
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
            //return true;
            return GM_.Instance.stats.fishCaught > initFishCaught;
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

    class Objective_PlaneSectionRepair : BaseObjective
    {

        int complete = GM_.Instance.stats.AmountOfSegmentsComplete();

        public override bool ObjectiveComplete()
        {
            return GM_.Instance.stats.AmountOfSegmentsComplete() > complete;
        }

    }

    class Objective_SceneStart : BaseObjective
    {
        public override bool ObjectiveComplete()
        {
            if(GM_.Instance.scene_manager.new_scene_loaded)
            {
                Debug.Log("Hello");
            }

            return GM_.Instance.scene_manager.new_scene_loaded;
        }

    }

    class Objective_SceneEnd : BaseObjective
    {
        public override bool ObjectiveComplete()
        {
            if (GM_.Instance.scene_manager.new_scene_loaded)
            {
            }

            return GM_.Instance.scene_manager.new_scene_loaded;
        }

    }
}
