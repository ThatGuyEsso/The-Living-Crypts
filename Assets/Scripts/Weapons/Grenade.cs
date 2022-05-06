using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float FuseTime;
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private AudioManager AM;
    [SerializeField] private string ExplosionSFX;
    [SerializeField] private CamShakeSetting CameraShake;
    [SerializeField] private ExplosionData ExplosionData;

    private GameObject _owner;

    public GameObject Owner { get { return _owner; } set { _owner = value; } }
    public AudioManager AudioManager
    {
        set
        {
            AM = value;

        }
    }
    private void OnEnable()
    {
        Invoke("SpawnExplosion", FuseTime);
    }

    public void SpawnExplosion()
    {
        ExplosionData._owner = Owner;
        if (ObjectPoolManager.instance)
        {
           
            IExplosion explosion = ObjectPoolManager.Spawn
                (ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<IExplosion>();
            if (explosion != null)
            {
          
                explosion.InitExplosion(ExplosionData);
            }
      
        if (CamShake.instance)
            {
                CamShake.instance.DoScreenShake(CameraShake);
            }
            if (AM)
            {
                AM.PlayThroughAudioPlayer(ExplosionSFX, transform.position);
            }
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            IExplosion explosion = ObjectPoolManager.Spawn
                (ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<IExplosion>();
            if (explosion != null)
            {
                explosion.InitExplosion(ExplosionData);
            }
            if (CamShake.instance)
            {
                CamShake.instance.DoScreenShake(CameraShake);
            }
            if (AM)
            {
                AM.PlayThroughAudioPlayer(ExplosionSFX, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
