using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobLogic : MonoBehaviour
{


    [Header("Attraction Properties")]
    [Tooltip("time between periodic attraction pulses sent to attract fish")]
    [SerializeField] float attractionPulseTimeInterval = 1.0f;   // time between periodic attraction pulses sent to attract fish

    [Header("Fish Bite Properties")]
    [Tooltip("min time for time between bite attempts")]     
    [SerializeField] float fishbiteTimerMin = 2.0f;         // min time for time between bite attempts
    [Tooltip("max time for time between bite attempts")]    
    [SerializeField] float fishbiteTimerMax = 4.0f;         // max time for time between bite attempts
    [Range(0, 1)]
    [Tooltip("chance of the fish bite attempt succeeding")] 
    [SerializeField] float fishbiteChance = 0.2f;           // chance of the fish bite attempt succeeding
    [Tooltip("how long the fish holds the bite before escaping")]
    [SerializeField] float fishHoldBiteTime = 2.0f;         // how long the fish holds the bite before escaping

    [Header("Pointers")]
    [Tooltip("Collider used for rigidbody physics")]
    [SerializeField] Collider physicsCollider;              // Collider used for rigidbody physics
    [Tooltip("Buoyancy physics script component")]
    [SerializeField] BuoyancyPhysics physicsBuoyancy;       // Buoyancy physics script component


    public enum STATE
    { 
        HIT_LAND_OR_FLOATING,
        HIT_WATER,
        MOVING,              // the bob is moving and fish will ignore it
        SETTLED,             // the bob is settled and will send periodic attraction pulses to the fish
        FISH_INTERACTING,    // a fish is interacting with the bob
        FISH_BITE            // a fish has bit the lure
    }
    STATE current_state;

    float current_attration_time = 0.0f;




    List<GameObject> nearbyFish = new List<GameObject>(); // list of nearby fish
    GameObject interactingFish;                           // the current interacting fish

    PlayerFishingState refPlayerFishing;                       // pointer to PlayerFishing script attached to the player
    int fishbiteFailCounter = 0;                          
    float fishbiteTimer = 0.0f;                                                                                                                    
    float currentFishHoldBitTime = 0.0f;                  


    private void Start()
    {
        current_state = STATE.MOVING;
    }




    private void FixedUpdate()
    {
        float XZVelocityMag = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).magnitude;
        Debug.Log(XZVelocityMag);

        switch (current_state)
        {
            case STATE.MOVING: // the bob is moving and fish will ignore it
                {
                    switch (physicsBuoyancy.GetCurrentState())
                    {
                        case BuoyancyPhysics.STATE.IN_AIR:
                            {
                                float test = (GetComponentInParent<Rigidbody>().velocity.magnitude);
                                if (GetComponentInParent<Rigidbody>().velocity.magnitude < 0.01)
                                {
                                    current_state = STATE.HIT_LAND_OR_FLOATING;
                                }
                                break;
                            }
                        case BuoyancyPhysics.STATE.IN_WATER:
                            {
                                current_state = STATE.HIT_WATER;
                                break;
                            }

                    }
                    break;
                }
            case STATE.HIT_LAND_OR_FLOATING:
                {
                    if (GetComponentInParent<Rigidbody>().velocity.magnitude > 0.1)
                    {
                        current_state = STATE.MOVING;
                    }
                    break;
                }
            case STATE.HIT_WATER:
                {
                    if (GetComponentInParent<Rigidbody>().velocity.magnitude < 0.1)
                    {
                           current_attration_time = attractionPulseTimeInterval;
                           current_state = STATE.SETTLED;
                    }
                    break;
                }
                        
            case STATE.SETTLED: // the bob is settled and will send periodic attraction pulses to the fish
                {
                    current_attration_time -= Time.deltaTime;
                    if (current_attration_time <= 0.0f) // send attraction pulse
                    {
                        AttractionPulse();
                        current_attration_time = attractionPulseTimeInterval;
                    }

                    BobUnsettledCheck();
                    break;
                }
            case STATE.FISH_INTERACTING: // a fish is interacting with the bob
                {
                    fishbiteTimer -= Time.fixedDeltaTime;
                    if (fishbiteTimer <= 0)
                    {
                        if ((Random.value <= fishbiteChance) || (fishbiteFailCounter == 2 && refPlayerFishing.GetFailedFishCounter() == 2)) // fish bite succeeded
                        {
                            current_state = STATE.FISH_BITE;
                            refPlayerFishing.ResetFailedFishCounter();
                            GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 10.0f, ForceMode.Impulse);
                            currentFishHoldBitTime = fishHoldBiteTime;

                        }
                        else // fish bite failed
                        {
                            fishbiteFailCounter++;
                            GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 3.0f, ForceMode.VelocityChange);

                            if (fishbiteFailCounter == 3) // 3 attempts have been made, so fish loses interest
                            {
                                refPlayerFishing.FailedFishCounterIncrement();
                                interactingFish.GetComponentInChildren<FishLogic>().LostInterestInFishingBob(4.0f);
                                nearbyFish.Remove(interactingFish.transform.parent.gameObject);
                                current_state = STATE.SETTLED;
                            }
                            else // set a time to try again
                            {
                                fishbiteTimer = Random.Range(fishbiteTimerMin, fishbiteTimerMax);
                            }
                            break;

                        }
                    }
                    else
                    {
                        BobUnsettledCheck();
                    }
                    break;
                }
            case STATE.FISH_BITE: // a fish has bit the lure
                {
                    GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 10.0f, ForceMode.Acceleration);
                    currentFishHoldBitTime -= Time.fixedDeltaTime;

                    if (currentFishHoldBitTime <= 0)
                    {
                        interactingFish.GetComponentInChildren<FishLogic>().LostInterestInFishingBob(4.0f);
                        nearbyFish.Remove(interactingFish.transform.parent.gameObject);
                        current_state = STATE.MOVING;
                    }
                    break;
                }

        }
    }


    // called at a periodic interval to nearby fish
    void AttractionPulse()
    {
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            nearbyFish[i].GetComponentInChildren<FishLogic>().AttractionAttempt(physicsCollider,this);
        }
    }


    void BobUnsettledCheck()
    {
        float XZVelocityMag = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).magnitude;
        if (XZVelocityMag >= 1.0f)
        {
            current_state = STATE.MOVING;
            current_attration_time = attractionPulseTimeInterval;
            for (int i = 0; i < nearbyFish.Count; i++)
            {
                nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(5.0f);
            }
        }
    }

    // something came into the bob's sphere of influence
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagsScript>() != null)
        {
            if (other.gameObject.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.FISH)) // is the collider a fish?
            {
                nearbyFish.Add(other.gameObject);

                Physics.IgnoreCollision(physicsCollider, other); // ignore the collisions between the 2 physics bodies, so they go through each other
            }
            else
            {
                Physics.IgnoreCollision(other, GetComponent<Collider>()); // the trigger collider wasn't interested in this collision so ignore any collisions between this trigger collider and the other collider
            }
        }
        else
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());  // the trigger collider wasn't interested in this collision so ignore any collisions between this trigger collider and the other collider
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nearbyFish.Remove(other.gameObject);
    }


    // -- Public Functions -- //
    //public void BobIsMoving() // bob is moving; reel in was pressed
    //{
    //    for (int i = 0; i < nearbyFish.Count; i++)
    //    {
    //        nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(5.0f);
    //        current_state = STATE.MOVING;
    //        current_attration_time = attractionPulseTimeInterval;
    //    }
    //}

    // call by FishLogic when an attracted fish reaches the bob
    public void FishStartsInteracting(GameObject interactingFish_)
    {
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            if (nearbyFish[i] != interactingFish_)
            {
                nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(0.0f);
            }
        }
        current_state = STATE.FISH_INTERACTING;
        fishbiteTimer = Random.Range(fishbiteTimerMin, fishbiteTimerMax);
        fishbiteFailCounter = 0;
        interactingFish = interactingFish_;
    }


   
    public void FishCaught()
    {
        Destroy(interactingFish.transform.parent.gameObject);

    }

    // Initialize pointers when this object is instantiated
    public void Setup(PlayerFishingState refPlayerFishing_)
    {
        refPlayerFishing = refPlayerFishing_;
    }


    public STATE GetState()
    {
        return current_state;
    }
 
}
