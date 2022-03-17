using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private bool _inDebug;
    public static RoomManager _instance;
    private bool _isLoadingRoom;
    private bool _isUnLoadingRoom;
    private List<Room> _loadedRooms = new List<Room>();

    //Events
    public Action OnRoomLoadComplete;
    public Action OnRoomUnloadComplete;
    private void Awake()
    {
        if (_inDebug) Init();
    }
    public void Init()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


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
        _loadedRooms[_loadedRooms.Count - 1].transform.parent.position = position;
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


    public List<Room> GetLoadedRooms( ) { return _loadedRooms; }


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
}

