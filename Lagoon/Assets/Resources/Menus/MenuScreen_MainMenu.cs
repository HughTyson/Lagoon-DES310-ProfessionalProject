using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen_MainMenu : MenuScreenBase
{

    [SerializeField] UnityEngine.UI.Image fadeInOutImage;
    [SerializeField] SelectableButton startButton;
    [SerializeField] SelectableButton optionsButton;
    [SerializeField] SelectableButton creditsButton;
    [SerializeField] SelectableButton exitButton;


    [SerializeField] MenuScreenBase creditsMenu;
    [SerializeField] MenuScreenBase gameOptionsMenu;



    // Start is called before the first frame update

    static TweenManager.TweenPathBundle fadeInTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0, 1, TweenManager.CURVE_PRESET.LINEAR)
            )
        );

    TweenManager.TweenPathBundle showButtonTween;

    void Awake()
    {
        showButtonTween = new TweenManager.TweenPathBundle(
            // button X position
            new TweenManager.TweenPath
            (
                new TweenManager.TweenPart_Start(-1100, -700, 1, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
            )
        );

        startButton.Event_Selected += buttonPressed;
        optionsButton.Event_Selected += buttonPressed;
        creditsButton.Event_Selected += buttonPressed;
        exitButton.Event_Selected += buttonPressed;

        startButton.Event_Selected += start_transitionToGame;
        optionsButton.Event_Selected += start_transitionToOptions;
        creditsButton.Event_Selected += start_transitionToCredits;
        exitButton.Event_Selected += start_transitionToExit;





        gameObject.SetActive(true);
    }

    ActionTimer actionTimer = new ActionTimer();

    private void Start()
    {
        SetupTypeRefArray();

        startButton.SetShowTweenBundle(showButtonTween, SelectableButton.TWEEN_PARAMETERS.POS_X);
        optionsButton.SetShowTweenBundle(showButtonTween, SelectableButton.TWEEN_PARAMETERS.POS_X);
        creditsButton.SetShowTweenBundle(showButtonTween, SelectableButton.TWEEN_PARAMETERS.POS_X);
        exitButton.SetShowTweenBundle(showButtonTween, SelectableButton.TWEEN_PARAMETERS.POS_X);

        fadeInOutImage.color = new Color(0, 0, 0, 1);

        GM_.Instance.tween_manager.StartTweenInstance(
            fadeInTween,
            new TypeRef<float>[] { fadeAlpha},
            tweenUpdatedDelegate_: init_update,
            tweenCompleteDelegate_: init_finished
            );

        actionTimer.AddAction(show_startButton, 0);
        actionTimer.AddAction(show_optionButton, 0.1f);
        actionTimer.AddAction(show_creditButton, 0.2f);
        actionTimer.AddAction(show_exitButton, 0.3f);

        Quaternion new_rotation = new Quaternion();
        new_rotation.eulerAngles = MenuTransitions.MainMenuVals.rotation;
        camera_.transform.position = MenuTransitions.MainMenuVals.position;
        camera_.transform.rotation = new_rotation;

    }


    TypeRef<float> refButtonAlpha = new TypeRef<float>();


    TypeRef<float> fadeAlpha = new TypeRef<float>();
    void init_update()
    {
        fadeInOutImage.color = new Color(0, 0, 0, fadeAlpha.value);
    }
    void init_finished()
    {
        startButton.HoveredOver();
    }



    public override void EnteredMenu()
    {
        buttonTweenInterfaces.Clear();

        gameObject.SetActive(true);

        actionTimer.Start();
        GM_.Instance.update_events.UpdateEvent += actionTimer.Update;
    }

    List<TweenManager.TweenInstanceInterface> buttonTweenInterfaces = new List<TweenManager.TweenInstanceInterface>();

    void show_startButton()
    {
        startButton.Show();
        startButton.HoveredOver();
    }
    void show_optionButton()
    {
        optionsButton.Show();
    }
    void show_creditButton()
    {
        creditsButton.Show();
    }
    void show_exitButton()
    {
        exitButton.Show();
        GM_.Instance.update_events.UpdateEvent -= actionTimer.Update;
    }

    void buttonPressed()
    {
        GM_.Instance.update_events.UpdateEvent -= actionTimer.Update;
    }

    void start_transitionToGame()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
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

        GM_.Instance.tween_manager.StartTweenInstance(
            fadeInTween,
            new TypeRef<float>[] { fadeAlpha },
            tweenUpdatedDelegate_: init_update,
            tweenCompleteDelegate_: end_transitionToExit,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );
    }
    void end_transitionToExit()
    {
        Application.Quit();
    }

    void start_transitionToOptions()
    {
        startButton.Hide();
        exitButton.Hide();
        optionsButton.Hide();
        creditsButton.Hide();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_MainToGameOptions,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToOptions,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void end_transitionToOptions()
    {
        gameObject.SetActive(false);
        gameOptionsMenu.EnteredMenu();
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
        new_rotation.eulerAngles = current_cameraRotation;
        camera_.transform.position = current_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }
}
