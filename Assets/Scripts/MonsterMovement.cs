using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindPlayer();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("FacePlayer: No object tagged 'Player' found in scene.");
        }
    }
    void FixedUpdate()
    {
        if (player == null) return;
        bool check = player.GetComponent<PlayerGeneral>().spriteRenderer.enabled;
        if (check)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        
    }

    void Update()
    {
        if (player == null) return; // safety check in case player wasn't found or was destroyed

        if (player.position.x > transform.position.x)
        {
            // Player is to the right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }  else
        {
            // Player is to the left — face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
    }
}
