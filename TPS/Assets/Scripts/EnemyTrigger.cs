using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private bool alreadyActived = false;
    
    public EnemyController enemy;
    public bool triggerState;
    
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
