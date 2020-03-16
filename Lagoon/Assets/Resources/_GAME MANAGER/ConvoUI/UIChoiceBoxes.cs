using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIChoiceBoxes : MonoBehaviour
{

    public event System.Action Event_Disappear;

    bool is_boxshowing = false;
    bool is_transitioning = false;

    [SerializeField] Image leftOptionImage;
    [SerializeField] Image leftOptionButtonImage;
    [SerializeField] TMPro.TextMeshProUGUI leftOptionText;
    [SerializeField] Canvas leftCanvas;

    [SerializeField] Image rightOptionImage;
    [SerializeField] Image rightOptionButtonImage;
    [SerializeField] TMPro.TextMeshProUGUI rightOptionText;
    [SerializeField] Canvas rightCanvas;

    TweenManager.TweenPathBundle showTween_inversed;
    TweenManager.TweenPathBundle optionSelectedTween;


    Vector2 leftOptionShowingPosition;
    Vector2 rightOptionShowingPosition;

    public event System.Action Event_FinishedSelection;
    private void Start()
    {
        leftOptionShowingPosition = leftOptionImage.rectTransform.anchoredPosition;
        leftOptionImage.enabled = false;
        leftOptionButtonImage.enabled = false;
        leftOptionText.enabled = false;

        rightOptionShowingPosition = rightOptionImage.rectTransform.anchoredPosition;
        rightOptionImage.enabled = false;
        rightOptionButtonImage.enabled = false;
        rightOptionText.enabled = false;


        showTween_inversed = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(                                                                                                                         //   Left Option Y 
                new TweenManager.TweenPart_Start(0, -300, 0.75f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"), inverse_curve_: true)                   //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Left Option Alpha
                new TweenManager.TweenPart_Start(1, 0, 0.75f, TweenManager.CURVE_PRESET.LINEAR)                                                                  //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Right Option Y
                new TweenManager.TweenPart_Start(0, -300, 0.75f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"), inverse_curve_: true)              //                                                                                           //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Right Option Alpha
                new TweenManager.TweenPart_Start(1, 0, 0.75f, TweenManager.CURVE_PRESET.LINEAR)                                                               //
                )
            );


        optionSelectedTween = new TweenManager.TweenPathBundle(                                                         //
            new TweenManager.TweenPath(                                                                                 // Selected Option Scale
                new TweenManager.TweenPart_Start(1, 1.2f, 0.25f, GM_.Instance.tween_curve_library.GetCurve("OVERSHOOT"))  //
                ),                                                                                                      //
            new TweenManager.TweenPath(                                                                                 // Selected Option Alpha
                new TweenManager.TweenPart_Start(1,1,1.5f,TweenManager.CURVE_PRESET.LINEAR),                            //
                new TweenManager.TweenPart_Continue(0, 1.0f, TweenManager.CURVE_PRESET.LINEAR)                          //
                ),                                                                                                      //
            new TweenManager.TweenPath(                                                                                 //
                new TweenManager.TweenPart_Start(1, 0, 1.0f, TweenManager.CURVE_PRESET.LINEAR)                          // Not Selected Option Alpha
                )

            );
    }


    public bool IsBoxShowing()
    {
        return is_boxshowing;
    }

  
    public bool IsTransitioning()
    {
        return is_transitioning;
    }

    TypeRef<float> leftOptionYVal = new TypeRef<float>();
    TypeRef<float> leftOptionAlphaVal = new TypeRef<float>();
    TypeRef<float> RightOptionYVal = new TypeRef<float>();
    TypeRef<float> RightOptionAlphaVal = new TypeRef<float>();

    TypeRef<float> SelectedOptionScaleVal = new TypeRef<float>();
    TypeRef<float> SelectedOptionAlphaVal = new TypeRef<float>();
    TypeRef<float> NotSelectedOptionAlphaVal = new TypeRef<float>();

    public void Appear(StoryManager.BranchEnterArgs args)
    {
        is_transitioning = true;

        leftOptionImage.enabled = true;
        leftOptionButtonImage.enabled = true;
        leftOptionText.enabled = true;
        rightOptionImage.enabled = true;
        rightOptionButtonImage.enabled = true;
        rightOptionText.enabled = true;

        leftOptionText.text = args.leftChoice;
        rightOptionText.text = args.rightChoice;

        GM_.Instance.tween_manager.StartTweenInstance(
            showTween_inversed,
            new TypeRef<float>[] { leftOptionYVal, leftOptionAlphaVal, RightOptionYVal, RightOptionAlphaVal },
            tweenUpdatedDelegate_: appearingUpdate,
            tweenCompleteDelegate_: appearingFinished,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );
    }


    Color leftColour = new Color(1, 1, 1, 0);
    Color rightColour = new Color(1, 1, 1, 0);
    Vector2 leftOffset = new Vector2(0, 0);
    Vector2 rightOffset = new Vector2(0, 0);

    void appearingUpdate()
    {
        leftColour.a = leftOptionAlphaVal.value;
        rightColour.a = RightOptionAlphaVal.value;
        leftOffset.y = leftOptionYVal.value;
        rightOffset.y = RightOptionYVal.value;

        leftOptionButtonImage.color = leftColour;
        leftOptionImage.color = leftColour;
        leftOptionText.color = new Color(0,0,0, leftColour.a);
        leftOptionImage.rectTransform.anchoredPosition = leftOffset + leftOptionShowingPosition;

        rightOptionImage.color = rightColour;
        rightOptionButtonImage.color = rightColour;
        rightOptionText.color = new Color(0, 0, 0, rightColour.a); ;
        rightOptionImage.rectTransform.anchoredPosition = rightOffset + rightOptionShowingPosition;


    }
    void appearingFinished()
    {
        is_transitioning = false;
    }


    BranchingNode.CHOICE choice;
    public void SelectOption(BranchingNode.CHOICE choice_)
    {
        is_transitioning = true;
        choice = choice_;

        switch(choice)
        {
            case BranchingNode.CHOICE.LEFT:
                {
                    leftCanvas.sortingOrder = 2;
                    rightCanvas.sortingOrder = 1;
                    break;
                }
            case BranchingNode.CHOICE.RIGHT:
                {
                    leftCanvas.sortingOrder = 1;
                    rightCanvas.sortingOrder = 2;
                    break;
                }
        }


        GM_.Instance.tween_manager.StartTweenInstance(
            optionSelectedTween,
            new TypeRef<float>[] { SelectedOptionScaleVal, SelectedOptionAlphaVal, NotSelectedOptionAlphaVal},
            tweenUpdatedDelegate_: selectingUpdate,
            tweenCompleteDelegate_: selectingFinished
            );
    }




    void selectingUpdate()
    {
        switch (choice)
        {
            case BranchingNode.CHOICE.LEFT:
                {
                    leftColour.a = SelectedOptionAlphaVal.value;
                    leftOptionImage.rectTransform.localScale = new Vector3(SelectedOptionScaleVal.value, SelectedOptionScaleVal.value, 1);
                    rightColour.a = NotSelectedOptionAlphaVal.value;
                    break;
                }
            case BranchingNode.CHOICE.RIGHT:
                {
                    rightColour.a = SelectedOptionAlphaVal.value;
                    rightOptionImage.rectTransform.localScale = new Vector3(SelectedOptionScaleVal.value, SelectedOptionScaleVal.value, 1);
                    leftColour.a = NotSelectedOptionAlphaVal.value;
                    break;
                }        
        }

        leftOptionImage.rectTransform.anchoredPosition = leftOptionShowingPosition;
        leftOptionButtonImage.color = leftColour;
        leftOptionImage.color = leftColour;
        leftOptionText.color = new Color(0, 0, 0, leftColour.a); ;

        rightOptionImage.color = rightColour;
        rightOptionButtonImage.color = rightColour;
        rightOptionText.color = new Color(0, 0, 0, rightColour.a); ;
        rightOptionImage.rectTransform.anchoredPosition = rightOptionShowingPosition;
    }

    public void SkipTransition()
    {

    }
    void selectingFinished()
    {
        is_transitioning = false;

        leftOptionImage.enabled = false;
        leftOptionButtonImage.enabled = false;
        leftOptionText.enabled = false;
        rightOptionImage.enabled = false;
        rightOptionButtonImage.enabled = false;
        rightOptionText.enabled = false;
        rightOptionImage.rectTransform.localScale = Vector3.one;
        leftOptionImage.rectTransform.localScale = Vector3.one;

        Event_FinishedSelection?.Invoke();

    }

}
