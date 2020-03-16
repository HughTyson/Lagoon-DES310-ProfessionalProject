using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDialogBox : MonoBehaviour
{
   // DialogStruct dialog;

    Vector2 showingPosition;
    Vector2 positionOffset = Vector2.zero;
    RectTransform rectTransform;
    TMPro.TextMeshProUGUI text;
    Image image;

    TweenManager.TweenPathBundle dissapearTween;
    TweenManager.TweenPathBundle textAppearTween;

    [SerializeField] Image ButtonA_Image;

    Vector2 AButtonShowingPosition;

    Color imageColour;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        textShowingPos = text.rectTransform.anchoredPosition;
        image = GetComponent<Image>();
        showingPosition = rectTransform.anchoredPosition;
        image.enabled = false;
        text.alpha = 0;
        imageColour = image.color;

        AButtonShowingPosition = ButtonA_Image.rectTransform.anchoredPosition;

        ButtonA_Image.color = new Color(1, 1, 1, 0);

        dissapearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 1.5f, GM_.Instance.tween_curve_library.GetCurve("TEST"))             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, -400, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)      // Y POS
            )
        );

        textAppearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, 0.25f, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 0, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(-50, 0, 0.5f, TweenManager.CURVE_PRESET.EASE_OUT)      // Y POS
            )
        );
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();

    public event System.Action Event_BoxFinishedAppearing;

    public void Appear()
    {
        image.enabled = true;

        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenCompleteDelegate_: boxFinishedAppearing,
            tweenUpdatedDelegate_: boxTransitionUpdate,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );
    }

    public void Disappear()
    {

        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenCompleteDelegate_: boxFinishedDissappearing,
            tweenUpdatedDelegate_: boxTransitionUpdate
            );
    }

    public void WriteText(string dialog_)
    {
        text.text = dialog_;
        GM_.Instance.tween_manager.StartTweenInstance(
            textAppearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenUpdatedDelegate_: textTransitionUpdate,
            tweenCompleteDelegate_: textFinishedAppearing
            );
    }

    void textFinishedAppearing()
    {
        _showAButton();
    }

    void _showAButton()
    {
        imageColour = new Color(1, 1, 1, 1);
        GM_.Instance.tween_manager.StartTweenInstance(
            textAppearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenUpdatedDelegate_: aButtonUpdate,
            tweenCompleteDelegate_: aButtonShowComplete

            );
    }


    void aButtonUpdate()
    {
        imageColour.a = alphaVal.value;
        positionOffset.x = positionValX.value;
        positionOffset.y = positionValY.value;

        ButtonA_Image.color = imageColour;
        ButtonA_Image.rectTransform.anchoredPosition = positionOffset + AButtonShowingPosition;
    }
    void aButtonShowComplete()
    {

    }


    void boxFinishedAppearing()
    {
        Event_BoxFinishedAppearing?.Invoke();
    }
    void boxFinishedDissappearing()
    {

    }
       

    public bool IsTransitioning()
    {
        return (state == STATE.APPEARING || state == STATE.DISSAPEARING || state == STATE.LEFT_STARTED_TALKING || state == STATE.RIGHT_STARTED_TALKING);
    }

    STATE state;
    enum STATE
    {
        UNACIVE,
        APPEARING,
        DISSAPEARING,
        SETTLED,
        LEFT_STARTED_TALKING,
        RIGHT_STARTED_TALKING
    }


    Vector2 textOffset = Vector2.zero;
    Vector2 textShowingPos;
    void textTransitionUpdate()
    {

        textOffset.x = positionValX.value;
        textOffset.y = positionValY.value;

        text.alpha = alphaVal.value;
        text.rectTransform.anchoredPosition = textShowingPos + textOffset;
    }
    void boxTransitionUpdate()
    {
        imageColour.a = alphaVal.value;
        positionOffset.x = positionValX.value;
        positionOffset.y = positionValY.value;

        image.color = imageColour;
        rectTransform.anchoredPosition = positionOffset + showingPosition;
    }

    public void StateUpdate()
    {

    }
}
