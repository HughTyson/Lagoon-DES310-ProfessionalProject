using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuTransitions
{

    static public readonly TweenManager.TweenPathBundle transition_MainMenuToCredits = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -5, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -30.0f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -180.0f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
            )
    );
    static public readonly TweenManager.TweenPathBundle transition_CreditsToMenu = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 5, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 30.0f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 180.0f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.LINEAR)
        )
);


    static public readonly TweenManager.TweenPathBundle transition_CreditToExtraCredits = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 8, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 1.5f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, -6, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, -3.3f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 17, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
);
    static public readonly TweenManager.TweenPathBundle transition_ExtraCreditsToCredits = new TweenManager.TweenPathBundle(
// Camera X
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -8, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Y
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -1.5f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Z
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, 6, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera X Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, 3.3f, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Y Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -17, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Z Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, 0, 1.5f, TweenManager.CURVE_PRESET.EASE_INOUT)
    )
);

}
