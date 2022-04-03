using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationCounterManager : MonoBehaviour
{
    [SerializeField] private Material OffMaterial, StandByMaterial, PendingMaterial, CompleteMaterial;
    [SerializeField] private Color StandByColour, PendingColour, CompleteColour;
    [SerializeField] private float LightIntensity;
    [SerializeField] private List<MeshRenderer> ProgressCounters;
    [SerializeField] private List<Light> ProgressCounterLights;
    private int CurrentPorgressIndex = 0;
    private DungeonGenerator _dungeonGenerator;

    private void Awake()
    {
        if (GameStateManager.instance)
        {
            if (GameStateManager.instance.GameManager)
            {
                GameStateManager.instance.GameManager.OnNewGamplayEvent += EvaluateGameplayEvent;
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

        _dungeonGenerator.OnDungeonGenerationComplete += OnNewProgress;
        _dungeonGenerator.OnSpecialRoomsBuilt += OnNewProgress;
        _dungeonGenerator.OnDungeonComplete += OnNewProgress;
        _dungeonGenerator.OnDungeonComplete += OnGenerationComplete;
    }

    public void OnGenerationComplete()
    {
        if (!_dungeonGenerator)
        {
            return;
        }

        _dungeonGenerator.OnDungeonGenerationComplete -= OnNewProgress;
        _dungeonGenerator.OnSpecialRoomsBuilt -= OnNewProgress;
        _dungeonGenerator.OnDungeonComplete -= OnNewProgress;
        _dungeonGenerator.OnDungeonComplete -= OnGenerationComplete;
    }
    public void OnNewProgress()
    {
        ProgressCounters[CurrentPorgressIndex].material = CompleteMaterial;
        ProgressCounterLights[CurrentPorgressIndex].color = CompleteColour;

        CurrentPorgressIndex++;
        if(CurrentPorgressIndex>= ProgressCounters.Count)
        {
            CurrentPorgressIndex = 0;
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
