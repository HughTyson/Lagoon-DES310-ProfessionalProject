using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{

    //[SerializeField] Transform bob_prefab;

    //private void Start()
    //{
    //    Physics.IgnoreCollision(bob_prefab.GetComponent<Collider>(), GetComponent<Collider>());
    //}

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
