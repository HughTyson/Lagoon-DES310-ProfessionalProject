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

    Color imageColour;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        showingPosition = rectTransform.anchoredPosition;
        image.enabled = false;
        imageColour = image.color;

        dissapearTween = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1, 0, 1, TweenManager.CURVE_PRESET.LINEAR)             // ALPHA
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 0, 1, TweenManager.CURVE_PRESET.LINEAR)             // X POS
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, -400, 1, TweenManager.CURVE_PRESET.EASE_INOUT)      // Y POS
            )
        );
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<float> positionValX = new TypeRef<float>();
    TypeRef<float> positionValY = new TypeRef<float>();

    public void Appear(ConversationCharacter character_)
    {

        character = character_;
        image.sprite = character_.characterIcon;
        image.enabled = true;

        state = STATE.APPEARING;
        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween, 
            new TypeRef<float>[] { alphaVal, positionValX, positionValY }, 
            tweenCompleteDelegate_: settledIn, 
            startingDirection_: TweenManager.DIRECTION.END_TO_START,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
    }

    public void Disappear()
    {

        state = STATE.DISSAPEARING;
        GM_.Instance.tween_manager.StartTweenInstance(
            dissapearTween, 
            new TypeRef<float>[] { alphaVal, positionValX, positionValY }, 
            tweenCompleteDelegate_: settledIn,
            tweenUpdatedDelegate_: portraitTransitionUpdate
            );
    }

    public void StartTalking()
    {

    }

    public void StopTalking()
    {

    }

    void settledIn()
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


    public void portraitTransitionUpdate()
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
