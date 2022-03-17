using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct RoomInfo
{
    public SceneIndex _roomSceneIndex;
    public int _weight;
    public Direction _roomDirection;
    public RoomType _roomType;



    public RoomInfo(SceneIndex roomSceneIndex,int weight, Direction roomDirection, RoomType roomType)
    {
        _roomSceneIndex = roomSceneIndex;
        _weight = weight;
        _roomDirection = roomDirection;
        _roomType = roomType;
    }
}
