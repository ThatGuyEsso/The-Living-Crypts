using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour, Iteam , IInitialisable
{
    [SerializeField] private bool InDebug ;
    [SerializeField] private float MaxSize, MinSize;
    [SerializeField] private bool UseRandomSize;
    [SerializeField] private bool UseRandomRotation=true;
    [SerializeField] private LayerMask OverlapLayers;

    private CharacterHealthManager _healthManager;
    private Vector3 _initialSize;
    private ObjectBounds _bounds;
    private void Awake()
    {
        if (InDebug)
        {
            Init();
        }
    }
    public void Init()
    {
        _initialSize = transform.localScale;
        if (UseRandomSize)
        {
            _initialSize = transform.localScale;
            float scale = Random.Range(MinSize, MaxSize);
            transform.localScale = Vector3.one * scale;
        }
        if (UseRandomRotation)
        {
            float rot = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, rot, 0f);
        }
        if (!_bounds)
        {
            _bounds = GetComponent<ObjectBounds>();
        }
        if (!_healthManager)
        {
            _healthManager = GetComponent<CharacterHealthManager>();
        }
        if (_healthManager)
        {
            _healthManager.Init();
            _healthManager.OnHurt += OnHurt;
            _healthManager.OnDie += OnKilled;
        }

        if (IsOverlapping(OverlapLayers)){
            if (ObjectPoolManager.instance)
            {
                if (gameObject)
                {
                    ObjectPoolManager.Recycle(gameObject);
                }
            }
            else
            {

                Destroy(gameObject);
            }
        }
      
    }

    public bool IsOverlapping(LayerMask overLapLayers)
    {

        if (!_bounds)
        {
            return false;
        }
        RaycastHit[] hits;


        hits = Physics.BoxCastAll(transform.position + _bounds.GetOffset() + Vector3.up * -1f,
            _bounds.GetHalfExtents(), Vector3.up, Quaternion.identity, _bounds.GetHalfExtents().y / 2f, overLapLayers);


        //Debug.Log(transform.parent.name);
        if (hits.Length > 0)
        {

            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.transform.root)
                {

                    if (hit.collider.transform.root.gameObject != gameObject)
                    {

                        return true;
                    }
                }
                else
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        return true;
                    }
                }

            }
        }



        return false;
    }

    public void OnKilled()
    {
        if (ObjectPoolManager.instance)
        {
            if (gameObject)
            {
                ObjectPoolManager.Recycle(gameObject);
            }else
            {
                if (gameObject)
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
        transform.localScale = _initialSize;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
