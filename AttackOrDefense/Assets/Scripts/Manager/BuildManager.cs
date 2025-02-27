﻿//
// @brief: 建造管理类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/20
// 
// 
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HighlightingSystem;

public class BuildManager : BaseManager
{
    //父物体
    private GameObject buildGo;
    private GameObject BuildGo
    {
        get
        {
            if (buildGo == null)
            {
                buildGo = new GameObject("Build(GameObject)");
            }
            return buildGo;
        }
    }

    //存储所有的建筑物Prefavb的路径
    private Dictionary<BuildingType, string> buildingPathDict;

    //建筑物
    private GameObject selectBuilding = null;

    //建筑物ID
    private BuildingType buildingType;

    //建筑物的Renderer组件
    private List<Renderer> cubeRenderers = null;

    //鼠标指针所在Land
    private GameObject selectLand = null;

    //所有用于建造土地的二维数组   0: 不能建造 1：ForTower 2:ForBarrack 3:isBuild
    public static int[,] g_buildmap = null;

    //目标位置
    private Vector3 target;

    //是否正在建造中
    private bool isBuilding = false;

    //判断是否可以建造
    private bool isAllowedBuild = false;

    //层级名称
    private string maskname = "";

    public BuildManager(GameFacade facade) : base(facade) {
        ParseBuildingTypeJson();
        GameData.g_buildingPathDict = buildingPathDict;
    }
    public override void OnInit()
    {
        Init();
    }
    private void Init() {
        target = new Vector3(0, 1, 0);
        selectLand = GameObject.Find("Box_A1");
        selectBuilding = null;

    }
   
