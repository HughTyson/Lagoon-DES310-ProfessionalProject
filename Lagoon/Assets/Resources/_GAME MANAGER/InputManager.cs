using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XInputDotNetPure; // Required in C#

public class InputManager // input manager for a single player controller game
{

    bool playerConnected = false;
    PlayerIndex playerIndex;
    GamePadState state;

    bool[] prevButtons = new bool[15];
    bool[] currentButtons = new bool[15];

    float[] currentAxis = new float[6];

    public enum BUTTON
    {
        A,
        B,
        X,
        Y,
        DPAD_UP,
        DPAD_DOWN,
        DPAD_RIGHT,
        DPAD_LEFT,
        RB,
        LB,
        R_STICK,
        L_STICK,
        BACK,
        START,
        GUIDE
    }
    public enum AXIS
    {
        LT,
        RT,
        LH,
        LV,
        RH,
        RV
    }

    public enum VIBRATION_MOTOR
    { 
    LEFT,
    RIGHT
    };

    public enum VIBRATION_PRESET
    { 
    MENU_BUTTON_PRESSED  ,
    MENU_CHANGE_SELECTION ,
    };


    class VibrationMotor
    {
        public enum VIBRATION_STATE
        {
            MANUAL,
            RANDOM_PULSE,
            SINGLE_PULSE
        };
        public VIBRATION_STATE vibration_state;

        public float currentAmplitude;

        public float timer;
        public float minTimer;
        public float maxTimer;

        public float minAmplitude;
        public float maxAmplitude;

        public float pulseDuration;
        
        public void ResetToDefault()
        {
            timer = 0;
            currentAmplitude = 0;
            vibration_state = VIBRATION_STATE.MANUAL;
            minAmplitude = 0;
            maxAmplitude = 1;
        }
    }
    VibrationMotor leftMotor = new VibrationMotor();
    VibrationMotor rightMotor = new VibrationMotor();




    public InputManager()
    {
    }

    public void FixedUpdate()
    {
        UpdateMotor(leftMotor);
        UpdateMotor(rightMotor);

        GamePad.SetVibration(playerIndex, leftMotor.currentAmplitude, rightMotor.currentAmplitude); // set in fixed update to keep consistent vibration
    }
    public void Update()
    {
        ConnectController();
        UpdateControllerValues();

    }


