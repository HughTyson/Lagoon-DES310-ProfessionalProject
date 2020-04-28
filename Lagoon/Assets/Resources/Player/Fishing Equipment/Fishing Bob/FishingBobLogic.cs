using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobLogic : MonoBehaviour
{

    [Header("General Properties")]
    [Tooltip("the mass the line precieves the bob to be (cannot be 0)")]
    [SerializeField] float precievedLineMass = 0.5f;

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

    [SerializeField] GameObject meshObject;
    [SerializeField] GameObject lineLoop;
    [SerializeField] FishingBobCollisionEvent collisionEvent;


    public enum STATE
    {
        NOT_ACTIVE,
        CASTING,
        CASTED,
        FISH_INTERACTING,
        FIGHTING_FISH,
        HOOKED_TO_CRATE,
        TUTORIAL
    }
    STATE current_state = STATE.NOT_ACTIVE;

    float current_attration_time = 0.0f;




    List<GameObject> nearbyFish = new List<GameObject>(); // list of nearby fish
    FishLogic interactingFish;
    // the current interacting fish            

    AudioSFX sfx_hitWater;

    AudioSFX sfx_fishBitLure;
    AudioSFX sfx_fishTestedLure;
    void hitWater()
    {
        if (GetComponentInParent<Rigidbody>().velocity.magnitude > 3.0f)
            GM_.Instance.audio.PlaySFX(sfx_hitWater, transform);
    }

    private void Start()
    {
        sfx_hitWater = GM_.Instance.audio.GetSFX("FishingBob_HitWater");
        sfx_fishBitLure = GM_.Instance.audio.GetSFX("Fish_Swerving");
        sfx_fishTestedLure = GM_.Instance.audio.GetSFX("Fish_Swerving");
        physicsBuoyancy.Event_HitWater += hitWater;
    }

    private void OnEnable()
    {
        GetComponentInParent<Rigidbody>().isKinematic = false;
        GetComponentInParent<Rigidbody>().useGravity = true;
        meshObject.SetActive(true);
        gameObject.transform.parent.SetParent(null, true);
    }

    private void OnDisable()
    {
        ScareNearbyFish(0.0f);
        current_state = STATE.NOT_ACTIVE;
        interactingFish = null;

        nearbyFish.Clear();
    }

    public void AttachToSupplyBox(SupplyBox supplyBox)
    {
        gameObject.transform.parent.SetParent(supplyBox.transform, true);
        current_state = STATE.HOOKED_TO_CRATE;
        GetComponentInParent<Rigidbody>().isKinematic = true;
        GetComponentInParent<Rigidbody>().useGravity = false;
    }
    public void DetachFromSupplyCrate()
    {
        gameObject.transform.parent.SetParent(null, true);
        current_state = STATE.NOT_ACTIVE;
        GetComponentInParent<Rigidbody>().isKinematic = true;
        GetComponentInParent<Rigidbody>().useGravity = false;
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
        current_attration_time = attractionPulseTimeInterval;
        current_state = STATE.CASTED;
        physicsBuoyancy.SetAirDrag(1.0f);
        physicsBuoyancy.SetWaterDrag(5.0f);
    }

    public void BeganFighting()
    {
        current_state = STATE.FIGHTING_FISH;
        transform.parent.transform.position = interactingFish.GetComponent<FishLogic>().GetHeadPosition();
        GetComponentInParent<Rigidbody>().isKinematic = true;
        GetComponentInParent<Rigidbody>().useGravity = false;

        meshObject.SetActive(false);

    }

    void SafelyRemoveNull()
    {
        for (int i = nearbyFish.Count - 1; i >= 0 ; i--) // iterate back to front to allow removing while iterating
        {
            if (nearbyFish[i] == null)
            {
                nearbyFish.Remove(nearbyFish[i]);
            }
        }
    }
    public void ScareNearbyFish(float time)
    {
        current_attration_time = attractionPulseTimeInterval;
        SafelyRemoveNull();
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            if (nearbyFish[i].GetComponentInChildren<FishLogic>() != null)
            {
                nearbyFish[i].GetComponentInChildren<FishLogic>().LostInterestInFishingBob(time);
            }
        }
    }

    public bool HasAttractedFish()
    {
        SafelyRemoveNull();
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            if (nearbyFish[i].GetComponentInChildren<FishLogic>().GetState() == FishLogic.STATE.ATTRACTED)
            {
                return true;
            }
        }
        return false;
    }
    private void Update()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

        float XZVelocityMag = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).magnitude;

        switch (current_state)
        {
            case STATE.CASTING: // the bob is moving and fish will ignore it
                {
                    Vector3 current_velocity = GetComponentInParent<Rigidbody>().velocity;
                 
                    if (current_velocity.magnitude > 0.001f)
                    {
                        Vector3 angles = Quaternion.LookRotation(current_velocity.normalized).eulerAngles;
                        angles.x -= 90;
                        GetComponentInParent<Rigidbody>().MoveRotation(Quaternion.Euler(angles));
                    }
                   
                    break;
                }
            case STATE.CASTED:
                {
                    current_attration_time -= Time.fixedDeltaTime;

                    // GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.GetEndOfLine() - transform.position) / precievedLineMass, ForceMode.VelocityChange);
                    //  GetComponentInParent<Rigidbody>().AddForce((fishingLineLogic.EndOfLineVelocity()) / precievedLineMass, ForceMode.VelocityChange);


                    //    float variable_dampening = Mathf.Lerp(0.1f, 1.0f, (Mathf.Max(Mathf.Min(fishingLineLogic.DistanceFromTipToBob(), 40.0f), 10.0f) - 10.0f) / (40.0f - 10.0f));


                    //      Debug.Log(variable_dampening);
                    //      float dampened_magnitude = (fishingLineLogic.EndOfLineVelocity() / precievedLineMass).magnitude - variable_dampening;
                    //      if (dampened_magnitude > 0)
                    ///  {
                    ///  
                    GetComponentInParent<Rigidbody>().AddForceAtPosition(fishingLineLogic.EndOfLineVelocity(), lineLoop.transform.position, ForceMode.VelocityChange);
                    //   GetComponentInParent<Rigidbody>().AddForce(fishingLineLogic.EndOfLineVelocity().normalized * dampened_magnitude, ForceMode.VelocityChange);
                    //     }

                    if (physicsBuoyancy.GetCurrentState() == BuoyancyPhysics.STATE.IN_AIR)
                    {
                        current_attration_time = attractionPulseTimeInterval;
                        ScareNearbyFish(0.5f);
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

                    GetComponentInParent<Rigidbody>().AddForce(fishingLineLogic.EndOfLineVelocity(), ForceMode.VelocityChange);


                    if (!interactingFish.IsInStateInteracting())
                    {
                        current_state = STATE.CASTED;
                        current_attration_time = attractionPulseTimeInterval;
                    }
                    break;
                }
            case STATE.FIGHTING_FISH:
                {
                    transform.parent.transform.position = interactingFish.GetComponent<FishLogic>().GetHeadPosition();
                    break;
                }
            case STATE.HOOKED_TO_CRATE:
                {

                    break;
                }
        }
    }

    // called at a periodic interval to nearby fish
    void AttractionPulse()
    {
        SafelyRemoveNull();
        for (int i = 0; i < nearbyFish.Count; i++)
        {
            nearbyFish[i].GetComponentInChildren<FishLogic>().AttractionAttempt(physicsCollider,this);
        }
    }

    public void FishStoppedInteracting()
    {
        current_state = STATE.CASTED;
        current_attration_time = attractionPulseTimeInterval;
    }
    public bool IsFishInteracting()
    {
        return (current_state == STATE.FISH_INTERACTING);
    }
    public FishLogic GetInteractingFish()
    {
        return interactingFish;
    }
    public bool HasHookedToCrate()
    {
        return (current_state == STATE.HOOKED_TO_CRATE);
    }


   
    public void FishBitLure()
    {
        GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 10.0f, ForceMode.VelocityChange);
        GM_.Instance.audio.PlaySFX(sfx_fishBitLure, transform);

    }
    public void FishTestedLure()
    {
        GetComponentInParent<Rigidbody>().AddForce(-Vector3.up * 3.0f, ForceMode.VelocityChange);
        GM_.Instance.audio.PlaySFX(sfx_fishTestedLure, transform);
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
        if (nearbyFish.Contains(other.gameObject))
        {
            other.gameObject.GetComponentInChildren<FishLogic>().LostInterestInFishingBob(0.0f);
        }
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

    public void Tutorial(STATE state_)
    {
        current_state = state_;
    }
 
}
