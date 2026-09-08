using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class PlayerGeneral : MonoBehaviour
{
    [SerializeField] private Jump jump;
    [SerializeField] private Teleportation teleport;
    [SerializeField] private GameObject corpse;

    [SerializeField] private GameObject buttonselect;
    [SerializeField] private float stepDelay = 0.175f;

    [SerializeField] private float moveSpeed = 5f;

    public Dictionary<int, Action> LevelAction;

    public int currentlevel;
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

        if (currentlevel != 0) {
            LevelAction= new Dictionary<int, Action>
            {
                [1] = StartDetectingKey_Jump,
                [2] = StartDetectingKey_Teleport,
                [3] = StartDetectingKey_Shoot
            };
        }
        


    } 
    void Start()
    {
        bool check1 = true;
        bool check2 = true;

        if (currentlevel >= 1)
        {
            check1 = inputhandler.RegisterByName("JumpUp", jump.JumpUp);
        }

        if (currentlevel >= 2)
        {
            check2 = inputhandler.RegisterByName("TeleportForward", teleport.TeleportForward);
        }

        if (!check1 || !check2)
        {
            spriteRenderer.enabled = false;
            Vector3 spawnPosition = transform.position;

            Quaternion spawnRotation = Quaternion.identity;
            buttonspawn = Instantiate(buttonselect, spawnPosition, spawnRotation);  

            LevelAction[currentlevel]();
        }
        

        
        
    }
    protected void KeyBoundedSuccess()
    {
        spriteRenderer.enabled = true;
    }
    private void FixedUpdate()
    {
        if (buttonspawn == null)
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
        
        inputhandler.DetectNextKey(jump.JumpUp, buttonspawn, KeyBoundedSuccess);

    }
    public void StartDetectingKey_Shoot()
    {
       Destroy(buttonspawn);
       KeyBoundedSuccess();
       
    }
    public void StartDetectingKey_Teleport()
    {
        inputhandler.DetectNextKey(teleport.TeleportForward, buttonspawn, KeyBoundedSuccess);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerAcid = LayerMask.NameToLayer("Acid");
        int layerPortal = LayerMask.NameToLayer("Portal");

        if (collision.gameObject.layer == layerAcid)
        {
            Death();

        }
        if (collision.gameObject.layer == layerPortal)
        {
            Portal();

        }
    }


    public void Death()
    {
        if (currentlevel == 1)
        {
            inputhandler.RemoveBinding("JumpUp");
        } else if (currentlevel == 2)
        {
            inputhandler.RemoveBinding("TeleportForward");
        }
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

    private void Portal()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }


}
