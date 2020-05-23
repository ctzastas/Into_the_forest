using UnityEngine;

/*
 * Inherits from the scriptable object class so that we can create
 * objects of that type. Allows Us to have different Input methods
 */

public abstract class IInputController : ScriptableObject
{
    public abstract bool LeftKeyPressed();
    public abstract bool RightKeyPressed();
    public abstract bool JumpKeyPressed();
    public abstract bool JumpKeyReleased();
    public abstract bool InteractKeyPressed();
}
