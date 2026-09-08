using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private float teleportDistance = 3f;
    private Rigidbody2D Rigidbody;

    private SpriteRenderer sr;

    void Awake()
    {
        Rigidbody = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();

        if (Rigidbody == null)
        {
            Debug.LogError("Jump: Could not find a Rigidbody2D on this object or its parent!");
        }
    }

    public bool isFacingRight()
    {
        return !sr.flipX;
    }

    public void TeleportForward()
    {

        Debug.Log(isFacingRight());
        Vector2 direction = isFacingRight() ? Vector2.right : Vector2.left;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, teleportDistance);


        Vector2 targetPosition = Rigidbody.position + (direction * teleportDistance);

        Rigidbody.linearVelocity = Vector2.zero;

        Rigidbody.position = targetPosition;

    }
}