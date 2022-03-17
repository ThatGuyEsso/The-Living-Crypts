using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitlescreenMenu : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
    public void OnPlay()
    {
        if (GameStateManager.instance)
        {
            graphicRaycaster.enabled = false;
            GameStateManager.instance.BeginNewState(GameState.GoToGameScene);
        }
    }


}
