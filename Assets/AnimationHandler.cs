using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;
    PlayerInput input;
    public Transform LedgeRaycastTest;
    RaycastHit2D hit;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }
    void Update()
    {
        hit = Physics2D.Raycast(LedgeRaycastTest.transform.position, Vector2.up * -1,Mathf.Infinity,mask);
        Debug.DrawRay(LedgeRaycastTest.transform.position, Vector2.up * -1, Color.green);
        SetLeanValue(hit.distance > 5 ? 1 : 0);
    }
    public void UpdateMovement(bool moving) 
    {
       animator.SetBool("IsMoving", moving);
    }
    public void TriggerJump() 
    {
        animator.SetTrigger("Jump");
    }
    public void SetFalling(bool falling) 
    {
        animator.SetBool("IsFalling", falling);
    }
    public void SetGrounded(bool grounded) 
    {
        animator.SetBool("IsGrounded", grounded);
    }
    public void SetLeanValue(int lean) 
    {
        animator.SetInteger("LeanValue", lean);
    }
    public void ResetJump() 
    {
        animator.ResetTrigger("Jump");
    }
}
