using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints;

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Update()
    {
        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    [SerializeField] private NavMeshAgent navMesh;

    void MoveToWaypoint()
    {
        if (waypoints.Count == 0) return;
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        navMesh.SetDestination(targetPosition);
        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    public AnimationClip look;
    public Animator animator;
    
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;

        animator.SetTrigger("Look");
        yield return new WaitForSeconds(look.length);
        
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        isWaiting = false;
    }

    public GameObject earZone;
    public GameObject visionZone;
    
    public void Dying()
    {
        GetComponent<RagdollController>().EnableRagdoll(true);
        earZone.SetActive(false);
        visionZone.SetActive(false);
    }
    
}
