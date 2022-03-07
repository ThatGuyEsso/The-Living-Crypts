using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iteam
{
    public Team GetTeam();
    public bool IsOnTeam(Team team);
}