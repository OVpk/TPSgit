using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private NavMeshAgent navMesh;
    
    [SerializeField] private AnimationClip look;
    [SerializeField] private Animator animator;
    
    [SerializeField] private EnemyHearing earZone;
    [SerializeField] private EnemyVision visionZone;
    [SerializeField] private GameObject[] punchTriggers;
    
    public event Action OnDying;
    
    private void Awake()
    {
        if (earZone != null && visionZone != null)
        {
            earZone.SetController((EnemyController)this);
            visionZone.SetController((EnemyController)this);
        }
        else
        {
            throw new Exception("ear or vision zone must be assigned");
        }
    }
    
    private IEnumerator MoveToTarget(Transform target, EnemyController.EnemyState stateAfterTarget)
    {
        while (Vector3.Distance(transform.position, target.position) >= 1.5f)
        {
            navMesh.SetDestination(target.position);
            yield return null;
        }
        
        ChangeState(stateAfterTarget);
    }

    protected void MoveToWaypoint()
    {
        if (waypoints.Count == 0) return;
        
        StopAllCoroutines();
        
        StartCoroutine(MoveToTarget(waypoints[currentWaypointIndex], EnemyController.EnemyState.Looking));
    }
    
    protected void MoveToPlayer()
    {
        StopAllCoroutines();

        navMesh.speed *= 8;
        
        animator.SetTrigger("PlayerDetected");
        
        StartCoroutine(MoveToTarget(PlayerController.Instance.transform, EnemyController.EnemyState.Punching));
    }

    protected void LookingAround()
    {
        StopAllCoroutines();
        
        StartCoroutine(WaitAtWaypoint());
    }

    private IEnumerator WaitAtWaypoint()
    {
        animator.SetTrigger("Look");
        yield return new WaitForSeconds(look.length);
        
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        ChangeState(EnemyController.EnemyState.Patrolling);
    }

    protected void Punching()
    {
        Vector3 targetPosition = PlayerController.Instance.transform.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
        PlayerController.Instance.enabled = false;
        animator.SetTrigger("Punch");
        foreach (var punchTrigger in punchTriggers)
        {
            punchTrigger.SetActive(true);
        }
    }
    
    public void Dying()
    {
        OnDying?.Invoke();
        
        StopAllCoroutines();
        GetComponent<RagdollController>().EnableRagdoll(true);
        earZone.gameObject.SetActive(false);
        visionZone.gameObject.SetActive(false);
        navMesh.enabled = false;
        this.enabled = false;
    }

    public abstract void ChangeState(EnemyController.EnemyState newState);
}
