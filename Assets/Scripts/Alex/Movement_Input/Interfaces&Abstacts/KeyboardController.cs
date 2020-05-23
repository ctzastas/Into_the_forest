using UnityEngine;

/*
 * Basic Keyboard Input for the game for quick Testing.
 */

[CreateAssetMenu(fileName ="KeyboardInput",menuName = "Input/Keyboard",order =0)]
public class KeyboardController : IInputController
{
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode interactKey;

    public override bool InteractKeyPressed()
    {
        return Input.GetKey(interactKey);
    }

    public override bool JumpKeyPressed()
    {
        return Input.GetKeyDown(jumpKey);
    }

    public override bool JumpKeyReleased()
    {
        return Input.GetKeyUp(jumpKey);
    }

    public override bool LeftKeyPressed()
    {
        return Input.GetKey(leftKey);
    }

    public override bool RightKeyPressed()
    {
        return Input.GetKey(rightKey);
    }
}