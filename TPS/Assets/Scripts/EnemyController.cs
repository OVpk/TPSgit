using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyMovement
{
    public bool playerDetected = false;
    
    public enum EnemyState
    {
        Patrolling,
        Looking,
        Chasing,
        Punching
    }

    public EnemyState currentState;
    
    private void Start()
    {
        ChangeState(EnemyState.Patrolling);
    }
    
    void HandleStateTransition()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                MoveToWaypoint();
                break;
            case EnemyState.Looking:
                LookingAround();
                break;
            case EnemyState.Chasing:
                MoveToPlayer();
                break;
            case EnemyState.Punching:
                Punching();
                break;
        }
    }

    public override void ChangeState(EnemyState newState)
    {
        currentState = newState;
        HandleStateTransition();
    }
    
}
