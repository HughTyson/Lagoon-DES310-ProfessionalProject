using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public List<int> activate;
    public List<int> deactivate;

    public bool on;

    public bool just_changed;//MIGHT DELETE

    public bool IsActive()
    {
        return on;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(on)
        {
            just_changed = true;
        }
        else if(!on)
        {
            on = true;
        }

    }

}
