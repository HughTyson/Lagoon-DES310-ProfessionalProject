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
    WATER
    };

    
    [SerializeField] List<TAGS> tags;


    public bool ContainsTheTag(TAGS tag)
    {
        return tags.Contains(tag);
    }


}
