using UnityEngine;

public class MouseAnimator : MonoBehaviour
{
    public Animator animator;
    public string moveAnimation = "mouseAnimation";
    public string idleAnimation = "mouseIdle";

    private Vector3 lastPosition;
    private bool isMoving;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        isMoving = (currentPosition - lastPosition).magnitude > 0.001f;

        animator.SetBool("isMoving", isMoving);

        lastPosition = currentPosition;
    }
}
