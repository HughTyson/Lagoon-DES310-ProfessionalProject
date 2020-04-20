using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuTransitions
{
    static float TRANSITION_TIME = 1.5f;

    public struct CameraPositionAndRotation
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    public static readonly CameraPositionAndRotation MainMenuVals = new CameraPositionAndRotation() { position = new Vector3(0, 0.77f, 0), rotation = new Vector3(0, -90, 0) };
    public static readonly CameraPositionAndRotation CreditVals = new CameraPositionAndRotation() { position = new Vector3(-1.68f,1.85f ,0), rotation = new Vector3(-30, -270, 0) };
    public static readonly CameraPositionAndRotation ExtraCreditVals = new CameraPositionAndRotation() { position = new Vector3(3.06f, 2.68f, -5.71f), rotation = new Vector3(0, -253, 0) };
    public static readonly CameraPositionAndRotation GameOptionsVals = new CameraPositionAndRotation() { position = new Vector3(-2.53f, 1.78f, 7.5f), rotation = new Vector3(18, 10, 0) };
    public static readonly CameraPositionAndRotation AudioOptionsVals = new CameraPositionAndRotation() { position = new Vector3(-2.04f, 1.82f, 12.9f), rotation = new Vector3(26, -155.2f, 0) };
    public static readonly CameraPositionAndRotation ControlOptionVals = new CameraPositionAndRotation() { position = new Vector3(-4.84f, 2.8f, 11.35f), rotation = new Vector3(38.4f, -258.8f, 0) };


    static public readonly TweenManager.TweenPathBundle transition_MainMenuToCredits = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.x, CreditVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.y, CreditVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.z, CreditVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.x, CreditVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.y, CreditVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.z, CreditVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            )
    );
    static public readonly TweenManager.TweenPathBundle transition_CreditsToMenu = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(CreditVals.position.x, MainMenuVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(CreditVals.position.y, MainMenuVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(CreditVals.position.z, MainMenuVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(CreditVals.rotation.x, MainMenuVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start( CreditVals.rotation.y, MainMenuVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start( CreditVals.rotation.z, MainMenuVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
);


    static public readonly TweenManager.TweenPathBundle transition_CreditToExtraCredits = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.position.x, ExtraCreditVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.position.y, ExtraCreditVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.position.z, ExtraCreditVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.rotation.x, ExtraCreditVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.rotation.y, ExtraCreditVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(CreditVals.rotation.z, ExtraCreditVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
);
    static public readonly TweenManager.TweenPathBundle transition_ExtraCreditsToCredits = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.position.x, CreditVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.position.y, CreditVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.position.z, CreditVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.rotation.x, CreditVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.rotation.y, CreditVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ExtraCreditVals.rotation.z, CreditVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    )
);

    static public readonly TweenManager.TweenPathBundle transition_MainToGameOptions = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.x, GameOptionsVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.y, GameOptionsVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.position.z, GameOptionsVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.x, GameOptionsVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.y, GameOptionsVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(MainMenuVals.rotation.z, GameOptionsVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToMain = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.x, MainMenuVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.y, MainMenuVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.z, MainMenuVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.x, MainMenuVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start( GameOptionsVals.rotation.y, MainMenuVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.z, MainMenuVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );
    

    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToControls = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.x, ControlOptionVals.position.x , TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.y, ControlOptionVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.z, ControlOptionVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
            ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.x, ControlOptionVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.y, ControlOptionVals.rotation.y + 360.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.z, ControlOptionVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );







    static public readonly TweenManager.TweenPathBundle transition_GameOptionsToAudio = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.x, AudioOptionsVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.y, GameOptionsVals.position.y + 1.0f, TRANSITION_TIME/2.0f, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(AudioOptionsVals.position.y, TRANSITION_TIME/2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.position.z, AudioOptionsVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.x, AudioOptionsVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.y, AudioOptionsVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(GameOptionsVals.rotation.z, AudioOptionsVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );

        static public readonly TweenManager.TweenPathBundle transition_AudioToGameOptions = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.x, GameOptionsVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.y, AudioOptionsVals.position.y + 1.0f, TRANSITION_TIME / 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT),
            new TweenManager.TweenPart_Continue(GameOptionsVals.position.y, TRANSITION_TIME / 2.0f, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.z, GameOptionsVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.x, GameOptionsVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.y, GameOptionsVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.z, GameOptionsVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );



    static public readonly TweenManager.TweenPathBundle transition_AudioToMain = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.position.x, MainMenuVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.position.y, AudioOptionsVals.position.y + 1.0f, TRANSITION_TIME/4, TweenManager.CURVE_PRESET.EASE_INOUT),
        new TweenManager.TweenPart_Continue(MainMenuVals.position.y, TRANSITION_TIME*3/4, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.position.z, MainMenuVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.x, MainMenuVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.y, MainMenuVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.z, MainMenuVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    )
    );



    static public readonly TweenManager.TweenPathBundle transition_AudioToControls = new TweenManager.TweenPathBundle(
        // Camera X
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.x, ControlOptionVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.y, ControlOptionVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.position.z, ControlOptionVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera X Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.x, ControlOptionVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Y Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.y, ControlOptionVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        ),
        // Camera Z Rot
        new TweenManager.TweenPath(
            new TweenManager.TweenPart_Start(AudioOptionsVals.rotation.z, ControlOptionVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
        )
        );


    static public readonly TweenManager.TweenPathBundle transition_ControlsToAudio = new TweenManager.TweenPathBundle(
    // Camera X
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.position.x, AudioOptionsVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Y
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.position.y, AudioOptionsVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Z
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.position.z, AudioOptionsVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera X Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.rotation.x, AudioOptionsVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Y Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.rotation.y, AudioOptionsVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    ),
    // Camera Z Rot
    new TweenManager.TweenPath(
        new TweenManager.TweenPart_Start(ControlOptionVals.rotation.z, AudioOptionsVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
    )
    );

    static public readonly TweenManager.TweenPathBundle transition_ControlsToGameOptions = new TweenManager.TweenPathBundle(
// Camera X
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.position.x, GameOptionsVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Y
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.position.y, GameOptionsVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Z
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.position.z, GameOptionsVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera X Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.rotation.x, GameOptionsVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Y Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.rotation.y, GameOptionsVals.rotation.y - 360.0f, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Z Rot
new TweenManager.TweenPath(
    new TweenManager.TweenPart_Start(ControlOptionVals.rotation.z, GameOptionsVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
)
);

    static public readonly TweenManager.TweenPathBundle transition_ControlsToMain = new TweenManager.TweenPathBundle(
// Camera X
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.position.x, MainMenuVals.position.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Y
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.position.y, MainMenuVals.position.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Z
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.position.z, MainMenuVals.position.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera X Rot
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.rotation.x, MainMenuVals.rotation.x, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Y Rot
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.rotation.y, MainMenuVals.rotation.y, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
),
// Camera Z Rot
new TweenManager.TweenPath(
new TweenManager.TweenPart_Start(ControlOptionVals.rotation.z, MainMenuVals.rotation.z, TRANSITION_TIME, TweenManager.CURVE_PRESET.EASE_INOUT)
)
);
}
