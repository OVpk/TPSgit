using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    
    public void StartGame()
    {
        player.gameObject.SetActive(true);
        gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
