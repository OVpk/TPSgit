using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bladder : MonoBehaviour
{
    private bool isPlayerDeath = false;
    private float timerBeforeDeath;
    [SerializeField] private PlayerController targetedPlayer;
    [SerializeField] private Image bladderVisual;
    [SerializeField] private GameObject peePrefab;

    public void Init(float timer)
    {
        isPlayerDeath = false;
        timerBeforeDeath = timer;
        StartCoroutine(BlinkVisual());
    }

    private void Update()
    {
        if (isPlayerDeath) return;
        timerBeforeDeath -= Time.deltaTime;

        if (timerBeforeDeath <= 0f)
        {
            isPlayerDeath = true;
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        targetedPlayer.Dying();
        Vector3 explosionCenter = targetedPlayer.transform.position;
        Explosion(explosionCenter);
    }
    
    [SerializeField] private int peeCount = 10;
    [SerializeField] private float explosionRadius = 5f;
    
    private void Explosion(Vector3 center)
    {
        for (int i = 0; i < peeCount; i++)
        {
            float theta = Random.Range(0, Mathf.PI * 2);
            float phi = Mathf.Acos(2 * Random.Range(0f, 1f) - 1);

            Vector3 randomDirection = new Vector3(
                Mathf.Sin(phi) * Mathf.Cos(theta),
                Mathf.Sin(phi) * Mathf.Sin(theta),
                Mathf.Cos(phi)
            );

            Vector3 spawnPosition = center + randomDirection * explosionRadius;

            GameObject peeInstance = Instantiate(peePrefab, spawnPosition, Quaternion.identity);

            Rigidbody rb = peeInstance.GetComponent<Rigidbody>();
            rb.AddForce(randomDirection * Random.Range(5f, 10f), ForceMode.Impulse);
        }
    }
    
    private IEnumerator BlinkVisual()
    {
        Color originalColor = bladderVisual.color;
        Color blinkColor = Color.red;

        while (timerBeforeDeath > 0)
        {
            bladderVisual.color = bladderVisual.color == originalColor ? blinkColor : originalColor;
            yield return new WaitForSeconds(0.2f);
        }
        bladderVisual.color = originalColor;
        bladderVisual.gameObject.SetActive(false);
    }
}
