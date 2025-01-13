using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            if (player.currentMoveState != PlayerController.MoveState.WalkingSilently &&
                player.currentMoveState != PlayerController.MoveState.Idle &&
                player.currentMoveState != PlayerController.MoveState.Punching)
            {
                player.StunPlayer();
            }
        }
    }

}
