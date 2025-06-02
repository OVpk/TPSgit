using UnityEngine;

public class TeleportationTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportationPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = teleportationPoint.position;
        }
    }
}
