using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, Controls.IUIActions
{
    [SerializeField] private GameObject PauseScreen;
    private GameManager GM;
    private Controls _input;
    private void Awake()
    {
        if (PauseScreen && PauseScreen.activeInHierarchy)
        {
            PauseScreen.SetActive(false);
        }
        _input = new Controls();
        _input.UI.SetCallbacks(this);
        _input.Enable();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
     
    }
    public void OnSubmit(InputAction.CallbackContext context)
    {
       // throw new System.NotImplementedException();
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance)
        {
            return;
        }

        if (context.performed)
        {
            OnTogglePause();
        }
    }

  
    public void OnTogglePause()
    {
        if (!GameStateManager.instance)
        {
            return;
        }
        if (GameStateManager.instance.CurrentState == GameState.GameRunning)
        {
            GameStateManager.instance.BeginNewState(GameState.GamePaused);
            if (PauseScreen)
            {
                PauseScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if (GameStateManager.instance.CurrentState == GameState.GamePaused)
        {
            GameStateManager.instance.BeginNewState(GameState.GameRunning);
            if (PauseScreen)
            {
                PauseScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void Resume()
    {
        OnTogglePause();
    }
   
    public void OnRestart()
    {
        if (!GM)
        {
            GM = GetGameManager();
        }
        OnTogglePause();
        if (GM)
        {
            GM.BeginNewGameplayEvent(GameplayEvents.Restart);
        }
    }
    public void GoToTitleScreen()
    {
        if (!GM)
        {
            GM = GetGameManager();
        }
        OnTogglePause();
        if (GM)
        {
            GM.BeginNewGameplayEvent(GameplayEvents.ExitLevel);
        }
    }

    public void QuitGame()
    {
        if (!GM)
        {
            GM = GetGameManager();
        }
        OnTogglePause();
        if (GM)
        {
            GM.BeginNewGameplayEvent(GameplayEvents.Quit);
        }
    }

    public GameManager GetGameManager()
    {
        if (!GameStateManager.instance)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager)
        {
            return null;
        }
        else
        {
            return GameStateManager.instance.GameManager;
        }
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }
}
