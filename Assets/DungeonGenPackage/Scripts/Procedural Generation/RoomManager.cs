using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private bool _inDebug;
    private bool _isLoadingRoom;
    private bool _isUnLoadingRoom;
    [SerializeField] private List<Room> _loadedRooms = new List<Room>();

    //Events
    public Action OnRoomLoadComplete;
    public Action OnRoomUnloadComplete;


    
    public void BeginRoomLoad(SceneIndex roomIndex, Vector3 position)
    {
        if (_isLoadingRoom)
        {
            Debug.Log("Already building room");
            return;
        }
        StartCoroutine(LoadRoom(roomIndex, position));
    }


    public IEnumerator LoadRoom(SceneIndex roomIndex, Vector3 position)
    {
        Debug.Log("loading room");
        _isLoadingRoom = true;

        AsyncOperation loadingRoom = SceneManager.LoadSceneAsync((int)(roomIndex), LoadSceneMode.Additive);
        while (_isLoadingRoom && !loadingRoom.isDone)
        {
            yield return null;
        }
        Debug.Log("Finished loading room");
        if (_loadedRooms.Count > 1)
        {
            if (_loadedRooms[_loadedRooms.Count - 1].transform.parent)
            {
                _loadedRooms[_loadedRooms.Count - 1].transform.parent.position = position;
            }
            else
            {
                _loadedRooms[_loadedRooms.Count - 1].transform.position = position;
            }
        }
        else
        {
            _loadedRooms[0].transform.position = Vector3.zero;
        }
   
  
        _loadedRooms[_loadedRooms.Count - 1].Init();
        OnRoomLoadComplete?.Invoke();
    }


    public void BeginUnload(Scene roomScene)
    {
        if (_isUnLoadingRoom) return;
        StartCoroutine(UnLoadRoom(roomScene));
    }
    private IEnumerator UnLoadRoom(Scene roomScene)
    {

        _isUnLoadingRoom = true;
        AsyncOperation unload= SceneManager.UnloadSceneAsync(roomScene);
        while (!unload.isDone)
        {
            yield return null;
        }
        _isUnLoadingRoom = false;
        OnRoomUnloadComplete?.Invoke();
    }


    public List<Room> GetLoadedRooms( ) 
    { 
        return _loadedRooms;
    }

    public List<Room> GetRoomsOfType(RoomType type)
    {
        if (_loadedRooms.Count==0)
        {
            return null;
        }
        List<Room> roomsOfType = new List<Room>();

        foreach(Room room in _loadedRooms)
        {
            if(room.GetRoomInfo()._roomType == type)
            {
                roomsOfType.Add(room);
            }
        }
        return roomsOfType;
    }

    public Room GetLastRoom()
    {
        return _loadedRooms[_loadedRooms.Count-1];
    }



    public void OnNewRoomLoaded(Room room)
    {
        if (room)
        {
            _loadedRooms.Add(room);
      
            _isLoadingRoom = false;
        }
    }

    public void BeginRemoveRoom(Room room)
    {
        _loadedRooms.Remove(room);
        StartCoroutine(UnLoadRoom(room.gameObject.scene));
   
    }

   /// <summary>
   /// Removes null references in loaded room list
   /// </summary>
    public void TidyLoadedRoomList()
    {
        List<Room> listToTidy = new List<Room>();
     

        for(int i =0; i< _loadedRooms.Count; i++)
        {
            if (_loadedRooms[i])
            {
                listToTidy.Add(_loadedRooms[i]);
            }
        }

        _loadedRooms = listToTidy;
    }
}

