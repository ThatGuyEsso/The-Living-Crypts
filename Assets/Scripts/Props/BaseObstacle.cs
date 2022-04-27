using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour, Iteam
{
    [SerializeField] private float MaxSize, MinSize;

    private CharacterHealthManager _healthManager;

    public Team GetTeam()
    {
        return Team.Neutral;
    }

    public void Init()
    {

    }

    public bool IsOnTeam(Team team)
    {
        return team == Team.Neutral;
    }
}
