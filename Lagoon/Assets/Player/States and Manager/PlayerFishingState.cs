using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerFishingState : BaseState
{




    // Start is called before the first frame update

    [Header("Fishing Cast Properties")]
    [Tooltip("when cast is at 0%, the velocity of the throw")]
    [SerializeField] float fishingCastVelocity = 20.0f;
    [SerializeField] float fishingCastVelocityDampener = 1.0f;


    [Header("Fishing Reel In Properties")]
    [Tooltip("speed of reeling in")]
    [SerializeField] float fishingReelInSpeed = 15;


    [Header("Fish Bite Attempt Properties")]
    [Tooltip("min time for time between bite attempts")]
    [SerializeField] float fishbiteTimerMin = 2.0f;         // min time for time between bite attempts
    [Tooltip("max time for time between bite attempts")]
    [SerializeField] float fishbiteTimerMax = 4.0f;         // max time for time between bite attempts

    [Header("Debugging Text")]
    [Tooltip("text for showing fishing cast power")]
    [SerializeField] Text textFishingCastPower;
    [SerializeField] Text DEBUG_FISH_UNHOOK;
    [SerializeField] Text DEBUG_FISH_LINE_SNAP;

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
    [SerializeField] GameObject fishingProjectileIndicator;
    [Tooltip("the fishing fish indicator")]
    [SerializeField] FishingUI fishingIndicatorLogic;
    [Tooltip("the fishing bob logic script")]
    [SerializeField] GameObject fishingBob;

    [SerializeField] Animator fixedRodAnimator;
    [SerializeField] Collider islandCollider;


    enum FISHING_STATE
    {
        PREPAIRING_ROD,
        IDLE,
        POWERING_UP,
        CASTING_ANIMATION,
        BOB_IS_FLYING,               // the bob has been thrown
        FISHING,
        INTERACTING_FISH,
        BITING_FISH,
        FISH_FIGHTING
    };


    private FISHING_STATE fishing_state;

  

    float casting_angle = 45.0f; // projectile angle 

    private void OnEnable()
    {
        fishing_state = FISHING_STATE.PREPAIRING_ROD;
        fishingRodMesh.SetActive(true);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        fixedRodAnimator.enabled = false;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FREE;

        fishingIndicatorLogic.gameObject.SetActive(true);
        fishingIndicatorLogic.AttachLookAtTransform(cameraTransform);
    }

    private void OnDisable()
    {
        fishingRodMesh.SetActive(false);
        fishingLineLogic.gameObject.SetActive(false);
        fishingBob.SetActive(false);

        fishingIndicatorLogic.gameObject.SetActive(false);
        fishingProjectileIndicator.gameObject.SetActive(false);

        thirdPersonCamera.look_at_target = transform; // set target back to player
    }

    // Update is called once per frame
    public override void StateUpdate()
    {

        switch (fishing_state)
        {
            case FISHING_STATE.PREPAIRING_ROD:
                {
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
                    characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
                    castingTime += Time.deltaTime;
                    //if (castingTime > 2.0f)
                    //{
                    //    CancelPowerUpThrow();
                    //}
                    //else
                    //{
                        TransformIndicator();
                        if (Input.GetButtonUp("PlayerRB")) // release the cast and throw the bob
                        {
                            fixedRodAnimator.enabled = true;
                            BeginCastingAnimation();
                        }
                        else if (Input.GetButtonDown("PlayerB"))
                        {
                            CancelPowerUpThrow();
                        }
                  //  }
                    break;
                }
            case FISHING_STATE.CASTING_ANIMATION: // the bob has been thrown
                {
                    TransformIndicator();
                   AnimatorStateInfo test = fixedRodAnimator.GetCurrentAnimatorStateInfo(0);
                    if (test.speed == 0)
                    {
                        fixedRodAnimator.enabled = false;
                        PowerUpThrow();
                    }


                    break;
                }
            case FISHING_STATE.BOB_IS_FLYING:
                {
                    DEBUG_FISH_UNHOOK.text = ""; // DEBUG STUFF


                    if (fishingBob.GetComponentInChildren<BuoyancyPhysics>().GetCurrentState() == BuoyancyPhysics.STATE.IN_WATER) // bob has settled
                    {
                        fishingLineLogic.BeganFishing();
                        fishingBob.GetComponentInChildren<FishingBobLogic>().BeganFishing();
                        fishingProjectileIndicator.SetActive(false);
                        fishing_state = FISHING_STATE.FISHING;
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
                    else if (fishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.FISH_INTERACTING)
                    {
                        BeginFishInteractingState();
                    }
                    
                    break;
                }
            case FISHING_STATE.INTERACTING_FISH:
                {
                    if (Input.GetButton("PlayerRB"))
                    {
                        ReelIn();
                    }

                    if (Input.GetButtonDown("PlayerB"))
                    {
                        CancelCasted();
                    }
                    else
                    {
                        if (interactingFish.IsInStateInteracting())
                        {
                            FishInteractingUpdate();
                        }
                        else
                        {
                            FishInteractingFailed();
                        }
                    }
                    break;
                }
            case FISHING_STATE.BITING_FISH:
                {
                    if (Input.GetButton("PlayerRB"))
                    {
                        FishFightingBegin();
                    }
                    else
                    {
                        FishBiteUpdate();
                    }

                    if (Input.GetButtonDown("PlayerB"))
                    {
                        CancelCasted();
                    }

                    break;
                }
            case FISHING_STATE.FISH_FIGHTING:
                {



                    switch(interactingFish.GetFightingState())
                    {
                        case FishLogic.FIGHTING_STATE.LEFT:
                            {
                                fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_LEFT);
                                fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x,GlobalVariables.WATER_LEVEL + 1.0f , interactingFish.transform.position.z));
                                break;
                            }
                        case FishLogic.FIGHTING_STATE.RIGHT:
                            {
                                fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_RIGHT);
                                fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x, GlobalVariables.WATER_LEVEL + 1.0f, interactingFish.transform.position.z));
                                break;
                            }
                        default:
                            {
                                fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.NOT_ACTIVE);
                                break;
                            }
                    }

                    if (Input.GetAxis("PlayerLH") < -0.5f)
                    {
                        staticFishingRodLogic.SetFishFightingState(StaticFishingRodLogic.FISH_FIGHTING_STATE.LEFT);
                    }
                    else if (Input.GetAxis("PlayerLH") > 0.5f)
                    {
                        staticFishingRodLogic.SetFishFightingState(StaticFishingRodLogic.FISH_FIGHTING_STATE.RIGHT);
                    }
                    else
                    {
                        staticFishingRodLogic.SetFishFightingState(StaticFishingRodLogic.FISH_FIGHTING_STATE.MIDDLE);
                    }

                    if (islandCollider.bounds.Contains(interactingFish.transform.position))
                    {
                        // fish caught
                        DEBUG_FISH_UNHOOK.text = "Fish Caught!";
                        FishFightSuccess();
                    }
                    else if (interactingFish.GetLineStrengthPercentageLeft() <= 0)
                    {
                        FishFightLineSnapped();
                    }
                    else if (Input.GetButtonDown("PlayerB"))
                    {
                        FishFightLineSnapped();
                    }
                    else if (Input.GetButton("PlayerRB"))
                    {
                        interactingFish.ReelingIn();
                    }
                    
                     


                    break;
                }
        }
    }




    float castingTime = 0.0f;
    void BeginPowerUpThrow()
    {
        fishing_state = FISHING_STATE.POWERING_UP;     
        castingTime = 0.0f;
        fishingProjectileIndicator.SetActive(true);
        TransformIndicator();
    }

    void CancelPowerUpThrow()
    {
        fishingBob.SetActive(false);
        fishingProjectileIndicator.SetActive(false);

        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        fishing_state = FISHING_STATE.IDLE;
    }

    void BeginCastingAnimation()
    {
        fishing_state = FISHING_STATE.CASTING_ANIMATION;
        GetComponentInChildren<Animator>().Play("Rod Flick");
    }


    void PowerUpThrow()
    {
        Vector3 cast_direction = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        cast_direction = Vector3.RotateTowards(cast_direction, Vector3.up, Mathf.Deg2Rad * casting_angle, 0);

        fishingLineLogic.gameObject.SetActive(true);
        fishingBob.SetActive(true);
        fishingLineLogic.CastLine();




        Collider[] colliders = fishingBob.gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) // ignore collisions to the player
        {
          //  Physics.IgnoreCollision(GetComponent<Collider>(), colliders[i]);
            Physics.IgnoreCollision(GetComponent<CharacterController>(), colliders[i]);
        }
        fishingBob.GetComponentInChildren<FishingBobLogic>().CastBob(fishingRodTip.position, cast_direction * Mathf.PingPong(castingTime, 1.0f) * fishingCastVelocity);




        fishing_state = FISHING_STATE.BOB_IS_FLYING;
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        thirdPersonCamera.look_at_target = fishingBob.transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.CLAMPED_LOOK_AT;
    }


    void TransformIndicator() // note. this prediction is only accurate if there is no air drag on the bob
    {
        // projectile motion calculations are done in 2d for simplicity and then converted to 3d

        Vector3 Velocity = transform.forward * Mathf.PingPong(castingTime,1.0f)* fishingCastVelocity;
        Vector3 cast_position = fishingRodTip.position;

        Vector2 XZVelocity = new Vector2(Velocity.x, Velocity.z);
        float range = calculateProjectileRange(XZVelocity.magnitude);


        cast_position.x += XZVelocity.normalized.x * range;
        cast_position.z += XZVelocity.normalized.y * range;
        cast_position.y = GlobalVariables.WATER_LEVEL + 0.01f;

        fishingProjectileIndicator.transform.position = cast_position;
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



    float currentReelInTime = 0;
    void ReelIn() // bring the bob closer by reeling in
    {
        fishingBob.GetComponentInChildren<FishingBobLogic>().ScareNearbyFish();
        currentReelInTime += Time.deltaTime;

        if (currentReelInTime > 1.0f / fishingReelInSpeed)
        {
            currentReelInTime = 0;
            fishingLineLogic.ReelIn();

            if (fishingLineLogic.IsFullyReeledIn())
            {
                CancelCasted();
            }
        }
    }

    void CancelCasted()
    {
        thirdPersonCamera.look_at_target = transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FREE;

        fishingLineLogic.gameObject.SetActive(false);
        fishingBob.SetActive(false);
        fishingProjectileIndicator.SetActive(false);

        fishing_state = FISHING_STATE.IDLE;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.FREE_MOVEMENT;

        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.NOT_ACTIVE);

        if (interactingFish != null)
        {
            if (interactingFish.IsInStateInteracting())
            {

                interactingFish.LostInterestInFishingBob(1.0f);
            }
            interactingFish = null;
        }
    }


    // -- Fish Interacting State -- //

    FishLogic interactingFish;

    float currenFishBiteAttemptTime = 0;
    int failedFishCounter = 0; // amount of times a fish tried 3x and lost interest
    int biteAttempts = 0;
    void BeginFishInteractingState()
    {
        interactingFish = fishingBob.GetComponentInChildren<FishingBobLogic>().GetInteractingFish();
        currenFishBiteAttemptTime = Random.Range(fishbiteTimerMin, fishbiteTimerMax);
        biteAttempts = 0;
        fishing_state = FISHING_STATE.INTERACTING_FISH;

    }

    void FishInteractingUpdate()
    {
        currenFishBiteAttemptTime -= Time.deltaTime;

        if (currenFishBiteAttemptTime <= 0)
        {
            currenFishBiteAttemptTime = Random.Range(fishbiteTimerMin, fishbiteTimerMax);

            if (interactingFish.BiteAttempt())
            {
                FishBiteBegin();
            }
            else
            {
                biteAttempts++;
                if (biteAttempts == 3)
                {
                    failedFishCounter++;
                    if (failedFishCounter == 3)
                    {
                        FishBiteBegin();
                    }
                    else
                    {
                        fishingBob.GetComponentInChildren<FishingBobLogic>().FishTestedLure();
                        fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x, GlobalVariables.WATER_LEVEL + 1.0f, interactingFish.transform.position.z));
                        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_TEST);
                        

                        interactingFish.LostInterestInFishingBob(5.0f);
                        FishInteractingFailed();
                    }
                }        
                else
                {
                    fishingBob.GetComponentInChildren<FishingBobLogic>().FishTestedLure();
                    fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x, GlobalVariables.WATER_LEVEL + 1.0f, interactingFish.transform.position.z));
                    fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_TEST);
                    
                }
            }
        }
    }

    void FishInteractingFailed()
    {
        fishing_state = FISHING_STATE.FISHING;
    }
    // -- //

    // -- Bite Fish State -- //

    float fishBiteHoldTimeLeft = 0;

    void FishBiteBegin()
    {
        failedFishCounter = 0;
        fishingBob.GetComponentInChildren<FishingBobLogic>().FishBitLure();
        fishing_state = FISHING_STATE.BITING_FISH;
        fishBiteHoldTimeLeft = interactingFish.GetFishBiteHoldTime();

        fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x, GlobalVariables.WATER_LEVEL + 1.0f, interactingFish.transform.position.z));
        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_BITE);

    }

    void FishBiteUpdate()
    {
        fishBiteHoldTimeLeft -= Time.deltaTime;
        if (fishBiteHoldTimeLeft <= 0)
        {
            FishBiteFailed();
        }
    }

    void FishBiteFailed()
    {
        interactingFish.LostInterestInFishingBob(5.0f);
        fishing_state = FISHING_STATE.FISHING;
    }

    // -- //

    // -- Fish Fighting State
    void FishFightingBegin()
    {
        fishing_state = FISHING_STATE.FISH_FIGHTING;
        fishingBob.GetComponentInChildren<FishingBobLogic>().BeganFighting();
        fishingLineLogic.BeganFighting(interactingFish);
        staticFishingRodLogic.SetFishFightingState(StaticFishingRodLogic.FISH_FIGHTING_STATE.MIDDLE);
        interactingFish.BeginFighting(new Vector2(transform.position.x, transform.position.z), staticFishingRodLogic, DEBUG_FISH_UNHOOK,DEBUG_FISH_LINE_SNAP);
    }

    void FishFightLineSnapped()
    {
        interactingFish.LostInterestInFishingBob(20.0f);

        interactingFish = null;
        CancelCasted();

    }

    void FishFightSuccess()
    {
        Destroy(interactingFish.transform.parent.gameObject);
        interactingFish = null;
        CancelCasted();
    }

    // -- // 
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
