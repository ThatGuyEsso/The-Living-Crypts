using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVFX : MonoBehaviour
{
    [SerializeField] private LineManager LineManager;


    [SerializeField] private float MaxLifeTime, MinLifeTime;


    public void Init(List<Vector3> points)
    {
        if (LineManager)
        {
            LineManager.Init();
            LineManager.DrawLinePositions(points);
            float randTime = Random.Range(MaxLifeTime, MinLifeTime);
            StopAllCoroutines();

            StartCoroutine(WaitToClear(randTime));

        }
        else
        {
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

    private IEnumerator WaitToClear(float time)
    {
        yield return new WaitForSeconds(time);

        {
            if (LineManager)
            {
                LineManager.ClearLine();

            }
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
