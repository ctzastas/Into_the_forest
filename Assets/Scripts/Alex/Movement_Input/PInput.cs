using UnityEngine;
/// <summary>
/// Old Implementation of PlayerInput.
/// </summary>
public class PInput : MonoBehaviour
{
    private Rigidbody2D _rb;

    public IInputController controller;

    private InteractionHandler interactionHandler;
    private PlayerState curentState;

    public void SetPlayerState(PlayerState state) 
    {
        curentState = state;
    }
    public enum PlayerState 
    {
        Normal,
        Interacting,
    }
    private CharacterController2D _CharacterController;

    public float moveSpeed = 40f;
    private bool jump = false;
    float horizontalMovement=0f;

    public void Start()
    {
        interactionHandler = GetComponent<InteractionHandler>();
        curentState = PlayerState.Normal;
        _rb = GetComponent<Rigidbody2D>();
        _CharacterController = GetComponent<CharacterController2D>();
    }
    public void Update()
    {
        if (controller.JumpKeyPressed()) 
        {
            Jump();
        }
        if (controller.LeftKeyPressed())
        {
            Left();
        }
        else if (controller.RightKeyPressed())
        {
            Right();
        }
        else 
        {
            horizontalMovement = 0;
        }
        if (controller.InteractKeyPressed())
        {
            Interact();
        }
        else
        {
            curentState = PlayerState.Normal;
        }
    }
    public void FixedUpdate()
    {
        if (curentState == PlayerState.Normal)
        {
            Debug.Log(horizontalMovement);
            _CharacterController.Move(horizontalMovement * moveSpeed * Time.fixedDeltaTime, false, jump, false);
            jump = false;
        }
        else if (curentState == PlayerState.Interacting) 
        {
           // Debug.Log("Running Second");
            _CharacterController.Move(horizontalMovement * moveSpeed * Time.fixedDeltaTime, false, false, true);
            jump = false;
        }
        _CharacterController.NormalizeSlope();
        
    }
    public void Jump() 
    {
        //_rb.AddForce(Vector2.up*5, ForceMode2D.Impulse);
        jump = true;
       // Debug.Log("Jumping");
    }
    public void Left() 
    {
        horizontalMovement = -1;
        //_rb.AddForce(Vector2.right * (-3), ForceMode2D.Force);
       // Debug.Log("Left " + horizontalMovement);
    }
    public void Right() 
    {
        horizontalMovement = 1;
        //_rb.AddForce(Vector2.right * 3, ForceMode2D.Force);
       // Debug.Log("Right " + horizontalMovement);
    }
    public void Interact()
    {
        interactionHandler.Interact();
      //  Debug.Log("Called");
    }
}
