using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToDestroy : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;
    private void OnEnable()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
