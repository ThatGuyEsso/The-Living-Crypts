using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
public class DebugController : MonoBehaviour { 
    private bool showConsole;
    private DebugControls inputAction;


    private string input = string.Empty;


    public static DebugCommand SPAWN_NORTH_DUNGEON;
    public static DebugCommand SPAWN_SOUTH_DUNGEON;
    public static DebugCommand SPAWN_WEST_DUNGEON;
    public static DebugCommand SPAWN_EAST_DUNGEON;
    public List<object> commandList;
    private RoomManager _roomManager;
    private void Awake()
    {

        inputAction = new DebugControls();
        inputAction.Console.Enable();
        inputAction.Console.ToggleConsole.performed += ToggleConsole;
        inputAction.Console.Return.performed += OnReturn;

        SPAWN_NORTH_DUNGEON = new DebugCommand("/Spawn_North_Dungeon", "Spawns North Dungeon","/Spawn_N_Dungeon",() => GenNorthDungeon());
        SPAWN_SOUTH_DUNGEON = new DebugCommand("/Spawn_South_Dungeon", "Spawns south Dungeon", "/Spawn_S_Dungeon", () => GenSouthDungeon());
        SPAWN_WEST_DUNGEON = new DebugCommand("/Spawn_West_Dungeon", "Spawns West Dungeon", "/Spawn_W_Dungeon", () => GenWestDungeon());
        SPAWN_EAST_DUNGEON = new DebugCommand("/Spawn_East_Dungeon", "Spawns East Dungeon", "/Spawn_E_Dungeon", () => GenEastDungeon());

        commandList = new List<object>
        {
            SPAWN_NORTH_DUNGEON,
            SPAWN_SOUTH_DUNGEON,
            SPAWN_WEST_DUNGEON,
            SPAWN_EAST_DUNGEON
        };
    }


    public void ToggleConsole(InputAction.CallbackContext context)
    {

        if(context.performed) showConsole = !showConsole;
   
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y=0;
        GUI.Box(new Rect(0, y, Screen.width, 40), "");
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        GUI.skin.textField.fontSize = 40;
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 60f), input);
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed&&showConsole)
        {
            HandleInput();
            input = string.Empty;
            showConsole = false;


        }

    }
    public void HandleInput()
    {
       
        for(int i=0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.CommandID))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).CallCommand();
                }
            }
        }
    }

    public void GenNorthDungeon()
    {
        Room initRoom = _roomManager.GetLoadedRooms()[0];

        if (DungeonGenerator._instance) DungeonGenerator._instance.BeginDungeonGeneration(initRoom, Direction.North);
    }
    public void GenSouthDungeon()
    {
        Room initRoom = _roomManager.GetLoadedRooms()[0];

        if (DungeonGenerator._instance) DungeonGenerator._instance.BeginDungeonGeneration(initRoom, Direction.South);
    }
    public void GenEastDungeon()
    {
        Room initRoom = _roomManager.GetLoadedRooms()[0];

        if (DungeonGenerator._instance) DungeonGenerator._instance.BeginDungeonGeneration(initRoom, Direction.East);
    }
    public void GenWestDungeon()
    {
        Room initRoom = _roomManager.GetLoadedRooms()[0];

        if (DungeonGenerator._instance) DungeonGenerator._instance.BeginDungeonGeneration(initRoom, Direction.West);
    }

}
