using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Apply this script to an object which can interact with liquid
[RequireComponent(typeof(Collider))]
public class BuoyancyPhysics : MonoBehaviour
{

    WaterPhysics currentWater = null; // used to apply correct WaterPhysics if traversing between different waters

    [SerializeField] float buoyancy = 1;
    [SerializeField] float waterDrag = 1;
    [SerializeField] float airDrag = 1;

    public enum STATE
    {
        NONE,
        IN_WATER,
        IN_AIR
    };


    void LateUpdate()
    {
        state_changed = false;
        if (previous_state != current_state)
        {
            previous_state = current_state;
            state_changed = true;
        }
       
    }

    STATE current_state;
    STATE previous_state;
    bool state_changed = false;

    public STATE GetCurrentState()
    {
        return current_state;
    }
    public bool StateChange()
    {
        return state_changed;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WaterPhysics>() != null)
        {
            GetComponentInParent<Rigidbody>().drag = waterDrag;
            current_state = STATE.IN_WATER;   
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<WaterPhysics>() != null)
        {
            if (other.bounds.Contains(transform.position))
            {
                Vector3 force = new Vector3(0,-Physics.gravity.y + buoyancy,0);
                GetComponentInParent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
            }
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WaterPhysics>() != null)
        {
            if (currentWater == other.GetComponent<WaterPhysics>())
            {
                currentWater = null;
                current_state = STATE.IN_AIR;
            }
            GetComponentInParent<Rigidbody>().drag = airDrag;
        }

    }
}
