using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepUiSignal : MonoBehaviour
{

    HDRP_Unlit_ManualAnimator manual_sprite_animator;
    Material material;

    bool run_ui = false;

    private void Awake()
    {

        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += Story_objective_Event_BarrierObjectiveComplete;
        GM_.Instance.story.Event_GameEventStart += Story_Event_GameEventStart;

        manual_sprite_animator = GetComponentInChildren<HDRP_Unlit_ManualAnimator>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Story_Event_GameEventStart(StoryManager.GameEventTriggeredArgs args)
    {
        if(args.event_type == EventNode.EVENT_TYPE.SLEEP)
        {
            run_ui = true;
            GM_.Instance.story.RequestGameEventContinue();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(run_ui)
        {
            Debug.Log("hello");
        }
    }

    private void Story_objective_Event_BarrierObjectiveComplete()
    {
        run_ui = false;
    }
}
