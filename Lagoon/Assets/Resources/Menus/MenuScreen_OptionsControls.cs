using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuScreen_OptionsControls : MenuScreenBase
{
    [SerializeField] MenuScreenBase mainMenu;
    [SerializeField] MenuScreenBase gameMenu;
    [SerializeField] MenuScreenBase audiolMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_Title;

    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton goToGameOptionsButton;
    [SerializeField] SelectableAndUnhoverableButton goToAudioOptionsButton;

    [SerializeField] Checkbox_ vibrationsCheckBox;
    [SerializeField] TMPro.TextMeshProUGUI text_vibrationsCheckBox;
    [SerializeField] TMPro.TextMeshProUGUI textnote_vibrationsCheckBox;

    SpecialText.SpecialTextData SpecialTextData_Title = new SpecialText.SpecialTextData();
    void Start()
    {
        Color32 default_colour_programmerTitle = ColourExtension.ColourtoColour32(SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color);

        SetupTypeRefArray();


        SpecialTextData_Title.CreateCharacterData(SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().text);


        Color32 default_colour_CreditsTitle = ColourExtension.ColourtoColour32(SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color);



        SpecialTextData_Title.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_CreditsTitle.r,default_colour_CreditsTitle.g,default_colour_CreditsTitle.b)
        },
        0,
        SpecialTextData_Title.fullTextString.Length
        );


        gameObject.SetActive(false);

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        goToGameOptionsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });
        goToAudioOptionsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });

        goBackButton.Event_Selected += start_transitionToMain;
        goToGameOptionsButton.Event_Selected += start_transitionToGame;
        goToAudioOptionsButton.Event_Selected += start_transitionToAudio;

        TypeRef<bool> buttonGrouper = new TypeRef<bool>(false);
        goBackButton.GroupWith(goToGameOptionsButton);
        goToGameOptionsButton.GroupWith(goToAudioOptionsButton);



        vibrationsCheckBox.Event_CompletedShow += completeShowVibrationCheckBox;

        vibrationsCheckBox.Event_ToggleChanged += vibrationToggled;
    }


    void vibrationToggled(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.input.VibrationsEnabled = args.newValue;
    }


    bool firstTimeEnterd = true;
    public override void EnteredMenu()
    {
        vibrationsCheckBox.SetToggle(GM_.Instance.input.VibrationsEnabled);

        gameObject.SetActive(true);

        goBackButton.Show();
        goToGameOptionsButton.Show();
        goToAudioOptionsButton.Show();
        vibrationsCheckBox.Show();

        goBackButton.ListenForSelection();
        goToGameOptionsButton.ListenForSelection();
        goToAudioOptionsButton.ListenForSelection();

        SpecialText_Title.Begin(SpecialTextData_Title);

        ShowText();
    }
    void completeShowVibrationCheckBox()
    {
        vibrationsCheckBox.HoverOver();
    }



    void start_transitionToMain()
    {
        vibrationsCheckBox.Event_CompletedHide += continue_transitionToMain;

        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToAudioOptionsButton.Hide();
        vibrationsCheckBox.Hide();

        HideText();


    }
    void continue_transitionToMain()
    {
        vibrationsCheckBox.Event_CompletedHide -= continue_transitionToMain;

                GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_ControlsToMain,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMain,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );

    }

    void start_transitionToGame()
    {
        vibrationsCheckBox.Event_CompletedHide += continue_transitionToGame;

        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToAudioOptionsButton.Hide();
        vibrationsCheckBox.Hide();

        HideText();



    }
    void continue_transitionToGame()
    {
        vibrationsCheckBox.Event_CompletedHide -= continue_transitionToGame;
        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_ControlsToGameOptions,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToGame,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void start_transitionToAudio()
    {
        vibrationsCheckBox.Event_CompletedHide += continue_transitionToAudio;

        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToAudioOptionsButton.Hide();
        vibrationsCheckBox.Hide();

        HideText();



    }
    void continue_transitionToAudio()
    {
        vibrationsCheckBox.Event_CompletedHide -= continue_transitionToAudio;
        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_ControlsToAudio,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToAudio,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }

    void ShowText()
    {

        TweenManager.TweenInstanceInterface inter = GM_.Instance.tween_manager.StartTweenInstance(
            default_hideTween,
            new TypeRef<float>[] { textAlpha },
            tweenUpdatedDelegate_: textShowUpdate,
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void textShowUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);

        text_vibrationsCheckBox.color = new_colour;
        textnote_vibrationsCheckBox.color = new_colour;
    }
    void HideText()
    {
        SpecialText_Title.End();

        TweenManager.TweenInstanceInterface inter = GM_.Instance.tween_manager.StartTweenInstance(
            default_hideTween,
            new TypeRef<float>[] { textAlpha },
            tweenUpdatedDelegate_: textHideUpdate,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    TypeRef<float> textAlpha = new TypeRef<float>();

    void textHideUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;

        text_vibrationsCheckBox.color = new_colour;
        textnote_vibrationsCheckBox.color = new_colour;
    }

    void end_transitionToMain()
    {
        mainMenu.EnteredMenu();
    }

    void end_transitionToGame()
    {
        gameMenu.EnteredMenu();
    }
    void end_transitionToAudio()
    {
        audiolMenu.EnteredMenu();
    }


    void transitionUpdate()
    {

        current_cameraPosition.x = cameraPositionRef_X.value;
        current_cameraPosition.y = cameraPositionRef_Y.value;
        current_cameraPosition.z = cameraPositionRef_Z.value;
        current_cameraRotation.x = cameraRotationRef_X.value;
        current_cameraRotation.y = cameraRotationRef_Y.value;

        Quaternion new_rotation = new Quaternion();
        new_rotation.eulerAngles = current_cameraRotation;
        camera_.transform.position = current_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }

    private void OnDestroy()
    {
        goBackButton.Event_Selected += start_transitionToMain;
        goToGameOptionsButton.Event_Selected += start_transitionToGame;
        goToAudioOptionsButton.Event_Selected += start_transitionToAudio;

        vibrationsCheckBox.Event_CompletedShow += completeShowVibrationCheckBox;

        vibrationsCheckBox.Event_ToggleChanged += vibrationToggled;
    }
}
