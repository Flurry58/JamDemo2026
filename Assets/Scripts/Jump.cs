using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistanceCheck = 1.1f; 

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerCollider = GetComponentInParent<Collider2D>();

        if (rb == null)
        {
            Debug.LogError("Jump: Could not find a Rigidbody2D on this object or its parent!");
        }
    }

    public void JumpUp()
    {
        if (rb == null) return;

        if (IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumped successfully!");
        }
        else
        {
            Debug.Log("Jump blocked: Player is in mid-air!");
        }
    }

    private bool IsGrounded()
    {
        Vector2 rayStart = rb.transform.position;

        bool originalColliderState = playerCollider.enabled;
        playerCollider.enabled = false;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundDistanceCheck);

        playerCollider.enabled = originalColliderState;

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rb.transform.position, (Vector2)rb.transform.position + Vector2.down * groundDistanceCheck);
        }
    }
}
