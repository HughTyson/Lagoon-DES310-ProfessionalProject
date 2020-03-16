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
    Image image;

    TweenManager.TweenPathBundle dissapearTween;
    TweenManager.TweenPathBundle startTalkingTween;
    TweenManager.TweenPathBundle stopTalkingTween;
    Color imageColour;

    bool transitioning = false;
    bool is_talking = false;

    public event System.Action Event_FinishedChanging;
    public event System.Action Event_FinishedAppearing;


    System.Action<TweenManager.STOP_COMMAND> transitioningTween;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        showingPosition = rectTransform.anchoredPosition;
        image.enabled = false;
        imageColour = image.color;

        dissapearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 1.25f, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 1.25f, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, -400, 1.25f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"))      // Y POS
            )
        );

        startTalkingTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 1.25f, 0.5f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"))
                )
            );

        stopTalkingTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.25f, 1, 0.5f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"), inverse_curve_: true)
                )
            );
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();
    TypeRef<float> scaleVal = new TypeRef<float>(1);
    public void Appear(ConversationCharacter character_)
    {
        transitioning = true;
        character = character_;
        image.sprite = character_.characterIcon;
        image.enabled = true;

        GM_.Instance.tween_manager.StartTweenInstance(
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
        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween, 
            new TypeRef<float>[] { alphaVal, positionValX, positionValY }, 
            tweenCompleteDelegate_: settledIn,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
    }

    public void Talking()
    {
        if (is_talking)
            return;

        is_talking = true;
        transitioning = true;
        GM_.Instance.tween_manager.StartTweenInstance(
            startTalkingTween,
            new TypeRef<float>[] { scaleVal},
            tweenCompleteDelegate_: settledIn,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
    }


    public void NotTalking()
    {
        if (!is_talking)
            return;

        is_talking = false;
        transitioning = true;
        GM_.Instance.tween_manager.StartTweenInstance(
            stopTalkingTween,
            new TypeRef<float>[] { scaleVal },
            tweenCompleteDelegate_: settledIn,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
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

    }
    public void portraitTransitionUpdate()
    {
        imageColour.a = alphaVal.value;
        positionOffset.x = positionValX.value;
        positionOffset.y = positionValY.value;

        rectTransform.localScale = new Vector3(scaleVal.value, scaleVal.value , 1);

        image.color = imageColour;
        rectTransform.anchoredPosition = positionOffset + showingPosition;
    }




    bool changingCharacterStep2Flag = false;
    ConversationCharacter changingCharacter;
    public void ChangeCharacter(ConversationCharacter newCharacter)
    {
        changingCharacterStep2Flag = false;
        changingCharacter = newCharacter;

        transitioning = true;

        if (is_talking)
        {
            is_talking = false;
            GM_.Instance.tween_manager.StartTweenInstance(
                stopTalkingTween,
                new TypeRef<float>[] { scaleVal }
                );
        }


        transitioning = true;
        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween,
            new TypeRef<float>[] { alphaVal, positionValX, positionValY },
            tweenCompleteDelegate_: changingCharacterFinished,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );


    }

    void changingCharacterFinished()
    {

        if (!changingCharacterStep2Flag)
        {
            changingCharacterStep2Flag = true;

            image.sprite = changingCharacter.characterIcon;
            GM_.Instance.tween_manager.StartTweenInstance(
                dissapearTween,
                new TypeRef<float>[] { alphaVal, positionValX, positionValY },
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
