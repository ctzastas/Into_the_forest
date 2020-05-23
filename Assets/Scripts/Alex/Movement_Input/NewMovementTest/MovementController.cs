using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    BoxCollider2D _Collider;
    const float skinWidth = .015f;
    RaycastOrigins raycastOrigins;
    public int horizontalRaycount = 4;
    public int verticalRaycount = 4;
    public LayerMask collisionMask;
    public CollisionInfo collisionsInfo;


    float horizontalRaySpacing;
    float verticalRaySpacing;

    public void Start()
    {
        _Collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;


        for (int i = 0; i < horizontalRaycount; i++)
        {
            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.bottomRight;
            }
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisionsInfo.left = directionX == -1;
                collisionsInfo.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;


        for (int i = 0; i < verticalRaycount; i++)
        {
            Vector2 rayOrigin;
            if (directionY == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.topLeft;
            }
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisionsInfo.below = directionY == -1;
                collisionsInfo.above = directionY == 1;
            }
        }
    }
    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisionsInfo.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
    }
    void UpdateRaycastOrigins()
    {
        Bounds bounds = _Collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    private void CalculateRaySpacing()
    {
        Bounds bounds = _Collider.bounds;
        bounds.Expand(skinWidth * -2);
        horizontalRaycount = Mathf.Clamp(horizontalRaycount, 2, int.MaxValue);
        verticalRaycount = Mathf.Clamp(verticalRaycount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRaycount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRaycount - 1);
    }
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
    public struct CollisionInfo
    {
        public bool above, below;
        public bool right, left;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}
