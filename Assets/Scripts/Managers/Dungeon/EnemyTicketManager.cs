using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTicketManager : MonoBehaviour
{
    [SerializeField] private int MaxTickets;

    private int _nCurrentTickets;



    public bool CanAttack()
    {
        if(_nCurrentTickets < MaxTickets)
        {
            _nCurrentTickets++;
            return true;
        }
        return false;   
    }

    public void TicketUsed()
    {
        _nCurrentTickets--;

        if(_nCurrentTickets < 0)
        {
            _nCurrentTickets = 0;
        }
    }
}
