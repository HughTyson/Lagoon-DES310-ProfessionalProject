using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Fishing Properties")]
    [Tooltip("success rate of a lure attraction pulse attracting the fish to go to it")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float lureAttractionSuccessRate = 1.0f;          //success rate of a lure attraction pulse attracting the fish to go to it
    [Range(0, 1)]
    [Tooltip("chance of the fish bite attempt succeeding")]
    [SerializeField] float fishbiteChance = 0.2f;           // chance of the fish bite attempt succeeding
    [Tooltip("how long the fish holds the bite before escaping")]
    [SerializeField] float fishHoldBiteTime = 2.0f;         // how long the fish holds the bite before escaping



    [Header("Required Pointers")]
    [Tooltip("The transform of the fish head")]
    [SerializeField] Transform fishheadTransform;
    [SerializeField] Transform MeshTransform;



    public struct VarsFromFishGenerator
    {
        public FishType.FISH_TEIR teir;

        public string fishTypeName;
        public Vector2 habitatRingOrigin;
        public float habitatRingMin;
        public float habitatRingMax;
        public float size;
        public float defaultForce;
        public float maxForce;
        public float defaultTurnForce;
        public float maxTurnForce;
        public float maxVelocity;
    
    }
    public VarsFromFishGenerator varsFromFishGenerator;

    // -- Fish States -- //
    public enum STATE
    {
        WANDERING,           // the fish is wandering aimlessly
        AVOIDING,            // the fish sees a threat and is avoiding it
        ATTRACTED,           // the fish is attracted to a location
        INTERACTING,          // the fish is interacting with a fishing lure
        FIGHTING,
        CAUGHT
    }
    STATE current_state;     // The current state of the fish



    const float wanderingTargetRadius = 0.75f;  // used for wandering algorithm
    const float wanderingTargetDistance = 1.0f; // used for wandering algorithm
    float targetRadius = 4.0f;            // used for arrival algorith



    List<Collider> avoidingObjects = new List<Collider>(); // list of objects the fish sees and is avoinding



    float currentAngle;                         // current angle of the circle used in the wandering algorithm 
    float angleChangeTime = 0;                  // time until the angle is changed

    // -- Avoidance Vars -- //
    float avoidanceRad;
    float sizeRad;

    // -- Attraction Vars -- //
    Collider attractorCollider;                 // the fishing bob collider pointer
    FishingBobLogic attractorLogic;             // the fishing bob logic pointer
    float NotInterestedInBobTime = 0.0f;        // time left until able to be interested in fishing bob
    Vector2 headVectorXZ = new Vector2();


    void Start()
    {

        currentAngle = Random.Range(0, Mathf.PI * 2.0f);
        current_state = STATE.WANDERING;
    }

    public void Init(VarsFromFishGenerator vars)
    {
        varsFromFishGenerator = vars;


        avoidanceRad = GetComponent<SphereCollider>().radius;

        CapsuleCollider capsule = GetComponentInParent<CapsuleCollider>();
        sizeRad = Mathf.Max(capsule.radius, capsule.height / 2.0f); // gets the radius that will encapsulate the whole fish

        capsule.radius *= vars.size;
        capsule.height *= vars.size;
        MeshTransform.localScale *= vars.size;
        fishheadTransform.localPosition *= vars.size;


    }

    void StateTick()
    {



        switch (current_state)
        {
            case STATE.WANDERING:
                {
                    Vector3 forward = transform.forward;
                    GetComponentInParent<Rigidbody>().AddForce(new Vector3(forward.x, 0, forward.z) * varsFromFishGenerator.defaultForce);

                 //   GetComponentInParent<MeshRenderer>().material.color = Color.green;      // used for debugging
                    Wander();

                    float distanceFromHabitatOrigin = Vector2.Distance(headVectorXZ, varsFromFishGenerator.habitatRingOrigin);
                    if (avoidingObjects.Count != 0) // if there are objects to avoid, start avoiding them
                    {
                        current_state = STATE.AVOIDING;
                    }
                    else if (distanceFromHabitatOrigin < varsFromFishGenerator.habitatRingMin || distanceFromHabitatOrigin > varsFromFishGenerator.habitatRingMax)
                    {
                 //       current_state = STATE.RETURN_HOME;
                    }
                    break;
                }
            case STATE.AVOIDING:
                {
                   // GetComponentInParent<MeshRenderer>().material.color = Color.red;      // used for debugging
                    if (avoidingObjects.Count == 0) // cant see the object which was being avoided, but move away from that location for a while longer
                    {
                        current_state = STATE.WANDERING;

                    }
                    else
                    {
                        Avoid();
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
                            float distanceFromBobToHabitOrigin = Vector2.Distance(new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z), varsFromFishGenerator.habitatRingOrigin);
                            if (distanceFromBobToHabitOrigin > varsFromFishGenerator.habitatRingMin && distanceFromBobToHabitOrigin < varsFromFishGenerator.habitatRingMax)
                            {
                                Arrive(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)); // move towards the fishing bob, stopping when it's been reached

                                if (Vector2.Distance(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)) < 1.0f) // the bob has been reached
                                {
                                    attractorLogic.FishStartsInteracting(this);
                                    avoidingObjects.Clear();
                                    current_state = STATE.INTERACTING;
                                }
                            }
                            else
                            {
                                LostInterestInFishingBob(0.0f);
                            }
                        }
                        else // another fish has started interacting with the bob, so forget about the bob
                        {
                            LostInterestInFishingBob(0.0f);
                        }

                    }

                    if (avoidingObjects.Count != 0)  // if there are objects to avoid, start avoiding them
                    {
                        for (int i = 0; i < avoidingObjects.Count; i++)
                        {
                            if (avoidingObjects[i].GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.FISH))
                            {
                                if (avoidingObjects[i].GetComponentInChildren<FishLogic>().varsFromFishGenerator.size - 0.0001 > varsFromFishGenerator.size) // only get scared if bigger fish is near
                                {
                                    current_state = STATE.AVOIDING;
                                    break;
                                }
                            }
                        }

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
                        //      GetComponentInParent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.2f); // used for debgging

                        float distanceFromBobToHabitOrigin = Vector2.Distance(new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z), varsFromFishGenerator.habitatRingOrigin);
                        if (distanceFromBobToHabitOrigin > varsFromFishGenerator.habitatRingMin && distanceFromBobToHabitOrigin < varsFromFishGenerator.habitatRingMax)
                        {
                            if (Vector2.Distance(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z)) > 1.0f)  // move towards the fishing bob, if drifted away
                            {
                                Arrive(headVectorXZ, new Vector2(attractorCollider.transform.position.x, attractorCollider.transform.position.z));
                            }
                        }
                        else
                        {
                            LostInterestInFishingBob(0.0f);
                        }
                    }
              //      GetComponentInParent<MeshRenderer>().material.color = Color.cyan; // used for debugging
                    break;
                }
            case STATE.FIGHTING:
                {

                    FightingStateUpdate();
                    break;
                }
            case STATE.CAUGHT:
                {

                    break;
                }
        }

    }


    public STATE GetState()
    {
        return current_state;
    }
    void FixedUpdate()
    {
        for (int i = 0; i < avoidingObjects.Count; i++)
        {
            if (avoidingObjects[i] == null)
            {
                avoidingObjects.Remove(avoidingObjects[i]);
            }
        }

            StateTick(); // update state


        if (current_state == STATE.FIGHTING)
        {

        }
        else if (current_state == STATE.CAUGHT)
        {

        }
        else
        {
            GetComponentInParent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponentInParent<Rigidbody>().velocity, varsFromFishGenerator.maxVelocity);

            NotInterestedInBobTime = Mathf.Max(0, NotInterestedInBobTime - Time.fixedDeltaTime); // countdown time 

            // Rotate the fish on the y axis dependant on the velocity on the XZ plane, so it faces the direction of its velocity
            Vector2 velocityUnitXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z).normalized;

            if (velocityUnitXZ.magnitude > 0.0001)
            {
                transform.parent.transform.rotation = Quaternion.LookRotation(new Vector3(velocityUnitXZ.x, 0, velocityUnitXZ.y), Vector3.up);
            }

          //  Vector3 currentPosition = transform.parent.transform.position;


        }
        

    }

    void Update()
    {
        avoidingObjects.RemoveNullReferences();

        headVectorXZ.x = transform.parent.transform.position.x;
        headVectorXZ.y = transform.parent.transform.position.z;
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
        }
        else
        {
            target += new Vector2(transform.position.x, transform.position.z); // orientate the target circle to be on the fish's coordinates
            target += velocityXZ.normalized * wanderingTargetDistance;         // move the target along the direction of the fish's velocity
        }

        float distanceToHabitOrigin = Vector2.Distance(target, varsFromFishGenerator.habitatRingOrigin);

        if (distanceToHabitOrigin < varsFromFishGenerator.habitatRingMin)
        {
            target += (target - varsFromFishGenerator.habitatRingOrigin).normalized * (varsFromFishGenerator.habitatRingMin - distanceToHabitOrigin)*2.0f;
        }
        else if (distanceToHabitOrigin > varsFromFishGenerator.habitatRingMax)
        {
            target += (varsFromFishGenerator.habitatRingOrigin - target).normalized * (distanceToHabitOrigin - varsFromFishGenerator.habitatRingMax)*2.0f;
        }

        Seek(target); // make fish try to reach the ever-changing target
    }

    // avoid threats
    void Avoid()
    {
        Vector2 desired_vector = Vector2.zero;
        Vector2 location = new Vector2(transform.position.x, transform.position.z); // XZ location of the fish

        float closestDangerDistance = float.MaxValue;

        // find closest points of the threat to the fish and set the desired vector to be the inverse of the vector from the fish to the threat
        for (int i = 0; i < avoidingObjects.Count; i++)
        {
            Vector3 closet_point =  avoidingObjects[i].ClosestPoint(transform.position);
            Vector2 fleeingFromObject = new Vector2(closet_point.x, closet_point.z);
         
            float distanceFromThis = Vector2.Distance(location, fleeingFromObject);


            float lerp_t = 1.0f - ((distanceFromThis - sizeRad) / (avoidanceRad - sizeRad));
            lerp_t *= lerp_t;

            Vector2 fromTo = (location - fleeingFromObject);
            desired_vector += fromTo.normalized * lerp_t;

            if (distanceFromThis < closestDangerDistance)
            {
                closestDangerDistance = distanceFromThis;
            }



        }

        float t = 1.0f - ((closestDangerDistance - sizeRad) / (avoidanceRad - sizeRad));
        t *= t; // change distribution curve to quadratic instead of linear when lerping

        Vector3 forward = transform.forward;
        GetComponentInParent<Rigidbody>().AddForce(new Vector3(forward.x, 0, forward.z) * Mathf.Lerp(varsFromFishGenerator.defaultForce, varsFromFishGenerator.maxForce, t));

        // average escape vector of all the threats

        float test = desired_vector.magnitude;
        desired_vector = desired_vector.normalized;


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
        desired_vector *= Mathf.Lerp(0, varsFromFishGenerator.maxTurnForce, t); // change magnitude of desired direction vector based on distance to closest threat



        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = desired_vector - velocityXZ;

        // move in the direction of the desired vector
        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));



    }

    // go to a specific location, without caring if it overshoots it
    void Seek(Vector2 target)


    {


        Vector3 forward = transform.forward;
        GetComponentInParent<Rigidbody>().AddForce(new Vector3(forward.x, 0, forward.z) * varsFromFishGenerator.defaultForce);

        Vector2 location = new Vector2(transform.position.x, transform.position.z);
        Vector2 desired = target - location;
        desired = desired.normalized;
        desired *= varsFromFishGenerator.defaultForce;

        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = desired - velocityXZ;



        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

    }

    // go to a specific location with care about slowing down and stopping at it
    void Arrive(Vector2 myPos, Vector2 target)
    {


        Vector2 location = myPos;
        Vector2 desired_turn = target - location;
        float desired_dist = desired_turn.magnitude;

        desired_turn = desired_turn.normalized;

        desired_turn *= Mathf.Lerp(0, varsFromFishGenerator.maxTurnForce, desired_dist / targetRadius);

        Vector3 forward = transform.forward;
        GetComponentInParent<Rigidbody>().AddForce(new Vector3(forward.x, 0, forward.z) * Mathf.Lerp(0, varsFromFishGenerator.maxForce, desired_dist / targetRadius));

        Vector2 velocityXZ = new Vector2(GetComponentInParent<Rigidbody>().velocity.x, GetComponentInParent<Rigidbody>().velocity.z);
        Vector2 steer = desired_turn - velocityXZ;

        GetComponentInParent<Rigidbody>().AddForce(new Vector3(steer.x, 0, steer.y));

       

    }


    // something entered the fish's sight
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<TagsScript>() != null)
        {
            if (other.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.ACTION_SCARES_FISH)) // does the thing that the fish sees scare the fish?
            {


                if (other.GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.FISH))
                {
                    if (other.GetComponentInChildren<FishLogic>().varsFromFishGenerator.size + 0.0001f > varsFromFishGenerator.size) // only add equal or bigger fish to avoid
                    {
                        avoidingObjects.Add(other);
                    }
                    Physics.IgnoreCollision(GetComponentInParent<CapsuleCollider>(), other); // fish don't collider with each other but they are scared of each other
                }
                else
                {
                    avoidingObjects.Add(other);
                }

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
            float BobDistancFromHabitOrigin = Vector2.Distance(new Vector2(fishBobLogic.transform.position.x, fishBobLogic.transform.position.z),varsFromFishGenerator.habitatRingOrigin);
            if (BobDistancFromHabitOrigin > varsFromFishGenerator.habitatRingMin && BobDistancFromHabitOrigin < varsFromFishGenerator.habitatRingMax) // is bob in fish's habitat
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
        

    }

    public void LostInterestInFishingBob(float duration) // fish lost interest in fishing bob
    {
        current_state = STATE.WANDERING;
        if (NotInterestedInBobTime < duration)
        {
            NotInterestedInBobTime = duration;
        }


    }

    public bool IsInStateInteracting()
    {
        return (current_state == STATE.INTERACTING);
    }

    public bool BiteAttempt()
    {
        return (Random.value <= fishbiteChance);
    }


    public Vector3 GetHeadPosition()
    {
        return fishheadTransform.position;
    }

    public float GetFishBiteHoldTime()
    {
        return fishHoldBiteTime;
    }


    public void FishCaught()
    {
        current_state = STATE.CAUGHT;
    }

    // -- Fighting State -- //

    public enum FIGHTING_STATE
    {
        GO_TO_STARTPOSITION,
        FIGHTING
    }


    struct FightingStateVars
    {
        public FIGHTING_STATE state;
        public float maxVelocity;

        public float currentVelocity;
        public float fightingVelocityMax;
        public Vector2 playerPosXZ;
        public float distanceToPlayer;

        public float maxLeftRighAmplitude;
        public float leftRighAmplitude;

        public float fightingAccellerationMin;
        public float fightingAccellerationMax;
        public float currentNextStateChangeTime;
        public float fightingAccelleration;

        public float fishAngleValue;
        public float fightingYDistance;

        public float fightingNextStateMinTime;
        public float fightingNextStateMaxTime;

        public float fleeingDriveForce;

        public float maxDistance;

        public float playerAccelleration;
        public float playerAccellerationMax;

    }
    FightingStateVars fightingStateVars = new FightingStateVars();


    
    public void BeginFighting(Vector2 playerPosXZ_)
    {

        fightingStateVars.state = FIGHTING_STATE.GO_TO_STARTPOSITION;
        current_state = STATE.FIGHTING;
        GetComponentInParent<Rigidbody>().isKinematic = true;
        fightingStateVars.playerPosXZ = playerPosXZ_;

        fightingStateVars.currentVelocity = 0;
        fightingStateVars.maxVelocity = 16;
       
        fightingStateVars.distanceToPlayer = Vector2.Distance(headVectorXZ, fightingStateVars.playerPosXZ);


        fightingStateVars.maxLeftRighAmplitude = 10.0f;
        fightingStateVars.leftRighAmplitude = fightingStateVars.maxLeftRighAmplitude;

        fightingStateVars.fightingAccellerationMin = 0.02f;
        fightingStateVars.fightingAccellerationMax = 0.04f;

        fightingStateVars.fightingVelocityMax = 0.5f;

        fightingStateVars.playerAccelleration = 0;
        fightingStateVars.playerAccellerationMax = 0.08f;
        fightingStateVars.fishAngleValue = 0.5f;

        fightingStateVars.fightingNextStateMinTime = 0.1f;
        fightingStateVars.fightingNextStateMinTime = 0.5f;

        fightingStateVars.maxDistance = 50.0f;


        fightingStateVars.fleeingDriveForce = 5.0f;

        Debug.Log("Init Player Distance: " + fightingStateVars.distanceToPlayer);

    }


    public void ReelingIn(float force)
    {
        if (fightingStateVars.state == FIGHTING_STATE.FIGHTING)
        {

                float near_edge = Mathf.Abs((fightingStateVars.fishAngleValue - 0.5f) * 2.0f);
                float min = 0.1f;

                float t = (1.0f - ((near_edge - min) / (1.0f - min)));

                float final_force = force * t + fightingStateVars.fleeingDriveForce * t; // applies fleeing drive force * t because that is a constant force of the fish fleeing


                fightingStateVars.fightingYDistance -= (final_force) * Time.fixedDeltaTime;

        }

    }

    public void SetPlayerAccelleration(float acc)
    {
        fightingStateVars.playerAccelleration = -(acc * fightingStateVars.playerAccellerationMax);
    }

    public bool IsFishCaught()
    {
        if (fightingStateVars.state == FIGHTING_STATE.FIGHTING)
        {
            return (fightingStateVars.fightingYDistance <= 2.0f);
        }
        return false;
    }
    public void FishEscaped()
    {
        LostInterestInFishingBob(20.0f);
        GetComponentInParent<Rigidbody>().isKinematic = false;
    }
    public void FightingStateUpdate()
    {

        switch (fightingStateVars.state)
        {

            case FIGHTING_STATE.GO_TO_STARTPOSITION:
                {
                    

                    Debug.DrawRay(transform.parent.transform.position, transform.parent.transform.forward * 20.0f, Color.green);

                    float angle = Vector2.Angle((headVectorXZ - fightingStateVars.playerPosXZ).normalized, Vector2.left);

                    float circumference = angle*Mathf.Deg2Rad * (headVectorXZ - fightingStateVars.playerPosXZ).magnitude;

                    Vector2 newPosXZ;

                    
                    float adding_angle = Mathf.Sign(Vector2.SignedAngle((headVectorXZ - fightingStateVars.playerPosXZ).normalized, Vector2.left)) * (5.0f / (headVectorXZ - fightingStateVars.playerPosXZ).magnitude)*Mathf.Rad2Deg * Time.fixedDeltaTime * 5.0f;

                    if (circumference < 5.0f)
                    {
                        Vector2 new_dir = Vector2Extension.Rotate((headVectorXZ - fightingStateVars.playerPosXZ).normalized, adding_angle * (Mathf.Max(circumference  - 2.0f + 0.5f, 0.5f) / (5.0f - 2.0f)));
                        newPosXZ = fightingStateVars.playerPosXZ + (new_dir * fightingStateVars.distanceToPlayer);

                        Vector2 dir = (headVectorXZ - fightingStateVars.playerPosXZ).normalized;
                        Vector2 rightleft_dir = Vector2Extension.Rotate((headVectorXZ - fightingStateVars.playerPosXZ).normalized, Mathf.Sign(adding_angle) * 90.0f);

                        dir = Vector2Extension.Slerp(rightleft_dir, dir, 1.0f - ((circumference - 2.0f) / (5.0f - 2.0f)));
                        Vector3 dir3D = new Vector3(dir.x, 0, dir.y);
                        transform.parent.transform.rotation = Quaternion.LookRotation(dir3D, Vector3.up);

                    }
                    else
                    {
                        Vector2 dir = Vector2Extension.Rotate((headVectorXZ - fightingStateVars.playerPosXZ).normalized, Mathf.Sign(adding_angle)*90.0f);
                        Vector3 dir3D = new Vector3(dir.x, 0, dir.y);
                        
                        dir3D = Vector3.RotateTowards(transform.parent.transform.forward, dir3D, Mathf.Deg2Rad * 720.0f * Time.fixedDeltaTime, 1);
                        Debug.DrawRay(transform.parent.transform.position, dir3D*20.0f,Color.red);
                       

                        transform.parent.transform.rotation = Quaternion.LookRotation(dir3D,Vector3.up);
                        newPosXZ = fightingStateVars.playerPosXZ + (Vector2Extension.Rotate((headVectorXZ - fightingStateVars.playerPosXZ).normalized, adding_angle) * fightingStateVars.distanceToPlayer);
                    }

                    Vector3 newPos = Vector3.zero;
                    newPos.x = newPosXZ.x;
                    newPos.y = transform.parent.transform.position.y;
                    newPos.z = newPosXZ.y;
                    
                    
                    transform.parent.transform.position = newPos;

                    if (circumference < 0.1f)
                    {
                        fightingStateVars.state = FIGHTING_STATE.FIGHTING;
                        fightingStateVars.currentVelocity = 0;
                        fightingStateVars.currentNextStateChangeTime = Random.Range(fightingStateVars.fightingNextStateMinTime, fightingStateVars.fightingNextStateMaxTime);
                        fightingStateVars.fightingAccelleration = Random.Range(fightingStateVars.fightingAccellerationMin, fightingStateVars.fightingAccellerationMax);
                        fightingStateVars.fishAngleValue = 0.5f;

                        if (fightingStateVars.distanceToPlayer > Mathf.Sqrt(2.0f * (fightingStateVars.maxLeftRighAmplitude* fightingStateVars.maxLeftRighAmplitude)))  
                        {
                            // Pythagoras: set B^2 based on A^2 and C^2  : B = sqrt(C^2 - A^2)
                            fightingStateVars.fightingYDistance = Mathf.Sqrt(Mathf.Pow(fightingStateVars.distanceToPlayer, 2) - Mathf.Pow(fightingStateVars.maxLeftRighAmplitude, 2 ));
                        }
                        else // Pythagoras: C^2 is smaller than A^2, so A^2 and B^2 need to be changed based on C^2  : A = B   => A = sqrt(C^2 / 2))
                        {
                            fightingStateVars.fightingYDistance = Mathf.Sqrt(Mathf.Pow(fightingStateVars.distanceToPlayer, 2) / 2.0f);
                            fightingStateVars.leftRighAmplitude = fightingStateVars.fightingYDistance;
                        }
                    }
                    break;
                }
            case FIGHTING_STATE.FIGHTING:
                {
                    fightingStateVars.fightingYDistance += fightingStateVars.fleeingDriveForce * Time.fixedDeltaTime;

                    if (fightingStateVars.fightingYDistance < fightingStateVars.maxLeftRighAmplitude)
                    {
                        fightingStateVars.leftRighAmplitude = fightingStateVars.fightingYDistance;
                    }
                    else
                    {
                        fightingStateVars.leftRighAmplitude = fightingStateVars.maxLeftRighAmplitude;
                    }

                    fightingStateVars.currentNextStateChangeTime -= Time.fixedDeltaTime;

                    if (fightingStateVars.currentNextStateChangeTime <= 0)
                    {
                        fightingStateVars.currentNextStateChangeTime = Random.Range(fightingStateVars.fightingNextStateMinTime, fightingStateVars.fightingNextStateMaxTime);
                        fightingStateVars.fightingAccelleration = Random.Range(fightingStateVars.fightingAccellerationMin, fightingStateVars.fightingAccellerationMax) * -Mathf.Sign(fightingStateVars.fightingAccelleration);
                    }

                    float output_accelleration = (fightingStateVars.fightingAccelleration + fightingStateVars.playerAccelleration);



                    fightingStateVars.currentVelocity += output_accelleration;
                    fightingStateVars.currentVelocity = Mathf.Clamp(fightingStateVars.currentVelocity, -fightingStateVars.fightingVelocityMax, fightingStateVars.fightingVelocityMax);

                    fightingStateVars.fishAngleValue += fightingStateVars.currentVelocity * Time.deltaTime;
                    fightingStateVars.fishAngleValue = Mathf.Clamp01(fightingStateVars.fishAngleValue);

                    Vector2 circle_centre = fightingStateVars.playerPosXZ;

                    Vector2 sideCirclePointLeft = circle_centre + new Vector2(-fightingStateVars.fightingYDistance, fightingStateVars.leftRighAmplitude);
                    Vector2 sideCirclePointRight = circle_centre + new Vector2(-fightingStateVars.fightingYDistance, -fightingStateVars.leftRighAmplitude);

                    Vector2 fishPosition = fightingStateVars.playerPosXZ + Vector2Extension.Slerp((sideCirclePointLeft - circle_centre).normalized, (sideCirclePointRight - circle_centre).normalized, fightingStateVars.fishAngleValue) * (sideCirclePointLeft - circle_centre).magnitude;

                    if (!float.IsNaN(fishPosition.x) && !float.IsNaN(fishPosition.y))
                    {
                        transform.parent.transform.position = new Vector3(fishPosition.x,transform.parent.transform.position.y , fishPosition.y);


                        Vector2 fishFromCentreDir = (fishPosition - circle_centre).normalized;

                        Vector2 lookatXZ = Vector2.zero;
                        if (fightingStateVars.currentVelocity <= 0)
                        {
                            Vector2 lookAtLeft = Vector2Extension.Rotate(fishFromCentreDir,-90.0f);
                            lookatXZ = Vector2Extension.Slerp(lookAtLeft, fishFromCentreDir, fightingStateVars.currentVelocity + 1.0f);
                        }
                        else
                        {
                            Vector2 lookAtRight = Vector2Extension.Rotate(fishFromCentreDir, 90.0f);
                            lookatXZ = Vector2Extension.Slerp(fishFromCentreDir, lookAtRight, fightingStateVars.currentVelocity);
                        }
                    
                        transform.parent.transform.rotation = Quaternion.LookRotation(new Vector3(lookatXZ.x,0, lookatXZ.y), Vector3.up);
                    }

                    break;
                }
        }
       
    }


   
    public float GetLineStrengthPercentageLeft()
    {
        float near_edge = Mathf.Abs((fightingStateVars.fishAngleValue - 0.5f) * 2.0f);
        float min = 0.1f;

        float line_t = 1.0f - ((near_edge - min) / (1.0f - min));

        float distance_t = 1.0f - (fightingStateVars.fightingYDistance - (fightingStateVars.maxDistance - 5.0f)) / (fightingStateVars.maxDistance - (fightingStateVars.maxDistance - 5.0f));


        return Mathf.Min(line_t, distance_t);

    }

    public void SetCaughtPosition(Vector3 pos)
    {
        transform.parent.transform.rotation = Quaternion.LookRotation(Vector3.up, -Vector3.right);
        transform.parent.transform.position = pos + (transform.parent.transform.position - fishheadTransform.position);
    }

    public bool IsInDespawnableState()
    {
        switch (current_state)
        {
            case STATE.ATTRACTED:
                {
                    return false;
                }
            case STATE.AVOIDING:
                {
                    return true;
                }
            case STATE.FIGHTING:
                {
                    return false;
                }
            case STATE.INTERACTING:
                {
                    return false;
                }
            case STATE.WANDERING:
                {
                    return true;
                }
            case STATE.CAUGHT:
                {
                    return false;
                }

        }
        return false;

    }

    
}
