using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    [SerializeField] private CinematicController introCinematic;
    [SerializeField] private float gameDuration;
    protected bool isGameStarted = false;
    
    public IEnumerator StartGame()
    {
        gameObject.SetActive(true);
        
        if (introCinematic != null)
        {
            yield return introCinematic.PlayCinematic();
        }
        
        InitGame();
        isGameStarted = true;
        yield return new WaitForSeconds(gameDuration);
        
        StopGame();
    }

    protected abstract void InitGame();
    
    private void StopGame()
    {
        gameObject.SetActive(false);
    }
}
