using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder :MonoBehaviour
{
    private DungeonGenData _genData;
  
    private Direction _ignoreDirection;
    //private GridDirection _currentDirection;
   [SerializeField] private Room _currentRoom;
    private Room _previosRoom;
    [SerializeField] private int _remainingSteps;
    private bool _isWalking;
    private Door _currentTargetDoor;
    private RoomInfo _currentRoomInfo;

    private RoomManager _roomManager;
    [SerializeField] private int _attemptsLeft;
    //Init builder
    public void InitBuilder(DungeonGenData data, Direction dir,Room startingRoom,int steps,RoomManager roomManager)
    {
        _genData = data;
        //_currentDirection = dir;
        _currentRoom = startingRoom;
        _remainingSteps = steps;
        _roomManager = roomManager;

        //remove opposite direction from start (No walking back)
        switch (dir)
        {
            case Direction.North:
          
                _ignoreDirection = Direction.South;
                break;
            case Direction.South:
          
                _ignoreDirection = Direction.North;
                break;
            case Direction.West:
                
                _ignoreDirection = Direction.East;
                break;
            case Direction.East:
              
                _ignoreDirection = Direction.West;
                break;
        }
    }



    public void TakeStep()
    {
        Debug.Log(gameObject.name + "Taking Step");
        _isWalking = true;
        _attemptsLeft = Random.Range(_genData._minBuildAttempts, _genData._maxBuildAttempts);
        //Decrement step
        _remainingSteps--;
        // if not in a room  end step;
        if (!_currentRoom) {
            Debug.Log(gameObject.name + "Invalid room");
            EndStep();
            return;
        } 

        //Get possible exit direction of current room;
        List<Direction> possibleDirections = _currentRoom.GetAvailableDirections();

        //Get all valid directions
        List<Direction> validDirections = new List<Direction>();
        foreach (Direction dir in possibleDirections)
        {
            if (dir != _ignoreDirection)
            {
                validDirections.Add(dir);
            }
        }

        //if there are no valid directions return
        if (validDirections.Count == 0)
        {
            Debug.Log(gameObject.name + "No valid direction");
            EndStep();
            return;
        }
        //Target Direction
        Direction direction = validDirections[Random.Range(0, validDirections.Count)];

        //Get door in target direction
        Door targetDoor = _currentRoom.GetDoorsInDirection(direction)
            [Random.Range(0, _currentRoom.GetDoorsInDirection(direction).Count)];

        //if not valid end step
        if (!targetDoor)
        {
            Debug.Log(gameObject.name + "Invalid target door");
            EndStep();
            return;
        }
        _currentTargetDoor = targetDoor;
        BuildRoom(_currentRoom.GetRoomInfo()._roomType, direction);
  
    }


    public void RetryStep()
    {
        Debug.Log(gameObject.name + "trying");
        _attemptsLeft--;

        // if not in a room  end step;
        if (!_currentRoom)
        {
            EndStep();
            return;
        }

        //Get possible exit direction of current room;
        List<Direction> possibleDirections = _currentRoom.GetAvailableDirections();

        //Get all valid directions
        List<Direction> validDirections = new List<Direction>();
        foreach (Direction dir in possibleDirections)
        {
            if (dir != _ignoreDirection)
            {
                validDirections.Add(dir);
            }
        }

        //if there are no valid directions return
        if (validDirections.Count == 0)
        {
            EndStep();
            return;
        }
        //Target Direction
        Direction direction = validDirections[Random.Range(0, validDirections.Count)];

        //Get door in target direction
        Door targetDoor = _currentRoom.GetDoorsInDirection(direction)
            [Random.Range(0, _currentRoom.GetDoorsInDirection(direction).Count)];

        //if not valid end step
        if (!targetDoor)
        {
            EndStep();
            return;
        }
        _currentTargetDoor = targetDoor;
        BuildRoom(_currentRoom.GetRoomInfo()._roomType, direction);

    }

    public void EndStep()
    {
       _isWalking = false;
        Debug.Log(gameObject.name + "finishing Step");
        if (_remainingSteps <= 0)
        {
            Debug.Log(gameObject.name + "Done Building");
            DungeonGenerator._instance.GenComplete();

        }
        else
        {
            Debug.Log(gameObject.name + "not done Building");
            DungeonGenerator._instance.EvaluateCanBuild();
        }
      
    }

  
 

    public bool CanWalk()
    {
        return _remainingSteps > 0;
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void BuildRoom( RoomType type, Direction direction)
    {
      
      
        if (!_currentTargetDoor.GetLinkedRoom())
        {
            Debug.Log(gameObject.name + "building new room");
            _roomManager.OnRoomLoadComplete += ValidateRoom;
       
            switch (type)
            {
                case RoomType.Crypt:

                    _currentRoomInfo = _genData.GetWeightedOfTypeInDirection(direction, RoomType.Corridor);
                    Debug.Log(gameObject.name + "building new corridor with info: " + _currentRoomInfo._roomSceneIndex);
                    if (_currentRoomInfo._weight != 0) {
                        Debug.Log(gameObject.name +  "Trying to spawn room");
                        _roomManager.BeginRoomLoad(_currentRoomInfo._roomSceneIndex, _currentTargetDoor.GetRoomSpawnPoint());
                    }
                    else
                        EndStep();

                    break;
                case RoomType.Corridor:
                    if (Random.value < _genData._percentageRepeatingCorridors)
                    {
                        _currentRoomInfo = _genData.GetWeightedOfTypeInDirection(direction, RoomType.Corridor);
                        Debug.Log(gameObject.name + "building new corridor with info: " + _currentRoomInfo);
                        if (_currentRoomInfo._weight != 0)
                        {
                            Debug.Log(gameObject.name + "Trying to spawn room");
                            _roomManager.BeginRoomLoad(_currentRoomInfo._roomSceneIndex, _currentTargetDoor.GetRoomSpawnPoint());
                        }
                        else
                            EndStep();
                    }
                    else
                    {
                        _currentRoomInfo = _genData.GetWeightedOfTypeInDirection(direction, RoomType.Crypt);
                        Debug.Log(gameObject.name + "building new Crypt with info: " + _currentRoomInfo);
                        if (_currentRoomInfo._weight != 0)
                        {
                            Debug.Log(gameObject.name + "Trying to spawn room");
                            _roomManager.BeginRoomLoad(_currentRoomInfo._roomSceneIndex, _currentTargetDoor.GetRoomSpawnPoint());
                        }
                        else
                            EndStep();
                    }
                    break;
            }
        }
        else
        {
            Debug.Log(gameObject.name + "Moving to existing room room");
            _currentRoom = _currentTargetDoor.GetLinkedRoom();
            EndStep();
        }
  
    }

    public void ValidateRoom()
    {
        Debug.Log("Begin room validation");
        _roomManager.OnRoomLoadComplete -= ValidateRoom;
        if (!_isWalking)
        {
            Debug.Log("not walking end ");
            EndStep();
        }
        _previosRoom = _currentRoom;
        _currentRoom = _roomManager.GetLastRoom();
        _currentRoom.SetRoomInfo(_currentRoomInfo);

        if (_currentRoom.IsOverlapping(_genData._roomLayers))
        {
            Debug.Log("Overlapping Remove room ");
            RemoveCurrentRoom();

        }
        else
        {
            Debug.Log("Succesful addition  room ");
            _currentTargetDoor.SetLinkedRoom(_currentRoom);
            _previosRoom.DecrementCrawlers();
            _currentRoom.IncrementCrawlers();
            EndStep();
        }

    }

    public void OnCurrentRoomRemoved()
    {
        _roomManager.OnRoomUnloadComplete -= OnCurrentRoomRemoved;
        Debug.Log("Room Removed");
        _currentRoom = _previosRoom;
        if (_attemptsLeft > 0)
        {
            RetryStep();
        }
        else
        {
            EndStep();
        }

    }
    public void RemoveCurrentRoom()
    {
        if (GameStateManager.instance)
        {
            _roomManager.OnRoomUnloadComplete += OnCurrentRoomRemoved;
            _roomManager.BeginUnload(_currentRoom.gameObject.scene);
        }
    
    }
}




