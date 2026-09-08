using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private float teleportDistance = 3f;
    private Rigidbody2D Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponentInParent<Rigidbody2D>();

        if (Rigidbody == null)
        {
            Debug.LogError("Jump: Could not find a Rigidbody2D on this object or its parent!");
        }
    }

    public void TeleportForward()
    {
        Vector2 direction = transform.right; 
        Vector2 targetPosition = Rigidbody.position + (direction * teleportDistance);

        Rigidbody.linearVelocity = Vector2.zero;

        Rigidbody.position = targetPosition;
    }
}