    public void StartBuilding(BuildingType bt, bool isBuildTower)
    {
        facade.HideAllBattlePanel();
        if (isBuildTower) maskname = "Land";
        else maskname = "Barrack";
        isBuilding = true;
        g_buildmap = GameData.GetBuildMap();
        if (selectBuilding == null)
        {
            buildingType = bt;
            string path = buildingPathDict.TryGet(bt);
            selectBuilding = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            selectBuilding.transform.SetParent( BuildGo.transform, false);
            cubeRenderers = new List<Renderer>();
        }
        //将所有可建造土地高亮
        LandFlashing(true, isBuildTower);
    }
    public override void Update()
    {
        if (isBuilding)
        {
            if(EventSystem.current.IsPointerOverGameObject() == false) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo,1000, LayerMask.GetMask(maskname)))
                {
                    if (selectLand != hitInfo.collider.gameObject)
                    {
                        selectLand = hitInfo.collider.gameObject;
                        target = selectLand.transform.position;
                        if (maskname == "Land") target.y = 2;
                        else target.y = 1;
                    }
                }
            }

            //判断是否可以建造
            if (g_buildmap[(int)target.x, (int)target.z] == 0 || g_buildmap[(int)target.x, (int)target.z] == 3 ||
                g_buildmap[(int)target.x + 1, (int)target.z] == 0 || g_buildmap[(int)target.x + 1, (int)target.z] == 3 ||
                g_buildmap[(int)target.x, (int)target.z + 1] == 0 || g_buildmap[(int)target.x, (int)target.z + 1] == 3 ||
                g_buildmap[(int)target.x + 1, (int)target.z + 1] == 0 || g_buildmap[(int)target.x + 1, (int)target.z + 1] == 3)
            {
                isAllowedBuild = false;
                if(cubeRenderers.Count == 0)
                {
                    if (GetLandData(target) != null)
                        cubeRenderers.Add(GetLandData(target).gameObject.GetComponent<Renderer>());
                    if (GetLandData(new Vector3(target.x + 1, target.y, target.z)) != null)
                        cubeRenderers.Add(GetLandData(new Vector3(target.x + 1, target.y, target.z)).gameObject.GetComponent<Renderer>());
                    if (GetLandData(new Vector3(target.x, target.y, target.z + 1)) != null)
                        cubeRenderers.Add(GetLandData(new Vector3(target.x, target.y, target.z + 1)).gameObject.GetComponent<Renderer>());
                    if (GetLandData(new Vector3(target.x + 1, target.y, target.z + 1)) != null)
                        cubeRenderers.Add(GetLandData(new Vector3(target.x + 1, target.y, target.z + 1)).gameObject.GetComponent<Renderer>());

                    foreach (Renderer renderer in cubeRenderers)
                    {
                        renderer.material.color = new Color(200f / 255, 1f / 255, 1f / 255, 255f / 255);
                    }
                }
                else
                {
                    if (target.x != cubeRenderers[0].gameObject.transform.position.x || target.z != cubeRenderers[0].gameObject.transform.position.z)
                    {
                        foreach (Renderer renderer in cubeRenderers)
                        {
                            renderer.material.color = new Color(188f / 255, 188f / 255, 188f / 255, 255f / 255);
                        }
                        cubeRenderers.Clear();
                    }
                }
                
            }
            else
            {
                foreach (Renderer renderer in cubeRenderers)
                {
                    renderer.material.color = new Color(188f / 255, 188f / 255, 188f / 255, 255f / 255);
                }
                isAllowedBuild = true;
            }
            
            //让建筑物跟随鼠标
            if (target != Vector3.zero)
            {
                selectBuilding.transform.position = new Vector3(target.x + 0.5f, target.y, target.z + 0.5f);
            }
            if (Input.GetMouseButtonDown(0) && isAllowedBuild)
            {
                facade.ShowBattleGetBuildingListButton();
                isBuilding = false;
                GetLandData(target).isBuild = true;
                GetLandData(new Vector3(target.x + 1, target.y, target.z)).isBuild = true;
                GetLandData(new Vector3(target.x, target.y, target.z + 1)).isBuild = true;
                GetLandData(new Vector3(target.x + 1, target.y, target.z + 1)).isBuild = true;
                if ((int)buildingType <= GameData.g_towerFactory.towerDataList.Count)
                {
                    facade.inputMono.frameInput.buildTower = new BuildTower(new FixVector2((Fix64)target.x + 0.5f, (Fix64)target.z + 0.5f), (int)buildingType);
                    LandFlashing(false, true);
                }
                else
                {
                    facade.inputMono.frameInput.createBarrack = new CreateBarrack(new FixVector2((Fix64)target.x + 0.5f, (Fix64)target.z + 0.5f), 
                        (int)buildingType - GameData.g_towerFactory.towerDataList.Count);
                    LandFlashing(false, false);
                }
                
                GameObject.Destroy(selectBuilding);
                Init();
            }
            if (Input.GetKeyDown(KeyCode.R)) selectBuilding.transform.Rotate(Vector3.up, 90);
            if (Input.GetMouseButtonDown(1))
            {
                foreach (Renderer renderer in cubeRenderers)
                {
                    renderer.material.color = new Color(188f / 255, 188f / 255, 188f / 255, 255f / 255);
                }
                cubeRenderers.Clear();
                facade.ShowBattleGetBuildingListButton();
                LandFlashing(false, true);
                LandFlashing(false, false);
                GameObject.Destroy(selectBuilding);
                isBuilding = false;
                Init();
            }
        }
    }

    private LandData GetLandData(Vector3 position)
    {
        LandData landData = null;
        for(int i = 0;i < GameData.g_listLand.Count; i++)
        {
            if ((int)GameData.g_listLand[i].localPosition.x == position.x && (int)GameData.g_listLand[i].localPosition.y == position.y - 1 
                && (int)GameData.g_listLand[i].localPosition.z == position.z)
            {
                landData = GameData.g_listLand[i];
                break;
            }
        }
        if(landData == null)
        {
            //Debug.LogError("无法找到对应的Land target:" + target.ToString());
        }
        return landData;
    }

    //- 开启高亮
    // 
    // @parm isOn 开启或关闭 isBuildingTower 是否是建造塔
    // @return none
    private void LandFlashing(bool isOn,bool isBuildTower)
    {
        if (isBuildTower)
        {
            for (int i = 1; i < 16; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    if (g_buildmap[i, j] == 1 && g_buildmap[i, j] != 3)
                    {
                        GetLandData(new Vector3(i, 2, j)).Flashing(isOn);
                    }
                }
            }
        }
        else
        {
            for (int i = 1; i < 16; i++)
            {
                for (int j = 1; j < 44; j++)
                {
                    if (g_buildmap[i, j] == 2 && g_buildmap[i, j] != 3)
                    {
                        GetLandData(new Vector3(i, 1, j)).Flashing(isOn);
                    }
                }
            }
        }
    }

    [Serializable]
    class BuildingTypeJson
    {
        public List<BuildingInfo> infoList = null;
    }
    private void ParseBuildingTypeJson()
    {
        buildingPathDict = new Dictionary<BuildingType, string>();

        TextAsset ta = Resources.Load<TextAsset>("Build/BuildingType");
        BuildingTypeJson jsonObject = JsonUtility.FromJson<BuildingTypeJson>(ta.text);

        foreach(BuildingInfo info in jsonObject.infoList)
        {
            buildingPathDict.Add(info.buildingType, info.path);
        }
    }
}
