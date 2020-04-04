using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;
public class SelectableButton : MenuSelectableBase
{
    SpecialText.SpecialText specialText;
    TextMeshProUGUI TMProText;

    RectTransform rectTransform;

    SpecialText.SpecialTextData specialTextData = new SpecialText.SpecialTextData();



    // Default Button Tweens
    static readonly TweenManager.TweenPathBundle default_selectedTween = new TweenManager.TweenPathBundle(
        // Scale X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.9f, 0.1f, TweenManager.CURVE_PRESET.LINEAR), 
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Scale Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.9f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       // Colour R
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.7f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       // Colour G
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.7f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
       // Colour B
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.7f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            )

        );
    static public readonly TweenManager.TweenPathBundle default_hideTween = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
    static public readonly TweenManager.TweenPathBundle default_showTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 1, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
        )
    );


    public RectTransform ThisRectTransform { get { return rectTransform; } }
    public TextMeshProUGUI Text { get { return TMProText; } }


    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        TMProText = GetComponentInChildren<TextMeshProUGUI>();
        specialText = GetComponentInChildren<SpecialText.SpecialText>();
        parentMenu = GetComponentInParent<MenuScreenBase>();

        specialTextData.CreateCharacterData(TMProText.text);
        specialTextData.AddPropertyToText(
            new List<SpecialText.TextProperties.Base>()
            {
            new SpecialText.TextProperties.Colour(255,0,0),
            new SpecialText.TextProperties.StaticAppear(),
            new SpecialText.TextProperties.WaveScaled(1,2,5)
            },
            0,
            TMProText.text.Length
            );

    }



    bool hasBeenInit = false;
    void Init()
    {
        hasBeenInit = true;

        tweenBundle_hide.SetDefaults(Text.color, rectTransform.anchoredPosition, rectTransform.localScale);
        tweenBundle_show.SetDefaults(Text.color, rectTransform.anchoredPosition, rectTransform.localScale);
        tweenBundle_select.SetDefaults(Text.color, rectTransform.anchoredPosition, rectTransform.localScale);

        tweenBundle_hide.CreateParameterFormat(default_hideTween, TWEEN_PARAMETERS.ALPHA);
        tweenBundle_show.CreateParameterFormat(default_showTween, TWEEN_PARAMETERS.ALPHA);
        tweenBundle_select.CreateParameterFormat(default_selectedTween, TWEEN_PARAMETERS.SCALE_X, TWEEN_PARAMETERS.SCALE_Y, TWEEN_PARAMETERS.COLOUR_R, TWEEN_PARAMETERS.COLOUR_G, TWEEN_PARAMETERS.COLOUR_B);
    }

    public void Start()
    {

        if (!hasBeenInit)
        {
            Init();
        }
    }

    enum STATE
    {
    NONE,
    HOVERING
    }
    STATE state = STATE.NONE;
    static readonly float OPTION_SWAP_COOLDOWN = 0.3f;
    float current_optionswap_timer = 0;


    TweenParametersWrapper tweenBundle_show = new TweenParametersWrapper();
    TweenParametersWrapper tweenBundle_select = new TweenParametersWrapper();
    TweenParametersWrapper tweenBundle_hide = new TweenParametersWrapper();

    TweenParametersWrapper currentTweenWrapper;


    public enum TWEEN_PARAMETERS
    { 
        POS_X,
        POS_Y,
        COLOUR_R,
        COLOUR_B,
        COLOUR_G,
        ALPHA,
        SCALE_X,
        SCALE_Y
    }
    class TweenParametersWrapper
    {
        public TypeRef<float> ref_posX = new TypeRef<float>();
        public TypeRef<float> ref_posY = new TypeRef<float>();
        public TypeRef<float> ref_colourR = new TypeRef<float>();
        public TypeRef<float> ref_colourG = new TypeRef<float>();
        public TypeRef<float> ref_colourB = new TypeRef<float>();
        public TypeRef<float> ref_alpha = new TypeRef<float>();
        public TypeRef<float> ref_scaleX = new TypeRef<float>();
        public TypeRef<float> ref_scaleY = new TypeRef<float>();

        public TypeRef<float>[] tweenParameterFormat;

        public TweenManager.TweenPathBundle tweenBundle;

        Color default_colour; 
        Vector2 default_position; 
        Vector2 default_scale;

        public void SetDefaults(Color colour, Vector2 position, Vector2 scale)
        {
            default_colour = colour;
            default_position = position;
            default_scale = scale;
            ApplyDefaults();
        }

        public void ApplyDefaults()
        {
            ref_posX.value = default_position.x;
            ref_posY.value = default_position.y;
            ref_colourR.value = default_colour.r;
            ref_colourG.value = default_colour.g;
            ref_colourB.value = default_colour.b;
            ref_alpha.value = default_colour.a;
            ref_scaleX.value = default_scale.x;
            ref_scaleY.value = default_scale.y;
        }
        
        public void CreateParameterFormat(TweenManager.TweenPathBundle bundle, params TWEEN_PARAMETERS[] tween_parameters )
        {
            ApplyDefaults();

            tweenBundle = bundle;
            tweenParameterFormat = new TypeRef<float>[tween_parameters.Length];

            for (int i = 0; i < tweenParameterFormat.Length; i++)
            {
                switch (tween_parameters[i])
                {
                    case TWEEN_PARAMETERS.ALPHA: tweenParameterFormat[i] = ref_alpha; break;
                    case TWEEN_PARAMETERS.COLOUR_R: tweenParameterFormat[i] = ref_colourR; break;
                    case TWEEN_PARAMETERS.COLOUR_G: tweenParameterFormat[i] = ref_colourG; break;
                    case TWEEN_PARAMETERS.COLOUR_B: tweenParameterFormat[i] = ref_colourB; break;
                    case TWEEN_PARAMETERS.POS_X: tweenParameterFormat[i] = ref_posX; break;
                    case TWEEN_PARAMETERS.POS_Y: tweenParameterFormat[i] = ref_posY; break;
                    case TWEEN_PARAMETERS.SCALE_X: tweenParameterFormat[i] = ref_scaleX; break;
                    case TWEEN_PARAMETERS.SCALE_Y: tweenParameterFormat[i] = ref_scaleY; break;
                }
            }
        }
    }



    public void SetShowTweenBundle(TweenManager.TweenPathBundle tweenBundle, params TWEEN_PARAMETERS[] tween_parameters)
    {
        tweenBundle_show.CreateParameterFormat(tweenBundle, tween_parameters);
    }
    public void SetSelectTweenBundle(TweenManager.TweenPathBundle tweenBundle, params TWEEN_PARAMETERS[] tween_parameters)
    {
        tweenBundle_select.CreateParameterFormat(tweenBundle, tween_parameters);
    }
    public void SetHideTweenBundle(TweenManager.TweenPathBundle tweenBundle, params TWEEN_PARAMETERS[] tween_parameters)
    {
        tweenBundle_hide.CreateParameterFormat(tweenBundle, tween_parameters);
    }

    public override void Selected()
    {
        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
        specialText.Revert();
        state = STATE.NONE;
        currentTweenWrapper = tweenBundle_select;

        GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
            currentTweenWrapper.tweenParameterFormat,
            tweenUpdatedDelegate_: transitioningUpdate,
            tweenCompleteDelegate_: EndSelected,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
    void transitioningUpdate()
    {
        rectTransform.localScale = new Vector3(currentTweenWrapper.ref_scaleX.value, currentTweenWrapper.ref_scaleY.value, 1);
        TMProText.color = new Color(currentTweenWrapper.ref_colourR.value, currentTweenWrapper.ref_colourG.value, currentTweenWrapper.ref_colourB.value, currentTweenWrapper.ref_alpha.value);
        rectTransform.anchoredPosition = new Vector3(currentTweenWrapper.ref_posX.value, currentTweenWrapper.ref_posY.value, 0);

    }
    void EndSelected()
    {
        state = STATE.NONE;
        Invoke_EventSelected();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        if (!hasBeenInit)
            Init();

        specialText.Revert();
        state = STATE.NONE;
        currentTweenWrapper = tweenBundle_show;

        GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
            currentTweenWrapper.tweenParameterFormat,
            tweenUpdatedDelegate_: transitioningUpdate,
            tweenCompleteDelegate_: finishedShow,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
   

    void finishedShow()
    {
        Invoke_FinishedShow();
    }

    public override void Hide()
    {
        currentTweenWrapper = tweenBundle_hide;
        state = STATE.NONE;

        GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
            currentTweenWrapper.tweenParameterFormat,
            tweenUpdatedDelegate_: transitioningUpdate,
            tweenCompleteDelegate_: finishedHide,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
    void finishedHide()
    {

        state = STATE.NONE;
        TMProText.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
        Invoke_FinishedHide();
    }

    public override void HoveredOver()
    {
        gameObject.SetActive(true);
        state = STATE.HOVERING;
        current_optionswap_timer = OPTION_SWAP_COOLDOWN;
        specialText.Begin(specialTextData);
    }


    void HoveredUpdate()
    {
        float verticalVal = GM_.Instance.input.GetAxis(InputManager.AXIS.LV);
        float horizontalVal = GM_.Instance.input.GetAxis(InputManager.AXIS.LH);


        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            Selected();
            return;
        }

        // always go for the axis with the the highest magnitude pushed on it
        if (Mathf.Abs(verticalVal) > Mathf.Abs(horizontalVal))
        {
            if (verticalVal > 0.5f)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingUp != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        EndHovered();
                        siblingUp.HoveredOver();
                    }
                }

            }
            else if (verticalVal < -0.5f)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingDown != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        EndHovered();
                        siblingDown.HoveredOver();
                    }
                }
            }
            else
            {
                current_optionswap_timer = -0.001f;
            }
        }
        else
        {
            if (horizontalVal > 0.5f)
            {
                current_optionswap_timer -= Time.unscaledDeltaTime;

                if (siblingRight != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        EndHovered();
                        siblingRight.HoveredOver();
                    }
                }

            }
            else if (horizontalVal < -0.5f)
            {
                if (siblingLeft != null)
                {
                    if (current_optionswap_timer < 0)
                    {
                        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_CHANGE_SELECTION);
                        EndHovered();
                        siblingLeft.HoveredOver();
                    }
                }
            }
            else
            {
                current_optionswap_timer = -0.001f;
            }
        }


    }

    void EndHovered()
    {
        
        specialText.Revert();
        state = STATE.NONE;
    }

    private void Update()
    {
        switch (state)
        {
            case STATE.HOVERING:
                {
                    HoveredUpdate();
                    break;
                }
        }
    }
}
