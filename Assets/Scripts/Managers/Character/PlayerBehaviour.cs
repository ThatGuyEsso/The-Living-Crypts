using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    [SerializeField] private IInitialisable[] ComponentsToInit;
    [SerializeField] private GameObject[] ManagerToInit;


    public void Init()
    {
        ComponentsToInit = GetComponents<IInitialisable>();

        foreach(IInitialisable comp in ComponentsToInit)
        {

            comp.Init();
        }

        if (ManagerToInit.Length > 0)
        {
            foreach (GameObject comp in ManagerToInit)
            {
                IInitialisable initComp = comp.GetComponent<IInitialisable>();
                if (initComp!=null)
                {
                    initComp.Init();
                }
            
            }
        }
    }
}
