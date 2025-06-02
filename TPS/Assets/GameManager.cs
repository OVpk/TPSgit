using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Game
    }

    private GameState currentGameState = GameState.MainMenu;

    public static GameManager Instance;

    [SerializeField] private CheckPoint startPoint;
    public Vector3 currentCheckPointPosition { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            NewCheckPoint(startPoint);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void NewCheckPoint(CheckPoint newCheckPoint)
    {
        currentCheckPointPosition = newCheckPoint.transform.position;
    }

    public void ChangeGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        switch (currentGameState)
        {
            case GameState.MainMenu : ; break;
            case GameState.Game : ; break;
        }
    }

    public void EndGame()
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(6f);
        ChangeGameState(GameState.MainMenu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
