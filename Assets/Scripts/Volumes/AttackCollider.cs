using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private GameObject _owner;

    public System.Action<GameObject> OnEnemyHit;
    private void Awake()
    {
        _owner = transform.root.gameObject;
        Debug.Log(_owner);
    }

    public void Init()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     
    }
    private void OnCollisionEnter(Collision other)
    {

    }
}
