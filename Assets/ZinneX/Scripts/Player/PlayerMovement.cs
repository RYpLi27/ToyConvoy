using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [TabGroup("Movement")] [SerializeField] private float jumpForce;
    [TabGroup("Movement")] [SerializeField] private float jumpBufferTime;
    [TabGroup("Movement")] [SerializeField] private float coyoteTime;
    [TabGroup("Movement")] [SerializeField] private float gravityScale;
    [TabGroup("Movement")] [SerializeField] private float runSpeedBonus;
    [TabGroup("Movement")] [SerializeField] private float jumpStaminaCost;
    [TabGroup("Movement")] [SerializeField] private float runStaminaCost;
    private bool isRunning, canJump;
    private float currentMoveSpeed;

    [TabGroup("Camera")] [SerializeField] private float sensitivity;
    [TabGroup("Camera")] [SerializeField] private Transform cameraFollowTransform;
    
    [TabGroup("Others")] [SerializeField] private Rigidbody rb;
    [TabGroup("Others")] [SerializeField] private Transform groundCheck;
    [TabGroup("Others")] [SerializeField] private LayerMask whatIsGround;
    [TabGroup("Others")] [SerializeField] private ExtendedStatsSO statsSO;
    [TabGroup("Others")] [SerializeField] private StaminaManager staminaManager;
    
    private Vector2 moveInputValues;

    private float lastJumpPressedTime, coyoteTimeCounter;
    private float yRotation, xRotation;

    private bool isCursorLocked;
    
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canJump = true;
        isCursorLocked = true;

        yRotation = transform.rotation.eulerAngles.y;
        xRotation = transform.rotation.eulerAngles.x;
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
        if (context.performed) isRunning = true;
        
        if (context.canceled) isRunning = false;

        if (staminaManager.isExhausted == true) isRunning = false;
    }
    
    private void Move() {
        Vector3 dir = transform.forward * moveInputValues.y + transform.right * moveInputValues.x;

        if (isRunning == true) { staminaManager.DrainStamina(runStaminaCost * Time.deltaTime); }

        currentMoveSpeed = (isRunning, staminaManager.isExhausted) switch {
            (false, false) => statsSO.moveSpeed,
            (true, false) => statsSO.runSpeed,
            (false, true) => statsSO.exhaustedSpeed,
            (true, true) => statsSO.exhaustedSpeed,
        };

        rb.linearVelocity = new Vector3(dir.x * currentMoveSpeed, rb.linearVelocity.y, dir.z * currentMoveSpeed);
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

    #region Rotation
    public void MousePositionInput(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 mouseInput = context.ReadValue<Vector2>();

            float mouseX = mouseInput.x * sensitivity;
            float mouseY = mouseInput.y * sensitivity;
            
            Rotate(mouseX, mouseY);
        }
    }

    
    private void Rotate(float xRot, float yRot) {
        if (isCursorLocked == false || GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        yRotation += xRot;
        xRotation -= yRot;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        rb.MoveRotation(Quaternion.Euler(0f, yRotation, 0f)); // HORIZONTAL ROTATION
        cameraFollowTransform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // VERTICAL ROTATION
    }
    #endregion
    
    public void LockCursorInput(InputAction.CallbackContext context) {
        if (GameManager.gameState != GameManager.GameState.Ongoing) return;
        
        if (context.performed) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            isCursorLocked = false;
        }
        
        if (context.canceled) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorLocked = true;
        }
    }
    
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
