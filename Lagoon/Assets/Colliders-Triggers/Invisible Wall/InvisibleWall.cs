using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<TagsScript>() != null)
        {
            if (!collision.collider.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.PLAYER))
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
            }
        }
        else
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }


    }
}
