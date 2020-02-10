using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Apply this script to an object which can interact with liquid
public class BuoyancyPhysics : MonoBehaviour
{

    WaterPhysics currentWater = null; // used to apply correct WaterPhysics if traversing between different waters

    [SerializeField] float buoyancy = 1;
    [SerializeField] float defaultWaterDrag = 1;
    [SerializeField] float defaultAirDrag = 0;

    [SerializeField] float equilibriumAcceptance = 0.2f;


    float WaterDrag = 1;
    float AirDrag = 1;

    public enum STATE
    {
        IN_WATER,
        IN_AIR
    };


    private void OnEnable()
    {
        WaterDrag = defaultWaterDrag;
        AirDrag = defaultAirDrag;
         current_state = STATE.IN_AIR;
         previous_state = STATE.IN_AIR;
         state_changed = false;
        GetComponentInParent<Rigidbody>().drag = AirDrag;
    }

    private void LateUpdate()
    {
        state_changed = false;
        if (previous_state != current_state)
        {
            previous_state = current_state;
            state_changed = true;
        }
       
    }

    STATE current_state = STATE.IN_AIR;
    STATE previous_state = STATE.IN_AIR;
    bool state_changed = false;


    public STATE GetCurrentState()
    {
        return current_state;
    }
    public bool StateChange()
    {
        return state_changed;
    }


    public void SetAirDrag(float drag)
    {
        AirDrag = drag;
    }
    public void SetWaterDrag(float drag)
    {
        WaterDrag = drag;
    }

    public void SetToDefaultAirDrag()
    {
        AirDrag = defaultAirDrag;
    }

    public void SetToDefaultWaterDrag()
    {
        WaterDrag = defaultWaterDrag;
    }

    public bool IsInEquilibrium()
    {
        return GetComponentInParent<Rigidbody>().velocity.magnitude < equilibriumAcceptance;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WaterPhysics>() != null)
        {
            GetComponentInParent<Rigidbody>().drag = WaterDrag;
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
            GetComponentInParent<Rigidbody>().drag = AirDrag;
        }

    }
}
