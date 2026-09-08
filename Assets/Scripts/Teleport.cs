using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private float teleportDistance = 3f;
    [SerializeField] private float groundDistanceCheck = 1.1f; 
    private Rigidbody2D Rigidbody;


    private int dashcount = 1;
    private SpriteRenderer sr;
    private Collider2D playerCollider;
    void Awake()
    {
        Rigidbody = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
        playerCollider = GetComponentInParent<Collider2D>();
        if (Rigidbody == null)
        {
            Debug.LogError("Jump: Could not find a Rigidbody2D on this object or its parent!");
        }
    }

    void Update()
    {
        if (IsGrounded())
        {
            dashcount = 1;
        }
    }

    private bool IsGrounded()
    {
        Vector2 rayStart = Rigidbody.transform.position;

        bool originalColliderState = playerCollider.enabled;
        playerCollider.enabled = false;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundDistanceCheck);

        playerCollider.enabled = originalColliderState;

        return hit.collider != null;
    }


    public bool isFacingRight()
    {
        return !sr.flipX;
    }

    public void TeleportForward()
    {
        if (dashcount > 0)
        {
            Vector2 direction = isFacingRight() ? Vector2.right : Vector2.left;
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, teleportDistance);


            Vector2 targetPosition = Rigidbody.position + (direction * teleportDistance);

            Rigidbody.linearVelocity = Vector2.zero;

            Rigidbody.position = targetPosition;
            dashcount--;
        }
        

    }
}