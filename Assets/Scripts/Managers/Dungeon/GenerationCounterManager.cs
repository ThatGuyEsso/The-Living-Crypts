using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationCounterManager : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private Material OffMaterial, StandByMaterial, PendingMaterial, CompleteMaterial;
    [SerializeField] private Color StandByColour, PendingColour, CompleteColour;
    [SerializeField] private CamShakeSetting DungeonGenEventVFX;

    [Header("VFX Settings")]
    [SerializeField] private float LightIntensity;
    [SerializeField] private List<MeshRenderer> ProgressCounters;
    [SerializeField] private List<Light> ProgressCounterLights;
    [Header("SFX Settings")]
    [SerializeField] private string DungeonGenEventSFX;


    private int CurrentPorgressIndex = 0;
    private DungeonGenerator _dungeonGenerator;
    private GameManager GM;
    private AudioManager AM;
    private void Awake()
    {
        if(!GM)
        {
            if (GameStateManager.instance)
            {
                if (GameStateManager.instance.GameManager)
                {
                    GM = GameStateManager.instance.GameManager;
                    GameStateManager.instance.GameManager.OnNewGamplayEvent += EvaluateGameplayEvent;
                }
            }
        }
       


        if (ProgressCounters.Count == 0)
        {
            return;
        }


        for (int i = 0; i < ProgressCounters.Count; i++)
        {
            ProgressCounters[i].material = OffMaterial;
            Light light = ProgressCounters[i].GetComponentInChildren<Light>();
            if (light)
            {
                ProgressCounterLights.Add(light);
            }
        }


        if (ProgressCounterLights.Count==0) 
        {
            return;
        }

        foreach(Light light in ProgressCounterLights)
        {
            light.intensity = 0;
            light.enabled = false;
        }
    }

    public void EvaluateGameplayEvent(GameplayEvents newEvent)
    {
        switch (newEvent)
        {
            case GameplayEvents.WeaponSelected:
                OnStandby();
                break;
            case GameplayEvents.DungeonInvoked:
                OnPending();
                break;
            case GameplayEvents.DungeonBegunGenerating:
                ListenToDungeonGenerator();
                break;
            case GameplayEvents.GameComplete:
                break;
            case GameplayEvents.PlayerDied:
                break;
            case GameplayEvents.PlayerRespawned:
                OnOff();
                break;
        }
    }

    public void ListenToDungeonGenerator()
    {
        if (!_dungeonGenerator)
        {
            if (!GameStateManager.instance) return;
            if (!GameStateManager.instance.GameManager) return;
            if (!GameStateManager.instance.GameManager.GetGenerationManager()) return;
            _dungeonGenerator = GameStateManager.instance.GameManager.GetGenerationManager();
        }

        _dungeonGenerator.OnFirstBuilderDone += OnNewProgress;
        _dungeonGenerator.OnDungeonGenerationComplete += OnNewProgress;
        _dungeonGenerator.OnDungeonComplete += OnNewProgress;
        _dungeonGenerator.OnDungeonComplete += OnGenerationComplete;
    }

    public void OnGenerationComplete()
    {
        if (!_dungeonGenerator)
        {
            return;
        }

        _dungeonGenerator.OnFirstBuilderDone -= OnNewProgress;
        _dungeonGenerator.OnDungeonGenerationComplete -= OnNewProgress;
        _dungeonGenerator.OnDungeonComplete -= OnNewProgress;
        _dungeonGenerator.OnDungeonComplete -= OnGenerationComplete;
    }
    public void OnNewProgress()
    {
        if (!AM && GM)
        {
            AM = GM.AudioManager;
        }

        if (AM)
        {
            AM.PlayThroughAudioPlayer(DungeonGenEventSFX, transform.position, true);
        }
        if(CamShake.instance )
        {
            CamShake.instance.DoScreenShake(DungeonGenEventVFX);
        }
        ProgressCounters[CurrentPorgressIndex].material = CompleteMaterial;
        ProgressCounterLights[CurrentPorgressIndex].color = CompleteColour;

        CurrentPorgressIndex++;
        if(CurrentPorgressIndex>= ProgressCounters.Count)
        {
            CurrentPorgressIndex = 0;
        }


    }
    public void OnOff()
    {
        CurrentPorgressIndex = 0;
        foreach (MeshRenderer renderer in ProgressCounters)
        {

            renderer.material = OffMaterial;

        }
        foreach (Light light in ProgressCounterLights)
        {
            light.enabled = false;
            light.intensity = 0;
       
        }
    }

    public void OnStandby()
    {
        foreach (MeshRenderer renderer in ProgressCounters)
        {

            renderer.material = StandByMaterial;
          
        }
        foreach (Light light in ProgressCounterLights)
        {
            light.enabled = true;
            light.intensity = LightIntensity;
            light.color = StandByColour;
        }
    }

    public void OnPending()
    {
        foreach (MeshRenderer renderer in ProgressCounters)
        {

            renderer.material = PendingMaterial;

        }
        foreach (Light light in ProgressCounterLights)
        {
            light.enabled = true;
            light.intensity = LightIntensity;
            light.color = PendingColour;
        }
    }

}