    void ConnectController()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerConnected = true;
                }
            }
        }
        state = GamePad.GetState(playerIndex);

        playerConnected = state.IsConnected;
   
    }
    void UpdateControllerValues()
    {
        for (int i = 0; i < currentButtons.Length; i++)
        {
            prevButtons[i] = currentButtons[i];
        }

        currentAxis[(int)AXIS.LT] = state.Triggers.Left;
        currentAxis[(int)AXIS.RT] = state.Triggers.Right;
        currentAxis[(int)AXIS.LH] = state.ThumbSticks.Left.X;
        currentAxis[(int)AXIS.LV] = state.ThumbSticks.Left.Y;
        currentAxis[(int)AXIS.RH] = state.ThumbSticks.Right.X;
        currentAxis[(int)AXIS.RV] = state.ThumbSticks.Right.Y;

        currentButtons[(int)BUTTON.X] = (state.Buttons.X == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.Y] = (state.Buttons.Y == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.A] = (state.Buttons.A == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.B] = (state.Buttons.B == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.BACK] = (state.Buttons.Back == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.DPAD_DOWN] = (state.DPad.Down == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.DPAD_LEFT] = (state.DPad.Left == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.DPAD_RIGHT] = (state.DPad.Right == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.DPAD_UP] = (state.DPad.Up == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.GUIDE] = (state.Buttons.Guide == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.LB] = (state.Buttons.LeftShoulder == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.L_STICK] = (state.Buttons.LeftStick == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.RB] = (state.Buttons.RightShoulder == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.R_STICK] = (state.Buttons.RightStick == ButtonState.Pressed) ? true : false;
        currentButtons[(int)BUTTON.START] = (state.Buttons.Start == ButtonState.Pressed) ? true : false;
    }
    public bool IsControllerConnected()
    {
        return playerConnected;
    }

    public bool GetButtonDown(BUTTON button)
    {
        return (!prevButtons[(int)button] && currentButtons[(int)button]);
    }
    public bool GetButtonUp(BUTTON button)
    {
        return (prevButtons[(int)button] && !currentButtons[(int)button]);
    }

    public bool GetButton(BUTTON button)
    {
        return currentButtons[(int)button];
    }

    public float GetAxis(AXIS axis)
    {
        return currentAxis[(int)axis];
    }


    public void SetVibrationWithPreset(VIBRATION_PRESET preset)
    {
        switch(preset)
        {
            case VIBRATION_PRESET.MENU_BUTTON_PRESSED:
                {
                    SetVibrationPulse(VIBRATION_MOTOR.LEFT, 0.1f, 0.5f);
                    SetVibrationPulse(VIBRATION_MOTOR.RIGHT, 0.1f, 0.5f);

                    break;
                }
            case VIBRATION_PRESET.MENU_CHANGE_SELECTION:
                {
                    SetVibrationPulse(VIBRATION_MOTOR.LEFT, 0.05f, 0.5f);
                    SetVibrationPulse(VIBRATION_MOTOR.RIGHT, 0.05f, 0.5f);

                    break;
                }
        }
    }


    public void SetVibration(VIBRATION_MOTOR motor, float vibration_amm)
    {
        switch(motor)
        {
            case VIBRATION_MOTOR.LEFT:
                {
                    leftMotor.ResetToDefault();
                    leftMotor.currentAmplitude = Mathf.Clamp01(vibration_amm);
                    break;
                }
            case VIBRATION_MOTOR.RIGHT:
                {
                    rightMotor.ResetToDefault();
                    rightMotor.currentAmplitude = Mathf.Clamp01(vibration_amm);
                    break;
                }
        }

    }
    public void SetVibrationBoth(float leftValue, float rightValue)
    {
        leftMotor.ResetToDefault();
        leftMotor.currentAmplitude = Mathf.Clamp01(leftValue);
        rightMotor.ResetToDefault();
        rightMotor.currentAmplitude = Mathf.Clamp01(rightValue);
    }

    public void SetVibrationPulse(VIBRATION_MOTOR motor, float pulseDuration_, float amplitude)
    {
        switch (motor)
        {
            case VIBRATION_MOTOR.LEFT:
                {
                    leftMotor.ResetToDefault();
                    leftMotor.vibration_state = VibrationMotor.VIBRATION_STATE.SINGLE_PULSE;
                    leftMotor.pulseDuration = pulseDuration_;
                    leftMotor.currentAmplitude = amplitude;
                    break;
                }
            case VIBRATION_MOTOR.RIGHT:
                {
                    rightMotor.ResetToDefault();
                    rightMotor.vibration_state = VibrationMotor.VIBRATION_STATE.SINGLE_PULSE;
                    rightMotor.pulseDuration = pulseDuration_;
                    rightMotor.currentAmplitude = amplitude;
                    break;
                }
        }

    }
    public void SetVibrationRandomPulsing(VIBRATION_MOTOR motor, float pulseDuration_, float minTimer_, float maxTimer_, float amplitude_)
    {
        switch (motor)
        {
            case VIBRATION_MOTOR.LEFT:
                {
                    leftMotor.ResetToDefault();
                    leftMotor.vibration_state = VibrationMotor.VIBRATION_STATE.RANDOM_PULSE;
                    leftMotor.pulseDuration = pulseDuration_;
                    leftMotor.minAmplitude = amplitude_;
                    leftMotor.maxAmplitude = amplitude_;
                    leftMotor.minTimer = minTimer_;
                    leftMotor.maxTimer = maxTimer_;
                    leftMotor.timer = 0;
                    break;
                }
            case VIBRATION_MOTOR.RIGHT:
                {
                    rightMotor.ResetToDefault();
                    rightMotor.vibration_state = VibrationMotor.VIBRATION_STATE.RANDOM_PULSE;
                    rightMotor.pulseDuration = pulseDuration_;
                    rightMotor.minAmplitude = amplitude_;
                    rightMotor.maxAmplitude = amplitude_;
                    rightMotor.minTimer = minTimer_;
                    rightMotor.maxTimer = maxTimer_;
                    rightMotor.timer = 0;
                    break;
                }
        }
    }


    void UpdateMotor(VibrationMotor motor)
    {
        switch(motor.vibration_state)
        {

            case VibrationMotor.VIBRATION_STATE.MANUAL:
                {

                    break;
                }
            case VibrationMotor.VIBRATION_STATE.RANDOM_PULSE:
                {

                    motor.timer -= Time.fixedDeltaTime;
                    
                    if (motor.timer <= 0)
                    {
                        motor.currentAmplitude = motor.minAmplitude;
                        if (motor.timer <= -motor.pulseDuration)
                        {
                            motor.timer = Random.Range(motor.minTimer, motor.maxTimer);
                        }
                    }
                    else
                    {
                        motor.currentAmplitude = 0;
                    }
                    break;
                }
            case VibrationMotor.VIBRATION_STATE.SINGLE_PULSE:
                {
                    motor.pulseDuration -= Time.fixedDeltaTime;
                    if (motor.pulseDuration < 0)
                    {
                        motor.currentAmplitude = 0;
                    }

                    break;
                }
        }
    }

}
