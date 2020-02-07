using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Movement Properties")]
    [Tooltip("maximum velocity the fish will reach")]
    [SerializeField]  float maxVelocity = 5;                    // maximum velocity the fish will reach

    [Tooltip("minimum terminal force choosen in RandomRange applied to the fish")]
    [SerializeField]  float TerminalForce_min = 0.1f;            // minimum terminal force choosen in RandomRange applied to the fish
    [Tooltip("maximum terminal force choosen in RandomRange applied to the fish")]
    [SerializeField]  float TerminalForce_max = 10;              // maximum terminal force applied to the fish

    [Tooltip("minimum terminal force time chosen in RandomRange before changing the terminal force")]
    [SerializeField]  float TerminalForceDuration_min = 0.1f;    // minimum terminal force time chosen in RandomRange before changing the terminal force
    [Tooltip("maximum terminal force time chosen in RandomRange before changing the terminal force")]
    [SerializeField]  float TerminalForceDuration_max = 5;       // maximum terminal force time chosen in RandomRange before changing the terminal force


    [Header("Fishing Properties")]
    [Tooltip("success rate of a lure attraction pulse attracting the fish to go to it")]
    [Range(0.0f,1.0f)]
    [SerializeField] float lureAttractionSuccessRate = 1.0f;          //success rate of a lure attraction pulse attracting the fish to go to it
    [Range(0, 1)]
    [Tooltip("chance of the fish bite attempt succeeding")]
    [SerializeField] float fishbiteChance = 0.2f;           // chance of the fish bite attempt succeeding
    [Tooltip("how long the fish holds the bite before escaping")]
    [SerializeField] float fishHoldBiteTime = 2.0f;         // how long the fish holds the bite before escaping


    [Header("Required Pointers")]
    [Tooltip("The physics collider of the fish")]
    [SerializeField] Collider physicsCollider;                        // the physics collider of the fish
    [Tooltip("The transform of the fish head")]
    [SerializeField] Transform fishheadTransform;                       





    // -- Fish States -- //
    enum STATE
    {
        WANDERING,           // the fish is wandering aimlessly
        AVOIDING,            // the fish sees a threat and is avoiding it
        AVOIDANCE_SETTLE,    // the fish doesn't see a threat anymore but is still moving away from it
        ATTRACTED,           // the fish is attracted to a location
        INTERACTING          // the fish is interacting with a fishing lure
    }
    STATE current_state;     // The current state of the fish



    const float wanderingTargetRadius = 0.75f;  // used for wandering algorithm
    const float wanderingTargetDistance = 1.0f; // used for wandering algorithm
    const float targetRadius = 3.0f;            // used for arrival algorith


    List<Collider> avoidingObjects = new List<Collider>(); // list of objects the fish sees and is avoinding

    
    // -- Movement Vars -- //
    float terminalForce = 0;                    // current terminal velocity
    float terminalForceDuration = 0;            // time until terminal velocity is changed within a RandomRnage
                                                
    float currentAngle;                         // current angle of the circle used in the wandering algorithm 
    float angleChangeTime = 0;                  // time until the angle is changed
                             
    // -- Avoidance Vars -- //
    float avoidanceSettleTick = 0;              // time until the a 
    Vector2 avoidanceTarget;                    // 
    
    // -- Attraction Vars -- //
    Collider attractorCollider;                 // the fishing bob collider pointer
    FishingBobLogic attractorLogic;             // the fishing bob logic pointer
    float NotInterestedInBobTime = 0.0f;        // time left until able to be interested in fishing bob
    Vector2 headVectorXZ = new Vector2();

    void Start()
    {
        currentAngle = Random.Range(0, Mathf.PI * 2.0f);
        current_state = STATE.WANDERING;
        terminalForceDuration = Random.Range(TerminalForceDuration_min, TerminalForceDuration_max);
        terminalForce = Random.Range(TerminalForce_min, TerminalForce_max);

    }

    void StateTick()
    {
        terminalForceDuration -= Time.fixedDeltaTime; // update time
        if (terminalForceDuration <= 0) // change terminal velocity values, changing the fish's turn and swim speed
        {
            terminalForceDuration = Random.Range(TerminalForceDuration_min, TerminalForceDuration_max);
            terminalForce = Random.Range(TerminalForce_min, TerminalForce_max);
        }
       

        switch (current_state)
        {
            case STATE.WANDERING:
                {
                    GetComponentInParent<MeshRenderer>().material.color = Color.green;      // used for debugging
                    Wander(); 

                    if (avoidingObjects.Count != 0) // if there are objects to avoid, start avoiding them
                    {
                        current_state = STATE.AVOIDING;
                    }
                    break;
                }
            case STATE.AVOIDING:
                {
                    GetComponentInParent<MeshRenderer>().material.color = Color.red;      // used for debugging
                    if (avoidingObjects.Count == 0) // cant see the object which was being avoided, but move away from that location for a while longer
                    {
                        current_state = STATE.AVOIDANCE_SETTLE;
                        avoidanceSettleTick = Random.Range(0.3f, 0.5f);

                    }
                    else
                    {
                        Avoid();
                    }

                    break;
                }
            case STATE.AVOIDANCE_SETTLE:
                {
                    GetComponentInParent<MeshRenderer>().material.color = Color.yellow;    // used for debugging
                    AvoidanceSettle();

                    if (avoidingObjects.Count != 0) // if there are objects to avoid, start avoiding them
                    {
                        current_state = STATE.AVOIDING;
                    }
                    break;
                }
            case STATE.ATTRACTED:
                {
                    if (attractorCollider == null) // fish bob was destroyed, so go back to wandering
                    {
                        current_state = STATE.WANDERING;
                    }
                    else
                    {
                        if (attractorLogic.GetState() != FishingBobLogic.STATE.FISH_INTERACTING)
                        {
                            GetComponentInParent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.2f); // used for debgging

                            Arrive(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)); // move towards the fishing bob, stopping when it's been reached

                            if (Vector2.Distance(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)) < 0.5f) // the bob has been reached
                            {
                                attractorLogic.FishStartsInteracting(this);
                                avoidingObjects.Clear();
                                current_state = STATE.INTERACTING;

                            }
                        }
                        else // another fish has started interacting with the bob, so forget about the bob
                        {
                            LostInterestInFishingBob(0.0f);
                        }

                    }

                    if (avoidingObjects.Count != 0)  // if there are objects to avoid, start avoiding them
                    {
                        current_state = STATE.AVOIDING;
                    }

                    break;
                }
            case STATE.INTERACTING:
                {
                    if (attractorCollider == null) // fish bob was destroyed, so go back to wandering
                    {
                        current_state = STATE.WANDERING;
                    }
                    else
                    {
                        GetComponentInParent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.2f); // used for debgging

                        if (Vector2.Distance(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)) > 1.0f)  // move towards the fishing bob, if drifted away
                        {
                            Arrive(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z));
                        }
                    }
                    GetComponentInParent<MeshRenderer>().material.color = Color.cyan; // used for debugging
                    break;
                }
        }

    }



    void FixedUpdate()
    {
        headVectorXZ.x = fishheadTransform.position.x;
        headVectorXZ.y = fishheadTransform.position.z;

        StateTick(); // update state

        NotInterestedInBobTime = Mathf.Max(0, NotInterestedInBobTime - Time.fixedDeltaTime); // countdown time 

        // Rotate the fish on the y axis dependant on the velocity on the XZ plane, so it faces the direction of its velocity
        Vector2 velocityUnitXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).normalized;

        if (velocityUnitXZ.magnitude > 0.0001)
        {
        transform.parent.transform.rotation = Quaternion.LookRotation(new Vector3(velocityUnitXZ.x, 0, velocityUnitXZ.y), Vector3.up);
        }

    }



    // Wander aimlessly
    void Wander()
    {
        // XZ velocity
        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);


        // angle on the circle set infront of the fish to be its target
        angleChangeTime -= Time.fixedDeltaTime;
        if (angleChangeTime < 0)
        {
            currentAngle = Random.Range(0, Mathf.PI * 2.0f);
            angleChangeTime = Random.Range(0.1f, 1.0f);
        }

        // target's position on the circle
        Vector2 target = new Vector2(Mathf.Cos(currentAngle) * wanderingTargetRadius, Mathf.Sin(currentAngle) * wanderingTargetRadius);


        if (velocityXZ.magnitude < 0.000001) // no direction vector
        {
            target += new Vector2(transform.position.x, transform.position.z); // orientate the target circle to be on the fish's coordinates
            Seek(target); // make fish try to reach the ever-changing target
        }
        else
        {
            target += new Vector2(transform.position.x, transform.position.z); // orientate the target circle to be on the fish's coordinates
            target += velocityXZ.normalized * wanderingTargetDistance;         // move the target along the direction of the fish's velocity
            Seek(target); // make fish try to reach the ever-changing target
        }
    }

    // avoid threats
    void Avoid()
    {
        Vector2 desired_vector = Vector2.zero;
        Vector2 location = new Vector2(transform.position.x, transform.position.z); // XZ location of the fish


        // find closest points of the threat to the fish and set the desired vector to be the inverse of the vector from the fish to the threat
        for (int i = 0; i < avoidingObjects.Count; i++)
        {
                Vector3 closet_point = avoidingObjects[i].ClosestPoint(transform.position);
                Vector2 fleeingFromObject = new Vector2(closet_point.x, closet_point.z);
                desired_vector += (location - fleeingFromObject).normalized;

        }

        // average escape vector of all the threats
        desired_vector /= avoidingObjects.Count;


        Vector2 tranformFrowardXZ = new Vector2(transform.forward.x, transform.forward.z); // XZ forward vector of the fish
        Vector2 tranformRightXZ = new Vector2(transform.right.x, transform.right.z);       // XZ right vector of the fish


        // limit the desired escape vector to only be on the front of the fish, preventint the fish from slowing down and completely turning around
        if (Vector2.Dot(desired_vector, tranformFrowardXZ) < 0) // the desired vector is on the back half of the fish
        {
            if (Vector2.Dot(desired_vector, tranformRightXZ) > 0) // the desired vecotr is on the right side of the fish
            {
                desired_vector = tranformRightXZ; 
            }
            else // the desired vecotor is on the left side of the fish
            {
                desired_vector = -tranformRightXZ;
            }
        }
        desired_vector *= maxVelocity; // change magnitude of desired direction vector to have a velocity

        avoidanceTarget = desired_vector;


       
        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired_vector - velocityXZ, terminalForce);

        // move in the direction of the desired vector
        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

    }

    
    // keep avoiding using the last desired vector for a set amount of time
    void AvoidanceSettle()
    {
        avoidanceSettleTick -= Time.fixedDeltaTime;

        if (avoidanceSettleTick <= 0) // change back to wandering
        {
            current_state = STATE.WANDERING;
            currentAngle = Random.Range(0, Mathf.PI * 2.0f);
            angleChangeTime = Random.Range(0.2f, 1.0f);

        }

        // move towards the last desired vector

       Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(avoidanceTarget - velocityXZ, terminalForce);

        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));
    }

    // go to a specific location, without caring if it overshoots it
    void Seek(Vector2 target)
    {
        Vector2 location = new Vector2(transform.position.x, transform.position.z);
        Vector2 desired = target - location;
        desired = desired.normalized;
        desired *= maxVelocity;

        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired - velocityXZ, terminalForce);


        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

    }

    // go to a specific location with care about slowing down and stopping at it
    void Arrive(Vector2 myPos,Vector2 target)
    {
        Vector2 location = myPos;
        Vector2 desired = target - location;
        float desired_dist = desired.magnitude;

        desired = desired.normalized;

        desired *= Mathf.Lerp(0, maxVelocity, desired_dist / targetRadius);


        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = Vector2.ClampMagnitude(desired - velocityXZ, terminalForce);

        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));



    }


    // something entered the fish's sight
    private void OnTriggerEnter(Collider other)
    {

            if (other.GetComponent<TagsScript>() != null)
            {
                if (other.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.ACTION_SCARES_FISH)) // does the thing that the fish sees scare the fish?
                {
                    avoidingObjects.Add(other);
                    

                }
            }
            else
            {
                Physics.IgnoreCollision(other, GetComponent<Collider>()); // fish doesn't care about this collision so ignore any future collisions with the other collider
            }
    }

    // something left the fish's sight
    private void OnTriggerExit(Collider other)
    {
            avoidingObjects.Remove(other);
    }


    // -- Public Functions -- //

    public void AttractionAttempt(Collider target, FishingBobLogic fishBobLogic) // fish bob attempts to attract fish
    {
        if (NotInterestedInBobTime <= 0)
        {
            if (Random.value <= lureAttractionSuccessRate) 
            {
                // attraction successful
                current_state = STATE.ATTRACTED;
                attractorCollider = target;
                attractorLogic = fishBobLogic;
            }
        }


    }

    public void LostInterestInFishingBob(float duration) // fish lost interest in fishing bob
    {
        current_state = STATE.WANDERING;
        NotInterestedInBobTime = duration;
    }

    public bool IsInStateInteracting()
    {
        return (current_state == STATE.INTERACTING);
    }

    public bool BiteAttempt()
    {
        return (Random.value <= fishbiteChance);
    }


    public float GetFishBiteHoldTime()
    {
        return fishHoldBiteTime;
    }
}
