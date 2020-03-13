using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharacterPortrait : MonoBehaviour
{

    Vector2 hiddenPosition;

    [SerializeField]
    Vector2 showingPosition;
  

    RectTransform rectTransform;
    Image image;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        showingPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = hiddenPosition;
        image.enabled = false;
    }

    TypeRef<float> alphaVal = new TypeRef<float>();
    TypeRef<Vector2> positionVal = new TypeRef<Vector2>();
    public void Appear(Sprite characterArt)
    {
        image.sprite = characterArt;

        GM_.Instance.tween_manager.CreateTween(
            new TweenManager.FullTween(
                settledIn,
                new TweenManager.CompositeTween<TweenManager.BaseTween>(
                    new TweenManager.Tween1D(alphaVal, 0, 0.5f, 0.3f, TweenManager.TRANSF.LINEAR),
                    new TweenManager.Tween1D(alphaVal, 0.5f, 1, 0.6f, TweenManager.TRANSF.LINEAR)
                    ),
                new TweenManager.CompositeTween<TweenManager.BaseTween>(
                    new TweenManager.Tween2D(positionVal, hiddenPosition, showingPosition, new Vector2(1, 1), new System.Tuple<TweenManager.TRANSF, TweenManager.TRANSF>(TweenManager.TRANSF.LINEAR, TweenManager.TRANSF.LINEAR))
                    )
                )
            );

        state = STATE.APPEARING;
    }

    public void Disappear()
    {        
        state = STATE.DISSAPEARING;
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


    public void Update()
    {
        switch(state)
        {
            case STATE.UNACIVE:
                {

                    break;
                }
            case STATE.APPEARING:
                {

                    break;
                }
            case STATE.DISSAPEARING:
                {

                    break;
                }
            case STATE.LEFT_STARTED_TALKING:
                {

                    break;
                }
            case STATE.RIGHT_STARTED_TALKING:
                {

                    break;
                }
            case STATE.SETTLED:
                {

                    break;
                }
            
            
        }

    }
}
