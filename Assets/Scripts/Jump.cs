using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Jump: Could not find a Rigidbody2D on this object or its parent!");
        }
        else
        {
            Debug.Log($"Jump: Found Rigidbody2D on {rb.gameObject.name}");
        }
    }

    public void JumpUp()
    {
        Debug.Log("JumpUp() was called!");

        if (rb == null)
        {
            Debug.LogError("JumpUp: Rigidbody2D is NULL!");
            return;
        }

        Debug.Log($"Velocity BEFORE: {rb.linearVelocity}");

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Debug.Log($"Velocity AFTER: {rb.linearVelocity}");
    }
}