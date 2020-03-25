using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuTransitions
{
    static float TRANSITION_TIME = 1.5f;

    static public readonly TweenManager.TweenPathBundle transition_MainMenuToCredits = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -5, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -30.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -180.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
            )
    );
    static public readonly TweenManager.TweenPathBundle transition_CreditsToMenu = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 5, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 30.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 180.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.LINEAR)
        )
);


    static public readonly TweenManager.TweenPathBundle transition_CreditToExtraCredits = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 8, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 1.5f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, -6, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 30.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 17, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
);
    static public readonly TweenManager.TweenPathBundle transition_ExtraCreditsToCredits = new TweenManager.TweenPathBundle(
// Camera X
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -8, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Y
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -1.5f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Z
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, 6, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera X Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -30.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Y Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, -17, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
// Camera Z Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    )
);

    static public readonly TweenManager.TweenPathBundle transition_MainToGameOptions = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -3, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0.7f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 7.5f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 18, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 100, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToMain = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 3, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -0.7f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -7.5f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -18, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -100, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );
    

    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToControls = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -3, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0.3f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 3.2f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 82, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );







    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToAudio = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0.4f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0,1.0f, TRANSITION_TIME/2.0f, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(0.0f, TRANSITION_TIME/2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 5.4f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 8, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -165.2f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

        static public readonly TweenManager.TweenPathBundle transition_AudioToGameOptions = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -0.4f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 1.0f, TRANSITION_TIME / 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(0.0f, TRANSITION_TIME / 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -5.4f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, -8, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 165.2f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(0, 0, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

}
