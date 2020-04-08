using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    [SerializeField] SupplyBox box_prefab;

    List<SupplyBox> box_list = new List<SupplyBox>();

    [SerializeField] List<DropPoint> drop_points;

    List<System.Type> required_items;

    bool do_stuff = false;
    int boxes_dropped;

    [SerializeField] AnimationClip clip;
    Animation animation;

    public void Start()
    {
        animation = GetComponent<Animation>();
        animation.AddClip(clip, clip.name);
        
    }

    public void Awake()
    {
        GM_.Instance.story.Event_GameEventStart += SupplyStart;       //start plane moving and setup
    }

    public void SupplyStart(StoryManager.GameEventTriggeredArgs args)
    {
        if(args.event_type == EventNode.EVENT_TYPE.FIRST_SUPPLY_DROP)
        {
            //start the update and act as OnEnable()
            GM_.Instance.story.EventRequest_GameEventContinue += Blocker; //called when requesting the node to continue

            do_stuff = true;
            animation.Play(clip.name);
            boxes_dropped = 0;

            for(int i = 0; i < drop_points.Count; i++)
            {
                drop_points[i].hit = false;
                drop_points[i].already_dropped = false;
            }

            required_items = new List<System.Type>();

            required_items.Add(typeof(SwitchItem));
            required_items.Add(typeof(Wrench));
            required_items.Add(typeof(ScrewDriver));
            
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
            for(int i = 0; i < drop_points.Count; i++)
            {
                if (drop_points[i].hit == true && drop_points[i].already_dropped == false)
                {
                    drop_points[i].already_dropped = true;
                    SpawnBox(i);
                    boxes_dropped += 1;

                }
            }
            
            if(boxes_dropped == drop_points.Count)
            {
                do_stuff = false;
                GM_.Instance.story.EventRequest_GameEventContinue -= Blocker;
                GM_.Instance.story.RequestGameEventContinue();
            }
        }
    }

    public void DestroyBox()
    {
        for(int i = 0; i < box_list.Count; i++)
        {
            Destroy(box_list[i].gameObject);
            box_list.Remove(box_list[i]);
        }
    }

    public void SpawnBox(int i)
    {
        SupplyBox new_box = new SupplyBox();

        new_box = Instantiate(box_prefab);
        new_box.box_state = SupplyBox.STATE.DROPPING;

        new_box.Fill(required_items.ToArray());
        required_items.Clear();

        Vector3 spawn_pos = drop_points[i].transform.position;
        new_box.transform.position = spawn_pos;
    }
}
