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
        if (enemyController.playerDetected) return;
        
        if (other.CompareTag("Player"))
        {
            
            if (PlayerController.Instance.currentMoveState == PlayerController.MoveState.Walking)
            {
                
                timeInBadState += Time.deltaTime;
                
                if (timeInBadState >= timeout)
                {
                    enemyController.playerDetected = true;
                    enemyController.ChangeState(EnemyController.EnemyState.Chasing);
                }
            }
            else
            {
                timeInBadState = 0f;
            }
        }
    }

}
