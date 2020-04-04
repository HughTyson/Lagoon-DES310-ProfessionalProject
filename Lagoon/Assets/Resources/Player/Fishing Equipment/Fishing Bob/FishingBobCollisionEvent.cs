using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobCollisionEvent : MonoBehaviour
{
    public event System.Action Event_EnterCollision;
    public event System.Action<SupplyBox> Event_HitSupplyDrop;
    public void OnCollisionEnter(Collision collision)
    {
        Event_EnterCollision?.Invoke();

        GameObject gameObject = collision.gameObject;

        if (gameObject.GetComponent<TagsScript>() != null)
        {
            if (gameObject.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.SUPPLY_CRATE))
            {
                Event_HitSupplyDrop?.Invoke(gameObject.GetComponent<SupplyBox>());
            }
        }
    }

}
