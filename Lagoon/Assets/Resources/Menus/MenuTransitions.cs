using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuTransitions
{

    static public readonly TweenManager.TweenPathBundle transition_MainMenuToCredits = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -5, 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -30.0f, 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 180.0f, 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 0.3f, TweenManager.CURVE_PRESET.LINEAR)
            )
    );

}
