using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour, Iteam , IInitialisable
{
    [SerializeField] private float MaxSize, MinSize;
    [SerializeField] private bool UseRandomSize;
 
    private CharacterHealthManager _healthManager;
    private Vector3 _initialSize;


    public void Init()
    {
        _initialSize = transform.localScale;
        if (UseRandomSize)
        {
            _initialSize = transform.localScale;
            float scale = Random.Range(MinSize, MaxSize);
            transform.localScale = Vector3.one * MinSize;
        }

        if (!_healthManager)
        {
            _healthManager = GetComponent<CharacterHealthManager>();
        }
        if (_healthManager)
        {
            _healthManager.OnHurt += OnHurt;
            _healthManager.OnDie += OnKilled;
        }
    }


    public void OnKilled()
    {
        if (ObjectPoolManager.instance)
        {
            if (gameObject.activeInHierarchy)
            {
                ObjectPoolManager.Recycle(gameObject);
            }else
            {
                if (gameObject.activeInHierarchy)
                {
                   Destroy(gameObject);

                }
            }
  
        }
    }

    

    public void OnHurt()
    {

    }
    public Team GetTeam()
    {
        return Team.Neutral;
    }

    public bool IsOnTeam(Team team)
    {
        return team == Team.Neutral;
    }

    protected void OnDisable()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnHurt;
            _healthManager.OnDie -= OnKilled;
        }
    }

    protected void OnDestroy()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnHurt;
            _healthManager.OnDie -= OnKilled;
        }
        transform.localScale = _initialSize;
    }
}
