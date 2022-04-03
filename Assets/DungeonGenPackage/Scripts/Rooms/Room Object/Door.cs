using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class Door : MonoBehaviour, Controls.IInteractActions
{
    [Header("Door Settings")]
    [SerializeField] private Transform _roomSpawnPoint;
    [SerializeField] private bool _isEntry;
    [SerializeField] private bool _requiresInput =false;
    [SerializeField] private Direction _faceDirection;
    [SerializeField] private float _width, _length, _height;

    [Header("Door GFX")]
    [SerializeField] private GameObject NothInUseMesh;
    [SerializeField] private GameObject[] InUseMeshes;
    [Header("Debug Settings")]
    [SerializeField] private bool _inDebug;
    [SerializeField] private GameObject _entryDebugPrefab;
    [SerializeField] private GameObject _exitDebugPrefab;
    [SerializeField] private GameObject _debugVisual;
    [SerializeField] private Vector3 _debugOffset;
    [SerializeField] private Vector3 _offset;

    [Header("Interaction")]
    [SerializeField] private string InteractPrompt = "[E] - To Open Door";
    private Controls _input;
    [Header("Door Animations")]
    [SerializeField] private string OpenAnimName, CloseAnimName;
    //Events
    public System.Action OnDoorOpened;
    public System.Action OnDoorClosed;
    public System.Action OnDoorTriggered;
    //States
    private bool _isInRange;
    private bool _canOpen;
    private bool _isOpen=false;

    //Object References
    private Room _parentRoom;
   [SerializeField] private Room _linkedRoom;
    private HUDPrompt Prompt;
    private Animator _animator;
    bool _isInitialised;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    public void Init()
    {
      
        _input = new Controls();
        _input.Interact.SetCallbacks(this);
        if (_inDebug)
        {
            SpawnDebugVisual();
        }

   
    }
    public void RegisterOnGrid(Grid2D<GridObject> grid)
    {

        if (_inDebug) SpawnDebugVisual();
    }

    private void OnDrawGizmos()
    {
        if (!_inDebug) return;
        Vector3 centre = transform.position +_offset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centre, new Vector3(_width, _height, _length));


    }

    public void SpawnDebugVisual()
    {

        if (_isEntry)
        {
            if (!_entryDebugPrefab) return;
            _debugVisual=Instantiate(_entryDebugPrefab, _roomSpawnPoint.position + transform.right*0.5f+_debugOffset, transform.rotation);
        }
        else
        {
            if (!_exitDebugPrefab) return;
            _debugVisual =Instantiate(_exitDebugPrefab, _roomSpawnPoint.position  + transform.right*0.5f + _debugOffset, transform.rotation);
        }
    }

    public Direction GetDirection() { return _faceDirection; }
    public bool IsEntry() { return _isEntry; }

    //public List<Vector2Int> GetCoordinateOfTargetCells() { return _targetCells; }
    public Vector3 GetRoomSpawnPoint()
    {
        return _roomSpawnPoint.position;
    }

    public void SetLinkedRoom(Room link) { _linkedRoom = link; }
    public Room GetLinkedRoom( ) { return _linkedRoom; }


    public void ToggleDoorLock(bool isLocked)
    {
        _canOpen = !isLocked;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_canOpen) return;
        if (other.CompareTag("Player"))
        {
            if (_requiresInput)
            {
                _isInRange = true;
                if (_input != null)
                {
                    _input.Enable();
                }

                if (Prompt)
                {
                    Prompt.ShowPrompt(InteractPrompt);
                }
                else
                {
                    Prompt = GetPromptFromGameManager();
                    if (Prompt)
                    {
                        Prompt.ShowPrompt(InteractPrompt);
                    }

                }
            }
            else
            {
                OpenDoor();
            }
  


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_canOpen) return;
        if (other.CompareTag("Player"))
        {
            if (_requiresInput)
            {
                _isInRange = false;
                if (_input != null)
                {
                    _input.Disable();
                }
                if (Prompt)
                {
                    Prompt.RemovePrompt(InteractPrompt);
                }
                else
                {
                    Prompt = GetPromptFromGameManager();
                    if (Prompt)
                    {
                        Prompt.RemovePrompt(InteractPrompt);
                    }

                }
            }
         
        }
    }
    public HUDPrompt GetPromptFromGameManager()
    {
        if (!GameStateManager.instance)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager.HUDManager)
        {
            return null;
        }
        return GameStateManager.instance.GameManager.HUDManager.PromptManager;
    }

    public void OnTryToInteract(InputAction.CallbackContext context)
    {
        if (context.performed && _isInRange)
        {
            _canOpen = false;
            OnDoorTriggered.Invoke();
            OpenDoor();
        }
    }


    public void OpenDoor()
    {
        if (!_animator)
        {
            _animator = GetComponent<Animator>();
        }
            
      
      
        _animator.enabled = true;
        _animator.Play(OpenAnimName, 0, 0f);
        
    }

    public void CloseDoor()
    {
        if (_animator)
        {
            _animator.enabled = true;
            _animator.Play(CloseAnimName, 0, 0f);
        }
    }
    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }
    private void OnDestroy()
    {
        if (_debugVisual)
        {
            Destroy(_debugVisual);
        }

        if (_input != null)
        {
            _input.Disable();
        }
    }

    public void OnDoorOpenComplete()
    {
        if (_animator)
        {
            _animator.enabled = false;
        }
    
        _isOpen = true;
        OnDoorOpened?.Invoke();
    }
    public void OnDoorCloseComplete()
    {
        if (_animator)
        {
            _animator.enabled = false;
        }
        _isOpen = false;
        OnDoorClosed?.Invoke();
    }

    public void DisableDoor()
    {
        if (NothInUseMesh)
        {
            NothInUseMesh.SetActive(true);
        }

        foreach(GameObject inUse in InUseMeshes)
        {
            inUse.SetActive(false);
        }

        Destroy(this);
    }
}
