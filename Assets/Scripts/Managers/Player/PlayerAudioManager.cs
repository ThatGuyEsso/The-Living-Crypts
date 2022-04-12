using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("SFX")]

    [SerializeField] private string PlayerHitSFX, PlayerDashSFX, PlayerFootStepSFX, PlayerHeartBeatSFX;
    [Header("SFX Settings")]
    [SerializeField] private float MaxTimeBetweenFootSteps;
    [SerializeField] private float MinHealthToPlayHeartBeatSFX;
    //Player Components 
    private FPSMovement _movement;
    private FPSDash _dash;
    private CharacterHealthManager _health;

    //Audio 
    [SerializeField] private AudioManager AM;
    private AudioPlayer _closeToDeathAudioPlayer;
    bool _isWalking;
    float _currentTimeBtwnFootSteps =0f;

    private void Awake()
    {
        _movement = GetComponent<FPSMovement>();

        if (_movement)
        {
            _movement.OnWalk += OnWalking;
            _movement.OnStop += OnStopWalking;
        }
        _dash = GetComponent<FPSDash>();
        if (_dash)
        {
            _dash.OnBeginDash += OnDash;
        }
        _health = GetComponent<CharacterHealthManager>();
        if (_health)
        {
            _health.OnHurt += OnHurt;
            _health.OnHealthUpdated += EvaluateNewHealth;
        }
    }

    private void Update()
    {
        if (_isWalking)
        {
            if(_currentTimeBtwnFootSteps <= 0)
            {
                OnPlayerStep();
                _currentTimeBtwnFootSteps = MaxTimeBetweenFootSteps;
            }
            else
            {
                _currentTimeBtwnFootSteps -= Time.deltaTime;
            }
        }
    }

    public void OnWalking()
    {
        _isWalking = true;
    }

    public void OnStopWalking()
    {
        _isWalking = false;
    }

    public void OnPlayerCloseToDeath()
    {
        if (!_closeToDeathAudioPlayer)
        {
            if (!AM)
            {
                AM = AudioManager;
            }
            if (!AM)
            {
                return;
            }
            _closeToDeathAudioPlayer = AM.PlayThroughAudioPlayer(PlayerHeartBeatSFX, transform.position);
        }
        else
        {

            _closeToDeathAudioPlayer.Play();
        }



    }

    public void OnPlayerHealthy()
    {
        if (_closeToDeathAudioPlayer)
        {
            _closeToDeathAudioPlayer.BeginFadeOut();
            _closeToDeathAudioPlayer = null;
        }
  
    }
    public void OnPlayerStep()
    {
        if (!AM)
        {
            AM = AudioManager;
        }
        if (!AM)
        {
            return;
        }

        AM.PlayThroughAudioPlayer(PlayerFootStepSFX, transform.position,true);
    }

    public void OnHurt()
    {
        if (!AM)
        {
            AM = AudioManager;
        }
        if (!AM)
        {
            return;
        }

        AM.PlayThroughAudioPlayer(PlayerHitSFX, transform.position,true);
    }

    public void OnDash()
    {
        if (!AM)
        {
            AM = AudioManager;
        }
        if (!AM)
        {
            return;
        }

        AM.PlayThroughAudioPlayer(PlayerDashSFX, transform.position,true);
    }

    public AudioManager AudioManager
    {
        get
        {
            if (AM)
            {
                return AM;
            }
            else
            {
                if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
                {
                    return null;
                }
                else
                {
                    return GameStateManager.instance.AudioManager;
                }
            }
        }
    }

    public void EvaluateNewHealth(float newHealth)
    {
        if(newHealth <= MinHealthToPlayHeartBeatSFX)
        {
            if(!_closeToDeathAudioPlayer|| !_closeToDeathAudioPlayer.IsPlaying())
            {
                OnPlayerCloseToDeath();
            }
        }
        else
        {
            if (_closeToDeathAudioPlayer && _closeToDeathAudioPlayer.IsPlaying())
            {
                OnPlayerHealthy();
            }
        }
    }
    private void OnDisable()
    {


        if (_movement)
        {
            _movement.OnWalk -= OnWalking;
            _movement.OnStop -= OnStopWalking;
        }

        if (_dash)
        {
            _dash.OnBeginDash -= OnDash;
        }

        if (_health)
        {
            _health.OnHurt -= OnHurt;
            _health.OnHealthUpdated -= EvaluateNewHealth;
        }
    }

    private void OnDestroy()
    {
       

        if (_movement)
        {
            _movement.OnWalk -= OnWalking;
            _movement.OnStop -= OnStopWalking;
        }
    
        if (_dash)
        {
            _dash.OnBeginDash -= OnDash;
        }

        if (_health)
        {
            _health.OnHurt -= OnHurt;
            _health.OnHealthUpdated -= EvaluateNewHealth;
        }
    }
}
