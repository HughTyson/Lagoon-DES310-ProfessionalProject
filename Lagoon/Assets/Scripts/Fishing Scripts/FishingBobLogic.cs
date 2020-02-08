using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobLogic : MonoBehaviour
{

    [Header("General Properties")]
    [Tooltip("the mass the line precieves the bob to be (cannot be 0)")]
    [SerializeField] float precievedLineMass = 0.5f;
    [Tooltip("the maximum velocity of the bob once it's been casted")]
    [SerializeField] float castedMaxVelocity = 20.0f;

    [Header("Attraction Properties")]
    [Tooltip("time between periodic attraction pulses sent to attract fish")]
    [SerializeField] float attractionPulseTimeInterval = 1.0f;   // time between periodic attraction pulses sent to attract fish
 

    [Header("Pointers")]
    [Tooltip("Collider used for rigidbody physics")]
    [SerializeField] Collider physicsCollider;              // Collider used for rigidbody physics
    [Tooltip("Buoyancy physics script component")]
    [SerializeField] BuoyancyPhysics physicsBuoyancy;       // Buoyancy physics script component
    [Tooltip("fishing line logic script component")]
    [SerializeField] FishingLineLogic fishingLineLogic;

    public enum STATE
    {
        NOT_ACTIVE,
        CASTING,
        CASTED,
        FISH_INTERACTING,
        FIGHTING_FISH

    }
    STATE current_state = STATE.NOT_ACTIVE;

    float current_attration_time = 0.0f;




    List<GameObject> nearbyFish = new List<GameObject>(); // list of nearby fish
    FishLogic interactingFish;                           // the current interacting fish            


    private void Start()
    {

    }

    private void OnEnable()
    {
        GetComponentInParent<Rigidbody>().isKinematic = false;
        GetComponentInParent<Rigidbody>().useGravity = true;
    }

    private void OnDisable()
    {
        ScareNearbyFish();
        current_state = STATE.NOT_ACTIVE;
        interactingFish = null;
        nearbyFish.Clear();
    }


    public void CastBob(Vector3 initialPosition, Vector3 initialVelocity)
    {
        physicsBuoyancy.SetToDefaultAirDrag();
        physicsBuoyancy.SetToDefaultWaterDrag();

        current_state = STATE.CASTING;
        transform.parent.transform.position = initialPosition;
        GetComponentInParent<Rigidbody>().velocity = initialVelocity;
    }

    public void BeganFishing()
    {
        current_state = STATE.CASTED;
        physicsBuoyancy.SetAirDrag(1.0f);
        physicsBuoyancy.SetWaterDrag(5.0f);
    }

    public void BeganFighting()
    {
        current_state = STATE.FIGHTING_FISH;
    }
    public void ScareNearbyFish()
    {
        current_attration_time = attractionPulseTimeInterval;
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(5.0f);
        }
    }
    private void FixedUpdate()
    {
        float XZVelocityMag = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).magnitude;

        switch (current_state)
        {
            case STATE.CASTING: // the bob is moving and fish will ignore it
                {
                    
                    break;
                }
            case STATE.CASTED:
                {
                    current_attration_time -= Time.fixedDeltaTime;

                    // GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.GetEndOfLine() - transform.position) / precievedLineMass, ForceMode.VelocityChange);
                  //  GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.EndOfLineVelocity()) / precievedLineMass, ForceMode.VelocityChange);
                  if ((fishingLineLogic.EndOfLineVelocity() / precievedLineMass).magnitude > 0.15f)
                  {
                     GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.EndOfLineVelocity()) / precievedLineMass, ForceMode.VelocityChange);
                  }

                 //   GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.EndOfLineForce()) / 1.0f, ForceMode.VelocityChange);
                    //  GetComponentInParent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponentInParent<Rigidbody>().velocity, 40);

                    if (current_attration_time <= 0.0f) // send attraction pulse
                    {
                        AttractionPulse();
                        current_attration_time = attractionPulseTimeInterval;
                    }
                    break;
                }
            case STATE.FISH_INTERACTING: // a fish is interacting with the bob
                {

                    if ((fishingLineLogic.EndOfLineVelocity() / precievedLineMass).magnitude > 0.15f)
                    {
                        GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.EndOfLineVelocity()) / precievedLineMass, ForceMode.VelocityChange);
                    }

                    if (!interactingFish.IsInStateInteracting())
                    {
                        current_state = STATE.CASTED;
                        current_attration_time = attractionPulseTimeInterval;
                    }
                    break;
                }
        }
    }
    private void Update()
    {
        switch (current_state)
        {
            case STATE.FIGHTING_FISH:
                {
                    transform.parent.transform.position = interactingFish.transform.position;

                    GetComponentInParent<Rigidbody>().isKinematic = true;
                    GetComponentInParent<Rigidbody>().useGravity = false;
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

    public bool IsFishInteracting()
    {
        return (current_state == STATE.FISH_INTERACTING);
    }
    public FishLogic GetInteractingFish()
    {
        return interactingFish;
    }


    public void FishBitLure()
    {
        GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 10.0f, ForceMode.VelocityChange);
    }
    public void FishTestedLure()
    {
        GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 3.0f, ForceMode.VelocityChange);
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

    // call by FishLogic when an attracted fish reaches the bob
    public void FishStartsInteracting(FishLogic interactingFish_)
    {
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            if (nearbyFish[i] != interactingFish_.transform.parent.gameObject)
            {
                nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(0.0f);
            }
        }
        current_state = STATE.FISH_INTERACTING;
        interactingFish = interactingFish_;
    }




    public STATE GetState()
    {
        return current_state;
    }
 
}
