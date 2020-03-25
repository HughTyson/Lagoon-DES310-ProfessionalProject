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

    [SerializeField] UnselectableButton goBackButton;
    [SerializeField] UnselectableButton goToGameOptionsButton;
    [SerializeField] UnselectableButton goToControlOptionsButton;

    SpecialText.SpecialTextData SpecialTextData_Title = new SpecialText.SpecialTextData();
    void Start()
    {
        Color32 default_colour_programmerTitle = ColourExtension.ColourtoColour32(SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color);




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
        goToGameOptionsButton.SetButtonsToCheckForPress(InputManager.BUTTON.LB);
        goToControlOptionsButton.SetButtonsToCheckForPress(InputManager.BUTTON.RB);

        goBackButton.Event_Selected += start_transitionToMain;
        goToGameOptionsButton.Event_Selected += start_transitionToGame;
        goToControlOptionsButton.Event_Selected += start_transitionToControls;

    }


    public override void EnteredMenu()
    {
        SetupDefaults();

        gameObject.SetActive(true);

        goBackButton.Show();
        goToGameOptionsButton.Show();
        goToControlOptionsButton.Show();

        SpecialText_Title.Begin(SpecialTextData_Title);

    }



    void start_transitionToMain()
    {
        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        HideText();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_GameOptionsToMain,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMain,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }

    void start_transitionToGame()
    {
        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        HideText();

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
        goBackButton.Hide();
        goToGameOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        HideText();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_GameOptionsToControls,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMain,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }


    void HideText()
    {
        SpecialText_Title.End();

        TweenManager.TweenInstanceInterface inter = GM_.Instance.tween_manager.StartTweenInstance(
            SelectableButton.default_hideTween,
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
        new_rotation.eulerAngles = current_cameraRotation + default_cameraRotation;
        camera_.transform.position = current_cameraPosition + default_cameraPosition;
        camera_.transform.rotation = new_rotation;
    }
}
