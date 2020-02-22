using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TagsScript : MonoBehaviour
{
    // Tagging system used to allow multiple tags to an object   

    public enum TAGS
    { 
    STATIC,
    FISH,
    PLAYER,
    ACTION_SCARES_FISH,
    WATER,
    TRIGGER,
    INVISIBLE_WALL,
    CAMERA
    };

    
    [SerializeField] List<TAGS> tags;


    public bool ContainsTheTag(TAGS tag)
    {
        return tags.Contains(tag);
    }

    public void outputTags()
    {
        for(int i = 0; i < tags.Count; i++)
        {
            Debug.Log(tags[i]);
        }
    }


}
