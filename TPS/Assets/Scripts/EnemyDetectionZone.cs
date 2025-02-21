using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDetectionZone : MonoBehaviour
{
    protected EnemyController ownEnemyController;

    protected bool playerDetected = false;

    private void Start()
    {
        ownEnemyController = transform.root.gameObject.GetComponent<EnemyController>();
    }
}
