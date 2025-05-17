using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera targetedCamera;      // La caméra à contrôler
    public Transform pointA;       // Position A
    public Transform pointB;       // Position B
    public Transform player;       // Transform du joueur

    [Range(0f, 1f)]
    public float smoothFactor = 0.1f; // Facteur de lissage pour le mouvement de la caméra

    public float cameraDistance = 5f; // Distance à maintenir avec le joueur

    void Update()
    {
        // Calcule la position relative du joueur entre A et B
        float t = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);

        // Interpole la position de la caméra entre A et B
        Vector3 targetPosition = Vector3.Lerp(pointA.position, pointB.position, t);

        // Calcule une position potentielle en gardant une distance avec le joueur
        Vector3 directionToPlayer = (player.position - targetPosition).normalized;
        Vector3 potentialPosition = targetPosition - directionToPlayer * cameraDistance;

        // Vérifie si la position potentielle reste entre A et B
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(potentialPosition.x, Mathf.Min(pointA.position.x, pointB.position.x), Mathf.Max(pointA.position.x, pointB.position.x)),
            Mathf.Clamp(potentialPosition.y, Mathf.Min(pointA.position.y, pointB.position.y), Mathf.Max(pointA.position.y, pointB.position.y)),
            Mathf.Clamp(potentialPosition.z, Mathf.Min(pointA.position.z, pointB.position.z), Mathf.Max(pointA.position.z, pointB.position.z))
        );

        // Applique un lissage au mouvement
        targetedCamera.transform.position = Vector3.Lerp(targetedCamera.transform.position, clampedPosition, smoothFactor);

        // Oriente la caméra pour regarder le joueur
        targetedCamera.transform.LookAt(player.position);
    }
}