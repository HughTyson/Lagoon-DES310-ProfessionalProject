using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{

    [SerializeField] Transform camera;


    [SerializeField] CharacterControllerMovement movement_;
    [SerializeField] PlayerConversationState convo;

    Vector3 look_at = new Vector3(0, 5, 0);


    [SerializeField] Transform radio;

    bool distance = false;
    bool next_scene = false;


    void Awake()
    {

        GM_.Instance.story.Event_BarrierStart += Story_EventRequest_BarrierStart;

        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += IntroStart;
        
        GM_.Instance.story.Event_GameEventStart += NextScene;

       
    }

    private void Story_EventRequest_BarrierStart(StoryManager.BarrierStartArgs args)
    {

        for(int i = 0; i < args.Barriers.Count; i++)
        {
            if(args.Barriers[i] == RootNode.BARRIER_STATE.CURRENT_SCENE_END)
            {

                GM_.Instance.scene_manager.ChangeScene(2);

                //clean up scene and laod new scene
            }
        }
    }

    private void IntroStart()
    {
        GM_.Instance.scene_manager.new_scene_loaded = false;
        Debug.Log(GM_.Instance.scene_manager.new_scene_loaded);
    }

    private void NextScene(StoryManager.GameEventTriggeredArgs args)
    {
        if(args.event_type == EventNode.EVENT_TYPE.NEXT_SCENE)
        {
            next_scene = true;
            GM_.Instance.scene_manager.new_scene_loaded = false;
            //GM_.Instance.story.EventRequest_GameEventContinue += Blocker;
        }
    }

    public void Blocker(StoryManager.EventRequestArgs args)
    {
        args.Block();
    }


    // Start is called before the first frame update
    void Start()
    {

        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

    }

    // Update is called once per frame
    void Update()
    {
        movement_.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        if(distance)
        {

            camera.transform.rotation = Quaternion.LookRotation(look_at - transform.position);
            Debug.Log(GM_.Instance.DayNightCycle.GetTime());
        }
        
        if(next_scene)
        {
            //GM_.Instance.story.EventRequest_GameEventContinue -= Blocker;
            next_scene = false;
            
            GM_.Instance.story.RequestGameEventContinue();

        }
    }


    bool quiting = false;
    private void OnApplicationQuit()
    {
        quiting = true;
    }
    private void OnDestroy()
    {
        if (!quiting)
        {
            GM_.Instance.story.Event_BarrierStart -= Story_EventRequest_BarrierStart;

            GM_.Instance.story_objective.Event_BarrierObjectiveComplete -= IntroStart;

            GM_.Instance.story.Event_GameEventStart -= NextScene;
        }
    }

}
