using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    bool changed = false;

    public List<ItemSprite> inventory_sprites = new List<ItemSprite>();

    private void Awake()
    {
        Application.quitting += Quiting;

        GM_.Instance.stats.CleanUp();

        GM_.Instance.story_objective.Event_BarrierObjectiveComplete += GameStart;

        GM_.Instance.story.Event_BarrierStart += Story_EventRequest_BarrierStart;

    }

    private void Start()
    {
        for (int i = 0; i < inventory_sprites.Count; i++)
        {
            GM_.Instance.inventory.item_images.Add(inventory_sprites[i]);
        }


        GM_.Instance.inventory.Reset();
    }


    private void Story_EventRequest_BarrierStart(StoryManager.BarrierStartArgs args)
    {

        for (int i = 0; i < args.Barriers.Count; i++)
        {
            if (args.Barriers[i] == RootNode.BARRIER_STATE.CURRENT_SCENE_END)
            {

                if(!changed)
                {
                    changed = true;
                    
                    GM_.Instance.scene_manager.ChangeScene(0);
                }


                //clean up scene and laod new scene
            }
        }
    }

    private void GameStart()
    {
        GM_.Instance.scene_manager.new_scene_loaded = false;
        Debug.Log(GM_.Instance.scene_manager.new_scene_loaded);
        GM_.Instance.story_objective.Event_BarrierObjectiveComplete -= GameStart;
    }



    private void OnDestroy()
    {
        if (!quiting)
        {
           
            GM_.Instance.story.Event_BarrierStart -= Story_EventRequest_BarrierStart;
        }
    }

    bool quiting = false;

    void Quiting()
    {
        quiting = true;
    }
}
