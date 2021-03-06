﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_ExtraCredits : MenuScreenBase
{

    [SerializeField] MenuScreenBase creditsMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_Title;

    [SerializeField] SelectableAndUnhoverableButton goBackButton;

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

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        goBackButton.Event_Selected += start_transitionToCredits;
        gameObject.SetActive(false);
    }


    public override void EnteredMenu()
    {


        gameObject.SetActive(true);
        goBackButton.Show();
        goBackButton.ListenForSelection();
        SpecialText_Title.Begin(SpecialTextData_Title);
    }

   
    void start_transitionToCredits()
    {
        goBackButton.Hide();

        HideText();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_ExtraCreditsToCredits,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToCredits,
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
    TypeRef<float> textAlpha = new TypeRef<float>();

    void textHideUpdate()
    {
        Color new_colour = new Color(textAlpha.value, textAlpha.value, textAlpha.value, textAlpha.value);
        SpecialText_Title.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
    }

    void end_transitionToCredits()
    {
        creditsMenu.EnteredMenu();
    }

    void transition_hideTextUpdate()
    {

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
        goBackButton.Event_Selected -= start_transitionToCredits;
    }
}
