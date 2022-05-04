using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Default,
    Fire,
    Poison
};

public class HazardVolume : MonoBehaviour
{
    [SerializeField] private float LifeTime;
    [SerializeField] private float TickRate;
    [SerializeField] private float MinDamage,MaxDamage;
    [SerializeField] private Team Team;
    [SerializeField] private bool EnableOnAwake;

    private GameObject _owner;
    private bool _isAlive;
    [SerializeField] private bool _isPersistent=false;
    private float _timeToDamage;
    private float _currentLifeTime;
    private List<GameObject> _objectsToAttack = new List<GameObject>();

    private void Awake()
    {
        if (EnableOnAwake)
        {
            _timeToDamage = TickRate;
            _currentLifeTime = LifeTime;
        }
    }
    public void Init(GameObject owner)
    {
        _owner = owner;
        if (owner)
        {
            Iteam ourTeam = _owner.GetComponent<Iteam>();
            if (ourTeam != null)
            {
                Team = ourTeam.GetTeam();
            }
        
        }
 
        _isAlive = true;
        _timeToDamage = TickRate;
        _currentLifeTime = LifeTime;
    }


    private void Update()
    {
        if (_isAlive)
        {

            if (_currentLifeTime <=0f) {
                _isAlive = false;
                RemoveHazard();
                return;
            }
            else
            {
                _currentLifeTime -= Time.deltaTime;
            }
            if (_timeToDamage <= 0f)
            {
                ApplyDamage();
                _timeToDamage = TickRate;
            }
            else
            {
                _timeToDamage -= Time.deltaTime;
            }

        }else if (_isPersistent)
        {
            if (_timeToDamage <= 0f)
            {
                ApplyDamage();
                _timeToDamage = TickRate;
            }
            else
            {
                _timeToDamage -= Time.deltaTime;
            }

        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Iteam otherTeam = other.GetComponent<Iteam>();
        if (otherTeam == null)
        {
            otherTeam = other.gameObject.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                return;

            }

        }
        if (!otherTeam.IsOnTeam(Team))
        {
            if (_objectsToAttack.Count == 0)
            {
                _objectsToAttack.Add(other.gameObject);
                ApplyDamage();
                _timeToDamage = TickRate;
            }
            else
            {
                _objectsToAttack.Add(other.gameObject);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (_objectsToAttack.Count>0)
        {
            if (_objectsToAttack.Contains(other.gameObject))
            {
                _objectsToAttack.Remove(other.gameObject);
            }
         
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Iteam otherTeam = other.gameObject.GetComponent<Iteam>();

        if (otherTeam == null)
        {
            otherTeam = other.gameObject.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                return;

            }

        }
        if (!otherTeam.IsOnTeam(Team))
        {
            if (_objectsToAttack.Count == 0)
            {
                _objectsToAttack.Add(other.gameObject);
                ApplyDamage();
                _timeToDamage = TickRate;
            }
            else
            {
                _objectsToAttack.Add(other.gameObject);
            }
        }
    }


    private void OnCollisionExit(Collision other)
    {
        if (_objectsToAttack.Count > 0)
        {
            if (_objectsToAttack.Contains(other.gameObject))
            {
                _objectsToAttack.Remove(other.gameObject);
            }

        }
    }
    public void ApplyDamage()
    {
        if (_objectsToAttack.Count == 0)
        {
            return;
        }


        foreach(GameObject toDamage in _objectsToAttack)
        {
            if (toDamage)
            {
                IDamage damageable = toDamage.GetComponent<IDamage>();
                if (damageable != null)
                {
                    float dmg = Random.Range(MinDamage, MaxDamage);
                    damageable.OnDamage(dmg, Vector3.zero, 0f, _owner, toDamage.transform.position);
                }
            }
         
      
        }
    }
    public void RemoveHazard()
    {
        if (_objectsToAttack.Count > 0)
        {
            _objectsToAttack.Clear();
        }
            if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }
}
