using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private float teleportDistance = 3f;
    private Rigidbody2D Rigidbody;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void TeleportForward()
    {
        Vector2 direction = transform.right; 
        Vector2 targetPosition = Rigidbody.position + (direction * teleportDistance);

        Rigidbody.linearVelocity = Vector2.zero;

        Rigidbody.position = targetPosition;
    }
}