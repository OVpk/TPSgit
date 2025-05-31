using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDetectionZone : MonoBehaviour
{
    public EnemyController enemyController;
    protected bool playerDetected = false;
}
