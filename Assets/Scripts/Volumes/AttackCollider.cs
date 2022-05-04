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


        if (other.transform.root.gameObject != _owner)
        {
            IDamage damage = other.transform.root.GetComponent<IDamage>();
            if (damage == null)
            {
                return;
            }
            Iteam ownerTeam = _owner.GetComponent<Iteam>();
            if (ownerTeam == null)
            {
                return;
            }
            Iteam otherTeam = other.transform.root.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                if (damage != null)
                {
                    IAttacker attacker = _owner.GetComponent<IAttacker>();
                    if (attacker != null)
                    {
                        AttackData aDAta = attacker.GetAttackData();
                        float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                        float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                        damage.OnDamage(dmg, _owner.transform.forward,
                                kBack, _owner, other.ClosestPoint(transform.position));

                    }



                }

            }
            else if (!ownerTeam.IsOnTeam(otherTeam.GetTeam()))
            {
                if (damage != null)
                {
                    IAttacker attacker = _owner.GetComponent<IAttacker>();
                    if (attacker != null)
                    {
                        AttackData aDAta = attacker.GetAttackData();
                        float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                        float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                        damage.OnDamage(dmg, _owner.transform.forward,
                                kBack, _owner, other.ClosestPoint(transform.position));

                        OnAttackPerfomed?.Invoke();
                    }



                }
            }



            OnObjectHit?.Invoke(other.gameObject);
            OnEnemyHit?.Invoke(_owner);
            OnAttackPerfomed?.Invoke();

        }
;
    }


    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (!IsEnabled) return;


        if (other.transform.root.gameObject != _owner)
        {
            IDamage damage = other.transform.root.GetComponent<IDamage>();
            if (damage == null)
            {
                return;
            }
            Iteam ownerTeam = _owner.GetComponent<Iteam>();
            if(ownerTeam == null)
            {
                return;
            }
            Iteam otherTeam = other.transform.root.GetComponentInParent<Iteam>();
            if (otherTeam == null)
            {
                if (damage != null)
                {
                    IAttacker attacker = _owner.GetComponent<IAttacker>();
                    if (attacker != null)
                    {
                        AttackData aDAta = attacker.GetAttackData();
                        float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                        float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                        damage.OnDamage(dmg, _owner.transform.forward,
                                kBack, _owner, (transform.position));
                       
                    }



                }
             
            }else if (!ownerTeam.IsOnTeam(otherTeam.GetTeam()))
            {
                if (damage != null)
                {
                    IAttacker attacker = _owner.GetComponent<IAttacker>();
                    if (attacker != null)
                    {
                        AttackData aDAta = attacker.GetAttackData();
                        float dmg = Random.Range(aDAta.MinDamage, aDAta.MaxDamage);
                        float kBack = Random.Range(aDAta.MinKnockBack, aDAta.MaxKnockBack);
                        damage.OnDamage(dmg, _owner.transform.forward,
                                kBack, _owner, (transform.position));
          
                        OnAttackPerfomed?.Invoke();
                    }



                }
           
            }

            OnObjectHit?.Invoke(other.gameObject);
            OnEnemyHit?.Invoke(_owner);
            OnAttackPerfomed?.Invoke();



        }
    }

}
