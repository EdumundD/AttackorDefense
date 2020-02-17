//
// @brief: 测战斗面板类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/1
// 
// 
//

using UnityEngine;
using UnityEngine.UI;

public class TestBattlePanel : BasePanel
{
    private Button btnBuildTower;
    private Button btnSendSoldier;
    private Button btnEndBattle;
    private Button btnReplay;


    private void Awake()
    {
        btnBuildTower = GameObject.Find("Button").GetComponent<Button>();
        btnSendSoldier = GameObject.Find("Button (1)").GetComponent<Button>();
        btnEndBattle = GameObject.Find("Button (2)").GetComponent<Button>();
        btnReplay = GameObject.Find("Button (3)").GetComponent<Button>();

        btnBuildTower.onClick.AddListener(delegate ()
        {
            PlayClickSound();
            uiMng.PushPanel(UIPanelType.BuildingList);
        });
        btnSendSoldier.onClick.AddListener(delegate ()
        {
            
        });
        btnEndBattle.onClick.AddListener(delegate ()
        {
            Facade.EndBattle();
        });
        btnReplay.onClick.AddListener(delegate ()
        {
            Facade.ReplayVideo();
        });
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public override void OnPause()
    {
        gameObject.SetActive(false);
    }
    public override void OnResume()
    {
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

}
