using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XInputDotNetPure; // Required in C#

public class InputManager // input manager for a single player controller game
{

    bool playerConnected = false;
    PlayerIndex playerIndex;
    GamePadState state;

    float leftVibration = 0;
    float rightVibration = 0;

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

    public InputManager()
    {
    }

    public void FixedUpdate()
    {
        GamePad.SetVibration(playerIndex, leftVibration, rightVibration); // set in fixed update to keep consistent vibration
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

    public void SetVibrationLeft(float leftValue)
    {
        leftVibration = Mathf.Clamp01(leftValue);
    }
    public void SetVibrationRight(float rightValue)
    {
        rightVibration = Mathf.Clamp01(rightValue);
    }

    public void SetVibration(float leftValue, float rightValue)
    {
        leftVibration = Mathf.Clamp01(leftValue);
        rightVibration = Mathf.Clamp01(rightValue);
    }
}
