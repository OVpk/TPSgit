using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private EnemyController requiredEnemy;
    private bool canBeOpen = false;
    private bool isOpen = false;
    [SerializeField] private GameObject keyPopup;

    private void Start()
    {
        if (requiredEnemy != null)
        {
            requiredEnemy.OnDying += () => canBeOpen = true;
        }
        else
        {
            canBeOpen = true;
        }
    }

    private void TryOpenDoor()
    {
        if (isOpen) return;
        if (canBeOpen)
        {
            isOpen = true;
            OpenDoor();
        }
        else
        {
            DisplayKeyIndication(true);
        }
    }

    private void DisplayKeyIndication(bool state)
    {
        keyPopup.SetActive(state);
    }

    private void OpenDoor()
    {
        transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            TryOpenDoor();
        }
    }

    private IEnumerator OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            yield return new WaitForSeconds(3f);
            DisplayKeyIndication(false);
        }
    }
}