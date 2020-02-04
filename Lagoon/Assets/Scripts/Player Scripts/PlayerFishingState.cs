using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFishingState : BaseState
{




    // Start is called before the first frame update

    [Header("Fishing Cast Properties")]
    [Tooltip("when cast is at 0%, the velocity of the throw")]
    [SerializeField] float fishingCastVelocityDampener = 1.0f;


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
    [Tooltip("the character controller movement script")]
    [SerializeField] CharacterControllerMovement characterControllerMovement;
    [Tooltip("the third person camera script")]
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [Tooltip("the static fishing rod logic script")]
    [SerializeField] StaticFishingRodLogic staticFishingRodLogic;
    [Tooltip("the flexible fishing rod logic script")]
    [SerializeField] FishingRodLogic fishingRodLogic;
    [Tooltip("the fishing indicator")]
    [SerializeField] GameObject TODO2;
    [Tooltip("the fishing fish indicator")]
    [SerializeField] GameObject TODO;
    [Tooltip("the fishing bob logic script")]
    [SerializeField] FishingBobLogic fishingBobLogic;


 

    enum FISHING_STATE
    {
        TEMP_EXPLORING_STATE,
        PREPAIRING_ROD,
        IDLE,
        POWERING_UP,
        CASTING_ANIMATION,
        BOB_IS_FLYING,               // the bob has been thrown
        FISHING,
        FISH_FIGHTING
    };


    private FISHING_STATE fishing_state;

    int failedFishCounter = 0; // amount of times a fish tried 3x and lost interest

    float casting_angle = 45.0f; // projectile angle 


    bool heldRTInPreviousState = false;
    private void OnEnable()
    {
        fishing_state = FISHING_STATE.TEMP_EXPLORING_STATE;
        //characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
        fishingRodMesh.SetActive(false);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.NORMAL;
        heldRTInPreviousState = false;
    }

    private void OnDisable()
    {
        fishingRodMesh.SetActive(false);

        fishingBobLogic.gameObject.SetActive(false);
        TODO.gameObject.SetActive(false);
        TODO2.gameObject.SetActive(false);

        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.NORMAL;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        switch (fishing_state)
        {
            case FISHING_STATE.TEMP_EXPLORING_STATE:
                {
                    if (Input.GetButtonDown("PlayerA"))
                    {
                       fishing_state = FISHING_STATE.PREPAIRING_ROD;
                    }
                    break;
                }
            case FISHING_STATE.PREPAIRING_ROD:
                {
                    fishingRodMesh.SetActive(true);
                    fishing_state = FISHING_STATE.IDLE;
                    characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
                    break;
                }
            case FISHING_STATE.IDLE:  // not fishing
                {

                    if (heldRTInPreviousState)
                    {
                        if (Input.GetAxis("PlayerRT") < 0.6)
                        {
                            heldRTInPreviousState = false;
                        }
                    }

                    if (Input.GetAxis("PlayerRT") > 0.9 && !heldRTInPreviousState) // begin powering up the cast
                    {
                        BeginPowerUpThrow();
                    }
                    else if (Input.GetButtonDown("PlayerB"))
                    {
                        fishing_state = FISHING_STATE.TEMP_EXPLORING_STATE;
                        OnDisable(); // TEMPORARY

                        fishingRodMesh.SetActive(false);
                        fishingBobLogic.gameObject.SetActive(false);
                        TODO.gameObject.SetActive(false);
                        TODO2.gameObject.SetActive(false);


                        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
                        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
                        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.NORMAL;

                    }
                    break;
                }
            case FISHING_STATE.POWERING_UP: // the casting force is increasing and the casting position can be seen 
                {
                    castingTime += Time.deltaTime;
                    if (castingTime > 2.0f)
                    {
                        CancelPowerUpThrow();
                    }
                    else
                    {


                        TransformIndicator();

                        // StaticFishingRodLogic.RotateRodBasedOnAnalogStick();

                        if (Input.GetAxis("PlayerRT") < 0.6) // release the cast and throw the bob
                        {
                            GetComponentInChildren<Animator>().enabled = true;
                            BeginCastingAnimation();
                        }
                        else if (Input.GetButtonDown("PlayerB"))
                        {
                            CancelPowerUpThrow();
                        }
                    }
                    break;
                }
            case FISHING_STATE.CASTING_ANIMATION: // the bob has been thrown
                {
                    TransformIndicator();
                   AnimatorStateInfo test = GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (test.speed == 0)
                    {
                        GetComponentInChildren<Animator>().enabled = false;
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
            case FISHING_STATE.FISH_FIGHTING:
                {

                    break;
                }
        }

        // debug info
       // textFishingCastPower.text = "Fishing Cast Power: " + ((int)(Mathf.PingPong(currentCastTimeToMax / fishingCastTimeToMax, 1) * 100)) + "%";
    }


    void BeginPowerUpThrowANALOG()
    {
 
        fishing_state = FISHING_STATE.POWERING_UP;

        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        staticFishingRodLogic.ChangeState(StaticFishingRodLogic.STATE.ANALOG_CONTROL);

        textFishingFishCaught.text = "";

        instanceFishingCastIndicator = Instantiate(prefabFishingCastIndicator);
        TransformIndicator();
    }


    float castingTime = 0.0f;
    void BeginPowerUpThrow()
    {

        fishing_state = FISHING_STATE.POWERING_UP;

       // characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
     
        castingTime = 0.0f;
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

        heldRTInPreviousState = true;
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
        staticFishingRodLogic.ChangeState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        fishing_state = FISHING_STATE.IDLE;
    }

    void BeginCastingAnimation()
    {
        fishing_state = FISHING_STATE.CASTING_ANIMATION;
        GetComponentInChildren<Animator>().Play("Rod Flick");
    }
    void PowerUpThrowANALOG()
    {
        Vector3 cast_direction = new Vector3(fishingRodLogic.GetFlexibleTipVelocity().x, 0, fishingRodLogic.GetFlexibleTipVelocity().z).normalized;

        cast_direction = Vector3.RotateTowards(cast_direction, Vector3.up, Mathf.Deg2Rad * casting_angle, 0);

        istantanceFishingBob = Instantiate(prefabFishingBob);
        istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().Setup(this, fishingLineLogic);
        istantanceFishingBob.transform.position = fishingRodTip.position;

        fishingLineLogic.setupLine(istantanceFishingBob);
        fishingLineLogic.LooseString();

        Collider[] colliders = istantanceFishingBob.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) // ignore collisions to the player
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), colliders[i]);
            Physics.IgnoreCollision(GetComponent<CharacterController>(), colliders[i]);
        }


        Vector2 test = new Vector2(fishingRodLogic.GetFlexibleTipVelocity().x, fishingRodLogic.GetFlexibleTipVelocity().z);
        istantanceFishingBob.GetComponent<Rigidbody>().velocity = (cast_direction * test.magnitude * fishingCastVelocityDampener);

        fishing_state = FISHING_STATE.BOB_IS_FLYING;
    }


    void PowerUpThrow()
    {
        Vector3 cast_direction = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        cast_direction = Vector3.RotateTowards(cast_direction, Vector3.up, Mathf.Deg2Rad * casting_angle, 0);

        istantanceFishingBob = Instantiate(prefabFishingBob);
        istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().Setup(this, fishingLineLogic);
        istantanceFishingBob.transform.position = fishingRodTip.position;

        fishingLineLogic.setupLine(istantanceFishingBob);
        fishingLineLogic.LooseString();

        Collider[] colliders = istantanceFishingBob.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) // ignore collisions to the player
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), colliders[i]);
            Physics.IgnoreCollision(GetComponent<CharacterController>(), colliders[i]);
        }


      
        istantanceFishingBob.GetComponent<Rigidbody>().velocity = (cast_direction * Mathf.PingPong(castingTime, 1) * 10.0f);

        fishing_state = FISHING_STATE.BOB_IS_FLYING;
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        thirdPersonCamera.look_at_target = istantanceFishingBob.transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FISHING;
    }

    // Calculate landing of Fishing Bob
    void TransformIndicatorANALOG() // note. this prediction is only accurate if there is no air drag on the bob
    {
        // projectile motion calculations are done in 2d for simplicity and then converted to 3d

        Vector3 Velocity = fishingRodLogic.GetFlexibleTipVelocity() * fishingCastVelocityDampener;
        Vector3 cast_position = fishingRodTip.position;

        Vector2 XZVelocity = new Vector2(Velocity.x, Velocity.z);
        float range = calculateProjectileRange(XZVelocity.magnitude);
        

        cast_position.x += XZVelocity.normalized.x* range;
        cast_position.z += XZVelocity.normalized.y * range;
        cast_position.y = GlobalVariables.WATER_LEVEL + 0.01f;

        

        instanceFishingCastIndicator.transform.position = cast_position;
    }

    void TransformIndicator() // note. this prediction is only accurate if there is no air drag on the bob
    {
        // projectile motion calculations are done in 2d for simplicity and then converted to 3d

        Vector3 Velocity = transform.forward * Mathf.PingPong(castingTime,1)* 10.0f;
        Vector3 cast_position = fishingRodTip.position;

        Vector2 XZVelocity = new Vector2(Velocity.x, Velocity.z);
        float range = calculateProjectileRange(XZVelocity.magnitude);


        cast_position.x += XZVelocity.normalized.x * range;
        cast_position.z += XZVelocity.normalized.y * range;
        cast_position.y = GlobalVariables.WATER_LEVEL + 0.01f;



        instanceFishingCastIndicator.transform.position = cast_position;
    }

    private float calculateProjectileRange(float initVelocity)
    {
        float init_velocity = initVelocity;
        float init_angle = casting_angle;
        float y_offset = fishingRodTip.position.y - GlobalVariables.WATER_LEVEL;

        // quadratic formula
        float a = 0.5f * Physics.gravity.y;
        float b = init_velocity * Mathf.Sin(Mathf.Deg2Rad * init_angle);
        float c = y_offset;

        float unchanging_part = Mathf.Sqrt((b * b) - 4.0f * a * c);
        float t1 = (-b + unchanging_part) / (2.0f * a);
        float t2 = (-b - unchanging_part) / (2.0f * a);

        float landing_time = Mathf.Max(t1, t2);

        float range = init_velocity * Mathf.Cos(Mathf.Deg2Rad * init_angle) * landing_time;

        return range;
    }

    void ReelIn() // bring the bob closer by reeling in
    {
        if (istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.FISH_BITE) //there is a fish attached
        {
            istantanceFishingBob.GetComponentInChildren<FishingBobLogic>().FishCaught();
            if (istantanceFishingBob != null)
            {
                FishHookedBegin();
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
                    staticFishingRodLogic.ChangeState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
                    characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
                    thirdPersonCamera.look_at_target = transform;
                    thirdPersonCamera.current_state = ThirdPersonCamera.STATE.NORMAL;
                }

            }
        }

    }

    void CancelCasted()
    {
        thirdPersonCamera.look_at_target = transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.NORMAL;

        if (istantanceFishingBob != null) // if there is a bob currecntly attached, destroy it
        {
            Destroy(istantanceFishingBob);
        }
        if (instanceFishingCastIndicator != null)
        {
            Destroy(instanceFishingCastIndicator);
        }

        fishing_state = FISHING_STATE.IDLE;
        staticFishingRodLogic.ChangeState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
    }


    void FishHookedBegin()
    {
        Destroy(istantanceFishingBob);
        fishing_state = FISHING_STATE.FISH_FIGHTING;
        textFishingFishCaught.color = Color.green;
        textFishingFishCaught.text = "FISH CAUGHT!";
        staticFishingRodLogic.ChangeState(StaticFishingRodLogic.STATE.ANALOG_CONTROL_H_ONLY);
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
