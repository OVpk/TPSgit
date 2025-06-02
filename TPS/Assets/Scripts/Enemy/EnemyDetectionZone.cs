using UnityEngine;

public abstract class EnemyDetectionZone : MonoBehaviour
{
    protected EnemyController enemyController;

    public void SetController(EnemyController controller)
    {
        enemyController = controller;
    }
}
