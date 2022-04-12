using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    private GameState currentState = GameState.Init;
    public Action<GameState> OnNewGameState;

    [HideInInspector]
    public SceneTransitionManager SceneManager;
    [HideInInspector]
    public AudioManager AudioManager;
    [HideInInspector]
    public MusicManager MusicManager;
    [HideInInspector]
    public LoadingScreen LoadingScreenManager;

    [HideInInspector]
    public GameManager GameManager;
    [SerializeField] private GameObject GameManagerPrefab;
    [SerializeField] private GameObject[] managersToInit;



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
  
    public void BeginNewState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Init:
                SceneManager.BeginLoadMenuScreen(SceneIndex.TitleScreen);
                break;
            case GameState.GoToTitle:
                break;
            case GameState.TitleScreen:
                LoadingScreenManager.BeginFadeOut();
                break;
            case GameState.BeginLevelLoad:
                break;

            case GameState.GoToGameScene:
                SceneManager.BeginLoadLevel(SceneIndex.GameRootScene);
                break;

            case GameState.GameSceneLoadComplete:
                if (GameManager)
                {
                    GameManager.InitGame();
                }
                else
                {
                    GameManager = Instantiate(GameManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<GameManager>();

                    if (GameManager)
                    {
                        GameManager.Init();
                        GameManager.InitGame();
                    }
                    else
                    {
                        Debug.LogError("No Game manager reference --- Check Prefab");
                    }
                }
                break;

            case GameState.GameSceneSetUpComplete:
                if (LoadingScreenManager.IsLoadingScreenOn())
                {

                    LoadingScreenManager.BeginFadeOut();
                 
                }
                break;
            case GameState.GamePaused:
                break;
            case GameState.GameRunning:
                break;
        }
        OnNewGameState?.Invoke(currentState);
    }
}