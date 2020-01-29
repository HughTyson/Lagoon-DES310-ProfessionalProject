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
    COLLIDES_WITH_ROPE__BOX_COLLIDER,
    COLLIDES_WITH_ROPE__SPHERE_COLLIDER,
    FISHING_LOCATION_TRIGGER
    };

    
    [SerializeField] List<TAGS> tags;


    public bool ContainsTheTag(TAGS tag)
    {
        return tags.Contains(tag);
    }


}
