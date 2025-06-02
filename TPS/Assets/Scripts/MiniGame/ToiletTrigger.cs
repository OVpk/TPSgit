using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToiletTrigger : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            score++;
            UpdateDisplayedScore();
        }
    }

    private void UpdateDisplayedScore()
    {
        scoreText.text = score.ToString();
    }
}
