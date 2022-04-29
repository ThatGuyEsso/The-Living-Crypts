using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLitter : MonoBehaviour, IInitialisable
{
    [SerializeField] private RandomSizeInRange Sizer;
    [SerializeField] private bool UseRandomSize;
    [SerializeField] private bool UseRandomRotation = true;

    private Vector3 _initialSize;
    protected GameManager _gameManager;
    public void Init()
    {
        _initialSize = transform.localScale;
        if (UseRandomSize && Sizer)
        {
            _initialSize = transform.localScale;
            Sizer.SetRandomSize();
        }
        if (UseRandomRotation)
        {
            float rot = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, rot, 0f);
        }


    }

    virtual protected void EvaluateNewGameplayEvent(GameplayEvents newEvent)
    {
        switch (newEvent)
        {

            case GameplayEvents.PlayerRespawnBegun:
                if (ObjectPoolManager.instance)
                {
                    if (gameObject)
                    {
                        ObjectPoolManager.Recycle(gameObject);
                    }
                    else
                    {
                        if (gameObject)
                        {
                            Destroy(gameObject);

                        }
                    }

                }
                break;

            case GameplayEvents.Restart:
                if (ObjectPoolManager.instance)
                {
                    if (gameObject)
                    {
                        ObjectPoolManager.Recycle(gameObject);
                    }
                    else
                    {
                        if (gameObject)
                        {
                            Destroy(gameObject);

                        }
                    }

                }
                break;
            case GameplayEvents.ExitLevel:
                if (ObjectPoolManager.instance)
                {
                    if (gameObject)
                    {
                        ObjectPoolManager.Recycle(gameObject);
                    }
                    else
                    {
                        if (gameObject)
                        {
                            Destroy(gameObject);

                        }
                    }

                }
                break;

        }
    }
    public virtual void OnEnable()
    {
       

        if (!_gameManager)
        {
            if (GameStateManager.instance && GameStateManager.instance.GameManager)
            {
                _gameManager = GameStateManager.instance.GameManager;
            }
        }


        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent += EvaluateNewGameplayEvent;
        }



    }
    protected void OnDisable()
    {
        transform.localScale = _initialSize;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvaluateNewGameplayEvent;
        }
    }


}
