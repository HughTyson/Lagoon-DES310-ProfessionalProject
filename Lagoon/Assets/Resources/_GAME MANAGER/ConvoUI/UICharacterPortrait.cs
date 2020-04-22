using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharacterPortrait : MonoBehaviour
{
    ConversationCharacter character;

    Vector2 showingPosition;
    Vector2 positionOffset = Vector2.zero;
    RectTransform rectTransform;
    [SerializeField] Image border_image;
    [SerializeField] Image character_image;

    TweenManager.TweenPathBundle dissapearTween;
    TweenManager.TweenPathBundle startTalkingTween;
    TweenManager.TweenPathBundle stopTalkingTween;
    TweenManager.TweenPathBundle changeCharacterTween;
    Color imageColour;

    bool transitioning = false;
    bool is_talking = false;
    public event System.Action Event_FinishedChanging;
    public event System.Action Event_FinishedAppearing;
    public event System.Action Event_FinishedDisappearing;

    System.Action<TweenManager.STOP_COMMAND> transitioningTween;

    [SerializeField] Sprite sprTalkingBorder;
    [SerializeField] Sprite sprNotTalkingBorder;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        showingPosition = rectTransform.anchoredPosition;
        border_image.enabled = false;
        character_image.enabled = false;
        imageColour = border_image.color;

        dissapearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 1.25f, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 1.25f, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, -400, 1.25f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")      // Y POS
            )
        );

        changeCharacterTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 0.25f, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            )
            );


startTalkingTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 1.25f, 0.5f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")
                )
            );

        stopTalkingTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.25f, 1, 0.5f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT", inverse_curve_: true)
                )
            );
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();
    TypeRef<float> scaleVal = new TypeRef<float>(1);


    TweenManager.TweenInstanceInterface currentTweenInstance = new TweenManager.TweenInstanceInterface(null);
    public void Appear(ConversationCharacter character_)
    {
        transitioning = true;
        character = character_;
        character_image.sprite = character_.characterIcon;
        border_image.enabled = true;
        character_image.enabled = true;

        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween, 
            new TypeRef<float>[] { alphaVal, positionValX, positionValY }, 
            tweenCompleteDelegate_: finishedAppearing, 
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );        
    }

    public void Disappear()
    {
        transitioning = true;
        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween, 
            new TypeRef<float>[] { alphaVal, positionValX, positionValY }, 
            tweenCompleteDelegate_: dissapeared,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
    }

    public void Talking()
    {
        if (is_talking)
            return;

        is_talking = true;
        transitioning = true;
        border_image.sprite = sprTalkingBorder;
        // character_image.rectTransform.sizeDelta = new Vector2(360, 360);
        character_image.rectTransform.localScale = new Vector3(1.32f, 1.32f, 1);
        character_image.rectTransform.anchoredPosition = new Vector2(-8, 12);
        settledIn();

        //GM_.Instance.tween_manager.StartTweenInstance(
        //    startTalkingTween,
        //    new TypeRef<float>[] { scaleVal},
        //    tweenCompleteDelegate_: settledIn,
        //    tweenUpdatedDelegate_: portraitTransitionUpdate
        //    );
    }

    void dissapeared()
    {
        settledIn();
        Event_FinishedDisappearing?.Invoke();
    }

    public void NotTalking()
    {
        if (!is_talking)
            return;

        is_talking = false;
        transitioning = true;
        border_image.sprite = sprNotTalkingBorder;
        character_image.rectTransform.localScale = new Vector3(1f, 1f, 1);
  
        character_image.rectTransform.anchoredPosition = new Vector2(0, 0);
        //character_image.rectTransform.sizeDelta = new Vector2(270, 270);

        settledIn();
        //GM_.Instance.tween_manager.StartTweenInstance(
        //    stopTalkingTween,
        //    new TypeRef<float>[] { scaleVal },
        //    tweenCompleteDelegate_: settledIn,
        //    tweenUpdatedDelegate_: portraitTransitionUpdate
        //    );
    }

    void finishedAppearing()
    {
        transitioning = false;
        Event_FinishedAppearing?.Invoke();
    }
    void settledIn()
    {
        transitioning = false;
    }

    public bool IsTransitioning()
    {
        return transitioning;
    }

    public void SkipTransition()
    {
        while (transitioning)
        {
            if (currentTweenInstance.Exists)
            {
                currentTweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
            }
            else
            {
                break;
            }
        }

    }
    public void portraitTransitionUpdate()
    {
        imageColour.a = alphaVal.value;
        positionOffset.x = positionValX.value;
        positionOffset.y = positionValY.value;

        rectTransform.localScale = new Vector3(scaleVal.value, scaleVal.value , 1);

        character_image.color = imageColour;
        border_image.color = imageColour;
        rectTransform.anchoredPosition = positionOffset + showingPosition;
        
    }




    bool changingCharacterStep2Flag = false;
    ConversationCharacter changingCharacter;
    public void ChangeCharacter(ConversationCharacter newCharacter)
    {
        NotTalking();

        changingCharacterStep2Flag = false;
        changingCharacter = newCharacter;

        transitioning = true;



        // use events instead of 2 tweens
        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            changeCharacterTween,
            new TypeRef<float>[] { alphaVal},
            tweenCompleteDelegate_: changingCharacterFinished,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );


    }

    void changingCharacterFinished()
    {

        if (!changingCharacterStep2Flag)
        {
            changingCharacterStep2Flag = true;

            character_image.sprite = changingCharacter.characterIcon;

            currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
                changeCharacterTween,
                new TypeRef<float>[] { alphaVal },
                tweenCompleteDelegate_: changingCharacterFinished,
                tweenUpdatedDelegate_: portraitTransitionUpdate,
                startingDirection_: TweenManager.DIRECTION.END_TO_START
                );
        }
        else
        {
            transitioning = false;
            Event_FinishedChanging?.Invoke();
        }
    }
}
