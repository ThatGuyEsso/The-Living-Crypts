using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    [SerializeField] private float LifeTime;
    [SerializeField] private float TickRate;
    [SerializeField] private float MinDamage,MaxDamage;
    [SerializeField] private Team Team;

    private GameObject _owner;
    private bool _isAlive;
    private float _timeToDamage;
    private float _currentLifeTime;
    private List<GameObject> _objectsToAttack = new List<GameObject>();

    public void Init(GameObject owner)
    {
        _owner = owner;
        if (owner)
        {
            Iteam ourTeam = _owner.GetComponent<Iteam>();
            Team = ourTeam.GetTeam();
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

        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Iteam otherTeam = other.GetComponent<Iteam>();

        if (otherTeam == null)
        {
            return;
        }
        Iteam ourTeam = _owner.GetComponent<Iteam>();

        if (ourTeam == null || !otherTeam.IsOnTeam(Team))
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
