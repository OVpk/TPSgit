using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera targetedCamera;  // La caméra à contrôler
    public Transform pointA;       // Position A
    public Transform pointB;       // Position B
    public Transform player;       // Transform du joueur

    [Range(0f, 1f)]
    public float smoothFactor = 0.1f; // Facteur de lissage pour le mouvement de la caméra

    public float cameraDistance = 5f; // Distance que la caméra doit tendre vers A

    void Update()
    {
        // Calcule la position relative du joueur entre A et B
        float t = Mathf.InverseLerp(pointA.position.x, pointB.position.x, player.position.x);

        // Interpole la position de la caméra entre A et B
        Vector3 basePosition = Vector3.Lerp(pointA.position, pointB.position, t);

        // Calcule la direction vers A depuis la position interpolée
        Vector3 directionToA = (pointA.position - basePosition).normalized;

        // Ajoute la distance choisie pour rapprocher la caméra de A
        Vector3 targetPosition = basePosition + directionToA * cameraDistance;

        // Vérifie si la position calculée reste entre A et B
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, Mathf.Min(pointA.position.x, pointB.position.x), Mathf.Max(pointA.position.x, pointB.position.x)),
            Mathf.Clamp(targetPosition.y, Mathf.Min(pointA.position.y, pointB.position.y), Mathf.Max(pointA.position.y, pointB.position.y)),
            Mathf.Clamp(targetPosition.z, Mathf.Min(pointA.position.z, pointB.position.z), Mathf.Max(pointA.position.z, pointB.position.z))
        );

        // Applique un lissage au mouvement
        targetedCamera.transform.position = Vector3.Lerp(targetedCamera.transform.position, clampedPosition, smoothFactor);

        // Oriente la caméra pour regarder le joueur
        targetedCamera.transform.LookAt(player.position);
    }
}