using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EntryTrigger : MonoBehaviour
{
    [SerializeField] private string TargetTag;


    public System.Action<GameObject> OnTargetObjectEntered;
    public System.Action<GameObject> OnTargetObjectExit;

    public System.Action OnTargetEntered;
    public System.Action OnTargetExit;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            OnTargetObjectEntered?.Invoke(other.gameObject);
            OnTargetEntered?.Invoke();

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            OnTargetObjectExit?.Invoke(other.gameObject);
            OnTargetExit?.Invoke();

        }
    }
}
