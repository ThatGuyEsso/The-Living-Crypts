using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public static BossRoomManager Instance;
    [Header("Room Components")]
    [SerializeField] private BaseBoss Boss;
    [SerializeField] private GameObject BossUIPrefab;
    [SerializeField] private bool InDebug;

    private BossUI _bossUI;
    private void Awake()
    {
        if (InDebug) Init();
    }
    public void Init()
    {
        if (!Boss)
        {
            Debug.LogError("No Boss");
            return;
        }
        
        _bossUI = Instantiate(BossUIPrefab, transform).GetComponent<BossUI>();

        if (!_bossUI)
        {
            Debug.LogError("No Boss UI");
            return;
        }
        Boss.Init();
        _bossUI.OnUISpawned += StartBossFight;
        Boss.InitBossUI(_bossUI);

      

    }

    public void StartBossFight()
    {
        if (_bossUI) _bossUI.OnUISpawned -= StartBossFight;
        Boss.StartBossFight();
    }
      


}
