using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerGeneral movement;
    private Vector2 lastMoveDir = Vector2.down;

    private void Awake()
    {
    animator = GetComponent<Animator>();
    movement = GetComponent<PlayerGeneral>();
    }
    private void Update()
    {   
        Vector2 moveDir = movement.GetMovementDirection();
        bool isMoving = moveDir.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
        lastMoveDir = moveDir;
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
        }
        else
        {
        // Keep facing last direction when idle
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
        }
    }
}