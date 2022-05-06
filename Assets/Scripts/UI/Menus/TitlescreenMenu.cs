using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitlescreenMenu : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Credits;
    [SerializeField] private string[] TitleAmbienceSFX;
    [SerializeField] private string TransitionSFXName;
    private AudioManager _audioManager;
    private List<AudioPlayer> sceneAudioPlayers = new List<AudioPlayer>();

    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        PlayAmbience();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (Credits)
        {
            Credits.SetActive(false);
        }
    }


    public void PlayAmbience()
    {
        if(TitleAmbienceSFX.Length == 0) {
            return;

        }
        if (!_audioManager)
        {
            if (!GameStateManager.instance.AudioManager)
            {
                return;
            }
            _audioManager = GameStateManager.instance.AudioManager;
    
        }

        foreach (string soundName in TitleAmbienceSFX)
        {
           AudioPlayer aPlayer= _audioManager.PlayUISound(soundName, Vector3.zero);
            if (aPlayer)
            {
                sceneAudioPlayers.Add(aPlayer);
            }
        }
    }

    private void StopAmbience()
    {
        if (sceneAudioPlayers.Count == 0)
        {
            return;

        }
        foreach(AudioPlayer audio in sceneAudioPlayers)
        {
            audio.BeginFadeOut();
        }
 
      
    }
    public void OnPlay()
    {
        if (GameStateManager.instance)
        {
            graphicRaycaster.enabled = false;
            GameStateManager.instance.BeginNewState(GameState.GoToGameScene);
            StopAmbience();
        }
    }

    public void OnCredits()
    {
        if (Credits)
        {
            Credits.SetActive(true);
        }
        else
        {
            return;
        }

        if (MainMenu)
        {
            MainMenu.SetActive(false);
        }
    }
    public void OnBackFromCredits()
    {
        if (Credits)
        {
            Credits.SetActive(false);
        }
        else
        {
            return;
        }

        if (MainMenu)
        {
            MainMenu.SetActive(true);
        }
    }

}
