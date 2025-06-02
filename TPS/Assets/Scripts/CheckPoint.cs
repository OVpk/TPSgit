using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool alreadyActived = false;
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyActived) return;
        if (other.tag == "Player")
        {
            alreadyActived = true;
            GameManager.Instance.NewCheckPoint(this);
        }
    }
}
