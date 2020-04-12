using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_OptionsGame : MenuScreenBase
{
    [SerializeField] MenuScreenBase mainMenu;
    [SerializeField] MenuScreenBase audioMenu;
    [SerializeField] MenuScreenBase controlMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_Title;

    [SerializeField] UnselectableButton goBackButton;
    [SerializeField] UnselectableButton goToAudioOptionsButton;
    [SerializeField] UnselectableButton goToControlOptionsButton;

    [SerializeField] Slider_ xSensitivitySlider;
    [SerializeField] Slider_ ySensitivitySlider;

    [SerializeField] Checkbox_ xInvertCheckBox;
    [SerializeField] Checkbox_ yInvertCheckBox;

    [SerializeField] TMPro.TextMeshProUGUI text_xSensitivitySlider;
    [SerializeField] TMPro.TextMeshProUGUI text_ySensitivitySlider;
    [SerializeField] TMPro.TextMeshProUGUI text_xInvertCheckBox;
    [SerializeField] TMPro.TextMeshProUGUI text_yInvertCheckBox;

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

        goBackButton.SetButtonsToCheckForPress(InputManager.BUTTON.B);
        goToAudioOptionsButton.SetButtonsToCheckForPress(InputManager.BUTTON.RB);
        goToControlOptionsButton.SetButtonsToCheckForPress(InputManager.BUTTON.LB);

        
        goToAudioOptionsButton.Event_Selected += start_transitionToAudio;
        goToControlOptionsButton.Event_Selected += start_transitionToControls;

        TypeRef<bool> buttonGrouper = new TypeRef<bool>(false);
        goBackButton.AssignToGroup(buttonGrouper);
        goToAudioOptionsButton.AssignToGroup(buttonGrouper);
        goToControlOptionsButton.AssignToGroup(buttonGrouper);




        xSensitivitySlider.Event_CompletedShow += finishedShowingSlider;

        xSensitivitySlider.Event_Selected += sliderSelected;
        ySensitivitySlider.Event_Selected += sliderSelected;


        xSensitivitySlider.Event_UnSelected += sliderUnSelected;
        ySensitivitySlider.Event_UnSelected += sliderUnSelected;

        xSensitivitySlider.Event_ValueChanged += setXSensitivity;
        ySensitivitySlider.Event_ValueChanged += setYSensitivity;




        xSensitivitySlider.Event_ValueChanged += changeXSense;
        ySensitivitySlider.Event_ValueChanged += changeYSense;
        xInvertCheckBox.Event_ToggleChanged += toggledXInvert;
        yInvertCheckBox.Event_ToggleChanged += toggledYInvert;


        xSensitivitySlider.ChangeSliderRange(PlayerSettings.MINMAX_X_SENSITIVITY);
        ySensitivitySlider.ChangeSliderRange(PlayerSettings.MINMAX_Y_SENSITIVITY);
    }

    void changeXSense(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.XSensitivity = args.newValue;
    }
    void changeYSense(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.YSensitivity = args.newValue;
    }

    void toggledXInvert(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.IsXInverted = args.newValue;
    }
    void toggledYInvert(Checkbox_.EventArgs_ValueChanged args)
    {
        GM_.Instance.settings.IsYInverted = args.newValue;
    }

    void sliderSelected()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
    }
    void sliderUnSelected()
    {
        goBackButton.Event_Selected += start_transitionToMain;
    }

    void finishedShowingSlider()
    {
        xSensitivitySlider.HoverOver();
    }


    public override void EnteredMenu()
    {
        goBackButton.Event_Selected += start_transitionToMain;

        xSensitivitySlider.SetValue(GM_.Instance.settings.XSensitivity);
        ySensitivitySlider.SetValue(GM_.Instance.settings.YSensitivity);
        xInvertCheckBox.SetToggle(GM_.Instance.settings.IsXInverted);
        yInvertCheckBox.SetToggle(GM_.Instance.settings.IsYInverted);

        gameObject.SetActive(true);

        goBackButton.Show();
        goToAudioOptionsButton.Show();
        goToControlOptionsButton.Show();



        xInvertCheckBox.Show();
        yInvertCheckBox.Show();
        xSensitivitySlider.Show();
        ySensitivitySlider.Show();

        ShowText();

        SpecialText_Title.Begin(SpecialTextData_Title);

    }

    void setXSensitivity(Slider_.EventArgs_ValueChanged args)
    {
        
    }
    void setYSensitivity(Slider_.EventArgs_ValueChanged args)
    {

    }


    void start_transitionToMain()
    {
        goBackButton.Event_Selected -= start_transitionToMain; 
        ySensitivitySlider.Event_CompletedHide += continue_transitionToMain;

        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        xSensitivitySlider.Hide();
        ySensitivitySlider.Hide();
        xInvertCheckBox.Hide();
        yInvertCheckBox.Hide();

        HideText();


    }
    void continue_transitionToMain()
    {

        ySensitivitySlider.Event_CompletedHide -= continue_transitionToMain;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_GameOptionsToMain,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMain,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }


    void start_transitionToAudio()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
        ySensitivitySlider.Event_CompletedHide += continue_transitionToAudio;

        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        xSensitivitySlider.Hide();
        ySensitivitySlider.Hide();
        xInvertCheckBox.Hide();
        yInvertCheckBox.Hide();

        HideText();


    }
    void continue_transitionToAudio()
    {
        ySensitivitySlider.Event_CompletedHide -= continue_transitionToAudio;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_GameOptionsToAudio,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToAudio,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void start_transitionToControls()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
        ySensitivitySlider.Event_CompletedHide += continue_transitionToControls;
        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        xSensitivitySlider.Hide();
        ySensitivitySlider.Hide();
        xInvertCheckBox.Hide();
        yInvertCheckBox.Hide();

        HideText();


    }
    void continue_transitionToControls()
    {
        ySensitivitySlider.Event_CompletedHide -= continue_transitionToControls;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_GameOptionsToControls,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToControls,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
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
    void ShowText()
    {
        TweenManager.TweenInstanceInterface inter = GM_.Instance.tween_manager.StartTweenInstance(
            default_hideTween,
            new TypeRef<float>[] { textAlpha },
            tweenUpdatedDelegate_: textShowUpdate,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );
    }


    TypeRef<float> textAlpha = new TypeRef<float>();

    void textHideUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        text_xSensitivitySlider.color = new_colour;
        text_ySensitivitySlider.color = new_colour;
        text_xInvertCheckBox.color = new_colour;
        text_yInvertCheckBox.color = new_colour;
    }

    void textShowUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        text_xSensitivitySlider.color = new_colour;
        text_ySensitivitySlider.color = new_colour;
        text_xInvertCheckBox.color = new_colour;
        text_yInvertCheckBox.color = new_colour;
    }

    void end_transitionToMain()
    {
        mainMenu.EnteredMenu();
    }

    void end_transitionToAudio()
    {
        audioMenu.EnteredMenu();
    }
    void end_transitionToControls()
    {
        controlMenu.EnteredMenu();
    }


    void transitionUpdate()
    {

        current_cameraPosition.x = cameraPositionRef_X.value;
        current_cameraPosition.y = cameraPositionRef_Y.value;
        current_cameraPosition.z = cameraPositionRef_Z.value;
        current_cameraRotation.x = cameraRotationRef_X.value;
        current_cameraRotation.y = cameraRotationRef_Y.value;

        Quaternion new_rotation = new Quaternion();
        new_rotation.eulerAngles = current_cameraRotation ;
        camera_.transform.position = current_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }
}
