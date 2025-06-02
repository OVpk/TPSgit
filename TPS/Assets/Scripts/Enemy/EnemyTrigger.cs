using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private bool alreadyActived = false;
    
    [SerializeField] private EnemyController enemy;
    [SerializeField] private bool triggerState;
    
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyActived) return;
        if (other.tag == "Player")
        {
            enemy.gameObject.SetActive(triggerState);
            alreadyActived = true;
        }
    }
}
