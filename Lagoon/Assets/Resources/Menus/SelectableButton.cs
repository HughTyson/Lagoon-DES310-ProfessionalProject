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

    TypeRef<float> refScale = new TypeRef<float>();
    TypeRef<float> refColour = new TypeRef<float>();

    static readonly TweenManager.TweenPathBundle selectedTween = new TweenManager.TweenPathBundle(
        // Scale
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.9f, 0.1f, TweenManager.CURVE_PRESET.LINEAR), 
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Colour
       new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0.7f, 0.1f, TweenManager.CURVE_PRESET.LINEAR),
            new TweenManager.TweenPart_Continue(1.0f, 0.1f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );

    static readonly TweenManager.TweenPathBundle showTween = new TweenManager.TweenPathBundle(
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 1, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
        )
    );
    static public readonly TweenManager.TweenPathBundle hideTween = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(1, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            )
        );


    public RectTransform ThisRectTransform { get { return rectTransform; } }
    public TextMeshProUGUI Text { get { return TMProText; } }


    Color default_colour; 
    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        TMProText = GetComponentInChildren<TextMeshProUGUI>();
        specialText = GetComponentInChildren<SpecialText.SpecialText>();
        parentMenu = GetComponentInParent<MenuScreenBase>();
        default_colour = TMProText.color;

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
    public void Start()
    {


    }

    enum STATE
    {
    NONE,
    HOVERING,
    SELECTING,
    HIDDEN
    }
    STATE state = STATE.NONE;
    static readonly float OPTION_SWAP_COOLDOWN = 0.3f;
    float current_optionswap_timer = 0;
    

    public override void Selected()
    {
        specialText.Revert();
        state = STATE.SELECTING;
        GM_.Instance.tween_manager.StartTweenInstance(selectedTween,
            new TypeRef<float>[] { refScale, refColour },
            tweenUpdatedDelegate_: SelectedUpdate,
            tweenCompleteDelegate_: EndSelected,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
    void SelectedUpdate()
    {
        rectTransform.localScale = new Vector3(refScale.value, refScale.value, 1);
        TMProText.color = new Color(refColour.value, refColour.value, refColour.value );
    }
    void EndSelected()
    {
        state = STATE.NONE;
        Invoke_EventSelected();
    }

    public override void InstantHide()
    {
        state = STATE.HIDDEN;
        gameObject.SetActive(false);
        TMProText.color = new Color(default_colour.r, default_colour.g, default_colour.b, 0);
    }
    public override void InstantShow()
    {
        state = STATE.NONE;
        gameObject.SetActive(true);
        TMProText.color = default_colour;
    }
    public override void Hide()
    {
        GM_.Instance.tween_manager.StartTweenInstance(hideTween,
            new TypeRef<float>[] { refColour },
            tweenUpdatedDelegate_: SelectedUpdate,
            tweenCompleteDelegate_: InstantHide,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        GM_.Instance.tween_manager.StartTweenInstance(showTween,
             new TypeRef<float>[] { refColour },
             tweenUpdatedDelegate_: SelectedUpdate,
             tweenCompleteDelegate_: InstantShow,
             TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }

    public override void HoveredOver()
    {
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
        else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.B))
        {
            state = STATE.NONE;
            Invoke_CancelledWhileHovering();
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
            case STATE.SELECTING:
                {
                    SelectedUpdate();
                    break;
                }      
        }
    }
    public void OnEnable()
    {

    }


}
