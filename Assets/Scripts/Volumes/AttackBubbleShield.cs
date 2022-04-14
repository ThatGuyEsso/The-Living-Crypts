using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBubbleShield : BaseShield
{
    [SerializeField] private string HitSFX, DespawnSFX;

    private AudioManager AM;
    protected override void DoShieldInteraction(GameObject other,Vector3 point)
    {
        PlaySFX(HitSFX, true);
        IProjectile projectile = other.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.BreakProjectile();
        }
        else
        {
            IDamage damage = other.GetComponent<IDamage>();
            if (damage!=null)
            {
                float dmg = Random.Range(_settings._minDamage, _settings._maxDamage);
                Vector3 kBackDir =( other.transform.position - transform.position).normalized;
                float kBackMag = Random.Range(_settings._minKnockBack, _settings._maxKnockBack);
                damage.OnDamage(dmg, kBackDir, kBackMag, _settings._owner, point);
            }
        }
    }
    protected override void DestroyShield()
    {
        PlaySFX(DespawnSFX, true);
        base.DestroyShield();
    }

    protected void PlaySFX(string SFX, bool randPitch)
    {
        if (!AM)
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return;
            }
            AM = GameStateManager.instance.AudioManager;
        }

        AM.PlayThroughAudioPlayer(SFX, transform.position, randPitch);
    }
}
