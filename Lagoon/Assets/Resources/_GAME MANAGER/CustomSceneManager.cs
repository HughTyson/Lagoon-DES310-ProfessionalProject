using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CustomSceneManager : MonoBehaviour
{
   public event System.Action Event_AboutToChangeScene;

    [SerializeField] Image Image_background;
    [SerializeField] Image Image_loadingIcon;
    [SerializeField] SpecialText.SpecialText SpecialTxt_loadingText;

    static readonly float FADE_IN_OR_OUT_TIME = 0.5f;
    static readonly float TOTAL_START_OR_END_TIME = 1.0f;
    static readonly float LOADING_SCREEN_LOOP_TIME = 1.0f;

    TweenManager.TweenPathBundle tween_startLoading;
    TweenManager.TweenPathBundle tween_continueLoading;
    TweenManager.TweenPathBundle tween_endLoading;

    TweenAnimator.Animation anim_startLoading;
    TweenAnimator.Animation anim_continueLoading;
    TweenAnimator.Animation anim_endLoading;



    SpecialText.SpecialTextData loadingScreenText_data = new SpecialText.SpecialTextData();
    private void Awake()
    {
        loadingScreenText_data.CreateCharacterData(SpecialTxt_loadingText.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text);
        loadingScreenText_data.AddPropertyToText
            (
            new List<SpecialText.TextProperties.Base>()
            {
            new SpecialText.TextProperties.CharSpeed(10),
            new SpecialText.TextProperties.Colour(255,255,255),
            new SpecialText.TextProperties.WaveScaled(2,3,5)
            },
            0,
            loadingScreenText_data.fullTextString.Length
            );



        tween_startLoading = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0, 1, FADE_IN_OR_OUT_TIME, TweenManager.CURVE_PRESET.LINEAR) // background alpha
            ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(0,1, TOTAL_START_OR_END_TIME, TweenManager.CURVE_PRESET.LINEAR) // Normalized time
                ),
            new TweenManager.TweenPath( // icon scale
                new TweenManager.TweenPart_Start(0,0, FADE_IN_OR_OUT_TIME, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(1, TOTAL_START_OR_END_TIME - FADE_IN_OR_OUT_TIME, TweenCurveLibrary.DefaultLibrary, "BIG_OVERSHOOT")
                ),
            new TweenManager.TweenPath( // icon rotation
                new TweenManager.TweenPart_Start(180, 180, FADE_IN_OR_OUT_TIME, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(0, TOTAL_START_OR_END_TIME - FADE_IN_OR_OUT_TIME, TweenManager.CURVE_PRESET.LINEAR)
                )
        );
        tween_continueLoading = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 1.0f, LOADING_SCREEN_LOOP_TIME, TweenManager.CURVE_PRESET.LINEAR)
            ),
            new TweenManager.TweenPath( // icon rotation
                new TweenManager.TweenPart_Start(359, 0, LOADING_SCREEN_LOOP_TIME, TweenManager.CURVE_PRESET.LINEAR)
                )
        );
        tween_endLoading = new TweenManager.TweenPathBundle(
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 0.0f, LOADING_SCREEN_LOOP_TIME, TweenManager.CURVE_PRESET.LINEAR)
            ),
            new TweenManager.TweenPath( // icon rotation
                new TweenManager.TweenPart_Start(359, 0, LOADING_SCREEN_LOOP_TIME, TweenManager.CURVE_PRESET.LINEAR)
                ),
            new TweenManager.TweenPath(
                new TweenManager.TweenPart_Start(1.0f, 1.0f, LOADING_SCREEN_LOOP_TIME, TweenManager.CURVE_PRESET.LINEAR),
                new TweenManager.TweenPart_Continue(0, FADE_IN_OR_OUT_TIME, TweenManager.CURVE_PRESET.LINEAR)
            )
        );

        anim_startLoading = new TweenAnimator.Animation(
            tween_startLoading,
            new TweenAnimator.Image_ (
                Image_background,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
             new TweenAnimator.Image_(
                Image_loadingIcon,
                color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.TransfRect_ (
                Image_loadingIcon.rectTransform,
                rotation: new TweenAnimator.TransfRect_.Rotation_(3, TweenAnimator.MOD_TYPE.ABSOLUTE)
                ),
            new TweenAnimator.SpecialText_(
                SpecialTxt_loadingText,
                hideCall: new TweenAnimator.SpecialText_.Hide_(
                    1,
                    0.0f,
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN
                    ),
                beginCall: new TweenAnimator.SpecialText_.Begin_(
                    1, 
                    (TOTAL_START_OR_END_TIME - FADE_IN_OR_OUT_TIME) / TOTAL_START_OR_END_TIME, 
                    TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.GREATEREQUAL_THAN, 
                    loadingScreenText_data
                    )
                )
            );

        anim_continueLoading = new TweenAnimator.Animation(
            tween_continueLoading,
            new TweenAnimator.Image_(
            Image_background,
            color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
            new TweenAnimator.TransfRect_(
                Image_loadingIcon.rectTransform,
                rotation: new TweenAnimator.TransfRect_.Rotation_(1, TweenAnimator.MOD_TYPE.ABSOLUTE)
            )
            );

        anim_endLoading = new TweenAnimator.Animation(
        tween_endLoading,
        new TweenAnimator.Image_(
            Image_background,
            color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
        new TweenAnimator.Image_(
            Image_loadingIcon,
            color: new TweenAnimator.Image_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
        new TweenAnimator.TransfRect_(
            Image_loadingIcon.rectTransform,
            rotation: new TweenAnimator.TransfRect_.Rotation_(1, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
        new TweenAnimator.TMPText_(
            SpecialTxt_loadingText.GetComponent<TMPro.TextMeshProUGUI>(), 
            new TweenAnimator.TMPText_.Color_(false, -1, false, -1, false, -1, true, 0, TweenAnimator.MOD_TYPE.ABSOLUTE)
            ),
        new TweenAnimator.SpecialText_(
            SpecialTxt_loadingText,
            hideCall: new TweenAnimator.SpecialText_.Hide_(0, 0.001f, TweenAnimator.Base.TriggerProperty.TRIGGER_TYPE.LESSEQUAL_THAN)
           
            )
        );



        gameObject.SetActive(false);
    }



    int transitionSceneID = 0;
    int transitioningStartFuncs = 0;


    void startTransition()
    {
        Event_AboutToChangeScene?.Invoke();
        transitioningStartFuncs = 2;
        SceneManager.sceneLoaded += sceneLoaded;
        gameObject.SetActive(true);
        GM_.Instance.input.InputEnabled = false;
        anim_startLoading.PlayAnimation(animationCompleteDelegate_: continueTransition, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
    void continueTransition()
    {
        anim_continueLoading.PlayAnimation(animationCompleteDelegate_: endTransition, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA, path_: TweenManager.PATH.LOOP);
        SceneManager.LoadSceneAsync(transitionSceneID, LoadSceneMode.Single);
    }
    void endTransition()
    {
        anim_endLoading.PlayAnimation(animationCompleteDelegate_: transitionCompleted, TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA);
    }
    void transitionCompleted()
    {
        SceneManager.sceneLoaded -= sceneLoaded;
        GM_.Instance.input.InputEnabled = true;
        gameObject.SetActive(false);
    }

    void sceneLoaded(Scene s, LoadSceneMode mode)
    {
        anim_continueLoading.StopAnimation(TweenManager.STOP_COMMAND.SEEMLESS_TO_END);
    }
    public void ChangeScene(int sceneID)
    {
        if (anim_continueLoading.IsPlaying || anim_startLoading.IsPlaying || anim_endLoading.IsPlaying)
        {
            Debug.LogError("Error, attempt to change scene, but scene is already changing!");
            Debug.Break();
        }

        transitionSceneID = sceneID;
        startTransition();
    }

}
