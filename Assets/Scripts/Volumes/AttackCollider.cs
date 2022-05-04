using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private bool EnableOnAwake=true;
    [SerializeField] private bool CanBeTrigger = true;
    public System.Action<GameObject> OnEnemyHit;
    public System.Action<GameObject> OnObjectHit;
    public System.Action OnAttackPerfomed;
    Collider _attackCollider;
    public bool IsEnabled;
    Iteam _ownerTeam;
    
    public void ToggleColiider(bool isEnabled)
    {
        IsEnabled = isEnabled;
        if (CanBeTrigger)
        {
            _attackCollider.isTrigger = isEnabled;
        }
    
    }
    public void Awake()
    {
        if (EnableOnAwake)
        {
            if (!_owner)
            {
                _owner = transform.root.gameObject;
            }
        }
        _attackCollider = GetComponent<Collider>();

    }
 
    public void OnTriggerEnter(Collider other)
    {
        if (!IsEnabled) return;

        Iteam otherTeam = other.GetComponent<Iteam>();
        if (otherTeam == null)
        {
            otherTeam = other.gameObject.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                OnObjectHit?.Invoke(other.gameObject);
                return;

            }

        }
        if (_owner && _ownerTeam==null)
        {
            _ownerTeam = _owner.GetComponent<Iteam>();

          
        }
        if(_ownerTeam == null || !_ownerTeam.IsOnTeam(otherTeam.GetTeam()))
        {
            IDamage damage = other.GetComponent<IDamage>();
            if (damage == null)
            {
                damage = other.GetComponentInParent<IDamage>();
                if (damage == null)
                {
                    OnObjectHit?.Invoke(other.gameObject);
                    return;

                }
            }

            IAttacker attacker = _owner.GetComponent<IAttacker>();
            if (attacker != null)
            {
                AttackData aDAta = attacker.GetAttackData();
                float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                damage.OnDamage(dmg, _owner.transform.forward,
                        kBack, _owner, other.ClosestPoint(transform.position));

                OnAttackPerfomed?.Invoke();

                OnEnemyHit?.Invoke(other.gameObject);
            }
        }

    }


    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }
    
    public void OnCollisionEnter(Collision other)
    {


        if (!IsEnabled) return;

        Iteam otherTeam = other.gameObject.GetComponent<Iteam>();
        if (otherTeam == null)
        {
            otherTeam = other.gameObject.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                OnObjectHit?.Invoke(other.gameObject);
                return;

            }

        }
        if (_owner && _ownerTeam == null)
        {
            _ownerTeam = _owner.GetComponent<Iteam>();


        }
        if (_ownerTeam == null || !_ownerTeam.IsOnTeam(otherTeam.GetTeam()))
        {
            IDamage damage = other.gameObject.GetComponent<IDamage>();
            if (damage == null)
            {
                damage = other.gameObject.GetComponentInParent<IDamage>();
                if (damage == null)
                {
                    OnObjectHit?.Invoke(other.gameObject);
                    return;

                }
            }

            IAttacker attacker = _owner.GetComponent<IAttacker>();
            if (attacker != null)
            {
                AttackData aDAta = attacker.GetAttackData();
                float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                damage.OnDamage(dmg, _owner.transform.forward,
                        kBack, _owner, other.transform.position);

                OnAttackPerfomed?.Invoke();

                OnEnemyHit?.Invoke(other.gameObject);
            }
        }
    }

}
