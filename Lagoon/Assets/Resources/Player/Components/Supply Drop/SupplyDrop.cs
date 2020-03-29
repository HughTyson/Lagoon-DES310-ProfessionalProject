using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    [SerializeField] SupplyBox box_prefab;

    [SerializeField] public List<SupplyBox> box_list = new List<SupplyBox>();

    [SerializeField] List<GameObject> drop_points;

    [Header("Plane Movement")]

    [SerializeField] GameObject plane;
    [SerializeField] Transform start_pos;
    [SerializeField] Transform end_pos;


    bool do_stuff = false;

    //static public readonly TweenManager.TweenPathBundle plane_animation_tween = new TweenManager.TweenPathBundle(

    //    //x pos
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(-27, )
    //        ),
    //    );

    public void Awake()
    {
        GM_.Instance.story.Event_GameEventStart += SupplyStart;       //start plane moving and setup
    }

    public void SupplyStart(StoryManager.GameEventTriggeredArgs args)
    {
        if(args.event_type == EventNode.EVENT_TYPE.SUPPLY_DROP)
        {
            //start the update and act as OnEnable()
            GM_.Instance.story.EventRequest_GameEventContinue += Blocker; //called when requesting the node to continue

            do_stuff = true;
        }
    }

    public void Blocker(StoryManager.EventRequestArgs args)
    {
        args.Block();
    }

    public void End()
    {

        

        //cleanUp
    }


    public void Update()
    {
        
        if(do_stuff)
        {


            Spawn();



            do_stuff = false;
            GM_.Instance.story.EventRequest_GameEventContinue -= Blocker;
            GM_.Instance.story.RequestGameEventContinue();
            


        }



    }

    public void Spawn()
    {
        StartCoroutine(SpawnBoxes());
    }

    public void DestroyBox()
    {
        for(int i = 0; i < box_list.Count; i++)
        {
            Destroy(box_list[i].gameObject);
            box_list.Remove(box_list[i]);
        }
    }

    public IEnumerator SpawnBoxes()
    {
        for (int i = 0; i < drop_points.Count; i++)
        {

            SupplyBox new_box = new SupplyBox();

            new_box = Instantiate(box_prefab);
            new_box.box_state = SupplyBox.STATE.DROPPING;

            Vector3 spawn_pos = drop_points[i].transform.position;
            new_box.transform.position = spawn_pos;

            yield return new WaitForSeconds(0.7f);

        }
    }
}
