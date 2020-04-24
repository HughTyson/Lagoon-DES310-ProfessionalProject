using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectableButton_TextButton : SelectableButton_
{
    private void Awake()
    {
        Init();
    }

    [SerializeField] SpecialText.SpecialText specialText;
    TMPro.TextMeshProUGUI TMPText;
    [SerializeField] bool dont_wave = false;


    static readonly TweenManager.TweenPathBundle defaultShowButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 1, 0.0f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
    static readonly TweenManager.TweenPathBundle defaultHideButtonTween = new TweenManager.TweenPathBundle(
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1.0f, 0.0f, 0.5f, TweenManager.CURVE_PRESET.LINEAR)
            )        
        );





    protected sealed override void ThisInit_Layer3() 
    {
        TMPText = specialText.GetComponent<TMPro.TextMeshProUGUI>();
    }



    protected override void ApplyDefaults()
    {
        TweenManager.TweenPathBundle defaultHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 1.5f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultUnHoverOverButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.2f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.5f, 1.0f, 0.2f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        TweenManager.TweenPathBundle defaultSelectButtonTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0.49f, 0.245f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(0.49f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 0.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.5f, 1.25f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
                 new TweenManager.TweenPart_Continue(1.5f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
                )
            );



        SpecialText.SpecialTextData specialTextData_Show = new SpecialText.SpecialTextData();
        specialTextData_Show.CreateCharacterData(TMPText.text);
        specialTextData_Show.AddPropertyToText
            (
            new List<SpecialText.TextProperties.Base>
            {
                new SpecialText.TextProperties.AppearAtOnce(),
                new SpecialText.TextProperties.Colour(255,255,255)
            },
            0,
            specialTextData_Show.specialTextCharacters.Count
            );

        TweenAnimator.Animation showAnimation = new TweenAnimator.Animation(
            defaultShowButtonTween,
            new TweenAnimator.Generic_(
                resetbaseColour,
                new TweenAnimator.Generic_.Trigger_(0, 0.0f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN)
                ),
            new TweenAnimator.SpecialText_(
                specialText,
                new TweenAnimator.SpecialText_.Begin_(
                    0,
                    0.0f,
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN,
                    specialTextData_Show
                    )
                 ),
            new TweenAnimator.TransfRect_(
                    TMPText.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    )
            );



        TweenAnimator.Animation hideAnimation = new TweenAnimator.Animation(
            defaultHideButtonTween,
            new TweenAnimator.SpecialText_(
                specialText,
                forceAllCall: new TweenAnimator.SpecialText_.ForceAll_(
                    0,
                    0.0f,
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN
                    )
                ),
            new TweenAnimator.TMPText_(
                TMPText,
                color: new TweenAnimator.TMPText_.Color_(false, -1, false, -1, false, -1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                )
            );



        // CHANGING THE HOVER COLOUR VALUE
        SpecialText.SpecialTextData specialTextData_Hover = new SpecialText.SpecialTextData();
        specialTextData_Hover.CreateCharacterData(TMPText.text);
        if(!dont_wave)
        {
            specialTextData_Hover.AddPropertyToText
                (
                new List<SpecialText.TextProperties.Base>
                {
                    new SpecialText.TextProperties.StaticAppear(),
                    new SpecialText.TextProperties.Colour(22,203,170), // <-----------------------------------
                    new SpecialText.TextProperties.WaveScaled(1,2.0f,2)
                },
                0,
                specialTextData_Hover.specialTextCharacters.Count
                );
        }
        else if(dont_wave)
        {
            specialTextData_Hover.AddPropertyToText
                (
                new List<SpecialText.TextProperties.Base>
                {
                                new SpecialText.TextProperties.StaticAppear(),
                                new SpecialText.TextProperties.Colour(22,203,170), // <-----------------------------------
                                //new SpecialText.TextProperties.WaveScaled(1,2.0f,2)
                },
                0,
                specialTextData_Hover.specialTextCharacters.Count
                );
        }


        TweenAnimator.Animation hoverAnimation = new TweenAnimator.Animation(
            defaultHoverOverButtonTween,
            new TweenAnimator.SpecialText_(
                specialText,
                new TweenAnimator.SpecialText_.Begin_(
                    0,
                    0.0f,
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN,
                    specialTextData_Hover
                    )
                ),
                new TweenAnimator.TransfRect_(
                    TMPText.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    )
            );
        

        SpecialText.SpecialTextData specialTextData_UnHover = new SpecialText.SpecialTextData();
        specialTextData_UnHover.CreateCharacterData(TMPText.text);
        specialTextData_UnHover.AddPropertyToText
            (
            new List<SpecialText.TextProperties.Base>
            {
                new SpecialText.TextProperties.StaticAppear(),
                new SpecialText.TextProperties.Colour(255,255,255)
            },
            0,
            specialTextData_UnHover.specialTextCharacters.Count
            );

        TweenAnimator.Animation unHoverAnimation = new TweenAnimator.Animation(
            defaultUnHoverOverButtonTween,
            new TweenAnimator.SpecialText_(
                specialText,
                new TweenAnimator.SpecialText_.Begin_(
                    0,
                    0.0f,
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN,
                    specialTextData_UnHover
                    )
                ),
                new TweenAnimator.TransfRect_(
                    TMPText.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 1, true, 1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    )
            );


        TweenAnimator.Animation selectAnimation = new TweenAnimator.Animation(
            defaultSelectButtonTween,
                new TweenAnimator.TransfRect_(
                    TMPText.rectTransform,
                    scale: new TweenAnimator.TransfRect_.Scale_(true, 2, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    ),
                new TweenAnimator.TMPText_(
                    TMPText,
                    color: new TweenAnimator.TMPText_.Color_(true, 0, true, 0, true, 1, false, -1, TweenAnimator.MOD_TYPE.ABSOLUTE)
                    )
            );

        OverrideShowAnimation(showAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideHideAnimation(hideAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginHoverOverAnimation(hoverAnimation,false ,new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginUnHoverOverAnimation(unHoverAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
        OverrideBeginSelectAnimation(selectAnimation, new TweenAnimator.Animation.PlayArgs(TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA));
    }

    void resetbaseColour()
    {
        TMPText.color = new Color(1, 1, 1, 1);
    }



}
