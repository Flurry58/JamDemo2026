using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGeneral : MonoBehaviour
{
    [SerializeField] private Jump jump;
    [SerializeField] private Teleportation teleport;
    [SerializeField] private GameObject corpse;
    [SerializeField] private float stepDelay = 0.175f;

    [SerializeField] private float moveSpeed = 5f;

     private float stepTimer = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    [SerializeField] private InputHandler inputhandler;

    private bool bDetectKey;
    private Key kCode;
    private AudioSource walkSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 
    void Start()
    {
        
       StartDetectingKey_Jump();
    }

    private void FixedUpdate()
    {

        float horizontal = 0f;
        if (Keyboard.current.aKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.dKey.isPressed)
            horizontal = 1f;

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );

        if (stepTimer > 0) //walking sound timer
        {
            stepTimer -= Time.fixedDeltaTime;
        }

        if (movement.magnitude > 0 && stepTimer <= 0)
        {
            walkSource.Play();
            stepTimer = stepDelay;
        }
    }
    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
        if (movement.x > 0) //flips sprite because we dont have left facing sprites
        {
            spriteRenderer.flipX = false; 
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;  
        }
    }

    public void OnTeleport(InputValue input)
    {
    //teleport.TeleportForward();
        Vector2 direction = transform.right; 
        Vector2 targetPosition = rb.position + (direction * 3f);

        rb.linearVelocity = Vector2.zero;

        rb.position = targetPosition;
    }


    public Vector2 GetMovementDirection()
    {
    return movement;
    }

    

    public void StartDetectingKey_Jump()
    {
        bDetectKey = true;
        inputhandler.DetectNextKey(OnKeyDetected);
    }
    public void StartDetectingKey_Shoot()
    {
        bDetectKey = false;
        inputhandler.DetectNextKey(OnKeyDetected);
    }
    public void StartDetectingKey_Teleport()
    {
        bDetectKey = false;
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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerAcid = LayerMask.NameToLayer("Acid");

        if (collision.gameObject.layer == layerAcid)
    {
        Death();

    }
    }


    public void Death()
    {
        moveSpeed = 0f;
        GetComponent<SpriteRenderer>().enabled = false;
        //gameOverVisual.SetActive(true);
        GetComponent<PlayerInput>().enabled = false;
        StartCoroutine(EndGame());
        Vector3 spawnPosition = transform.position;

        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedInstance = Instantiate(corpse, spawnPosition, spawnRotation);  
    }
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.5f);
        RestartLevel();
    }
    private void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
