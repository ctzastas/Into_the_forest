using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullInteractor : IInteractor
{
    [SerializeField] private LayerMask m_Interactable;
    [SerializeField] private float raycastDistance;
    private Collider2D raycastHit;
    [SerializeField] private BoxCollider2D interactionCollider;
    bool foundObject;
    Player player;

    private void Awake()
    {
        interactionCollider = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();
    }
    public override void InteractCondition()
    {
        allowMovement = true;
        //raycastHit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x,0), raycastDistance, m_Interactable);
        //Debug.DrawRay(transform.position, new Vector2(transform.localScale.x, 0) * raycastDistance, Color.green);
        //raycastHit = Physics2D.BoxCast(transform.position + new Vector3(transform.localScale.x*1.5f,-.2f,0), transform.localScale * raycastDistance,0,new Vector2(transform.position.x * transform.localScale.x,transform.position.y),raycastDistance,m_Interactable);
        //Debug.Log(canInteract);
        //Debug.Log(raycastHit.point);

        //if (raycastHit._Collider != null)
        //{
        //    if (raycastHit._Collider.gameObject != gameObject)
        //    {
        //        if (raycastHit.distance <= raycastDistance)
        //        {
        if (foundObject && player.IsGrounded())
        {
            Debug.Log("Hittin Collider");
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }
    }
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Hit : " + raycastHit.name);
        GameObject obj = raycastHit.gameObject;
        float xDistance = GetComponent<BoxCollider2D>().size.x / 2 + obj.transform.lossyScale.x / 2;
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, (transform.position + transform.right * xDistance * transform.localScale.x), 0.9f);
        // = transform.position + transform.right * xDistance/2 * transform.localScale.x *1.3f;
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        obj.transform.eulerAngles = new Vector3(0,0,0);
        obj.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable")) 
        {
            Debug.Log(collision.name);
            raycastHit = collision;
            foundObject = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            raycastHit = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Debug.Log(collision.name);
            foundObject = false;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position + new Vector3(transform.localScale.x*1.5f,-.2f, 0), transform.localScale * raycastDistance);
    //}
}
