using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : EnemyDetectionZone
{
    public LayerMask layerMask;
    public GameObject originObject;
    public GameObject targetObject;


    private void OnTriggerStay(Collider other)
    {
        if (enemyController.playerDetected) return;
        
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
                    enemyController.playerDetected = true;
                    enemyController.ChangeState(EnemyController.EnemyState.Chasing);
                }
            }
        }
    }

}

