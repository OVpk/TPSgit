using UnityEngine;

public class EnemyHearing : EnemyDetectionZone
{
    [SerializeField] private float timeout = 2f;
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
