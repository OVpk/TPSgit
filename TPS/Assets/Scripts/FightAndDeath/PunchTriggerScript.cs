using UnityEngine;

public class PunchTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && tag == "Player")
        {
            other.GetComponent<EnemyMovement>().Dying();
        }

        if (other.tag == "Player" && tag == "Enemy")
        {
            PlayerController.Instance.Dying();
        }
    }
}
