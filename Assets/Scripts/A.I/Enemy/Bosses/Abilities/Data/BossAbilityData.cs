using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossAbilityData
{
    [Header("Ability Prefab")]
    public BaseBossAbility AbilityPrefab;
    [Header("Animations")]
    public string attackAnimationName;

    [Header("Ability Settings")]
    [Tooltip("Priorities can't be skipped by boss or other abilities")]
    public bool IsPriority;
    [Tooltip("Incase Player us close this ability can be used")]
    public bool IsCloseRange;
    public float AttackRange;
    public float MaxAttackDamage;
    public float MinAttackDamage;
    [Tooltip("Cooldown before boss can execute next attack")]
    public float AttackCooldown;
    [Tooltip("Cooldown of this ability")]
    public float AbilityCooldown;
}

