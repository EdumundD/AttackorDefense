using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour
{
    private int id;
    private Text buildingName;
    private Text buildingAtt;  //攻击力
    private Text buildingDef;
    private Text buildingAts;  //攻速
    private Text buildingPrice;
    private Button buyButton;
    private BuildingType buildingType;
    private BuildingListPanel buildingListPanel;

    private bool isBuildTower = false;

    void Awake()
    {
        buildingName = transform.Find("BuildingName").GetComponent<Text>();
        buildingAtt = transform.Find("BuildingAtt").GetComponent<Text>();
        buildingDef = transform.Find("BuildingDef").GetComponent<Text>();
        buildingAts = transform.Find("BuildingAts").GetComponent<Text>();
        buildingPrice = transform.Find("BuildingPrice").GetComponent<Text>();
        buyButton = transform.GetComponent<Button>();
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyClick);
        }
    }
    public void SetBuildingInfo(int id, string buildingname, int buildingatt, int buildingdef, int buildingats, int buildingprice, BuildingType buildingType, BuildingListPanel buildingListPanel)
    {
        isBuildTower = true;
        this.id = id;
        buildingName.text = buildingname;
        buildingAtt.text = "攻击力:" + buildingatt;
        buildingDef.text = " ";
        buildingAts.text = "攻  速:" + buildingats;
        buildingPrice.text = "价  格:" + buildingprice;
        this.buildingType = buildingType;
        this.buildingListPanel = buildingListPanel;
    }
    public void SetBarrackInfo(int id, string soldiername, int soldieratt, int soldierhp, int soldierspeed, int soldierprice, BuildingType buildingType, BuildingListPanel buildingListPanel)
    {
        isBuildTower = false;
        this.id = id;
        buildingName.text = soldiername;
        buildingAtt.text = "攻击力:" + soldieratt;
        buildingDef.text = "血  量:" + soldierhp;
        buildingAts.text = "移 速:" + soldierspeed;
        buildingPrice.text = "价  格:" + soldierprice;
        this.buildingType = buildingType;
        this.buildingListPanel = buildingListPanel;
    }
    private void OnBuyClick()
    {
        buildingListPanel.Facade.StartBuilding(buildingType, isBuildTower);
    }
    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
