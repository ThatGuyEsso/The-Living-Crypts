using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DungeonGenData",menuName ="New Dungeon Gen Data")]
public class DungeonGenData : ScriptableObject
{
    public int _maxSteps;
    public int _minSteps;
    public int _maxWeight;
    public int _minWeight;
    public int _maxBuildAttempts;
    public int _minBuildAttempts;
    public int _numberOfCrawlers;
    public LayerMask _roomLayers;

    public float _percentageRepeatingCorridors;

    public List<RoomInfo> _roomPool = new List<RoomInfo>();


  
    public List<RoomInfo> GetRoomsOfTypeInDirection(Direction dir, RoomType type)
    {
        List<RoomInfo> availableRooms = _roomPool.FindAll(room => room._roomDirection == dir && room._roomType == type);

        return availableRooms;

       
    }


    public RoomInfo GetWeightedOfTypeInDirection(Direction dir, RoomType type)
    {
        RoomInfo room;
        List<RoomInfo> validRooms = GetRoomsOfTypeInDirection(dir, type);
        List<RoomInfo> availableRooms = new List<RoomInfo>();
        int weight = Random.Range(_minWeight, _maxWeight+1);


        foreach(RoomInfo info in validRooms)
        {
            if (info._weight >= weight) availableRooms.Add(info);
        }
        room = availableRooms[Random.Range(0, availableRooms.Count)];
        return room;
        
    }

    public RoomInfo GetWeightedOfTypeInDirection(Direction dir, RoomType type,int weight)
    {
        RoomInfo room;
        List<RoomInfo> validRooms = GetRoomsOfTypeInDirection(dir, type);
        List<RoomInfo> availableRooms = new List<RoomInfo>();


        foreach (RoomInfo info in validRooms)
        {
            if (info._weight >= weight) availableRooms.Add(info);
        }
        room = availableRooms[Random.Range(0, availableRooms.Count)];
        return room;

    }

}
