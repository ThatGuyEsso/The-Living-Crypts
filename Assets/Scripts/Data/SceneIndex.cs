using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneIndex
{
    //RootScenes
    RootScene = 0,
    GameRootScene = 1,

    //Menu Scenes       
    TitleScreen = 2,

    //Entrance
    SpawnRoom = 3,
    HubRoom = 4,
    EntranceCorridor = 5,

    //Component Scene
    PlayerScene = 6,
    //UI
    HUDscene = 7,
    PauseScene = 8,


    //Corridors

    E_Corridor6X10 = 9,
    N_Corridor6X10 = 10,
    S_Corridor6X10 =  11,
    W_Corridor6X10 = 12,

    //Rooms
    E_C_4D_8x8 = 15,
    N_C_4D_8x8 = 16,
    S_C_4D_8x8 = 17,
    W_C_4D_8x8 = 18
};