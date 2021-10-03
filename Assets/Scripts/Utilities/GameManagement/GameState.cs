using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{

    Init,
    GoToTitle,
    TitleScreen,
    BeginLevelLoad,
    GameLevelLoaded,

    GameSetUp,
    LevelGenerated,
    PlayerSpawned,
    GamePaused,
    GameRunning,
    PlayerDied,
    HighScoreTable


};