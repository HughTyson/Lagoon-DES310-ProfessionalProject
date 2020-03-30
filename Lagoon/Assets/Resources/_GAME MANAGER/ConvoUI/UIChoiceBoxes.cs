using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIChoiceBoxes : MonoBehaviour
{

    public event System.Action Event_Disappear;

    bool is_boxshowing = false;
    bool is_transitioning = false;

    [SerializeField] SpecialText.SpecialText specialText_Left;
    [SerializeField] SpecialText.SpecialText specialText_Right;


 
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

    TweenManager.TweenPathBundle buttonShowTween;


    SpecialText.SpecialTextData specialTextData_Left;
    SpecialText.SpecialTextData specialTextData_Right;


    Vector2 leftOptionShowingPosition;
    Vector2 rightOptionShowingPosition;

    public event System.Action Event_FinishedSelection;

    TweenManager.TweenInstanceInterface currentTweenInstance = new TweenManager.TweenInstanceInterface(null);

    private void Start()
    {
        showTween_inversed = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(                                                                                                                         //   Left Option Y 
                new TweenManager.TweenPart_Start(0, -400, 0.75f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT", inverse_curve_: true)                   //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Left Option Alpha
                new TweenManager.TweenPart_Start(1, 0, 0.75f, TweenManager.CURVE_PRESET.LINEAR)                                                                  //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Right Option Y
                new TweenManager.TweenPart_Start(0, -400, 0.75f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT", inverse_curve_: true)              //                                                                                           //
                ),                                                                                                                                              //
            new TweenManager.TweenPath(                                                                                                                         //   Right Option Alpha
                new TweenManager.TweenPart_Start(1, 0, 0.75f, TweenManager.CURVE_PRESET.LINEAR)                                                               //
                )
            );
        optionSelectedTween = new TweenManager.TweenPathBundle(                                                         //
            new TweenManager.TweenPath(                                                                                 // Selected Option Scale
                new TweenManager.TweenPart_Start(1, 1.15f, 0.25f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")  //
                ),                                                                                                      //
            new TweenManager.TweenPath(                                                                                 // Selected Option Y
                new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR),                            //
                new TweenManager.TweenPart_Continue(-400, 0.75f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")                          //
                ),                                                                                                      //
            new TweenManager.TweenPath(                                                                                 //
                new TweenManager.TweenPart_Start(0, -400, 0.75f, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")                          // Not Selected Option Y
                )
            );
        buttonShowTween = new TweenManager.TweenPathBundle(
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
        leftOptionShowingPosition = leftOptionImage.rectTransform.anchoredPosition;
        leftOptionImage.enabled = false;
        leftOptionButtonImage.enabled = false;
        leftOptionText.enabled = false;

        rightOptionShowingPosition = rightOptionImage.rectTransform.anchoredPosition;
        rightOptionImage.enabled = false;
        rightOptionButtonImage.enabled = false;
        rightOptionText.enabled = false;



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
    TypeRef<float> SelectedOptionYVal = new TypeRef<float>();
    TypeRef<float> NotSelectedOptionYVal = new TypeRef<float>();

    TypeRef<float> Ref_ButtonAlpha = new TypeRef<float>();
    TypeRef<float> Ref_ButtonXPos = new TypeRef<float>();
    TypeRef<float> Ref_ButtonYPos = new TypeRef<float>();

    public void Appear(StoryManager.BranchEnterArgs args)
    {
        is_transitioning = true;

        leftOptionImage.enabled = true;

        leftOptionButtonImage.enabled = true;
        leftOptionText.enabled = true;
        rightOptionImage.enabled = true;
        rightOptionButtonImage.enabled = true;
        rightOptionText.enabled = true;

        leftOptionText.text = "";
        rightOptionText.text = "";

        leftOptionButtonImage.color = new Color(1, 1, 1, 0);
        rightOptionButtonImage.color = new Color(1, 1, 1, 0);

        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            showTween_inversed,
            new TypeRef<float>[] { leftOptionYVal, leftOptionAlphaVal, RightOptionYVal, RightOptionAlphaVal },
            tweenUpdatedDelegate_: appearingUpdate,
            tweenCompleteDelegate_: appearingFinished,
            startingDirection_: TweenManager.DIRECTION.END_TO_START
            );

        specialTextData_Left = args.leftChoice;
        specialTextData_Right = args.rightChoice;

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

        leftOptionImage.color = leftColour;
        leftOptionText.alpha = leftColour.a;
        leftOptionImage.rectTransform.anchoredPosition = leftOffset + leftOptionShowingPosition;

        rightOptionImage.color = rightColour;
        rightOptionText.alpha = rightColour.a ;
        rightOptionImage.rectTransform.anchoredPosition = rightOffset + rightOptionShowingPosition;


    }
    void appearingFinished()
    {
        counterTextFinished = 2;
        specialText_Left.End();
        specialText_Left.Begin(specialTextData_Left, textFinished);

        specialText_Right.End();
        specialText_Right.Begin(specialTextData_Right, textFinished);
    }


    int counterTextFinished;
    void textFinished()
    {
        counterTextFinished--;
        if (counterTextFinished == 0)
            currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
                buttonShowTween,
                new TypeRef<float>[] { Ref_ButtonAlpha , Ref_ButtonXPos, Ref_ButtonYPos },
                tweenUpdatedDelegate_: buttonsShowUpdate,
                tweenCompleteDelegate_: buttonsShown
                );
    }

    void buttonsShowUpdate()
    {
        leftOptionButtonImage.color = new Color(1, 1, 1, Ref_ButtonAlpha.value);
        rightOptionButtonImage.color = new Color(1, 1, 1, Ref_ButtonAlpha.value);
    }
    void buttonsShown()
    {
        is_transitioning = false;
    }

    BranchingNode.CHOICE choice;
    public void SelectOption(BranchingNode.CHOICE choice_)
    {
        is_transitioning = true;
        choice = choice_;

        specialText_Left.End();
        specialText_Right.End();

        switch (choice)
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


        currentTweenInstance = GM_.Instance.tween_manager.StartTweenInstance(
            optionSelectedTween,
            new TypeRef<float>[] { SelectedOptionScaleVal, SelectedOptionYVal, NotSelectedOptionYVal},
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
                    leftOffset.y = SelectedOptionYVal.value;
                    leftOptionImage.rectTransform.localScale = new Vector3(SelectedOptionScaleVal.value, SelectedOptionScaleVal.value, 1);
                    rightOffset.y = NotSelectedOptionYVal.value;
                    break;
                }
            case BranchingNode.CHOICE.RIGHT:
                {
                    rightOffset.y = SelectedOptionYVal.value;
                    rightOptionImage.rectTransform.localScale = new Vector3(SelectedOptionScaleVal.value, SelectedOptionScaleVal.value, 1);
                    leftOffset.y = NotSelectedOptionYVal.value;
                    break;
                }        
        }

        leftOptionImage.rectTransform.anchoredPosition = leftOffset + leftOptionShowingPosition;
        leftOptionButtonImage.color = leftColour;
        leftOptionImage.color = leftColour;
        leftOptionText.alpha = leftColour.a; 

        rightOptionImage.color = rightColour;
        rightOptionButtonImage.color = rightColour;
        rightOptionText.alpha = rightColour.a; 
        rightOptionImage.rectTransform.anchoredPosition = rightOffset + rightOptionShowingPosition;
    }

    public void SkipTransition()
    {


        if (!specialText_Left.AreAllCompleted())
        {
            specialText_Left.ForceAll();
        }
        if (!specialText_Right.AreAllCompleted())
        {
            specialText_Right.ForceAll();
        }
        while (is_transitioning)
        {
            if (currentTweenInstance.Exists)
            {
                currentTweenInstance.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE_TO_END);
                if (counterTextFinished != 0)
                {
                    specialText_Left.ForceAll();
                    specialText_Right.ForceAll();
                }
            }
            else
            {
                break;
            }
        }
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
