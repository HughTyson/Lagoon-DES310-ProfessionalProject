using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobCollisionEvent : MonoBehaviour
{
    public event System.Action Event_EnterCollision;

    public void OnCollisionEnter(Collision collision)
    {
        Event_EnterCollision?.Invoke();
    }

}
