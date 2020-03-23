using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen_MainMenu : MenuScreenBase
{
    [SerializeField] Camera camera_;

    [SerializeField] SelectableButton startButton;
    [SerializeField] SelectableButton optionsButton;
    [SerializeField] SelectableButton creditsButton;
    [SerializeField] SelectableButton exitButton;


    ValueWithDefault<Vector3> cameraPosition = new ValueWithDefault<Vector3>();
    ValueWithDefault<Vector3> cameraRotation = new ValueWithDefault<Vector3>();
    ValueWithDefault<Color> buttonColour = new ValueWithDefault<Color>();
    // Start is called before the first frame update
    void Start()
    {
        startButton.Event_Selected += start_transitionToGame;
        optionsButton.Event_Selected += start_transitionToOptions;
        creditsButton.Event_Selected += start_transitionToCredits;
        exitButton.Event_Selected += start_transitionToExit;


        cameraPosition.SetDefault(camera_.transform.position);
        cameraRotation.SetDefault(camera_.transform.rotation.eulerAngles);
        buttonColour.SetDefault(new Color(1, 1, 1, 1));
        startButton.HoveredOver();
    }


    TypeRef<float> refButtonAlpha = new TypeRef<float>();

    // Transition to credits
    //static TweenManager.TweenPathBundle transitionToCraditsTween = new TweenManager.TweenPathBundle(
    //    // Buttons Alpha
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(1, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
    //        ),
    //    // Camera X
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(0,0, 0.3f, TweenManager.CURVE_PRESET.LINEAR),
    //        new TweenManager.TweenPart_Continue()
    //        ),
    //    // Camera Y
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR),
    //        new TweenManager.TweenPart_Continue()
    //        ),
    //    // Camera Z
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR),
    //        new TweenManager.TweenPart_Continue()
    //        ),
    //    // Camera X Rot
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR),
    //        new TweenManager.TweenPart_Continue()
    //        ),
    //    // Camera Y Rot
    //    new TweenManager.TweenPath(
    //        new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR),
    //        new TweenManager.TweenPart_Continue()
    //        )
    //    );

    void hideMyButtons_Update()
    {

    }

    void start_transitionToGame()
    {
        
    }

    void start_transitionToCredits()
    {

    }

    void start_transitionToExit()
    {

    }

    void start_transitionToOptions()
    {

    }
    

    void transitionUpdate()
    {
        startButton.Text.color = new Color(1, 1, 1, refButtonAlpha.value);
        optionsButton.Text.color = new Color(1, 1, 1, refButtonAlpha.value);
        creditsButton.Text.color = new Color(1, 1, 1, refButtonAlpha.value);
        exitButton.Text.color = new Color(1, 1, 1, refButtonAlpha.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
