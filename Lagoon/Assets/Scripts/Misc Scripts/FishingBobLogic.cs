using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BuoyancyPhysics))]
public class FishingBobLogic : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float fishLatchedForce = 4;
    [SerializeField] float fishLatchedTimeMin = 1;
    [SerializeField] float fishLatchedTimeMax = 2;
    [SerializeField] float fishHoldsTime = 0.5f;

    float currentFishLatchedTime = 0;
    float currentFishHoldTime = 0;

    enum CURRENT_STATE
    { 
    FLYING,
    IDLE,
    FISH_ATTACHED
    };
    CURRENT_STATE current_state;


    public bool WasFishCaught()
    {
        if (current_state == CURRENT_STATE.FISH_ATTACHED)
        {
            return true;
        }
        return false;
    }
    void Start()
    {
        current_state = CURRENT_STATE.FLYING;
    }

    // Update is called once per frame

    void Update()
    {
       
        if (GetComponent<BuoyancyPhysics>().StateChange())
        {
            switch (GetComponent<BuoyancyPhysics>().GetCurrentState())
            {
                case BuoyancyPhysics.STATE.IN_WATER: // changed to water
                    {
                        current_state = CURRENT_STATE.IDLE;
                        currentFishLatchedTime = Random.Range(fishLatchedTimeMin, fishLatchedTimeMax);
                        break;
                    }
            }
        }

        switch (current_state)
        {

            case CURRENT_STATE.IDLE:
                {
                    currentFishLatchedTime -= Time.deltaTime;

                    if (currentFishLatchedTime <= 0.0f)
                    {
                        current_state = CURRENT_STATE.FISH_ATTACHED;
                        currentFishHoldTime = fishHoldsTime;
                        GetComponentInParent<Rigidbody>().AddForce(new Vector3(0,-fishLatchedForce,0), ForceMode.VelocityChange);
                    }
                    break;
                }
            case CURRENT_STATE.FISH_ATTACHED:
                {
                    currentFishHoldTime -= Time.deltaTime;

                    if (currentFishHoldTime <= 0.0f)
                    {
                        current_state = CURRENT_STATE.IDLE;
                        currentFishLatchedTime = Random.Range(fishLatchedTimeMin, fishLatchedTimeMax);
                    }

                    break;
                }            
        }
    }
}
