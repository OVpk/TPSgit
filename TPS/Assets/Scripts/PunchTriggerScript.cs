using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyMovement>().Dying();
            other.GetComponent<EnemyMovement>().enabled = false;
        }
    }
}
