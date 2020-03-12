using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public List<int> activate;
    public List<int> deactivate;

    public bool on;

    public bool just_changed;

    public Material mat_on = null;
    public Material mat_off = null;

    public bool IsActive()
    {
        return on;
    }

    public void selectedSwitch()
    {
        Debug.Log(just_changed);

        just_changed = true;

        if (on)
        {
            on = false;
        }
        else if (!on)
        {
            on = true;
        }

        
    }

    public void SetMatOff()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_off;
    }

    public void SetMatOn()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_on;
    }
}
