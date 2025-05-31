using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    private bool alreadyActived = false;
    [SerializeField] private MiniGame miniGame;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (alreadyActived) yield break;
        if (other.tag == "Player")
        {
            alreadyActived = true;
            other.gameObject.SetActive(false);
            yield return miniGame.StartGame();
            other.gameObject.SetActive(true);
        }
    }
}
