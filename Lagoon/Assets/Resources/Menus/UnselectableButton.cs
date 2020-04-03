using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnselectableButton : MonoBehaviour
{

    TextMeshProUGUI TMProText;
    RectTransform TextRectTransform;
    
    RectTransform rectTransform;
    Image image;

    public event System.Action Event_Selected;
    public event System.Action Event_FinishedHide;
    public event System.Action Event_FinishedShow;


    // This type ref is given to all buttons in a group. Only one button in a group can be selected at a time
    TypeRef<bool> isGroupingButtonSelected = new TypeRef<bool>(false);



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
            ),
                new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );
    static public readonly TweenManager.TweenPathBundle default_showTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 1, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
        ),
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
        TextRectTransform = TMProText.GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();

    }


    bool hasBeenInit = false;
    void Init()
    {
        hasBeenInit = true;
        tweenBundle_hide.SetDefaults(image.color, image.rectTransform.anchoredPosition, image.rectTransform.localScale, TMProText.color, TMProText.rectTransform.anchoredPosition, TMProText.rectTransform.localScale, rectTransform.anchoredPosition, rectTransform.localScale);
        tweenBundle_show.SetDefaults(image.color, image.rectTransform.anchoredPosition, image.rectTransform.localScale, TMProText.color, TMProText.rectTransform.anchoredPosition, TMProText.rectTransform.localScale, rectTransform.anchoredPosition, rectTransform.localScale);
        tweenBundle_select.SetDefaults(image.color, image.rectTransform.anchoredPosition, image.rectTransform.localScale, TMProText.color, TMProText.rectTransform.anchoredPosition, TMProText.rectTransform.localScale, rectTransform.anchoredPosition, rectTransform.localScale);

        tweenBundle_hide.CreateParameterFormat(default_hideTween, TWEEN_PARAMETERS.TEXT_ALPHA, TWEEN_PARAMETERS.IMAGE_ALPHA);
        tweenBundle_show.CreateParameterFormat(default_showTween, TWEEN_PARAMETERS.TEXT_ALPHA, TWEEN_PARAMETERS.IMAGE_ALPHA);
        tweenBundle_select.CreateParameterFormat(default_selectedTween, TWEEN_PARAMETERS.IMAGE_SCALE_X, TWEEN_PARAMETERS.IMAGE_SCALE_Y, TWEEN_PARAMETERS.IMAGE_COLOUR_R, TWEEN_PARAMETERS.IMAGE_COLOUR_G, TWEEN_PARAMETERS.IMAGE_COLOUR_B, TWEEN_PARAMETERS.TEXT_COLOUR_R, TWEEN_PARAMETERS.TEXT_COLOUR_G, TWEEN_PARAMETERS.TEXT_COLOUR_B);
    }
    public void Start()
    {
        if (!hasBeenInit)
            Init();
    }


    static readonly float OPTION_SWAP_COOLDOWN = 0.3f;
    float current_optionswap_timer = 0;
 //   bool isPressable = false;
    bool isTransitioning = false;

    TweenParametersWrapper tweenBundle_show = new TweenParametersWrapper();
    TweenParametersWrapper tweenBundle_select = new TweenParametersWrapper();
    TweenParametersWrapper tweenBundle_hide = new TweenParametersWrapper();

    TweenParametersWrapper currentTweenWrapper;


    enum TWEEN_ID_TYPE
    {
        SELECTED,
        SHOW,
        HIDE
    }

    public enum TWEEN_PARAMETERS
    {
        TEXT_POS_X,
        TEXT_POS_Y,
        TEXT_COLOUR_R,
        TEXT_COLOUR_B,
        TEXT_COLOUR_G,
        TEXT_ALPHA,
        TEXT_SCALE_X,
        TEXT_SCALE_Y,
        IMAGE_POS_X,
        IMAGE_POS_Y,
        IMAGE_COLOUR_R,
        IMAGE_COLOUR_B,
        IMAGE_COLOUR_G,
        IMAGE_ALPHA,
        IMAGE_SCALE_X,
        IMAGE_SCALE_Y,
        PARENT_POS_X,
        PARENT_POS_Y,
        PARENT_SCALE_X,
        PARENT_SCALE_Y
    }
    class TweenParametersWrapper
    {
        public TypeRef<float> ref_ParentposX = new TypeRef<float>();
        public TypeRef<float> ref_ParentposY = new TypeRef<float>();
        public TypeRef<float> ref_ParentscaleX = new TypeRef<float>();
        public TypeRef<float> ref_ParentscaleY = new TypeRef<float>();
        public TypeRef<float> ref_ImageposX = new TypeRef<float>();
        public TypeRef<float> ref_ImageposY = new TypeRef<float>();
        public TypeRef<float> ref_ImagescaleX = new TypeRef<float>();
        public TypeRef<float> ref_ImagescaleY = new TypeRef<float>();
        public TypeRef<float> ref_ImagecolourR = new TypeRef<float>();
        public TypeRef<float> ref_ImagecolourG = new TypeRef<float>();
        public TypeRef<float> ref_ImagecolourB = new TypeRef<float>();
        public TypeRef<float> ref_Imagealpha = new TypeRef<float>();
        public TypeRef<float> ref_TextposX = new TypeRef<float>();
        public TypeRef<float> ref_TextposY = new TypeRef<float>();
        public TypeRef<float> ref_TextscaleX = new TypeRef<float>();
        public TypeRef<float> ref_TextscaleY = new TypeRef<float>();
        public TypeRef<float> ref_TextcolourR = new TypeRef<float>();
        public TypeRef<float> ref_TextcolourG = new TypeRef<float>();
        public TypeRef<float> ref_TextcolourB = new TypeRef<float>();
        public TypeRef<float> ref_Textalpha = new TypeRef<float>();

        public TypeRef<float>[] tweenParameterFormat;

        public TweenManager.TweenPathBundle tweenBundle;

        Color default_text_colour;
        Vector2 default_text_position;
        Vector2 default_text_scale;
        Color default_image_colour;
        Vector2 default_image_position;
        Vector2 default_image_scale;
        Vector2 default_parent_position;
        Vector2 default_parent_scale;
        public void SetDefaults(Color image_colour_, Vector2 image_position_, Vector2 image_scale_, Color text_colour_, Vector2 text_position_, Vector2 text_scale_, Vector2 parent_position_,  Vector2 parent_scale_)
        {
            default_text_colour = text_colour_;
            default_text_position = text_position_;
            default_text_scale = text_scale_;
            default_image_colour = image_colour_;
            default_image_position = image_position_;
            default_image_scale = image_scale_;
            default_parent_position = parent_position_;
            default_parent_scale = parent_scale_;
            ApplyDefaults();
        }

        public void ApplyDefaults()
        {
            ref_ParentposX.value = default_parent_position.x;
            ref_ParentposY.value = default_parent_position.y;
            ref_ParentscaleX.value = default_parent_scale.x;
            ref_ParentscaleY.value = default_parent_scale.y;
            ref_ImageposX.value = default_image_position.x;
            ref_ImageposY.value = default_image_position.y;
            ref_ImagescaleX.value = default_image_scale.x;
            ref_ImagescaleY.value = default_image_scale.y;
            ref_ImagecolourR.value = default_image_colour.r;
            ref_ImagecolourG.value = default_image_colour.g;
            ref_ImagecolourB.value = default_image_colour.b;
            ref_Imagealpha.value = default_image_colour.a;
            ref_TextposX.value = default_text_position.x;
            ref_TextposY.value = default_text_position.y;
            ref_TextscaleX.value = default_text_scale.x;
            ref_TextscaleY.value = default_text_scale.y;
            ref_TextcolourR.value = default_text_colour.r;
            ref_TextcolourG.value = default_text_colour.g;
            ref_TextcolourB.value = default_text_colour.b;
            ref_Textalpha.value = default_text_colour.a;
        }

        public void CreateParameterFormat(TweenManager.TweenPathBundle bundle, params TWEEN_PARAMETERS[] tween_parameters)
        {
            ApplyDefaults();

            tweenBundle = bundle;
            tweenParameterFormat = new TypeRef<float>[tween_parameters.Length];

            for (int i = 0; i < tweenParameterFormat.Length; i++)
            {
                switch (tween_parameters[i])
                {
                    case TWEEN_PARAMETERS.TEXT_POS_X         :tweenParameterFormat[i] = ref_TextposX;     break;
                    case TWEEN_PARAMETERS.TEXT_POS_Y         :tweenParameterFormat[i] = ref_TextposY;     break;
                    case TWEEN_PARAMETERS.TEXT_COLOUR_R      :tweenParameterFormat[i] = ref_TextcolourR;    break;
                    case TWEEN_PARAMETERS.TEXT_COLOUR_B      :tweenParameterFormat[i] = ref_TextcolourG;    break;
                    case TWEEN_PARAMETERS.TEXT_COLOUR_G      :tweenParameterFormat[i] = ref_TextcolourB;     break;
                    case TWEEN_PARAMETERS.TEXT_ALPHA         :tweenParameterFormat[i] = ref_Textalpha;     break;
                    case TWEEN_PARAMETERS.TEXT_SCALE_X       :tweenParameterFormat[i] = ref_TextscaleX;     break;
                    case TWEEN_PARAMETERS.TEXT_SCALE_Y       :tweenParameterFormat[i] = ref_TextscaleY;     break;
                    case TWEEN_PARAMETERS.IMAGE_POS_X        :tweenParameterFormat[i] = ref_ParentposX;     break;
                    case TWEEN_PARAMETERS.IMAGE_POS_Y        :tweenParameterFormat[i] = ref_ParentposY;     break;
                    case TWEEN_PARAMETERS.IMAGE_COLOUR_R     :tweenParameterFormat[i] = ref_ImagecolourR;     break;
                    case TWEEN_PARAMETERS.IMAGE_COLOUR_B     :tweenParameterFormat[i] = ref_ImagecolourB;     break;
                    case TWEEN_PARAMETERS.IMAGE_COLOUR_G     :tweenParameterFormat[i] = ref_ImagecolourG;     break;
                    case TWEEN_PARAMETERS.IMAGE_ALPHA        :tweenParameterFormat[i] = ref_Imagealpha;     break;
                    case TWEEN_PARAMETERS.IMAGE_SCALE_X      :tweenParameterFormat[i] = ref_ImagescaleX;     break;
                    case TWEEN_PARAMETERS.IMAGE_SCALE_Y      :tweenParameterFormat[i] = ref_ImagescaleY;     break;
                    case TWEEN_PARAMETERS.PARENT_POS_X       :tweenParameterFormat[i] = ref_ParentposX;     break;
                    case TWEEN_PARAMETERS.PARENT_POS_Y       :tweenParameterFormat[i] = ref_ParentposY;     break;
                    case TWEEN_PARAMETERS.PARENT_SCALE_X     :tweenParameterFormat[i] = ref_ParentscaleX;     break;
                    case TWEEN_PARAMETERS.PARENT_SCALE_Y     :tweenParameterFormat[i] = ref_ParentscaleY;     break;
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



    public void Selected()
    {

        GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);

        if (current_tweenInstance.Exists)
        {
            current_tweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        }

        currentTweenWrapper = tweenBundle_select;
        isTransitioning = true;
        isGroupingButtonSelected.value = true;


        current_tweenInstance = GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
                currentTweenWrapper.tweenParameterFormat,
                tweenUpdatedDelegate_: transitioningUpdate,
                tweenCompleteDelegate_: EndSelected,
                TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA,
                instanceID: TWEEN_ID_TYPE.SELECTED);
    }
    void transitioningUpdate()
    {
        rectTransform.localScale = new Vector3(currentTweenWrapper.ref_ParentscaleX.value, currentTweenWrapper.ref_ParentscaleY.value, 1);
        rectTransform.anchoredPosition = new Vector3(currentTweenWrapper.ref_ParentposX.value, currentTweenWrapper.ref_ParentposY.value, 0);

        TMProText.color = new Color(currentTweenWrapper.ref_TextcolourR.value, currentTweenWrapper.ref_TextcolourG.value, currentTweenWrapper.ref_TextcolourB.value, currentTweenWrapper.ref_Textalpha.value);
        TMProText.rectTransform.localScale = new Vector3(currentTweenWrapper.ref_TextscaleX.value, currentTweenWrapper.ref_TextscaleY.value, 1);
        TMProText.rectTransform.anchoredPosition = new Vector3(currentTweenWrapper.ref_TextposX.value, currentTweenWrapper.ref_TextposY.value, 0);

        image.color = new Color(currentTweenWrapper.ref_ImagecolourR.value, currentTweenWrapper.ref_ImagecolourG.value, currentTweenWrapper.ref_ImagecolourB.value, currentTweenWrapper.ref_Imagealpha.value);
        image.rectTransform.localScale = new Vector3(currentTweenWrapper.ref_ImagescaleX.value, currentTweenWrapper.ref_ImagescaleY.value, 1);
        image.rectTransform.anchoredPosition = new Vector3(currentTweenWrapper.ref_ImageposX.value, currentTweenWrapper.ref_ImageposY.value, 0);

    }
    void EndSelected()
    {
        isTransitioning = false;
        isGroupingButtonSelected.value = false;
        Event_Selected?.Invoke();
    }

    public void AssignToGroup(TypeRef<bool> referenceLink)
    {
        isGroupingButtonSelected = referenceLink;
    }

    TweenManager.TweenInstanceInterface current_tweenInstance = new TweenManager.TweenInstanceInterface(null);

    //public void IsPressable(bool isPressable_)
    //{
    //    isPressable = isPressable_;
    //}
    public void Show()
    {
        if (!hasBeenInit)
            Init();

        if (current_tweenInstance.Exists)
        {
            current_tweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        }

        gameObject.SetActive(true);
        currentTweenWrapper = tweenBundle_show;
        isTransitioning = false;




        current_tweenInstance = GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
            currentTweenWrapper.tweenParameterFormat,
            tweenUpdatedDelegate_: transitioningUpdate,
            tweenCompleteDelegate_: finishedShow,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA,
            instanceID: TWEEN_ID_TYPE.SHOW);
    }
    void finishedShow()
    {
        isTransitioning = false;
        Event_FinishedShow?.Invoke();
    }

    public void Hide()
    {
        if (current_tweenInstance.Exists)
        {
            current_tweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
        }

        isTransitioning = true;
        currentTweenWrapper = tweenBundle_hide;


        current_tweenInstance = GM_.Instance.tween_manager.StartTweenInstance(currentTweenWrapper.tweenBundle,
            currentTweenWrapper.tweenParameterFormat,
            tweenUpdatedDelegate_: transitioningUpdate,
            tweenCompleteDelegate_: finishedHide,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA,
            instanceID: TWEEN_ID_TYPE.HIDE );
    }
    void finishedHide()
    {
        isTransitioning = false;
        gameObject.SetActive(false);
        Event_FinishedHide?.Invoke();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        isTransitioning = false;
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
        isTransitioning = false;
    }
    InputManager.BUTTON[] buttonsToCheck = new InputManager.BUTTON[0];
    public void SetButtonsToCheckForPress(params InputManager.BUTTON[] button)
    {
        buttonsToCheck = button;
    }
    void Update()
    {
        if (!isGroupingButtonSelected.value)
        {
            if (!isTransitioning)
            {
                for (int i = 0; i < buttonsToCheck.Length; i++)
                {
                    if (GM_.Instance.input.GetButtonDown(buttonsToCheck[i]))
                    {
                        Selected();
                        break;
                    }
                }
            }
        }
    }

}
