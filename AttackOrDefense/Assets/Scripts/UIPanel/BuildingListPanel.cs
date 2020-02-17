//
// @brief: 建造面板类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/22
// 
// 
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingListPanel : BasePanel
{
    private RectTransform buildingList;
    private VerticalLayoutGroup buildingLayout;
    private GameObject buildingItemPrefab;
    private ListBuildingsRequest listBuildingRequest;
    private List<TowerData> bdList = null;

    private bool isUpdate = false;

    private void Awake()
    {
        buildingList = transform.Find("BuildingList").GetComponent<RectTransform>();
        buildingLayout = transform.Find("BuildingList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        buildingItemPrefab = Resources.Load("UIPanel/BuildingItem") as GameObject;
        listBuildingRequest = GetComponent<ListBuildingsRequest>();

        transform.Find("BuildingList/BuildingFactory").GetComponent<Button>().onClick.AddListener(LoadBuildingItem);
        transform.Find("BuildingList/SoldierFactory").GetComponent<Button>().onClick.AddListener(LoadSolderItem);
        transform.Find("BuildingList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }
    private void Update()
    {
        //if (isUpdate)
        //{
        //    if (bdList != null)
        //    {
        //        LoadBuildingItem();
        //        isUpdate = false;
        //    }
        //}
    }
    public override void OnEnter()
    {
        if (listBuildingRequest == null)
            listBuildingRequest = GetComponent<ListBuildingsRequest>();
        EnterAnim();
        //if (bdList == null)
        //{
        //    listBuildingRequest.SendRequest();
        //}
        //else LoadBuildingItem();
        LoadBuildingItem();
    }
    public override void OnExit()
    {
        HideAnim();
    }
    public override void OnPause()
    {
        HideAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }


    private void EnterAnim()
    {
        gameObject.SetActive(true);
        buildingList.localPosition = new Vector3(920, 0);
        buildingList.DOLocalMoveX(0, 0.5f);
        
    }
    private void HideAnim()
    {
        buildingList.DOLocalMoveX(920, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void LoadBuildingItem()
    {
        BuildingItem[] riArray = buildingLayout.GetComponentsInChildren<BuildingItem>();
        foreach (BuildingItem ri in riArray)
        {
            ri.DestroySelf();
        }
        //int count = bdList.Count;
        int count = GameData.g_towerFactory.towerDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject buildingItem = GameObject.Instantiate(buildingItemPrefab);
            buildingItem.transform.SetParent(buildingLayout.transform);
            TowerData bd = GameData.g_towerFactory.towerDataList[i];
            buildingItem.GetComponent<BuildingItem>().SetBuildingInfo(bd.Id, bd.BuildingName, bd.BuildingAtt, bd.BuildingDef, bd.BuildingAts,bd.BuildingPrice,bd.BuildingType, this);
        }
        int buildingCount = GetComponentsInChildren<BuildingItem>().Length;
        Vector2 size = buildingLayout.GetComponent<RectTransform>().sizeDelta;
        buildingLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            buildingCount * (buildingItemPrefab.GetComponent<RectTransform>().sizeDelta.y + buildingLayout.spacing));
    }
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
            buildingItem.GetComponent<BuildingItem>().SetBarrackInfo(sd.Id, sd.SoldierName, sd.SoldierAtt, sd.SoldierHP, sd.SoldierSpeed, sd.SoldierPrice, sd.SoldierType, this);
        }
        int buildingCount = GetComponentsInChildren<BuildingItem>().Length;
        Vector2 size = buildingLayout.GetComponent<RectTransform>().sizeDelta;
        buildingLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            buildingCount * (buildingItemPrefab.GetComponent<RectTransform>().sizeDelta.y + buildingLayout.spacing));
    }
    public void LoadBuildingItemSync(List<TowerData> buildingDatas)
    {
        bdList = buildingDatas;
        isUpdate = true;
    }
}
