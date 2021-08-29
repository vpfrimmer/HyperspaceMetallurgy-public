using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonPersistent<InputManager>
{
    public delegate void KeyInputEvent(KeyCode key, int playerId);
    public static event KeyInputEvent OnKeyDown;

    public static void Dispose()
    {
        OnKeyDown = null;
    }

    const float JOYSTICK_DELAY = 0.2f;

    float joystick1Delay = 0f;
    float joystick2Delay = 0f;

    private void Update()
    {
        if (joystick1Delay > 0)
        {
            joystick1Delay -= Time.deltaTime;
        }

        if (joystick2Delay > 0)
        {
            joystick2Delay -= Time.deltaTime;
        }

        ABXY_Handling();

        StartBack_Handling();

        // Left Stick Player 1
        StickHandling(ref joystick1Delay, 1, "x");

        // Left Stick Player 2
        StickHandling(ref joystick2Delay, 2, "x");

        DpadHandling(ref joystick1Delay, ref joystick2Delay);
    }

    private void DpadHandling(ref float joystick1Delay, ref float joystick2Delay)
    {
#if UNITY_STANDALONE_WIN
        // D-Pad Player 1
        StickHandling(ref joystick1Delay, 1, "6");

        // D-Pad Player 2
        StickHandling(ref joystick2Delay, 2, "6");
#endif

#if UNITY_STANDALONE_LINUX
        // D-Pad Player 1
        StickHandling(ref joystick1Delay, 1, "7");

        // D-Pad Player 2
        StickHandling(ref joystick2Delay, 2, "7");

        // Left D-Pad (wireless)
        if (Input.GetKeyDown("joystick 1 button 11"))
        {
            OnJoystickButtonPressed(KeyCode.LeftArrow, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 11"))
        {
            OnJoystickButtonPressed(KeyCode.LeftArrow, 1);
        }

        // Right D-Pad (wireless)
        if (Input.GetKeyDown("joystick 1 button 12"))
        {
            OnJoystickButtonPressed(KeyCode.RightArrow, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 12"))
        {
            OnJoystickButtonPressed(KeyCode.RightArrow, 1);
        }
#endif

#if UNITY_STANDALONE_OSX
        // Left D-Pad
        if (Input.GetKeyDown("joystick 1 button 7"))
        {
            OnJoystickButtonPressed(KeyCode.LeftArrow, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 7"))
        {
            OnJoystickButtonPressed(KeyCode.LeftArrow, 1);
        }

        // Right D-Pad
        if (Input.GetKeyDown("joystick 1 button 8"))
        {
            OnJoystickButtonPressed(KeyCode.RightArrow, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 8"))
        {
            OnJoystickButtonPressed(KeyCode.RightArrow, 1);
        }
#endif
    }

    private void StartBack_Handling()
    {
#if !UNITY_STANDALONE_OSX 
        // START BUTTON
        if (Input.GetKeyDown("joystick 1 button 7") || Input.GetKeyDown(KeyCode.S))
        {
            OnJoystickButtonPressed(KeyCode.Return, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 7") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnJoystickButtonPressed(KeyCode.Return, 1);
        }

        // BACK BUTTON
        if (Input.GetKeyDown("joystick 1 button 6") || Input.GetKeyDown(KeyCode.Escape))
        {
            OnJoystickButtonPressed(KeyCode.Escape, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 6"))
        {
            OnJoystickButtonPressed(KeyCode.Escape, 1);
        }
#endif

#if UNITY_STANDALONE_OSX
        // START BUTTON
        if (Input.GetKeyDown("joystick 1 button 9") || Input.GetKeyDown(KeyCode.S))
        {
            OnJoystickButtonPressed(KeyCode.Return, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 9") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnJoystickButtonPressed(KeyCode.Return, 1);
        }

        // BACK BUTTON
        if (Input.GetKeyDown("joystick 1 button 10") || Input.GetKeyDown(KeyCode.Escape))
        {
            OnJoystickButtonPressed(KeyCode.Escape, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 10"))
        {
            OnJoystickButtonPressed(KeyCode.Escape, 1);
        }
#endif
    }

    private void ABXY_Handling()
    {
        // A BUTTON
        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown(KeyCode.S))
        {
            OnJoystickButtonPressed(KeyCode.A, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnJoystickButtonPressed(KeyCode.A, 1);
        }

        // B BUTTON
        if (Input.GetKeyDown("joystick 1 button 1") || Input.GetKeyDown(KeyCode.D))
        {
            OnJoystickButtonPressed(KeyCode.B, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 1") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnJoystickButtonPressed(KeyCode.B, 1);
        }

        // X BUTTON
        if (Input.GetKeyDown("joystick 1 button 2") || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A))
        {
            OnJoystickButtonPressed(KeyCode.X, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 2") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnJoystickButtonPressed(KeyCode.X, 1);
        }

        // Y BUTTON
        if (Input.GetKeyDown("joystick 1 button 3") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))
        {
            OnJoystickButtonPressed(KeyCode.Y, 0);
        }

        if (Input.GetKeyDown("joystick 2 button 3") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnJoystickButtonPressed(KeyCode.Y, 1);
        }
    }

    private void StickHandling(ref float stickDelay, int player, string axis)
    {
        if (stickDelay <= 0f)
        {
            // Left on left stick
            if (Input.GetAxisRaw("joystick " + player +" axis " + axis) < -0.1f)
            {
                OnJoystickButtonPressed(KeyCode.LeftArrow, player-1);
                stickDelay = JOYSTICK_DELAY;
            }
            // Right on left stick
            else if (Input.GetAxisRaw("joystick " + player + " axis " + axis) > 0.1f)
            {
                OnJoystickButtonPressed(KeyCode.RightArrow, player-1);
                stickDelay = JOYSTICK_DELAY;
            }
        }
    }

    private void OnJoystickButtonPressed(KeyCode key, int playerId)
    {
        OnKeyDown?.Invoke(key, playerId);
    }
}
