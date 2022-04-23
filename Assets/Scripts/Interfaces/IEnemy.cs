using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void SetTarget(Transform target);

    public void SetTicketManager(EnemyTicketManager ticketManager);
}
