using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDPrompt Prompt;




    public HUDPrompt PromptManager 
    { get
        {
            if (Prompt)
            {
                return Prompt;
            }
            else
            {
                Prompt= GetComponentInChildren<HUDPrompt>();
                return Prompt;
            }
        } 
    }
}
