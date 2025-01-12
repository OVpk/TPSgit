using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints; // Liste des waypoints
    public float speed = 2f;          // Vitesse de déplacement
    public float waitTime = 2f;       // Temps d'arrêt à chaque waypoint
    public float rotationSpeed = 5f; // Vitesse de rotation

    private int currentWaypointIndex = 0; // Index du waypoint actuel
    private bool isWaiting = false;      // L'ennemi est-il en train d'attendre ?

    void Update()
    {
        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Count == 0) return; // Aucun waypoint défini

        // Position actuelle et position cible
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // Oriente l'ennemi vers le waypoint
        RotateTowards(targetPosition);

        // Déplace l'ennemi vers le waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Vérifie si l'ennemi a atteint le waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calcul de la direction vers la cible
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Si la direction n'est pas nulle, calcule la rotation
        if (direction.magnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public AnimationClip look;
    public Animator animator;
    
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;

        animator.SetTrigger("Look");
        // Attend avant de passer au waypoint suivant
        yield return new WaitForSeconds(look.length);

        // Passe au waypoint suivant
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Boucle sur la liste
        isWaiting = false;
    }
}
