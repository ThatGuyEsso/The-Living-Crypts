using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour, IManager, IInitialisable
{
    public static SceneIndex currentScene;
    private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    private bool isFading;
    LoadingScreen _loadingScreen;


    public System.Action OnSceneAdded;
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
        if (GameStateManager.instance)
        {
            GameStateManager.instance.SceneManager = this;
            BindToGameStateManager();
        }
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

    public void AddNewScene(SceneIndex sceneIndex)
    {
        StopAllCoroutines();
        StartCoroutine(AddScene(sceneIndex));
    }
    private IEnumerator LoadLevel(SceneIndex newLevel)
    {
        if (!ValidateHasLoadingScreen())
        {

            currentScene = newLevel;
            GameStateManager.instance.BeginNewState(GameState.GameSceneLoadComplete);
            Debug.LogError("No Loading screen");

            yield break;
        }
   
        if (!_loadingScreen)
        {
            _loadingScreen = GameStateManager.instance.LoadingScreenManager;
            if (!_loadingScreen)
            {
                //clear scens loading
                sceneLoading.Clear();
                currentScene = newLevel;
                GameStateManager.instance.BeginNewState(GameState.GameSceneLoadComplete);
                yield return null;
            }
        }
       
        isFading = true;
        _loadingScreen.OnFadeComplete += OnFadeComplete;
        _loadingScreen.BeginFadeIn();
        while (isFading)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)currentScene));
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)newLevel, LoadSceneMode.Additive));
        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        currentScene = newLevel;
        EvaluateSceneLoaded(currentScene);


    }
    private IEnumerator AddScene(SceneIndex newScene)
    {
    
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive));
        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        OnSceneAdded?.Invoke();
        EvaluateSceneLoaded(newScene);


    }
    private IEnumerator LoadMenuScreen(SceneIndex menuSceneIndex)
    {
        if (!ValidateHasLoadingScreen())
        {

            currentScene = menuSceneIndex;
            GameStateManager.instance.BeginNewState(GameState.GameSceneLoadComplete);
            Debug.LogError("No Loading screen");

            yield break;
        }
        Scene[] loadedScenes = GetAllActiveScenes();

        if (!_loadingScreen.IsLoadingScreenOn())
        {
            isFading = true;
            _loadingScreen.OnFadeComplete += OnFadeComplete;
            _loadingScreen.BeginFadeIn();
            while (isFading)
            {
                yield return null;
            }
        }
        if (WeaponManager._instance)
        {
            WeaponManager._instance.UnequipWeapon();
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
            case SceneIndex.GameRootScene:
                GameStateManager.instance.BeginNewState(GameState.GameSceneLoadComplete);

                break;

        }
    }

    public bool ValidateHasLoadingScreen()
    {
        if (!_loadingScreen)
        {
            _loadingScreen = GameStateManager.instance.LoadingScreenManager;
            if (!_loadingScreen)
            {
              
                return false;
            }
        }
        return true;
    }
    public void OnFadeComplete()
    {
        isFading = false;
        _loadingScreen.OnFadeComplete -= OnFadeComplete;
    }
}
