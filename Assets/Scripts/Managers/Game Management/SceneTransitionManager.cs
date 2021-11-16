using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour, IManager, IInitialisable
{
    public static SceneTransitionManager instance;
    public static SceneIndex currentScene;
    private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    private bool isFading;
    public void BindToGameStateManager()
    {
        GameStateManager.instance.OnNewGameState += EvaluateGameState;
    }

    public void EvaluateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Init:
                BeginLoadMenuScreen(SceneIndex.TitleScreen);
            break;
        }
    }

    public void Init()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private Scene[] GetAllActiveScenes()
    {
        //Get all number of scenes loaded
        int countLoaded = SceneManager.sceneCount;

        //create array of respective size
        Scene[] loadedScenes = new Scene[countLoaded];

        //get all loaded scnes
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = (SceneManager.GetSceneAt(i));
        }
        //retun loaded scenes
        return loadedScenes;
    }

    public void BeginLoadMenuScreen(SceneIndex menuSceneIndex)
    {
        StopAllCoroutines();
        StartCoroutine(LoadMenuScreen(menuSceneIndex));    
    }
    public void BeginLoadLevel(SceneIndex sceneIndex)
    {
        StopAllCoroutines();
        StartCoroutine(LoadLevel(sceneIndex));
    }
    private IEnumerator LoadLevel(SceneIndex newLevel)
    {
        GameStateManager.instance.BeginNewState(GameState.BeginLevelLoad);
        if (!LoadingScreen.instance.IsLoadingScreenOn())
        {
            isFading = true;
            LoadingScreen.instance.OnFadeComplete += OnFadeComplete;
            LoadingScreen.instance.BeginFadeIn();
            while (isFading)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.5f);
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)currentScene));
        //sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.PlayerScene, LoadSceneMode.Additive));
        //sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.HUDscene, LoadSceneMode.Additive));
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)newLevel, LoadSceneMode.Additive));
        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        //clear scens loading
        sceneLoading.Clear();
        currentScene = newLevel;
        GameStateManager.instance.BeginNewState(GameState.GameLevelLoaded);
    }
    private IEnumerator LoadMenuScreen(SceneIndex menuSceneIndex)
    {
        Scene[] loadedScenes = GetAllActiveScenes();

        if (!LoadingScreen.instance.IsLoadingScreenOn())
        {
            isFading = true;
            LoadingScreen.instance.OnFadeComplete += OnFadeComplete;
            LoadingScreen.instance.BeginFadeIn();
            while (isFading)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.5f);
        //add and unload operations
        foreach (Scene scene in loadedScenes)
        {
            if (scene.buildIndex != (int)SceneIndex.RootScene)
                sceneLoading.Add(SceneManager.UnloadSceneAsync(scene));
        }

        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        //clear scens loading
        sceneLoading.Clear();

        //begin loading title screen
        AsyncOperation menuScene = SceneManager.LoadSceneAsync((int)menuSceneIndex, LoadSceneMode.Additive);

        while (!menuScene.isDone)
        {
            yield return null;

        }

        currentScene = menuSceneIndex;
        EvaluateSceneLoaded(currentScene);
    }

    public void EvaluateSceneLoaded(SceneIndex scene)
    {
        switch (scene)
        {
            case SceneIndex.TitleScreen:
                GameStateManager.instance.BeginNewState(GameState.TitleScreen);
                
                break;

            case SceneIndex.HighScoreScreen:
                GameStateManager.instance.BeginNewState(GameState.HighScoreTable);
           
                break;
        }
    }

    public void OnFadeComplete()
    {
        isFading = false;
        LoadingScreen.instance.OnFadeComplete -= OnFadeComplete;
    }
}
