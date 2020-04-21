using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_OptionsAudio : MenuScreenBase
{
    [SerializeField] MenuScreenBase mainMenu;
    [SerializeField] MenuScreenBase gameMenu;
    [SerializeField] MenuScreenBase controlMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_Title;

    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton goToGameOptionsButton;
    [SerializeField] SelectableAndUnhoverableButton goToControlOptionsButton;

    [SerializeField] Slider_ masterSlider;
    [SerializeField] Slider_ musicSlider;
    [SerializeField] Slider_ sfxSlider;

    [SerializeField] TMPro.TextMeshProUGUI text_MasterSlider;
    [SerializeField] TMPro.TextMeshProUGUI text_MusicSlider;
    [SerializeField] TMPro.TextMeshProUGUI text_SFXSlider;

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
        goToGameOptionsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });
        goToControlOptionsButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });


        goToGameOptionsButton.Event_Selected += start_transitionToGame;
        goToControlOptionsButton.Event_Selected += start_transitionToControls;


        TypeRef<bool> buttonGrouper = new TypeRef<bool>(false);
        goBackButton.GroupWith(goToGameOptionsButton);
        goToGameOptionsButton.GroupWith(goToControlOptionsButton);



        masterSlider.Event_CompletedShow += finishedShowingSlider;


        masterSlider.Event_Selected += sliderSelected;
        musicSlider.Event_Selected += sliderSelected;
        sfxSlider.Event_Selected += sliderSelected;

        masterSlider.Event_UnSelected += sliderUnSelected;
        musicSlider.Event_UnSelected += sliderUnSelected;
        sfxSlider.Event_UnSelected += sliderUnSelected;

    }


    void sliderSelected()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
    }
    void sliderUnSelected()
    {
        goBackButton.Event_Selected += start_transitionToMain;
    }


    public override void EnteredMenu()
    {

        goBackButton.Event_Selected += start_transitionToMain;

        masterSlider.SetValue(GM_.Instance.audio.MasterVolume);
        musicSlider.SetValue(GM_.Instance.audio.MusicVolume);
        sfxSlider.SetValue(GM_.Instance.audio.SFXVolume);

        masterSlider.Event_ValueChanged += setMasterVolume;
        musicSlider.Event_ValueChanged += setMusicVolume;
        sfxSlider.Event_ValueChanged += setSFXVolume;

        gameObject.SetActive(true);

        masterSlider.Show();
        musicSlider.Show();
        sfxSlider.Show();

        goBackButton.Show();
        goToGameOptionsButton.Show();
        goToControlOptionsButton.Show();

        goBackButton.ListenForSelection();
        goToGameOptionsButton.ListenForSelection();
        goToControlOptionsButton.ListenForSelection();

        SpecialText_Title.Begin(SpecialTextData_Title);

        ShowText();
    }

    void setMasterVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.MasterVolume = args.newValue;
    }
    void setMusicVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.MusicVolume = args.newValue;
    }
    void setSFXVolume(Slider_.EventArgs_ValueChanged args)
    {
        GM_.Instance.audio.SFXVolume = args.newValue;
    }


    void finishedShowingSlider()
    {
        masterSlider.HoverOver();
    }



    void start_transitionToMain()
    {
        goBackButton.Event_Selected -= start_transitionToMain;

        sfxSlider.Event_CompletedHide += continue_transitionToMain;
        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();
        masterSlider.Hide();
        musicSlider.Hide();
        sfxSlider.Hide();

        HideText();


    }
    void continue_transitionToMain()
    {
        sfxSlider.Event_CompletedHide -= continue_transitionToMain;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_AudioToMain,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMain,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }

    void start_transitionToGame()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
        sfxSlider.Event_CompletedHide += continue_transitionToGame;

        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();
        masterSlider.Hide();
        musicSlider.Hide();
        sfxSlider.Hide();

        HideText();


    }
    void continue_transitionToGame()
    {
        sfxSlider.Event_CompletedHide -= continue_transitionToGame;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_AudioToGameOptions,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToGame,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }

    void start_transitionToControls()
    {
        goBackButton.Event_Selected -= start_transitionToMain;
        sfxSlider.Event_CompletedHide += continue_transitionToControls;

        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();
        masterSlider.Hide();
        musicSlider.Hide();
        sfxSlider.Hide();



        HideText();




    }
    void continue_transitionToControls()
    {
        sfxSlider.Event_CompletedHide -= continue_transitionToControls;

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_AudioToControls,
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


    void textShowUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        text_MasterSlider.color = new_colour;
        text_MusicSlider.color = new_colour;
        text_SFXSlider.color = new_colour;
    }
    void textHideUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        text_MasterSlider.color = new_colour;
        text_MusicSlider.color = new_colour;
        text_SFXSlider.color = new_colour;
    }

    void end_transitionToMain()
    {
        mainMenu.EnteredMenu();
    }

    void end_transitionToGame()
    {
        gameMenu.EnteredMenu();
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
        new_rotation.eulerAngles = current_cameraRotation;
        camera_.transform.position = current_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }
}
