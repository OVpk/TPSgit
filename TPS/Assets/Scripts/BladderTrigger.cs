using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladderTrigger : MonoBehaviour
{
    private bool alreadyActived = false;
    [SerializeField] private Bladder bladder;
    public bool triggerState;
    public float timer;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyActived) return;
        if (other.tag == "Player")
        {
            alreadyActived = true;
            bladder.gameObject.SetActive(triggerState);
            if (triggerState)
            {
                bladder.Init(timer);
            }
        }
    }
}
