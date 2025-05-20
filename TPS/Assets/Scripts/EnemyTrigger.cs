using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public EnemyController enemy;
    public bool triggerState;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemy.gameObject.SetActive(triggerState);
        }
    }
}
