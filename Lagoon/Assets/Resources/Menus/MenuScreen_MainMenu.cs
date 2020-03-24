using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_MainMenu : MenuScreenBase
{

    [SerializeField] SelectableButton startButton;
    [SerializeField] SelectableButton optionsButton;
    [SerializeField] SelectableButton creditsButton;
    [SerializeField] SelectableButton exitButton;


    [SerializeField] MenuScreenBase creditsMenu;
    [SerializeField] MenuScreenBase optionsMenu;



    // Start is called before the first frame update


    void Awake()
    {
        startButton.Event_Selected += start_transitionToGame;
        optionsButton.Event_Selected += start_transitionToOptions;
        creditsButton.Event_Selected += start_transitionToCredits;
        exitButton.Event_Selected += start_transitionToExit;
        startButton.Event_FinishedShow += show_finshed;

        gameObject.SetActive(true);
    }
    private void Start()
    {
        SetupDefaults();
        startButton.HoveredOver();
    }


    TypeRef<float> refButtonAlpha = new TypeRef<float>();






    public override void EnteredMenu()
    {
        gameObject.SetActive(true);
        startButton.Show();
        optionsButton.Show();
        creditsButton.Show();
        exitButton.Show();

    }
    void show_finshed()
    {
        startButton.HoveredOver();
    }


    void start_transitionToGame()
    {
        
    }

    void start_transitionToCredits()
    {
        startButton.Hide();
        exitButton.Hide();
        optionsButton.Hide();
        creditsButton.Hide();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_MainMenuToCredits,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToCredits,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void end_transitionToCredits()
    {
        gameObject.SetActive(false);
        creditsMenu.EnteredMenu();
    }

    void start_transitionToExit()
    {

    }
    void end_transitionToExit()
    {

    }

    void start_transitionToOptions()
    {

    }
    void end_transitionToOptions()
    {
        gameObject.SetActive(false);

    }


    void transitionUpdate()
    {
        current_cameraPosition.x = cameraPositionRef_X.value;
        current_cameraPosition.y = cameraPositionRef_Y.value;
        current_cameraPosition.z = cameraPositionRef_Z.value;
        current_cameraRotation.x = cameraRotationRef_X.value;
        current_cameraRotation.y = cameraRotationRef_Y.value;
        current_cameraRotation.z = cameraRotationRef_Z.value;

        Quaternion new_rotation = new Quaternion();
        new_rotation.eulerAngles = current_cameraRotation + default_cameraRotation;
        camera_.transform.position = current_cameraPosition + default_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }
}
