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


    [SerializeField] JournalLogic journal;
    [SerializeField] BasePagePair testPageForSupplyDrop;






    //  AudioSFX sfx_reeling;
    //   AudioManager.SFXInstanceInterface sfx_reeling_interface;

    AudioSFX fish_caught_sfx;
    AudioManager.SFXInstanceInterface fish_caught_sfx_interface;

    TypeRef<float> reelingDesiredPitch = new TypeRef<float>();

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
        FISHING_VICTORY,
        HOOKED_TO_SUPPLY_CRATE,
        SUPPLY_DROP_CONTENT
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


    Animator characterAnimator;
    private void Awake()
    {
        //     sfx_reeling = GM_.Instance.audio.GetSFX("Fishing_Reeling");
        fish_caught_sfx = GM_.Instance.audio.GetSFX("Completion Noise");

        characterAnimator = GetComponent<Animator>();
    }

    public void OnEnable()
    {

        fishing_state = FISHING_STATE.PREPAIRING_ROD;

        staticFishingRodLogic.gameObject.SetActive(true);
        fishingRodLogic.gameObject.SetActive(true);

        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
       
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;

        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.FREE;

        fishingIndicatorLogic.gameObject.SetActive(true);
        fishingIndicatorLogic.AttachLookAtTransform(cameraTransform);

        GAME_UI.Instance.helperButtons.DisableAll();
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GAME_UI.Instance.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");

        fishingBob.GetComponent<FishingBobCollisionEvent>().Event_HitSupplyDrop += supplyCrateHit;

        firstUpdate = true;

        characterAnimator.SetBool("Fishing", true);

    }

    public void OnDisable()
    {
        //       sfx_reeling_interface?.Stop();

        characterAnimator.SetBool("Fishing", false);
        characterAnimator.SetBool("Casting", false);

        if (fishingLineLogic != null)
            fishingLineLogic.gameObject.SetActive(false);

        if (staticFishingRodLogic != null)
            staticFishingRodLogic.gameObject.SetActive(false);

        if (fishingRodLogic != null)
            fishingRodLogic.gameObject.SetActive(false);

        if (fishingBob != null)
        {
            fishingBob.GetComponent<FishingBobCollisionEvent>().Event_HitSupplyDrop -= supplyCrateHit;
            fishingBob.SetActive(false);
        }


        if (fishingIndicatorLogic != null)
            fishingIndicatorLogic.gameObject.SetActive(false);

        if (fishingProjectileIndicator != null)
            fishingProjectileIndicator.gameObject.SetActive(false);

        GAME_UI.Instance.helperButtons.DisableAll();

        thirdPersonCamera.look_at_target = transform; // set target back to player

        if (!firstUpdate)
            GM_.Instance.audio.PlayMusic(GM_.Instance.audio.GetMUSIC("Island"), fadeInOfNewMusic: GM_.Instance.audio.GetMusicFadePreset(AudioManager.MUSIC_FADE_PRESETS.DEFAULT_FADEIN), fadeOutOfOldMusic: GM_.Instance.audio.GetMusicFadePreset(AudioManager.MUSIC_FADE_PRESETS.DEFAULT_FADEOUT));
    }


    bool firstUpdate = false;
    // Update is called once per frame
    public override void StateUpdate()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            GM_.Instance.audio.PlayMusic(GM_.Instance.audio.GetMUSIC("Fishing"), fadeInOfNewMusic: GM_.Instance.audio.GetMusicFadePreset(AudioManager.MUSIC_FADE_PRESETS.DEFAULT_FADEIN), fadeOutOfOldMusic: GM_.Instance.audio.GetMusicFadePreset(AudioManager.MUSIC_FADE_PRESETS.DEFAULT_FADEOUT));

        }

        switch (fishing_state)
        {
            case FISHING_STATE.PREPAIRING_ROD:
                {
                    fishing_state = FISHING_STATE.IDLE;

                    break;
                }
            case FISHING_STATE.IDLE:  // not fishing
                {

                    Tutorial(TutorialManager.TutorialType.CASTING);

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
                    if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Casting"))
                    {
                        if (characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                        {
                            PowerUpThrow();
                        }


                    }


                    break;
                }
            case FISHING_STATE.BOB_IS_FLYING:
                {
                    

                    float bob_velocity_magnitude = fishingBob.GetComponent<Rigidbody>().velocity.magnitude;
                   
                    reelingDesiredPitch.value = 1.2f;

          //          sfx_reeling_interface.Volume = Mathf.Min((sfx_reeling_interface.Pitch - 0.7f) / (0.95f - 0.7f), 0.6f);

                    if (fishingBob.GetComponentInChildren<BuoyancyPhysics>().GetCurrentState() == BuoyancyPhysics.STATE.IN_WATER || fishingBob.GetComponentInChildren<BuoyancyPhysics>().IsInEquilibrium() || GM_.Instance.input.GetAxis(InputManager.AXIS.RT) > 0.5f) // bob has settled
                    {
                        StartFishing();
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

                    NoFish();

                    Tutorial(TutorialManager.TutorialType.OTHER);

                    float reelAxis = GM_.Instance.input.GetAxis(InputManager.AXIS.RT);

                    if (fishingBob.GetComponentInChildren<FishingBobLogic>().HasAttractedFish())
                    {
                        if (!previousFrameIncludedAttractedFish)
                        {
                            GM_.Instance.input.SetVibrationRandomPulsing(InputManager.VIBRATION_MOTOR.LEFT, 0.2f, 0.1f, 1.0f, 0.25f);
                            previousFrameIncludedAttractedFish = true;
                        }
                    }
                    else  if (!fishingBob.GetComponentInChildren<FishingBobLogic>().IsFishInteracting())
                    {
                        GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.LEFT ,0);
                        previousFrameIncludedAttractedFish = false;
                    }
                    else if (fishingBob.GetComponentInChildren<FishingBobLogic>().HasHookedToCrate())
                    {


                    }

         //           sfx_reeling_interface.Volume = Mathf.Min((sfx_reeling_interface.Pitch - 0.7f) / (0.95f - 0.7f),0.6f);
                    reelingDesiredPitch.value = Mathf.Lerp(0.7f, 1.2f , reelAxis);

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
                    float reelAxis = GM_.Instance.input.GetAxis(InputManager.AXIS.RT);

       //             sfx_reeling_interface.Volume = Mathf.Min((sfx_reeling_interface.Pitch - 0.7f) / (0.95f - 0.7f), 0.6f);
                    reelingDesiredPitch.value = Mathf.Lerp(0.7f, 1.2f, reelAxis);

                    if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
                    {
                        CancelCasted();
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                        return;
                    }
                    else if (reelAxis > 0.1f)
                    {
                        GM_.Instance.input.SetVibration(InputManager.VIBRATION_MOTOR.RIGHT, reelAxis * 0.1f);
                        ReelIn(reelAxis);

                        if (interactingFishWontFrightenTime < 0.0f)
                        {
                            FailedHookAttempt();
                            return;
                        }
                    }

                    if (interactingFish.IsInStateInteracting())
                    {
                        FishInteractingUpdate();
                    }
                    else
                    {
                        FishInteractingFailed();
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

                    Tutorial(TutorialManager.TutorialType.REEL);

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
            case FISHING_STATE.HOOKED_TO_SUPPLY_CRATE:
                {
                    if (GAME_UI.Instance.transition.IsInWaitingTransition())
                    {
                        fishingBob.GetComponentInChildren<FishingBobLogic>().DetachFromSupplyCrate();

                        GM_.Instance.audio.PlaySFX(GM_.Instance.audio.GetSFX("Crate_Break"),null);

                        CancelCasted();
                        fishing_state = FISHING_STATE.SUPPLY_DROP_CONTENT;
                        hookedSupplyBox.gameObject.SetActive(false);
                    }
                    break;
                }
            case FISHING_STATE.SUPPLY_DROP_CONTENT:
                {
                    if (!GAME_UI.Instance.transition.IsTransitioning())
                    {
                        fishing_state = FISHING_STATE.IDLE;

                        hookedSupplyBox.GetBoxContents();
                       
                        journal.RequestShowPage(testPageForSupplyDrop); // change test page to know it's type, so additional arguments can be added
                        Destroy(hookedSupplyBox.gameObject);                  
                    }
                    break;
                }
        }
    }


    SupplyBox hookedSupplyBox;
    void supplyCrateHit(SupplyBox supplyBox)
    {
        hookedSupplyBox = supplyBox;
        fishing_state = FISHING_STATE.HOOKED_TO_SUPPLY_CRATE;
        fishingBob.GetComponentInChildren<FishingBobLogic>().AttachToSupplyBox(supplyBox);
        GAME_UI.Instance.transition.FadeInOut(0.5f,2,0.5f);
        GM_.Instance.input.SetVibrationBoth(0, 0);
        GM_.Instance.input.SetVibrationPulse(InputManager.VIBRATION_MOTOR.LEFT, 0.15f, 1.0f);
        GM_.Instance.input.SetVibrationPulse(InputManager.VIBRATION_MOTOR.RIGHT, 0.15f, 1.0f);
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


    // bob has landed in water and can now fish
    void StartFishing()
    {
        fishingBob.GetComponent<FishingBobCollisionEvent>().Event_EnterCollision -= StartFishing;
        fishingLineLogic.BeganFishing();
        fishingBob.GetComponentInChildren<FishingBobLogic>().BeganFishing();
        fishingProjectileIndicator.SetActive(false);
        Tutorial(TutorialManager.TutorialType.ATTRACT);
        fishing_state = FISHING_STATE.FISHING;
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.RT, "Reel In / Hook Fish");

        


    }
    void BeginPowerUpThrow()
    {
        GAME_UI.Instance.helperButtons.EnableLeftStick(false, false, true, false, "Cast");
        fishing_state = FISHING_STATE.POWERING_UP;     
        castingTime = 0.0f;
        castingTimeout = 0.0f;
        fishingProjectileIndicator.SetActive(true);
        TransformIndicator();
    }

    void CancelPowerUpThrow()
    {
        fishingBob.GetComponent<FishingBobCollisionEvent>().Event_EnterCollision -= StartFishing;
        fishingBob.SetActive(false);
        fishingProjectileIndicator.SetActive(false);

        GAME_UI.Instance.helperButtons.DisableAll();
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GAME_UI.Instance.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");

        characterAnimator.SetBool("Casting", false);


        characterControllerMovement.current_state = CharacterControllerMovement.STATE.ROT_CAMERA;
        staticFishingRodLogic.SetState(StaticFishingRodLogic.STATE.GO_TO_DEFAULT_POSITION);
        fishing_state = FISHING_STATE.IDLE;
    }

    void BeginCastingAnimation()
    {
        GAME_UI.Instance.helperButtons.DisableAll();

        fishing_state = FISHING_STATE.CASTING_ANIMATION;
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;
        characterAnimator.SetBool("Casting", true);
    }


    void PowerUpThrow()
    {
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Cast");
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.RT, "Hook Early");

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
        fishingBob.GetComponent<FishingBobCollisionEvent>().Event_EnterCollision += StartFishing;



   //     sfx_reeling_interface = GM_.Instance.audio.PlaySFX(sfx_reeling, transform, settingVolume: new SFXSettings.AnyFloatSetting.Constant(0), settingPitch: new SFXSettings.AnyFloatSetting.SmoothStep(reelingDesiredPitch, 0.7f, 5)); // create realling sound effect, but set volume to 0

        fishing_state = FISHING_STATE.BOB_IS_FLYING;
        thirdPersonCamera.look_at_target = fishingBob.transform;
        thirdPersonCamera.current_state = ThirdPersonCamera.STATE.CLAMPED_LOOK_AT;
    }


    void TransformIndicator() // note. this prediction is only accurate if there is no air drag on the bob
    {
        // projectile motion calculations are done in 2d for simplicity and then converted to 3d

        Vector3 Velocity = transform.forward * Mathf.PingPong(castingTime,1.0f)* fishingCastVelocity;
        Vector3 cast_position;

        Vector2 XZVelocity = new Vector2(Velocity.x, Velocity.z);
        float range = calculateProjectileRange(XZVelocity.magnitude);


        cast_position.x = XZVelocity.normalized.x * range;
        cast_position.z = XZVelocity.normalized.y * range;
        cast_position.y = GlobalVariables.WATER_LEVEL;

        float offset = 1.25f; // offset due to animation 

        cast_position += transform.right * offset;

        //float sin = Mathf.Sin(degrees_offset * Mathf.Deg2Rad);
        //float cos = Mathf.Cos(degrees_offset * Mathf.Deg2Rad);

        //float tx = cast_position.x;
        //float tz = cast_position.z;
        //cast_position.x = (cos * tx) - (sin * tz);
        //cast_position.z = (sin * tx) + (cos * tz);


        cast_position.x += fishingRodTip.position.x;
        cast_position.z += fishingRodTip.position.z;

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
        characterAnimator.SetBool("Casting", false);

        fishingBob.GetComponent<FishingBobCollisionEvent>().Event_EnterCollision -= StartFishing;

        GAME_UI.Instance.helperButtons.DisableAll();
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fishing");
        GAME_UI.Instance.helperButtons.EnableLeftStick(false, false, false, true, "Begin Cast");

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

        GAME_UI.Instance.helperButtons.DisableAll();
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.RT, "Reel In");
        GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.B, "Cancel Fight");
        GAME_UI.Instance.helperButtons.EnableLeftStick(true, true, false, false, "Fight Fish");


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






    readonly TweenManager.TweenPathBundle fishCelebrationMusicLower_Tween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0,-100.0f,1.0f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );


    void FishFightSuccess()
    {
        GAME_UI.Instance.helperButtons.DisableAll();

        fishing_state = FISHING_STATE.FISHING_VICTORY;
        interactingFish.FishCaught();
        GAME_UI.Instance.transition.FadePreset(UITransition.FADE_PRESET.DEFAULT);
        characterControllerMovement.current_state = CharacterControllerMovement.STATE.NO_MOVEMENT;

        call_waiting_transition_once = false;


        GM_.Instance.audio.PlaySFX(fish_caught_sfx, null, Event_SoundStopped: celebrationSoundComplete);

        GM_.Instance.tween_manager.StartTweenInstance(
            fishCelebrationMusicLower_Tween,
            new TypeRef<float>[] { typeRef_RelativeMusicVolume },
            tweenUpdatedDelegate_: update_FishCelebrationMusicLower,
            speed_: 100.0f
            );

        
    }
    TypeRef<float> typeRef_RelativeMusicVolume = new TypeRef<float>();

    void update_FishCelebrationMusicLower()
    {
        GM_.Instance.audio.AdditionalMusicVolume = typeRef_RelativeMusicVolume.value;
    }

    void celebrationSoundComplete()
    {

        GM_.Instance.tween_manager.StartTweenInstance(
            fishCelebrationMusicLower_Tween,
            new TypeRef<float>[] { typeRef_RelativeMusicVolume },
            tweenUpdatedDelegate_: update_FishCelebrationMusicLower,
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            speed_: 1.0f
            );

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
                GAME_UI.Instance.state_fishVictory.Hide();
                CancelCasted();
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
            }
        }
        else
        {
            if (GAME_UI.Instance.transition.IsInWaitingTransition())
            {
                call_waiting_transition_once = true;


                GAME_UI.Instance.helperButtons.DisableAll();
                GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.A, "Continue");


                GM_.Instance.stats.UpdateFishStats(interactingFish.varsFromFishGenerator);

                GAME_UI.Instance.state_fishVictory.SetVictoryStats(interactingFish.varsFromFishGenerator.fishTypeName, interactingFish.varsFromFishGenerator.teir, interactingFish.varsFromFishGenerator.size);
                GAME_UI.Instance.state_fishVictory.Show();
                
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


    bool fishing_tutorial_complete = false;
    bool displaying_tutorial_box = false;
    FishingBobLogic.STATE temp_bob_state;

    void Tutorial(TutorialManager.TutorialType type)
    {
        
        if(!fishing_tutorial_complete)
        {
            if(!displaying_tutorial_box)
            {
                displaying_tutorial_box = GM_.Instance.tutorial_manger.WhatTutorial(type);

                if (displaying_tutorial_box)
                {
                    GM_.Instance.input.InputEnabled = false;
                    Debug.Log(Time.timeScale);
                    Time.timeScale = 0;
                    temp_bob_state = fishingBob.GetComponentInChildren<FishingBobLogic>().GetState();
                    fishingBob.GetComponentInChildren<FishingBobLogic>().Tutorial(FishingBobLogic.STATE.TUTORIAL);
                }

            }
            else
            {
                if(GM_.Instance.input.GetButtonDownDisabled(InputManager.BUTTON.A))
                {
                    displaying_tutorial_box = GM_.Instance.tutorial_manger.CloseTutorial(type);


                    
                    
                    if(!displaying_tutorial_box)
                    {
                        Time.timeScale = 1; 
                        GM_.Instance.input.InputEnabled = true;
                        fishingBob.GetComponentInChildren<FishingBobLogic>().Tutorial(temp_bob_state);

                        if (type == TutorialManager.TutorialType.REEL)
                        {
                            fishing_tutorial_complete = true;
                        }
                        
                    }

                }
            }
        }
    }

    float no_fish_time = 0;
    void NoFish()
    {
        if (no_fish_time > 15 && !GM_.Instance.tutorial_manger.no_fish_complete)
        {
            Tutorial(TutorialManager.TutorialType.NOFISH);

        }

        if (!GM_.Instance.tutorial_manger.no_fish_complete)
        {
            no_fish_time += Time.deltaTime;
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
