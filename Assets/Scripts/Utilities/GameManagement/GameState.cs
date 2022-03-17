using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{

    Init,
    GoToTitle,
    TitleScreen,
    BeginLevelLoad,

    GoToGameScene,
    GameSceneLoadComplete,
    GameSceneSetUpComplete,
    LevelGenerated,
    GamePaused,
    GameRunning,


};