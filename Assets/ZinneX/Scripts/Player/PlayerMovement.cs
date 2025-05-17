using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [TabGroup("Vertical")] [SerializeField] private float jumpForce;
    [TabGroup("Vertical")] [SerializeField] private float jumpBufferTime;
    [TabGroup("Vertical")] [SerializeField] private float jumpStaminaCost;
    [TabGroup("Vertical")] [SerializeField] private float coyoteTime;
    [TabGroup("Vertical")] [SerializeField] private float gravityScale;
    
    
    [TabGroup("Horizontal")] [SerializeField] private float moveSpeed;
    [TabGroup("Horizontal")] [SerializeField] private float runSpeed;
    [TabGroup("Horizontal")] [SerializeField] private float exhaustedSpeed;
    [TabGroup("Horizontal")] [SerializeField] private float runStaminaCost;
    private bool isRunning, canJump;
    private float currentMoveSpeed;
    
    [TabGroup("Others")] [SerializeField] private Rigidbody rb;
    [TabGroup("Others")] [SerializeField] private Transform groundCheck;
    [TabGroup("Others")] [SerializeField] private LayerMask whatIsGround;
    [TabGroup("Others")] [SerializeField] private StaminaManager staminaManager;
    [TabGroup("Others")] [SerializeField] private RunTrail runTrail;
    
    private Vector2 moveInputValues;

    private float lastJumpPressedTime, coyoteTimeCounter;
    
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canJump = true;
    }

    private void FixedUpdate() {
        if (GameManager.gameState != GameManager.GameState.Ongoing) {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        
        Move();
        ApplyGravity();
        
        if(GroundedCheck() && Time.time - lastJumpPressedTime < jumpBufferTime) Jump();
    }

    #region Horizontal Movement
    public void MoveInput(InputAction.CallbackContext context) {
        moveInputValues = context.ReadValue<Vector2>();
    }

    public void RunInput(InputAction.CallbackContext context) {
        if (context.performed && staminaManager.isExhausted == false) {
            isRunning = true;
            runTrail.EnableTrail();
        }
        
        if (context.canceled) {
            isRunning = false;
            runTrail.DisableTrail();
        }

        if (staminaManager.isExhausted == true) {
            isRunning = false;
            runTrail.DisableTrail();
        }
    }
    
    private void Move() {
        Vector3 dir = transform.forward * moveInputValues.y + transform.right * moveInputValues.x;
        
        currentMoveSpeed = (isRunning, staminaManager.isExhausted) switch {
            (false, false) => moveSpeed,
            (true, false) => runSpeed,
            _ => exhaustedSpeed
        };

        rb.linearVelocity = new Vector3(dir.x * currentMoveSpeed, rb.linearVelocity.y, dir.z * currentMoveSpeed);

        if (isRunning == true && Mathf.Abs(rb.linearVelocity.magnitude) >= .1f && staminaManager.isExhausted == false)
            if(staminaManager.DrainStamina(runStaminaCost * Time.deltaTime) == true) runTrail.DisableTrail();
    }
    #endregion
    
    #region Vertical Movement
    public void JumpInput(InputAction.CallbackContext context) {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        if (context.performed) {
            lastJumpPressedTime = Time.time;
        }

        // if (context.canceled) {
        //     JumpCut();
        // }
    }

    private void Jump() {
        if (staminaManager.isExhausted || canJump == false) return;

        canJump = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        staminaManager.DrainStamina(jumpStaminaCost);
        Invoke(nameof(AllowJumping), .1f);
    }

    private void AllowJumping() {
        canJump = true;
    }
    
    // private void JumpCut() {
    //     if (rb.linearVelocity.y <= 0) return;
    //
    //     rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * .4f, rb.linearVelocity.z);
    // }

    private void ApplyGravity() {
        if (GroundedCheck() == false) rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
    #endregion
    
    private bool GroundedCheck() {
        if (Physics.CheckBox(groundCheck.position, new Vector3(.45f, .1f, .45f), Quaternion.identity, whatIsGround, QueryTriggerInteraction.Ignore)) {
            coyoteTimeCounter = coyoteTime;
        } else { coyoteTimeCounter -= Time.fixedDeltaTime; }

        return coyoteTimeCounter > 0;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(.9f, .1f, .9f));
    }
}
