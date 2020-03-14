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


    [Header("Fishing Reel In Properties")]
    [Tooltip("speed of reeling in")]
    [SerializeField] float fishingReelInMaxSpeed = 15;


    [Header("Fish Bite Attempt Properties")]
    [Tooltip("min time for time between bite attempts")]
    [SerializeField] float fishbiteTimerMin = 2.0f;         // min time for time between bite attempts
    [Tooltip("max time for time between bite attempts")]
    [SerializeField] float fishbiteTimerMax = 4.0f;         // max time for time between bite attempts


    [Header("Pointers")]
    [Tooltip("the transform of the camera")]
    [SerializeField] Transform cameraTransform;
    [Tooltip("the flexible fishing rod tip")]
    [SerializeField] Transform fishingRodTip;
    [Tooltip("the fishing line logic in the fishing line object")]
    [SerializeField] FishingLineLogic fishingLineLogic;
    [Tooltip("the character controller movement script")]
    [SerializeField] CharacterControllerMovement characterControllerMovement;
    [Tooltip("the third person camera script")]
    [SerializeField] ThirdPersonCamera thirdPersonCamera;
    [Tooltip("the static fishing rod logic script")]
    [SerializeField] StaticFishingRodLogic staticFishingRodLogic;
    [Tooltip("the fishing rod logic script")]
    [SerializeField] FishingRodLogic fishingRodLogic;
    [Tooltip("the fishing indicator")]
    [SerializeField] GameObject fishingProjectileIndicator;
    [Tooltip("the fishing fish indicator")]
    [SerializeField] FishingUI fishingIndicatorLogic;
    [Tooltip("the fishing bob logic script")]
    [SerializeField] GameObject fishingBob;

    [SerializeField] Transform handTransform;
    [SerializeField] CelebrationCamera celebrationCamera;

    [SerializeField] Animator fixedRodAnimator;

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
        FISH_FIGHTING,
        FISHING_VICTORY
    };


    private FISHING_STATE fishing_state;

  

    float casting_angle = 45.0f; // projectile angle 



    bool previousFrameIncludedAttractedFish = false;

    // On Enabled Components set
        // fishingRodMesh
        // static fishing rod origin 
        // character controller
        // fixed rod animator
        // third person camera
        // fishing indicator
    public void OnEnable()
    {
        fishing_state = FISHING_STATE.PREPAIRING_ROD;

        staticFishingRodLogic.gameObject.SetActive(true);
        fishingRodLogic.gameObject.SetActive(true);

        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
       
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
        fixedRodAnimator.enabled = true;

        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FREE;

        fishingIndicatorLogic.gameObject.SetActive(true);
        fishingIndicatorLogic.AttachLookAtTransform(cameraTransform);

        GM_.Instance.ui.helperButtons.DisableAll();
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GM_.Instance.ui.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");
    }

    public void OnDisable()
    {
        if (fishingLineLogic != null)
            fishingLineLogic.gameObject.SetActive(false);

        if (staticFishingRodLogic != null)
            staticFishingRodLogic.gameObject.SetActive(false);

        if (fishingRodLogic != null)
            fishingRodLogic.gameObject.SetActive(false);

        if (fishingBob != null)
            fishingBob.SetActive(false);

        if (fixedRodAnimator != null)
            fixedRodAnimator.enabled = false;

        if (fishingIndicatorLogic != null)
            fishingIndicatorLogic.gameObject.SetActive(false);

        if (fishingProjectileIndicator != null)
            fishingProjectileIndicator.gameObject.SetActive(false);

        GM_.Instance.ui.helperButtons.DisableAll();

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



                    if (GM_.Instance.input.GetAxis(InputManager.AXIS.LV) < -0.8f) // begin powering up the cast
                    {
                        BeginPowerUpThrow();
                    }
                    else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);

                        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
                    }
                    break;
                }
            case FISHING_STATE.POWERING_UP: // the casting force is increasing and the casting position can be seen 
                {
                    characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
                    castingTime += Time.deltaTime;
                    TransformIndicator();

                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                        StateManager.ChangeState(PlayerScriptManager.STATE.EXPLORING);
                    }
                    else if (castingTimeout >= 0.3f)
                    {
                        CancelPowerUpThrow();
                    }
                    else if (GM_.Instance.input.GetAxis(InputManager.AXIS.LV) < -0.9f)
                    {
                        castingTimeout = 0.0f;
                    }
                    else if (GM_.Instance.input.GetAxis(InputManager.AXIS.LV) > 0.9f) // release the cast and throw the bob
                    {
                        fixedRodAnimator.enabled = true;
                        BeginCastingAnimation();
                    }
                    else
                    {
                        castingTimeout += Time.deltaTime;
                    }

                    break;
                }
            case FISHING_STATE.CASTING_ANIMATION: // the bob has been thrown
                {
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


                    if (fishingBob.GetComponentInChildren<BuoyancyPhysics>().GetCurrentState() == BuoyancyPhysics.STATE.IN_WATER || fishingBob.GetComponentInChildren<BuoyancyPhysics>().IsInEquilibrium()) // bob has settled
                    {
                        fishingLineLogic.BeganFishing();
                        fishingBob.GetComponentInChildren<FishingBobLogic>().BeganFishing();
                        fishingProjectileIndicator.SetActive(false);
                        fishing_state = FISHING_STATE.FISHING;

                        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.RT, "Reel In / Hook Fish");


                    }
                    else // bob is moving through the air
                    {
                        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                        {
                            CancelCasted();
                            GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                        }
                    }
                    break;
                }
            case FISHING_STATE.FISHING:
                {
                    float reelAxis = GM_.Instance.input.GetAxis(InputManager.AXIS.RT);

                    if (fishingBob.GetComponentInChildren<FishingBobLogic>().HasAttractedFish())
                    {
                        if (!previousFrameIncludedAttractedFish)
                        {
                            GM_.Instance.input.SetVibrationRandomPulsing(InputManager.VIBRATION_MOTOR.LEFT, 0.2f, 0.1f, 1.0f, 0.25f);
                            previousFrameIncludedAttractedFish = true;
                        }
                    }
                    else
                    {
                        if (!fishingBob.GetComponentInChildren<FishingBobLogic>().IsFishInteracting())
                        {
                            GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.LEFT ,0);
                            previousFrameIncludedAttractedFish = false;
                        }
                    }

                    GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.RIGHT, reelAxis *0.1f);
                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        CancelCasted();
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    }
                    else if (GM_.Instance.input.GetAxis(InputManager.AXIS.RT) > 0.01f) // bring the bob closer by reeling in
                    {
                        ReelIn(reelAxis);
                    }
                    else if (fishingBob.GetComponentInChildren<FishingBobLogic>().GetState() == FishingBobLogic.STATE.FISH_INTERACTING)
                    {
                        BeginFishInteractingState();
                    }
                    
                    break;
                }
            case FISHING_STATE.INTERACTING_FISH:
                {


                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        CancelCasted();
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    }
                    else if (GM_.Instance.input.GetAxis(InputManager.AXIS.RT) > 0.5f)
                    {
                        if (interactingFishWontFrightenTime < 0.0f)
                        {
                            FailedHookAttempt();
                        }
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
                    if (GM_.Instance.input.GetAxis(InputManager.AXIS.RT) > 0.5f)
                    {
                        FishFightingBegin();
                    }
                    else
                    {
                        FishBiteUpdate();
                    }

                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        CancelCasted();
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                    }

                    break;
                }
            case FISHING_STATE.FISH_FIGHTING:
                {
                    GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.LEFT, 1.0f - interactingFish.GetLineStrengthPercentageLeft());

                    bool game_over = false;
                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        game_over = true;
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                        FishFightLineSnapped();
                    }

                    staticFishingRodLogic.SetFishFightingState(GM_.Instance.input.GetAxis(InputManager.AXIS.LH));

                    if (game_over != true)
                    {
                        GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.RIGHT, GM_.Instance.input.GetAxis(InputManager.AXIS.RT) * 0.5f);


                        interactingFish.SetPlayerAccelleration(GM_.Instance.input.GetAxis(InputManager.AXIS.LH));

                        if (interactingFish.GetLineStrengthPercentageLeft() <= 0.001f)
                        {
                            GM_.Instance.input.SetVibrationBoth(0, 0);
                            FishFightLineSnapped();
                        }
                        else if (interactingFish.IsFishCaught())
                        {
                            //fish caught
                            GM_.Instance.input.SetVibrationBoth(0,0);
                            FishFightSuccess();
                        }
                    }
                    break;
                }
            case FISHING_STATE.FISHING_VICTORY:
                {
                    FishingVictoryUpdate();
                    break;
                }
        }
    }


    public override void StateFixedUpdate()
    {
        switch(fishing_state)
        {
            case FISHING_STATE.FISH_FIGHTING:
                {
                    if (GM_.Instance.input.GetAxis(InputManager.AXIS.RT) > 0.1f)
                    {
                        interactingFish.ReelingIn(GM_.Instance.input.GetAxis(InputManager.AXIS.RT) * 2.5f);
                    }
                    break;
                }
        }
    }

    float castingTime = 0.0f;
    float castingTimeout = 0.0f;
    void BeginPowerUpThrow()
    {
        GM_.Instance.ui.helperButtons.EnableLeftStick(false, false, true, false, "Cast");
        fishing_state = FISHING_STATE.POWERING_UP;     
        castingTime = 0.0f;
        castingTimeout = 0.0f;
        fishingProjectileIndicator.SetActive(true);
        TransformIndicator();
    }

    void CancelPowerUpThrow()
    {
        fishingBob.SetActive(false);
        fishingProjectileIndicator.SetActive(false);

        GM_.Instance.ui.helperButtons.DisableAll();
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GM_.Instance.ui.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");


        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        fishing_state = FISHING_STATE.IDLE;
    }

    void BeginCastingAnimation()
    {
        GM_.Instance.ui.helperButtons.DisableAll();

        fishing_state = FISHING_STATE.CASTING_ANIMATION;
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        GetComponentInChildren<Animator>().Play("Rod Flick");
    }


    void PowerUpThrow()
    {
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Cast");

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
        cast_position.y = GlobalVariables.WATER_LEVEL;

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


    void ReelIn(float reelAxis) // bring the bob closer by reeling in
    {
        float reelInSpeed = Mathf.Lerp(0, fishingReelInMaxSpeed, reelAxis);
        currentReelInTime += Time.deltaTime*reelInSpeed;

        if (currentReelInTime > 1.0f)
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
        GM_.Instance.ui.helperButtons.DisableAll();
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GM_.Instance.ui.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");

        thirdPersonCamera.look_at_target = transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FREE;

        fishingLineLogic.gameObject.SetActive(false);
        fishingBob.SetActive(false);
        fishingProjectileIndicator.SetActive(false);

        fishing_state = FISHING_STATE.IDLE;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;

        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.NOT_ACTIVE);

        GM_.Instance.input.SetVibrationBoth(0, 0);

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
    float interactingFishWontFrightenTime;
    void BeginFishInteractingState()
    {
        interactingFish = fishingBob.GetComponentInChildren<FishingBobLogic>().GetInteractingFish();
        currenFishBiteAttemptTime = Random.Range(fishbiteTimerMin, fishbiteTimerMax);
        biteAttempts = 0;
        interactingFishWontFrightenTime = 0.5f;
        fishing_state = FISHING_STATE.INTERACTING_FISH;

        fishingIndicatorLogic.SetPosition(new Vector3(interactingFish.transform.position.x, GlobalVariables.WATER_LEVEL + 1.0f, interactingFish.transform.position.z));
        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.FISH_INTERACTING);

    }

    void FailedHookAttempt()
    {
            currentReelInTime = 0;
            fishingLineLogic.ReelIn();
            interactingFish.LostInterestInFishingBob(5.0f);
            FishInteractingFailed();
            fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.NOT_ACTIVE);
    }

    void FishInteractingUpdate()
    {
        currenFishBiteAttemptTime -= Time.deltaTime;
        interactingFishWontFrightenTime -= Time.deltaTime;

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
        fishingIndicatorLogic.SetIndicator(FishingUI.ANIMATION_STATE.NOT_ACTIVE);
        fishingBob.GetComponentInChildren<FishingBobLogic>().FishStoppedInteracting();
    }

    // -- //

    // -- Fish Fighting State
    void FishFightingBegin()
    {

        GM_.Instance.ui.helperButtons.DisableAll();
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.RT, "Reel In");
        GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fight");
        GM_.Instance.ui.helperButtons.EnableLeftStick(true, true, false, false, "Fight Fish");


        GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.LEFT ,0);

        fishing_state = FISHING_STATE.FISH_FIGHTING;
        fishingBob.GetComponentInChildren<FishingBobLogic>().BeganFighting();

        thirdPersonCamera.look_at_target = interactingFish.gameObject.transform;
        
        fishingLineLogic.BeganFighting(interactingFish);
        interactingFish.BeginFighting(new Vector2(transform.position.x, transform.position.z));
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
    }

    void FishFightLineSnapped()
    {

        interactingFish.FishEscaped();

        interactingFish = null;
        CancelCasted();

    }



    bool call_waiting_transition_once = false;
    void FishFightSuccess()
    {
        GM_.Instance.ui.helperButtons.DisableAll();

        fishing_state = FISHING_STATE.FISHING_VICTORY;
        interactingFish.FishCaught();
        GM_.Instance.ui.transition.FadePreset(UITransition.FADE_PRESET.DEFAULT);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        call_waiting_transition_once = false;      
    }


    void FishingVictoryUpdate()
    {
        if (call_waiting_transition_once)
        {
            if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
            {
              

                thirdPersonCamera.enabled = true;
                celebrationCamera.enabled = false;
                Destroy(interactingFish.transform.parent.gameObject);
                interactingFish = null;
                GM_.Instance.ui.state_fishVictory.Hide();
                CancelCasted();
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
            }
        }
        else
        {
            if (GM_.Instance.ui.transition.IsInWaitingTransition())
            {
                call_waiting_transition_once = true;


                GM_.Instance.ui.helperButtons.DisableAll();
                GM_.Instance.ui.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Continue");


                GM_.Instance.ui.state_fishVictory.SetVictoryStats(interactingFish.varsFromFishGenerator.fishTypeName, interactingFish.varsFromFishGenerator.teir, interactingFish.varsFromFishGenerator.size);
                GM_.Instance.ui.state_fishVictory.Show();
                
                interactingFish.SetCaughtPosition(handTransform.position);
                staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
                fishingLineLogic.gameObject.SetActive(false);
                fishingBob.SetActive(false);
                fishingProjectileIndicator.SetActive(false);
                thirdPersonCamera.enabled = false;
                celebrationCamera.enabled = true;

            }
        }

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
