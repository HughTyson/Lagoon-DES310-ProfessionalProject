using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_Credits : MenuScreenBase
{

    [SerializeField] SelectableButton_ extraCreditsButton;
    [SerializeField] SelectableAndUnhoverableButton goBackButton;


    [SerializeField] MenuScreenBase mainMenu;
    [SerializeField] MenuScreen_ExtraCredits extraCreditsMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_CreditsTitle;

    [SerializeField] SpecialText.SpecialText SpecialText_progammerTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_progammerNames;
    [SerializeField] SpecialText.SpecialText SpecialText_artistNames;
    [SerializeField] SpecialText.SpecialText SpecialText_artistTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_designerTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_designerNames;

    // Start is called before the first frame update


    SpecialText.SpecialTextData TextData_CreditsTitle = new SpecialText.SpecialTextData();

    SpecialText.SpecialTextData TextData_programmerNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_artistNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_designerNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_programmerTitle = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_artistTitle = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_designerTitle = new SpecialText.SpecialTextData();


    TweenManager.TweenPathBundle tweenShowButton;
    private void Start()
    {
        tweenShowButton = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                // Y POS
                new TweenManager.TweenPart_Start(-720, -450, 1, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        SetupTypeRefArray();


        TextData_CreditsTitle.CreateCharacterData(SpecialText_CreditsTitle.GetComponent<TMPro.TextMeshProUGUI>().text);

        TextData_programmerNames.CreateCharacterData(SpecialText_progammerNames.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_artistNames.CreateCharacterData(SpecialText_artistNames.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_designerNames.CreateCharacterData(SpecialText_designerNames.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_programmerTitle.CreateCharacterData(SpecialText_progammerTitle.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_artistTitle.CreateCharacterData(SpecialText_artistTitle.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_designerTitle.CreateCharacterData(SpecialText_designerTitle.GetComponent<TMPro.TextMeshProUGUI>().text);

        Color32 default_colour_CreditsTitle = ColourExtension.ColourtoColour32(SpecialText_CreditsTitle.GetComponent<TMPro.TextMeshProUGUI>().color);

        Color32 default_colour_programmerNames = ColourExtension.ColourtoColour32(SpecialText_progammerNames.GetComponent<TMPro.TextMeshProUGUI>().color);
        Color32 default_colour_programmerTitle = ColourExtension.ColourtoColour32(SpecialText_progammerTitle.GetComponent<TMPro.TextMeshProUGUI>().color);
        Color32 default_colour_artistNames = ColourExtension.ColourtoColour32(SpecialText_artistNames.GetComponent<TMPro.TextMeshProUGUI>().color);
        Color32 default_colour_artistTitle = ColourExtension.ColourtoColour32(SpecialText_artistTitle.GetComponent<TMPro.TextMeshProUGUI>().color);
        Color32 default_colour_designNames = ColourExtension.ColourtoColour32(SpecialText_designerNames.GetComponent<TMPro.TextMeshProUGUI>().color);
        Color32 default_colour_designTitle = ColourExtension.ColourtoColour32(SpecialText_designerTitle.GetComponent<TMPro.TextMeshProUGUI>().color);


        TextData_CreditsTitle.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_CreditsTitle.r,default_colour_CreditsTitle.g,default_colour_CreditsTitle.b)
        },
        0,
        TextData_CreditsTitle.fullTextString.Length
        );


        TextData_programmerTitle.AddPropertyToText(
            new List<SpecialText.TextProperties.Base> {
                new SpecialText.TextProperties.AppearAtOnce(),
                new SpecialText.TextProperties.Colour(default_colour_programmerTitle.r,default_colour_programmerTitle.g,default_colour_programmerTitle.b)
            },
            0,
            TextData_programmerTitle.fullTextString.Length
        );
        TextData_programmerTitle.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.4f, -1));

        TextData_artistTitle.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_artistTitle.r,default_colour_artistTitle.g,default_colour_artistTitle.b)
        },
        0,
        TextData_artistTitle.fullTextString.Length
        );
        TextData_artistTitle.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.2f, -1));

        TextData_designerTitle.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_designTitle.r,default_colour_designTitle.g,default_colour_designTitle.b)
        },
        0,
        TextData_designerTitle.fullTextString.Length
        );
        TextData_designerTitle.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.4f, -1));


        TextData_programmerNames.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_programmerNames.r,default_colour_programmerNames.g,default_colour_programmerNames.b)
        },
        0,
        TextData_programmerNames.fullTextString.Length
        );
        TextData_programmerNames.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.4f, -1));

        TextData_artistNames.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_artistNames.r,default_colour_artistNames.g,default_colour_artistNames.b)
        },
        0,
        TextData_artistNames.fullTextString.Length
        );
        TextData_artistNames.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.2f, -1));


        TextData_designerNames.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce(),
                    new SpecialText.TextProperties.Colour(default_colour_designNames.r,default_colour_designNames.g,default_colour_designNames.b)
        },
        0,
        TextData_designerNames.fullTextString.Length
        );
        TextData_designerNames.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.4f, -1));


        extraCreditsButton.Event_Selected += start_transitionToExtraCredits;

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
        goBackButton.Event_Selected += start_transitionToMainMenu;


        gameObject.SetActive(false);
    }

    public override void EnteredMenu()
    {


        gameObject.SetActive(true);

        extraCreditsButton.Event_CompletedShow += finishedShowingButtons;
        extraCreditsButton.Show();

        goBackButton.Show();
        goBackButton.ListenForSelection();

        SpecialText_CreditsTitle.Begin(TextData_CreditsTitle);

        SpecialText_progammerTitle.Begin(TextData_programmerTitle);
        SpecialText_artistTitle.Begin(TextData_artistTitle);
        SpecialText_designerTitle.Begin(TextData_designerTitle);

        SpecialText_progammerNames.Begin(TextData_programmerNames);
        SpecialText_artistNames.Begin(TextData_artistNames);
        SpecialText_designerNames.Begin(TextData_designerNames);




    }

    void start_transitionToExtraCredits()
    {
        extraCreditsButton.Hide();
        goBackButton.Hide();
        HideText();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_CreditToExtraCredits,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToExtraCredits,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }


    void end_transitionToExtraCredits()
    {
        gameObject.SetActive(false);
        extraCreditsMenu.EnteredMenu();
    }



    public void finishedShowingButtons()
    {
        extraCreditsButton.Event_CompletedShow -= finishedShowingButtons;
        extraCreditsButton.HoverOver();
    }


    void HideText()
    {
        SpecialText_CreditsTitle.End();
        SpecialText_progammerTitle.End();
        SpecialText_progammerNames.End();
        SpecialText_artistNames.End();
        SpecialText_artistTitle.End();
        SpecialText_designerTitle.End();
        SpecialText_designerNames.End();

        GM_.Instance.tween_manager.StartTweenInstance(
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
        SpecialText_CreditsTitle.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_progammerNames.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_artistNames.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_designerNames.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_progammerTitle.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_artistTitle.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
        SpecialText_designerTitle.GetComponent<TMPro.TextMeshProUGUI>().color = new_colour;
    }

    void start_transitionToMainMenu()
    {
        extraCreditsButton.Hide();
        goBackButton.Hide();

        HideText();

        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_CreditsToMenu,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMainMenu,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }
    void end_transitionToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenu.EnteredMenu();
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
        extraCreditsButton.Event_Selected -= start_transitionToExtraCredits;
        goBackButton.Event_Selected -= start_transitionToMainMenu;
        extraCreditsButton.Event_CompletedShow -= finishedShowingButtons;
    }
}
