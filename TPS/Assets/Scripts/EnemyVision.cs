using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public PlayerController player;
    public LayerMask layerMask;
    public GameObject originObject;
    public GameObject targetObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHead"))
        {
            Vector3 direction = (targetObject.transform.position - originObject.transform.position).normalized;

            Ray ray = new Ray(originObject.transform.position, direction);
            RaycastHit hit;

            float rayLength = Vector3.Distance(originObject.transform.position, targetObject.transform.position);

            if (Physics.Raycast(ray, out hit, rayLength, layerMask))
            {
                if (hit.collider.gameObject == targetObject)
                {
                    player.StunPlayer();
                }
            }
        }
    }

}

