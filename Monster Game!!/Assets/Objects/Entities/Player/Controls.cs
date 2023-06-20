using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls
{
    bool usingController { get => Gamepad.current != null; }
    Gamepad controller { get => Gamepad.current; }

    public Results GetInput()
    {
        //  If the player is using a controller.
        if (usingController)
        {
            return new Results
                (
                controller.leftStick.ReadValue(),
                controller.rightStick.ReadValue(),
                controller.buttonSouth.wasPressedThisFrame,
                controller.leftShoulder.wasPressedThisFrame || controller.rightShoulder.wasPressedThisFrame,
                controller.buttonWest.wasPressedThisFrame
                );
        }

        //  If the player is using a keyboard.

        var leftInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) leftInput.y++;
        if (Input.GetKey(KeyCode.S)) leftInput.y--;
        if (Input.GetKey(KeyCode.A)) leftInput.x--;
        if (Input.GetKey(KeyCode.D)) leftInput.x++;

        var rightInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) rightInput.y++;
        if (Input.GetKey(KeyCode.DownArrow)) rightInput.y--;
        if (Input.GetKey(KeyCode.LeftArrow)) rightInput.x--;
        if (Input.GetKey(KeyCode.RightArrow)) rightInput.x++;

        return new Results
            (
            leftInput.normalized,
            rightInput.normalized,
            Input.GetKeyDown(KeyCode.Space),
            Input.GetKeyDown(KeyCode.LeftShift),
            Input.GetKeyDown(KeyCode.F)
            );
    }

    public struct Results
    {
        public readonly Vector2 leftInput;
        public readonly Vector2 rightInput;

        public readonly bool jumpButtonPressed;
        public readonly bool dashButtonPressed;
        public readonly bool grabButtonPressed;

        public Results(Vector2 left, Vector2 right, bool jump, bool dash, bool grab)
        {
            leftInput = left;
            rightInput = right;
            jumpButtonPressed = jump;
            dashButtonPressed = dash;
            grabButtonPressed = grab;
        }
    }
}
