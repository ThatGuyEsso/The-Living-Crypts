using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossAbilityData
{
    [Header("Ability Prefab")]
    public BaseBossAbility AbilityPrefab;

    [Header("Ability Settings")]
    [Tooltip("Priorities can't be skipped by boss or other abilities")]
    public bool IsPriority;
    public bool IsTransitionAbility;
    [Tooltip("Incase Player us close this ability can be used")]
    public bool IsCloseRange;
    [Tooltip("if range <0 attack is global")]
    public float AttackRange;
    public float MaxAttackDamage;
    public float MinAttackDamage;
    public float MaxKnockBack;
    public float MinKnockBack;
    public float MaxTimeToAttempt;
    [Tooltip("Cooldown before boss can execute next attack")]
    public float AttackCooldown;
    [Tooltip("Cooldown of this ability")]
    public float AbilityCooldown;
}

