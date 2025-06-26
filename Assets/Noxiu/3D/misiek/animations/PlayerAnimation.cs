using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool hasWeaponEquipped = false;
    private int currentWeaponSlot = -1;

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
        animator.SetFloat("SpeedMultiplier", isRunning ? 1.5f : 1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (currentWeaponSlot == i)
                {
                    animator.SetTrigger("Weapon_PutAway");
                    hasWeaponEquipped = false;
                    currentWeaponSlot = -1;
                    animator.SetBool("HasWeaponEquipped", false);
                }
                else
                {
                    if (hasWeaponEquipped)
                    {
                        StartCoroutine(SwapWeapon(i));
                    }
                    else
                    {
                        animator.SetTrigger("Weapon_TakeOut");
                        hasWeaponEquipped = true;
                        currentWeaponSlot = i;
                        animator.SetBool("HasWeaponEquipped", true);
                    }
                }
            }
        }
    }

    private System.Collections.IEnumerator SwapWeapon(int newSlot)
    {
        animator.SetTrigger("Weapon_PutAway");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Weapon_TakeOut");
        hasWeaponEquipped = true;
        currentWeaponSlot = newSlot;
        animator.SetBool("HasWeaponEquipped", true);
    }
}
