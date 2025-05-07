using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    public float MovementSpeed = 10.0f, RotationSpeed = 5.0f, JumpForce = 10.0f, Gravity = -30.0f;

    private float _rotationY;
    private float _verticalVelocity;
    void Start()
    {
      _characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector2 movementVector)
    {
        Vector3 move = transform.forward * movementVector.y + transform.right * movementVector.x;
        move = MovementSpeed * Time.deltaTime * move;
        _characterController.Move(move);

        _verticalVelocity = _verticalVelocity + Gravity * Time.deltaTime;
        // Parsing this into character controller 
        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }
    public void Rotate(Vector2 rotationVector)
    {
        _rotationY += rotationVector.x * RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, _rotationY, 0);
    }
    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = JumpForce;
        }
    }
}
