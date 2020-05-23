///
///Summary
///Old Implementation of Movement!
///

using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_AirSpeed = .36f;             // Amount of maxSpeed applied to Air Movement 1 = 100%
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_LeftSideCheck;                         // A Position Marking the left side of the player to check if grounded
    [SerializeField] private Transform m_RightSideCheck;                        // A Position Marking the right side of the player to check if grounded
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A _Collider that will be disabled when crouching
    [Range(0,1)] [SerializeField] private float m_slopeFriction;                // A generic value of the friction in the game.

    private Animator m_animator;          //Animator component of our Object
    //const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    public bool IsGrounded() 
    {
        return m_Grounded;
    }
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        //Player is grounded if the Raycast Hits a _Collider within the mask GROUND
        RaycastHit2D colliderHitRight = Physics2D.Raycast(m_RightSideCheck.position, Vector2.down,100, m_WhatIsGround);
        RaycastHit2D colliderHitLeft = Physics2D.Raycast(m_LeftSideCheck.position, Vector2.down, 100, m_WhatIsGround);
        if (colliderHitRight.collider != null || colliderHitLeft.collider !=null)
        {
            if (colliderHitRight.collider.gameObject != gameObject || colliderHitLeft.collider.gameObject!=gameObject)
            {
                Debug.Log("Hit Ground");
                if (colliderHitRight.distance <= 0.1f || colliderHitLeft.distance < 0.1f)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                        OnLandEvent.Invoke();
                    if (colliderHitRight.distance <= 1f && colliderHitLeft.distance <= 1f)
                    {
                        Debug.Log("Both Rays Hitting Ground");
                        m_animator.SetInteger("LeanValue", 0);
                    }
                    else if (colliderHitRight.distance > 1f && colliderHitLeft.distance <= 1f)
                    {
                        //ToDo Play Lean Animation
                        Debug.Log("Leaning to Right side");
                        m_animator.SetInteger("LeanValue", 1);
                    }
                    else 
                    {
                        Debug.Log("Leaning to Left side");
                        m_animator.SetInteger("LeanValue", 0);
                    }
                }
            }
        }
        Debug.DrawRay(m_LeftSideCheck.position, Vector2.down,Color.red);
        Debug.DrawRay(m_RightSideCheck.position, Vector2.down, Color.red);
        m_animator.SetBool("IsGrounded", m_Grounded);
    }


    public void NormalizeSlope()
    {
       // Attempt vertical normalization
        if (m_Grounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, m_WhatIsGround);
            Debug.DrawLine(transform.position, transform.position - Vector3.up * 100);
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
            Debug.Log(hit.normal.x);
            if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0f)
            {

                // Apply the opposite force against the slope force 
                // You will need to provide your own slopeFriction to stabalize movement


                // ORIGINAL SCRIPT!!!!! 
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - (hit.normal.x * m_slopeFriction), m_Rigidbody2D.velocity.y);

                ////Move Player up or down to compensate for the slope below them
                //Vector3 pos = transform.position;
                //pos.y += -hit.normal.x * Mathf.Abs(m_Rigidbody2D.velocity.x) * Time.deltaTime * (m_Rigidbody2D.velocity.x - hit.normal.x > 0 ? 1 : -1);
                //transform.position = pos;
                Debug.Log("Function Works");
            }
        }
    }

    public void Move(float move, bool crouch, bool jump,bool pullingObject)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }
        if (pullingObject) 
        {
            move *= m_CrouchSpeed;
        }
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the _Collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            if (m_Grounded)
            {
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * 10, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                m_animator.SetBool("IsFalling", false);
            }
            else
            {
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * 10, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) 
                {
                    m_animator.SetBool("IsFalling", true);
                }
            }
           
           
            if (move != 0)
            {
                m_animator.SetBool("IsMoving", true);
            }
            else 
            {
                m_animator.SetBool("IsMoving", false);
            }
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight && !pullingObject)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight && !pullingObject)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_animator.SetTrigger("Jump");
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}