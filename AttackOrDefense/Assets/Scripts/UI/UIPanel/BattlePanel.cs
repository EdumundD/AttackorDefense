//
// @brief: 战斗面板类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/17
// 
// 
//

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattlePanel : BasePanel
{
    private RectTransform BuildingList;
    private VerticalLayoutGroup buildingLayout;
    private GameObject buildingItemPrefab;
    private RectTransform BuildingInfo;
    private Button GetBuildingListButton;
    private RectTransform GetBuildingListButtonTr;

    private Text BuildingName;
    private Text BuildingAtt;
    private Text BuildingCooling;
    private Text BuildingAttackRange;

    private Button BuildingLvUp;

    private void Awake()
    {
        //BuildingListPanel
        BuildingList = GameObject.Find("BattlePanel(Clone)/BuildingList").GetComponent<RectTransform>();
        buildingLayout = transform.Find("BuildingList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        buildingItemPrefab = Resources.Load("UIPanel/BuildingItem") as GameObject;
        GetBuildingListButton = transform.Find("GetBuildingListButton").GetComponent<Button>();
        GetBuildingListButtonTr = transform.Find("GetBuildingListButton").GetComponent<RectTransform>();
        transform.Find("BuildingList/BuildingFactory").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            PlayClickSound();
            LoadBuildingItem();
        });
        transform.Find("BuildingList/SoldierFactory").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            PlayClickSound();
            LoadSolderItem();
        });
        transform.Find("BuildingList/CloseButton").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            PlayClickSound();
            OnBuildingListCloseClick();
        });
        GetBuildingListButton.onClick.AddListener(delegate ()
        {
            PlayClickSound();
            ShowBuildingListPanel();
        });

        //BuildingInfo
        BuildingInfo = GameObject.Find("BattlePanel(Clone)/BuildingInfo").GetComponent<RectTransform>();
        BuildingName = transform.Find("BuildingInfo/BuildingName").GetComponent<Text>();
        BuildingAtt = transform.Find("BuildingInfo/BuildingAtt").GetComponent<Text>();
        BuildingCooling = transform.Find("BuildingInfo/BuildingCooling").GetComponent<Text>();
        BuildingAttackRange = transform.Find("BuildingInfo/BuildingAttackRange").GetComponent<Text>();

        BuildingLvUp = transform.Find("BuildingInfo/LeveUp").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        EnterAnim();
    }
    public override void OnPause()
    {
        ExitAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }
    public override void OnExit()
    {
        ExitAnim();
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        BuildingList.localPosition = new Vector3(Screen.width / 2 + 450, 0, 0);
        BuildingList.gameObject.SetActive(false);
        BuildingInfo.transform.localPosition = new Vector3(0, -Screen.height / 2 - 100, 0);
        BuildingInfo.gameObject.SetActive(false);
        GetBuildingListButtonTr.gameObject.SetActive(true);
        GetBuildingListButtonTr.localPosition = new Vector3(Screen.width / 2 + 40, 0, 1);
        GetBuildingListButtonTr.DOLocalMoveX(Screen.width / 2, 0.2f);
    }
    private void ExitAnim()
    {
        BuildingList.DOLocalMoveX(Screen.width / 2 + 450, 0.3f).OnComplete(() => BuildingList.gameObject.SetActive(false));
        BuildingInfo.DOLocalMoveY(-Screen.height / 2 - 100, 0.3f).OnComplete(() => BuildingInfo.gameObject.SetActive(false));
        GetBuildingListButtonTr.DOMoveX(Screen.width / 2 + 40, 0.2f).OnComplete(() => gameObject.SetActive(false));
    }

    //显示较建筑物列表面板
    public void ShowBuildingListPanel()
    {
        BuildingList.gameObject.SetActive(true);
        BuildingList.localPosition = new Vector3(Screen.width / 2 + 450, 0, 0);
        BuildingList.DOLocalMoveX(Screen.width / 2 + 0, 0.3f);
        GetBuildingListButtonTr.DOLocalMoveX(Screen.width / 2 + 40, 0.2f).OnComplete(() => GetBuildingListButtonTr.gameObject.SetActive(false));
        BuildingInfo.DOLocalMoveY(-Screen.height / 2 - 100, 0.3f).OnComplete(() => BuildingInfo.gameObject.SetActive(false));
        LoadBuildingItem();
    }

    //关闭较建筑物列表面板
    private void OnBuildingListCloseClick()
    {
        PlayClickSound();
        GetBuildingListButtonTr.gameObject.SetActive(true);
        GetBuildingListButtonTr.DOLocalMoveX(Screen.width / 2, 0.2f);
        BuildingList.DOLocalMoveX(Screen.width / 2 + 450, 0.4f).OnComplete(() => BuildingList.gameObject.SetActive(false));
    }

    //加载防御塔信息
    private void LoadBuildingItem()
    {
        BuildingItem[] riArray = buildingLayout.GetComponentsInChildren<BuildingItem>();
        foreach (BuildingItem ri in riArray)
        {
            ri.DestroySelf();
        }
        int count = GameData.g_towerFactory.towerDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject buildingItem = GameObject.Instantiate(buildingItemPrefab);
            buildingItem.transform.SetParent(buildingLayout.transform);
            TowerData bd = GameData.g_towerFactory.towerDataList[(BuildingType)i+1];
            buildingItem.GetComponent<BuildingItem>().SetBuildingInfo((int)bd.Id, bd.BuildingName, (int)bd.BuildingAtt, (float)bd.BuildingCooling,
                (int)bd.BuildingAttackRange, (int)bd.BuildingPrice, bd.BuildingType);
        }
        int buildingCount = GetComponentsInChildren<BuildingItem>().Length;
        Vector2 size = buildingLayout.GetComponent<RectTransform>().sizeDelta;
        buildingLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            buildingCount * (buildingItemPrefab.GetComponent<RectTransform>().sizeDelta.y + buildingLayout.spacing));
    }

    //加载兵营信息
    private void LoadSolderItem()
    {
        BuildingItem[] riArray = buildingLayout.GetComponentsInChildren<BuildingItem>();
        foreach (BuildingItem ri in riArray)
        {
            ri.DestroySelf();
        }
        int count = GameData.g_soldierFactory.soldierDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject buildingItem = GameObject.Instantiate(buildingItemPrefab);
            buildingItem.transform.SetParent(buildingLayout.transform);
            SoldierData sd = GameData.g_soldierFactory.soldierDataList[i];
            buildingItem.GetComponent<BuildingItem>().SetBarrackInfo(sd.Id, sd.SoldierName, sd.SoldierAtt, sd.SoldierHP, sd.SoldierSpeed, sd.SoldierPrice, sd.SoldierType);
        }
        int buildingCount = GetComponentsInChildren<BuildingItem>().Length;
        Vector2 size = buildingLayout.GetComponent<RectTransform>().sizeDelta;
        buildingLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            buildingCount * (buildingItemPrefab.GetComponent<RectTransform>().sizeDelta.y + buildingLayout.spacing));
    }

    //显示建筑物详细信息面板
    public void ShowBuildingInfo(LiveObject building)
    {
        if (null == building.TowerData) return;
        var towerData = building.TowerData;
        BuildingName.text = towerData.BuildingName;
        BuildingAtt.text = "攻击力:" + towerData.BuildingAtt;
        BuildingCooling.text = "冷却:" + towerData.BuildingCooling;
        BuildingAttackRange.text = "射程:" + towerData.BuildingAttackRange;
        BuildingInfo.gameObject.SetActive(true);
        BuildingInfo.localPosition = new Vector3(0, -Screen.height / 2 - 100);
        BuildingInfo.DOLocalMoveY(-Screen.height / 2, 0.3f);

        BuildingList.DOLocalMoveX(Screen.width / 2 + 450 + 0, 0.3f).OnComplete(() => BuildingList.gameObject.SetActive(false));
        if (GetBuildingListButtonTr.gameObject.activeSelf == false)
        {
            GetBuildingListButtonTr.gameObject.SetActive(true);
            GetBuildingListButtonTr.DOLocalMoveX(Screen.width / 2, 0.2f);
        }

        BuildingLvUp.onClick.AddListener(delegate () { this.OnLvUpClick(building); });
    }

    //隐藏建筑物详细信息面板
    public void HideBuildingInfo()
    {
        BuildingInfo.DOLocalMoveY(-Screen.height / 2 - 100, 0.3f).OnComplete(() => BuildingInfo.gameObject.SetActive(false));
        if (GetBuildingListButtonTr.gameObject.activeSelf == false)
        {
            GetBuildingListButtonTr.gameObject.SetActive(true);
            GetBuildingListButtonTr.DOLocalMoveX(Screen.width / 2, 0.2f);
        }
    }

    //隐藏所有面板
    public void HideAll()
    {
        ExitAnim();
    }

    //显示GetBuildingListButton
    public void ShowGetBuildingListButton()
    {
        EnterAnim();
    }

    public void OnLvUpClick(LiveObject building)
    {
        building.lvUp();
    }
}
