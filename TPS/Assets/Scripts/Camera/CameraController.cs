using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera targetedCamera;
    [SerializeField] private  Transform pointA;
    [SerializeField] private  Transform pointB;
    [SerializeField] private  Transform player;

    [Range(0f, 1f), SerializeField]
    private float smoothFactor = 0.1f;

    [SerializeField] private float cameraDistance = 5f;

    void Update()
    {
        float t = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);

        Vector3 basePosition = Vector3.Lerp(pointA.position, pointB.position, t);

        Vector3 directionToA = (pointA.position - basePosition).normalized;

        Vector3 targetPosition = basePosition + directionToA * cameraDistance;

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, Mathf.Min(pointA.position.x, pointB.position.x), Mathf.Max(pointA.position.x, pointB.position.x)),
            Mathf.Clamp(targetPosition.y, Mathf.Min(pointA.position.y, pointB.position.y), Mathf.Max(pointA.position.y, pointB.position.y)),
            Mathf.Clamp(targetPosition.z, Mathf.Min(pointA.position.z, pointB.position.z), Mathf.Max(pointA.position.z, pointB.position.z))
        );

        targetedCamera.transform.position = Vector3.Lerp(targetedCamera.transform.position, clampedPosition, smoothFactor);

        targetedCamera.transform.LookAt(player.position);
    }
}