using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : EnemyDetectionZone
{
    public float timeout = 2f;
    private float timeInBadState = 0f;
    private void OnTriggerStay(Collider other)
    {
        if (playerDetected) return;
        
        if (other.CompareTag("Player"))
        {
            
            if (PlayerController.Instance.currentMoveState != PlayerController.MoveState.WalkingSilently &&
                PlayerController.Instance.currentMoveState != PlayerController.MoveState.Idle &&
                PlayerController.Instance.currentMoveState != PlayerController.MoveState.Punching)
            {
                
                timeInBadState += Time.deltaTime;
                
                if (timeInBadState >= timeout)
                {
                    playerDetected = true;
                    ownEnemyController.ChangeState(EnemyController.EnemyState.Chasing);
                }
            }
            else
            {
                timeInBadState = 0f;
            }
        }
    }

}
