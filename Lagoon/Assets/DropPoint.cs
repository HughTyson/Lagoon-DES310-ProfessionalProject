using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{

    public bool hit;
    public bool already_dropped;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        already_dropped = false;
    }

    private void OnTriggerStay(Collider other)
    {
       hit = true;
        Debug.Log("hit");
    }



}
