using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDrop : MonoBehaviour
{

    [SerializeField] SupplyBox box_prefab;

    List<SupplyBox> box_list = new List<SupplyBox>();

    [SerializeField] List<DropPoint> drop_points;

    List<System.Type> required_items;
    List<int> required_amount;

    List<AdditionalInfoNode_CertainItemsInNextSupplyDrop.ItemData> info;

    bool do_stuff = false;
    int boxes_dropped;

    [SerializeField] AnimationClip clip;
    Animation animation;

    AudioSFX plane_sfx;
    AudioManager.SFXInstanceInterface plane_sfx_interface;
    TypeRef<float> volume = new TypeRef<float>();

    TweenManager.TweenPathBundle sound_fall_off;
    TweenManager.TweenInstanceInterface sound_interface;

    public void Awake()
    {
        GM_.Instance.story.Event_GameEventStart += SupplyStart;       //start plane moving and setup
        Application.quitting += Quitting;

        plane_sfx = GM_.Instance.audio.GetSFX("Plane Noise");
    }

    public void Start()
    {
        animation = GetComponent<Animation>();
        animation.AddClip(clip, clip.name);

        volume.value = plane_sfx.Volume;

        sound_fall_off = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(plane_sfx.Volume, 0, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ));

    }

    public void SupplyStart(StoryManager.GameEventTriggeredArgs args)
    {
        if(args.event_type == EventNode.EVENT_TYPE.SUPPLY_DROP)
        {
            //start the update and act as OnEnable()
            GM_.Instance.story.EventRequest_GameEventContinue += Blocker; //called when requesting the node to continue

            do_stuff = true;
            animation.animatePhysics = true;
            animation.Play(clip.name);
            boxes_dropped = 0;

            for(int i = 0; i < drop_points.Count; i++)
            {
                drop_points[i].hit = false;
                drop_points[i].already_dropped = false;
            }

            info = new List<AdditionalInfoNode_CertainItemsInNextSupplyDrop.ItemData>();

            required_items = new List<System.Type>();
            required_amount = new List<int>();

            for (int i = 0; i < args.certainItemDrops.Count; i++)
            {
                info.Add(args.certainItemDrops[i]);

                required_items.Add(args.certainItemDrops[i].type);
                required_amount.Add(args.certainItemDrops[i].ammount);
                
            }

            plane_sfx_interface = GM_.Instance.audio.PlaySFX(plane_sfx,transform);
            Debug.Log(plane_sfx_interface.Volume);

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
            
            if(boxes_dropped > 0)
            {
                
                if(!animation.isPlaying)
                {
                    do_stuff = false;
                    
                    GM_.Instance.story.EventRequest_GameEventContinue -= Blocker;
                    GM_.Instance.story.RequestGameEventContinue();
                    
                }
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

        new_box.Fill(required_items.ToArray(), required_amount.ToArray());
        new_box.SetTransform();
        required_items.Clear();

        Vector3 spawn_pos = drop_points[i].transform.position;
        new_box.transform.position = spawn_pos;
    }

    public void VolumeLower()
    {
        GM_.Instance.tween_manager.StartTweenInstance(
            sound_fall_off,
            new TypeRef<float>[] { volume },
            tweenUpdatedDelegate_: UpdateVolume,
            tweenCompleteDelegate_: StopSound
            );
    }

    void UpdateVolume()
    {
        plane_sfx_interface.Volume = volume.value;
    }

    void StopSound()
    {
        plane_sfx_interface.Stop();
    }
    

    bool quitting = false;

    void Quitting()
    {
        quitting = true;
    }
    private void OnDestroy()
    {
        if (!quitting)
        {
        GM_.Instance.story.Event_GameEventStart -= SupplyStart;       //start plane moving and setup
        }
    }
}
