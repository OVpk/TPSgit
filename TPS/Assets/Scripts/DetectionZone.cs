using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{

    public float timeout = 2f;
    private float timeInBadState = 0f;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.currentMoveState != PlayerController.MoveState.WalkingSilently &&
                player.currentMoveState != PlayerController.MoveState.Idle &&
                player.currentMoveState != PlayerController.MoveState.Punching)
            {
                
                timeInBadState += Time.deltaTime;
                
                if (timeInBadState >= timeout)
                {
                    player.StunPlayer();
                }
            }
            else
            {
                timeInBadState = 0f;
            }
        }
    }

}
