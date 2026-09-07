using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsTest : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Debug.Log("RB: " + rb);
        Debug.Log("Body Type: " + rb.bodyType);
        Debug.Log("Simulated: " + rb.simulated);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("ADDING IMPULSE");

            rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        }
    }
}