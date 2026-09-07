using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGeneral : MonoBehaviour
{
    [SerializeField] private Jump jump;

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    [SerializeField] private InputHandler inputhandler;

    private bool bDetectKey;
    private Key kCode;

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    } 
    void Start()
    {
        
        Debug.Log($"Starting");
       StartDetectingKey();
    }

    public void StartDetectingKey()
    {
        bDetectKey = true;
        inputhandler.DetectNextKey(OnKeyDetected);
    }

    private void OnKeyDetected(Key key)
    {
        kCode = key;
        bDetectKey = false;

        Debug.Log($"Detected key: {key}");

        // Register whatever action you want here.
        inputhandler.RegisterKey(jump.JumpUp, key);
    }

    void Update()
    {
        rb.AddForce(Vector2.up * 500, ForceMode2D.Impulse);
    }
}
