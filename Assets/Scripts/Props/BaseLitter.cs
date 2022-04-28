using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLitter : MonoBehaviour, IInitialisable
{
    [SerializeField] private RandomSizeInRange Sizer;
    [SerializeField] private bool UseRandomSize;
    [SerializeField] private bool UseRandomRotation = true;

    private Vector3 _initialSize;

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

    protected void OnDisable()
    {
        transform.localScale = _initialSize;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }


}
