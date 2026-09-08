using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerGeneral : MonoBehaviour
{
    [SerializeField] private Jump jump;
    [SerializeField] private Teleportation teleport;
    [SerializeField] private GameObject corpse;

    [SerializeField] private GameObject buttonselect;
    [SerializeField] private float stepDelay = 0.175f;

    [SerializeField] private float moveSpeed = 5f;

     private float stepTimer = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    GameObject buttonspawn;
    [SerializeField] private InputHandler inputhandler;

    public AudioClip walkSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 
    void Start()
    {
        spriteRenderer.enabled = false;
        Vector3 spawnPosition = transform.position;

        Quaternion spawnRotation = Quaternion.identity;
        buttonspawn = Instantiate(buttonselect, spawnPosition, spawnRotation);  


        StartDetectingKey_Teleport();
    }

    private void FixedUpdate()
    {
        if (buttonspawn == null)
        {
            if (!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            }
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
                if (walkSource != null) {
                    AudioSource.PlayClipAtPoint(walkSource, transform.position);
                }
                stepTimer = stepDelay;
            }
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

    public Vector2 GetMovementDirection()
    {
        return movement;
    }

    

    public void StartDetectingKey_Jump()
    {
        
        inputhandler.DetectNextKey(jump.JumpUp, buttonspawn);

    }
    public void StartDetectingKey_Shoot()
    {
       // inputhandler.DetectNextKey(, buttonspawn);
    }
    public void StartDetectingKey_Teleport()
    {
        inputhandler.DetectNextKey(teleport.TeleportForward, buttonspawn);
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
