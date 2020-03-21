using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairLight : MonoBehaviour
{
    public Material mat_on = null;
    public Material mat_off = null;

    public void SetMatOff()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_off;
    }

    public void SetMatOn()
    {
        GetComponent<MeshRenderer>().sharedMaterial = mat_on;
    }
}
