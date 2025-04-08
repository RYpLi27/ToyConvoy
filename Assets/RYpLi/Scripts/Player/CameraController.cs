using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowCharacter, LookCharacter;
    public float FollowSpeed = 10.0f;


    private void LateUpdate()
    {
        Vector3 targetPosition = FollowCharacter.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);

        transform.LookAt(LookCharacter);
    }
}
