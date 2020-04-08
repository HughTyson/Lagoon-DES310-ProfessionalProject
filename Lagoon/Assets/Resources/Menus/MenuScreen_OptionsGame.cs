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

    SpecialText.SpecialTextData SpecialTextData_Title = new SpecialText.SpecialTextData();
    void Start()
    {
        Color32 default_colour_programmerTitle = ColourExtension.ColourtoColour32(SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color);

        SetupTypeRefArray();
        //hiddenButton.Event_CancelledWhileHovering += start_transitionToMain;

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

        goBackButton.Event_Selected += start_transitionToMain;
        goToAudioOptionsButton.Event_Selected += start_transitionToAudio;
        goToControlOptionsButton.Event_Selected += start_transitionToControls;

        TypeRef<bool> buttonGrouper = new TypeRef<bool>(false);
        goBackButton.AssignToGroup(buttonGrouper);
        goToAudioOptionsButton.AssignToGroup(buttonGrouper);
        goToControlOptionsButton.AssignToGroup(buttonGrouper);
    }


    public override void EnteredMenu()
    {


        gameObject.SetActive(true);

        goBackButton.Show();
        goToAudioOptionsButton.Show();
        goToControlOptionsButton.Show();

        SpecialText_Title.Begin(SpecialTextData_Title);

    }



    void start_transitionToMain()
    {
        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
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

    void start_transitionToAudio()
    {
        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        HideText();

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
        goBackButton.Hide();
        goToAudioOptionsButton.Hide();
        goToControlOptionsButton.Hide();

        HideText();

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
