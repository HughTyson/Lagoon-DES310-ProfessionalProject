using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalLogic : MonoBehaviour
{
    TweenManager.TweenPathBundle showTween;
    TweenAnimator.Animation showAnimation;

    [SerializeField] GameObject journal;
    [SerializeField] GameObject page;
    [SerializeField] GameObject frontCover;
    [SerializeField] GameObject backCover;

    //static float TIME_BOOK_SHOW = 1.5f;
    //static float TIME_BOOK_OPEN = 1.5f;

    Animator animator;
    int showBookAnimID = Animator.StringToHash("showJournal");
    int pageTurnLeftID = Animator.StringToHash("showJournal");
    int pageTurnRightID = Animator.StringToHash("turnPageRight");
    int hideBookAnimID = Animator.StringToHash("showJournal");
    void Awake()
    {
        animator = GetComponent<Animator>();
        //showTween = new TweenManager.TweenPathBundle(
        //    // Journal X Position
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(0, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_OUT)
        //            ),
        //    // Journal Y Position
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(-0.31f, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_OUT)
        //            ),
        //    // Journal Z Position
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(0.175f, 0, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_OUT)
        //            ),
        //    // Journal X Rotation
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(30, 60, TIME_BOOK_SHOW, TweenManager.CURVE_PRESET.EASE_OUT)
        //            ),

        //    // Front Cover Z Rotation
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(180.0f, 180.0f, 0, TweenManager.CURVE_PRESET.LINEAR),
        //        new TweenManager.TweenPart_Continue(-1.07f, TIME_BOOK_OPEN, TweenManager.CURVE_PRESET.EASE_OUT)
        //            ),

        //    // Page X Position
        //    new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(180.0f, 180.0f, 0, TweenManager.CURVE_PRESET.LINEAR)
        //            ),
        //   // Page Y Position
        //   new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(180.0f, 180.0f, 0, TweenManager.CURVE_PRESET.LINEAR)
        //            ),
        //   // Page Z Position
        //   new TweenManager.TweenPath(
        //        new TweenManager.TweenPart_Start(180.0f, 180.0f, 0, TweenManager.CURVE_PRESET.LINEAR)
        //            )
        //    //// Cover Open, Page Z Rotation
        //    //new TweenManager.TweenPath(
        //    //    new TweenManager.TweenPart_Start(90.0f, 90.0f, 0, TweenManager.CURVE_PRESET.LINEAR),
        //    //    new TweenManager.TweenPart_Continue(-86.4f, TIME_BOOK_OPEN, TweenManager.CURVE_PRESET.EASE_OUT)
        //    //        )
        //    );

        //showAnimation = new TweenAnimator.Animation(
        //    showTween,
        //    new TweenAnimator.Transf_(
        //        journal.transform,
        //        local_position: new TweenAnimator.Transf_.LocalPosition_(true, 0, true, 1, true, 2, TweenAnimator.MOD_TYPE.ABSOLUTE),
        //        local_rotation: new TweenAnimator.Transf_.LocalRotation_(true, 3, false, -1, false ,-1, TweenAnimator.MOD_TYPE.ABSOLUTE)
        //        ),
        //    new TweenAnimator.Transf_(
        //        frontCover.transform,
        //        local_rotation: new TweenAnimator.Transf_.LocalRotation_(false, -1, false, -1, true, 4, TweenAnimator.MOD_TYPE.ABSOLUTE)
        //        ),
        //    new TweenAnimator.Transf_(
        //        page.transform,
        //        local_rotation: new TweenAnimator.Transf_.LocalRotation_(false, -1, false, -1, true, 5, TweenAnimator.MOD_TYPE.ABSOLUTE)
        //        )
        //    );
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            //if (!showAnimation.IsPlaying )
            //{
            //    showAnimation.PlayAnimation();
            //}
            animator.Play(showBookAnimID, 0);
        }
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        {
            animator.Play(pageTurnLeftID, 0);
        }
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.RB))
        {
            animator.Play(pageTurnRightID, 0);
        }
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        {
            animator.Play(hideBookAnimID, 0);
        }
    }


    public void Show()
    {

    }

    public void Hide()
    {

    }
}
