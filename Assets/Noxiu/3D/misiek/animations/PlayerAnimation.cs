using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        bool isMoving = moveX != 0 || moveY != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalkingBack = moveY < 0;

        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalkingBackward", isWalkingBack);
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        // animator.SetFloat("SpeedMultiplier", isRunning ? 1.5f : 1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
    }
}
