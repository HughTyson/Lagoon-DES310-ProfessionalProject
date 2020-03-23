using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_Credits : MenuScreenBase
{

    [SerializeField] SelectableButton nextPrevButton;


    [SerializeField] MenuScreenBase mainMenu;

    [SerializeField] SpecialText.SpecialText SpecialText_progammerTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_progammerNames;
    [SerializeField] SpecialText.SpecialText SpecialText_artistNames;
    [SerializeField] SpecialText.SpecialText SpecialText_artistTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_designerTitle;
    [SerializeField] SpecialText.SpecialText SpecialText_designerNames;

    // Start is called before the first frame update


    SpecialText.SpecialTextData TextData_programmerNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_artistNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_designerNames = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_programmerTitle = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_artistTitle = new SpecialText.SpecialTextData();
    SpecialText.SpecialTextData TextData_designerTitle = new SpecialText.SpecialTextData();

    void Start()
    {
        TextData_programmerNames.CreateCharacterData(SpecialText_progammerTitle.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_artistNames.CreateCharacterData(SpecialText_artistNames.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_designerNames.CreateCharacterData(SpecialText_designerNames.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_programmerTitle.CreateCharacterData(SpecialText_progammerTitle.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_artistTitle.CreateCharacterData(SpecialText_artistTitle.GetComponent<TMPro.TextMeshProUGUI>().text);
        TextData_designerTitle.CreateCharacterData(SpecialText_designerTitle.GetComponent<TMPro.TextMeshProUGUI>().text);

        TextData_programmerTitle.AddPropertyToText(
            new List<SpecialText.TextProperties.Base> { 
                new SpecialText.TextProperties.AppearAtOnce()
            },
            0,
            TextData_programmerTitle.fullTextString.Length
        );


        TextData_artistTitle.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce()
        },
        0,
        TextData_artistTitle.fullTextString.Length
        );
        TextData_artistTitle.propertyDataList.Add(new SpecialText.TextProperties.Delay(0.5f, -1));

        TextData_designerTitle.AddPropertyToText(
        new List<SpecialText.TextProperties.Base> {
                    new SpecialText.TextProperties.AppearAtOnce()
        },
        0,
        TextData_designerTitle.fullTextString.Length
        );
        TextData_designerTitle.propertyDataList.Add(new SpecialText.TextProperties.Delay(1.0f, -1));


        //specialTextData.FillTextWithProperties(
        //    new List<SpecialText.TextPropertyData.Base>()
        //    {
        //    new SpecialText.TextPropertyData.Colour(255,0,0),
        //    new SpecialText.TextPropertyData.StaticAppear(),
        //    new SpecialText.TextPropertyData.WaveScaled(1,2,5)
        //    },
        //    0,
        //    TMProText.text.Length
        //    );


        gameObject.SetActive(false);
        nextPrevButton.Event_CancelledWhileHovering += start_transitionToMainMenu;
        SetupDefaults();
        nextPrevButton.Hide();


    }


    public override void EnteredMenu()
    {
        gameObject.SetActive(true);
        SpecialText_progammerTitle.Begin(TextData_programmerTitle);
        SpecialText_artistTitle.Begin(TextData_artistNames);
        SpecialText_designerTitle.Begin(TextData_designerTitle);
    }


    void enteringTransitionUpdate()
    {

    }

    void start_transitionToMainMenu()
    {
        GM_.Instance.tween_manager.StartTweenInstance(
            MenuTransitions.transition_MainMenuToCredits,
            transitionOutputs,
            tweenUpdatedDelegate_: transitionUpdate,
            tweenCompleteDelegate_: end_transitionToMainMenu,
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            TimeFormat_: TweenManager.TIME_FORMAT.FIXED_DELTA
            );
    }
    void end_transitionToMainMenu()
    {

    }





    void transitionUpdate()
    {
      //  Color new_colour = new Color(1, 1, 1, refButtonAlpha.value);
      //  nextPrevButton.Text.color = new_colour;

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
