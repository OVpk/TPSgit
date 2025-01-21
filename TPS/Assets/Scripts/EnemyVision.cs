using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public PlayerController player;
    public LayerMask layerMask;
    public GameObject originObject; // L'objet d'origine du raycast
    public GameObject targetObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            // Calcule la direction du raycast
            Vector3 direction = (targetObject.transform.position - originObject.transform.position).normalized;

            // Crée le raycast
            Ray ray = new Ray(originObject.transform.position, direction);
            RaycastHit hit;

            // Longeur du raycast entre l'origine et la cible
            float rayLength = Vector3.Distance(originObject.transform.position, targetObject.transform.position);

            // Effectue le raycast
            if (Physics.Raycast(ray, out hit, rayLength, layerMask))
            {
                // Vérifie si l'objet touché est la cible
                if (hit.collider.gameObject == targetObject)
                {
                    player.StunPlayer();
                }
            }
        }
    }

}

