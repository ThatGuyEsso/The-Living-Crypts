using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    private GameState currentState = GameState.Init;
    public Action<GameState> OnNewGameState;

    [SerializeField] private GameObject[] managersToInit;
    [SerializeField] private GameObject[] dungeonManagersToInit;
    private void Awake()
    {

        if (instance == false)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }


    private void Start()
    {
        InitManagers();

    }


    private void InitManagers()
    {
        foreach (GameObject manager in managersToInit)
        {
            GameObject currManager = Instantiate(manager, Vector3.zero, Quaternion.identity);
            IInitialisable init = currManager.GetComponent<IInitialisable>();
            if (init != null) init.Init();

    
        }
        BeginNewState(GameState.Init);


    }
    private void InitDungeonManagers()
    {
        foreach (GameObject manager in dungeonManagersToInit)
        {
            GameObject currManager = Instantiate(manager, Vector3.zero, Quaternion.identity);
            IInitialisable init = currManager.GetComponent<IInitialisable>();
            if (init != null) init.Init();


        }
        BeginNewState(GameState.GameSetUp);

    }
    public void BeginNewState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Init:
                SceneTransitionManager.instance.BeginLoadMenuScreen(SceneIndex.TitleScreen);
                break;
            case GameState.GoToTitle:
                break;
            case GameState.TitleScreen:
                LoadingScreen.instance.BeginFadeOut();
                break;
            case GameState.BeginLevelLoad:
                break;
            case GameState.GameLevelLoaded:
                InitDungeonManagers();
                break;
            case GameState.GameSetUp:
                break;
            case GameState.LevelGenerated:
                LoadingScreen.instance.BeginFadeOut();
                break;
            case GameState.PlayerSpawned:
                break;
            case GameState.GamePaused:
                break;
            case GameState.GameRunning:
                break;
            case GameState.PlayerDied:
                break;
            case GameState.HighScoreTable:
                break;
        }
        OnNewGameState?.Invoke(currentState);
    }
}