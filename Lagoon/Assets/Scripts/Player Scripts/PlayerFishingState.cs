using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFishingState : BaseState
{




    // Start is called before the first frame update

    [Header("Fishing Cast Properties")]
    [Tooltip("when cast is at 0%, the velocity of the throw")]
    [SerializeField] float fishingCastPowerMin = 5;
    [Tooltip("when cast is at 100%, the velocity of the throw")]
    [SerializeField] float fishingCastPowerMax = 10;
    [Tooltip("time it takes to charge up throw from 0% to 100%")]
    [SerializeField] float fishingCastTimeToMax = 1;

    [Header("Fishing Reel In Properties")]
    [Tooltip("speed of reeling in")]
    [SerializeField] float fishingReelInSpeed = 15;

    [Header("Prefabs")]
    [Tooltip("FishingBob Prefab")]
    [SerializeField] GameObject prefabFishingBob;
    [Tooltip("Fishing Cast Indicator Prefab")]
    [SerializeField] GameObject prefabFishingCastIndicator;

    [Header("Debugging Text")]
    [Tooltip("text for showing fishing cast power")]
    [SerializeField] Text textFishingCastPower;
    [Tooltip("text for showing if a fish was caught")]
    [SerializeField] Text textFishingFishCaught;

    [Header("Pointers")]
    [Tooltip("the transform of the camera")]
    [SerializeField] Transform cameraTransform;
    [Tooltip("the flexible fishing rod tip")]
    [SerializeField] Transform fishingRodTip;
    [Tooltip("the fishing line logic in the fishing line object")]
    [SerializeField] FishingLineLogic fishingLineLogic;
    [Tooltip("the fishing rod mesh")]
    [SerializeField] GameObject fishingRodMesh;

    float currentCastTimeToMax = 0;


    enum FISHING_STATE
    {
        PREPAIRING_ROD,
        IDLE,
        POWERING_UP,
        CASTING_ANIMATION,
        BOB_IS_FLYING,               // the bob has been thrown
        FISHING,
    };


    private FISHING_STATE fishing_state;
    private GameObject istantanceFishingBob; // the instantiated fishing bob
    private GameObject instanceFishingCastIndicator;

    int failedFishCounter = 0; // amount of times a fish tried 3x and lost interest

    float casting_angle = 45.0f; // projectile angle 

    private void OnEnable()
    {
        fishing_state = FISHING_STATE.PREPAIRING_ROD;
    }

    private void OnDisable()
    {

        fishingRodMesh.SetActive(false);
        if (istantanceFishingBob != null)
        {
            Destroy(istantanceFishingBob);
        }
        if (instanceFishingCastIndicator != null)
        {
            Destroy(instanceFishingCastIndicator);
        }
    }

    // Update is called once per frame
    public override void StateUpdate()
    {





        switch (fishing_state)
        {
            case FISHING_STATE.PREPAIRING_ROD:
                {
                    fishingRodMesh.SetActive(true);
                    fishing_state = FISHING_STATE.IDLE;
                    break;
                }
            case FISHING_STATE.IDLE:  // not fishing
                {

                    if (Input.GetButtonDown("PlayerRB")) // begin powering up the cast
                    {
                        BeginPowerUpThrow();
                    }
                    else if (Input.GetButtonDown("PlayerB"))
                    {
                        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
                    }
                    break;
                }
            case FISHING_STATE.POWERING_UP: // the casting force is increasing and the casting position can be seen 
                {

                    currentCastTimeToMax += Time.deltaTime;
                    TransformIndicator();

                    if (currentCastTimeToMax >= fishingCastTimeToMax * 2.0f)
                    {
                        CancelPowerUpThrow();
                    } 
                    else if (!Input.GetButton("PlayerRB")) // release the cast and throw the bob
                    {
                        BeginCastingAnimation();
                    }
                    else if (Input.GetButtonDown("PlayerB"))
                    {
                        CancelPowerUpThrow();
                    }
                    break;
                }
            case FISHING_STATE.CASTING_ANIMATION: // the bob has been thrown
                {
                    TransformIndicator();
                   AnimatorStateInfo test = GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (test.speed == 0)
                    {
                        PowerUpThrow();
                    }


                    break;
                }
            case FISHING_STATE.BOB_IS_FLYING:
                {

                    if (istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.HIT_LAND_OR_FLOATING || istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.HIT_WATER) // bob has settled
                    {
                        Destroy(instanceFishingCastIndicator);
                        fishing_state = FISHING_STATE.FISHING;
                        fishingLineLogic.SetState(FishingLineLogic.STATE.NORMAL);
                    }
                    else // bob is moving through the air
                    {
                        if (Input.GetButtonDown("PlayerB"))
                        {
                            CancelCasted();
                        }
                    }



                    break;
                }
            case FISHING_STATE.FISHING:
                {
                    if (Input.GetButton("PlayerRB")) // bring the bob closer by reeling in
                    {
                        ReelIn();
                    }
                    else if (Input.GetButtonDown("PlayerB"))
                    {
                        CancelCasted();
                    }
                    break;
                }
        }

        // debug info
        textFishingCastPower.text = "Fishing Cast Power: " + ((int)(Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1) * 100)) + "%";
    }


    void BeginPowerUpThrow()
    {
        currentCastTimeToMax = 0;
        fishing_state = FISHING_STATE.POWERING_UP;

        textFishingFishCaught.text = "";

        instanceFishingCastIndicator = Instantiate(prefabFishingCastIndicator);
        TransformIndicator();
    }

    void CancelPowerUpThrow()
    {
        if (istantanceFishingBob != null) // if there is a bob currecntly attached, destroy it
        {
            Destroy(istantanceFishingBob);
        }
        if (instanceFishingCastIndicator != null)
        {
            Destroy(instanceFishingCastIndicator);
        }

        fishing_state = FISHING_STATE.IDLE;
    }

    void BeginCastingAnimation()
    {
        fishing_state = FISHING_STATE.CASTING_ANIMATION;
        GetComponentInChildren<Animator>().Play("Rod Flick");
    }
    void PowerUpThrow()
    {
        Vector3 cast_direction = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = Vector3.Cross(cast_direction, Vector3.up); // get perpendicular vector to cast direction
        Quaternion quat = Quaternion.AngleAxis(casting_angle, right).normalized; // create a quaternion which rotates 45 degrees around the right vector 
        cast_direction = (quat * cast_direction).normalized; // multiply the cast direction by the quaternion to rotate the cast direction by 45 degress around the right vector, giving the correct y value to the direction

        istantanceFishingBob = Instantiate(prefabFishingBob);
        istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().Setup(this);
        istantanceFishingBob.transform.position = fishingRodTip.position;

        fishingLineLogic.setupLine(istantanceFishingBob);
        fishingLineLogic.LooseString();

        Collider[] colliders = istantanceFishingBob.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) // ignore collisions to the player
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), colliders[i]);
            Physics.IgnoreCollision(GetComponent<CharacterController>(), colliders[i]);
        }


        istantanceFishingBob.GetComponent<Rigidbody>().velocity = (cast_direction * Mathf.Lerp(fishingCastPowerMin, fishingCastPowerMax, Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1)));

        fishing_state = FISHING_STATE.BOB_IS_FLYING;
    }



    // Calculate landing of Fishing Bob
    void TransformIndicator() // note. this prediction is only accurate if there is no air drag on the bob
    {
        // projectile motion calculations are done in 2d for simplicity and then converted to 3d

        float init_velocity = Mathf.Lerp(fishingCastPowerMin,fishingCastPowerMax, Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1));
        float init_angle = casting_angle;
        float y_offset = fishingRodTip.position.y - GlobalVariables.WATER_LEVEL;

        // calculate time of landing of projectile

        // quadratic formula
        float a = 0.5f * Physics.gravity.y;
        float b = init_velocity * Mathf.Sin(Mathf.Deg2Rad*init_angle);
        float c = y_offset;

        float unchanging_part = Mathf.Sqrt((b * b) - 4.0f * a * c) ;
        float t1 = (-b + unchanging_part)  / (2.0f * a);
        float t2 = (-b - unchanging_part) / (2.0f * a);

        float landing_time = Mathf.Max(t1, t2);

        float XZ_range = init_velocity * Mathf.Cos(Mathf.Deg2Rad * init_angle) * landing_time;


        Vector3 cast_position = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        cast_position *= XZ_range;
        cast_position += fishingRodTip.position;
        cast_position.y = GlobalVariables.WATER_LEVEL + 0.01f;

        

        instanceFishingCastIndicator.transform.position = cast_position;
    }


    void ReelIn() // bring the bob closer by reeling in
    {
        if (istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.FISH_BITE) //there is a fish attached
        {
            istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().FishCaught();
            if (istantanceFishingBob != null)
            {
                Destroy(istantanceFishingBob);
                fishing_state = FISHING_STATE.IDLE;
                textFishingFishCaught.color = Color.green;
                textFishingFishCaught.text = "FISH CAUGHT!";
            }
        }
        else // no fish attached
        {
            Vector3 directionVector = (fishingRodTip.position - istantanceFishingBob.transform.position).normalized;
         //   istantanceFishingBob.GetComponent<Rigidbody>().AddForce(directionVector * fishingReelInSpeed, ForceMode.Acceleration);

            fishingLineLogic.ReelIn();

            if (Vector3.Distance(istantanceFishingBob.transform.position, fishingRodTip.position) < 1.0f) // bob is at the player
            {
                if (istantanceFishingBob != null)
                {
                    Destroy(istantanceFishingBob);
                    fishing_state = FISHING_STATE.IDLE;
                }

            }
        }

    }

    void CancelCasted()
    {
        if (istantanceFishingBob != null) // if there is a bob currecntly attached, destroy it
        {
            Destroy(istantanceFishingBob);
        }
        if (instanceFishingCastIndicator != null)
        {
            Destroy(instanceFishingCastIndicator);
        }

        fishing_state = FISHING_STATE.IDLE;
    }

    // -- Public Functions -- //

    public void ResetFailedFishCounter()
    {
        failedFishCounter = 0;
    }
    public void FailedFishCounterIncrement()
    {
        failedFishCounter++;
    }
    public int GetFailedFishCounter()
    {
        return failedFishCounter;
    }
}
