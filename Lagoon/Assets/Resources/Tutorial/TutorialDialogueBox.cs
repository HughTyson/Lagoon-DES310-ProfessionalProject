using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueBox : MonoBehaviour
{
    // DialogStruct dialog;

    Vector2 showingPosition;
    Vector2 positionOffset = Vector2.zero;
    RectTransform rectTransform;
    public TMPro.TextMeshProUGUI tmp;
    [SerializeField] public SpecialText.SpecialText specialText;
    public SpecialText.SpecialTextData string_data;
    Image image;

    TweenManager.TweenPathBundle boxTweenDissAndAppearTween;
    TweenManager.TweenPathBundle textAppearTween;

    [SerializeField] Image ButtonA_Image;

    Vector2 AButtonShowingPosition;

    Color imageColour;
    public bool transitioning = false;
    bool isBoxShowing = false;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        textShowingPos = tmp.rectTransform.anchoredPosition;
        image = GetComponent<Image>();
        showingPosition = rectTransform.anchoredPosition;
        image.enabled = false;
        tmp.alpha = 1.0f;
        imageColour = image.color;

        AButtonShowingPosition = ButtonA_Image.rectTransform.anchoredPosition;

        ButtonA_Image.color = new Color(1, 1, 1, 0);

        boxTweenDissAndAppearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 1.0f, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(-760, -1160, 1.0f, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 1.0f, TweenManager.CURVE_PRESET.EASE_INOUT)      // Y POS
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
                new TweenManager.TweenPart_Start(-50, 0, 0.25f, TweenManager.CURVE_PRESET.EASE_OUT)      // Y POS
            )
        );
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();


    TweenManager.TweenInstanceInterface currentTweenInstance = new TweenManager.TweenInstanceInterface(null);
    public void Appear()
    {

        ButtonA_Image.color = new Color(1, 1, 1, 0);
        specialText.Hide();

        transitioning = true;
        image.enabled = true;
        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            boxTweenDissAndAppearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenCompleteDelegate_: boxFinishedAppearing,
            tweenUpdatedDelegate_: boxTransitionUpdate,
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA

            );
    }

    public void Disappear()
    {
        ButtonA_Image.color = new Color(1, 1, 1, 0);

        transitioning = true;
        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            boxTweenDissAndAppearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenCompleteDelegate_: boxFinishedDissappearing,
            tweenUpdatedDelegate_: boxTransitionUpdate,
            //startingDirection_: TweenManager.DIRECTION.START_TO_END,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
            );
    }

    public void WriteText(SpecialText.SpecialTextData specialTextData)
    {
        ButtonA_Image.color = new Color(1, 1, 1, 0);
        transitioning = true;
        //text.text = dialog_;
        //GM_.Instance.tween_manager.StartTweenInstance(
        //    textAppearTween,
        //    new TypeRef<float>[] { alphaVal, positionValX, positionValY },
        //    tweenUpdatedDelegate_: textTransitionUpdate,
        //    tweenCompleteDelegate_: textFinishedAppearing
        //    );
        specialText.End();
        specialText.Begin(specialTextData, textFinishedAppearing);
    }
    public void ClearContinueSymbol()
    {
        ButtonA_Image.color = new Color(1, 1, 1, 0);
    }
    void textFinishedAppearing()
    {
        transitioning = false;
        _showAButton();
    }


    void _showAButton()
    {
        transitioning = true;
        imageColour = new Color(1, 1, 1, 1);
        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            textAppearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenUpdatedDelegate_: aButtonUpdate,
            tweenCompleteDelegate_: aButtonShowComplete,
            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA

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
        transitioning = false;
    }


    void boxFinishedAppearing()
    {
        isBoxShowing = true;
        transitioning = false;
        tmp.enabled = true;
        specialText.Begin(string_data, textFinishedAppearing);

       
    }
    void boxFinishedDissappearing()
    {
        transitioning = false;
        isBoxShowing = false;

        

        string_data.CreateCharacterData("");

        specialText.Begin(string_data);

    }

    public void SkipTransition()
    {

        
        if (!specialText.AreAllCompleted())
        {
            specialText.ForceAll();
        }

        while (transitioning)
        {
            if (currentTweenInstance.Exists)
            {
                currentTweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
                if (!specialText.AreAllCompleted())
                {
                    specialText.ForceAll();
                }
            }
            else
            {
                break;
            }
        }
    }
    public bool IsTransitioning()
    {
        return transitioning;
    }
    public bool IsBoxShowing()
    {
        return isBoxShowing;
    }

    Vector2 textOffset = Vector2.zero;
    Vector2 textShowingPos;

    void boxTransitionUpdate()
    {
        imageColour.a = alphaVal.value;
        positionOffset.x = positionValX.value;
        positionOffset.y = positionValY.value;

        image.color = imageColour;
        rectTransform.anchoredPosition = positionOffset + showingPosition;
    }
}
