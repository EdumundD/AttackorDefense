//
// @brief: 建造建筑物详细面板类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/22
// 
// 
//

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
    public void SetBuildingInfo(int id, string name, int att, float cooling, int attackRenge, int price, BuildingType buildingType)
    {
        isBuildTower = true;
        this.id = id;
        buildingName.text = name;
        buildingAtt.text = "攻击力:" + att;
        buildingDef.text = "冷  却 " + cooling;
        buildingAts.text = "射  程:" + attackRenge;
        buildingPrice.text = "价  格:" + price;
        this.buildingType = buildingType;
    }
    public void SetBarrackInfo(int id, string soldiername, int soldieratt, int soldierhp, int soldierspeed, int soldierprice, BuildingType buildingType)
    {
        isBuildTower = false;
        this.id = id;
        buildingName.text = soldiername;
        buildingAtt.text = "攻击力:" + soldieratt;
        buildingDef.text = "血  量:" + soldierhp;
        buildingAts.text = "移 速:" + soldierspeed;
        buildingPrice.text = "价  格:" + soldierprice;
        this.buildingType = buildingType;
    }
    private void OnBuyClick()
    {
        GameFacade.Instance.StartBuilding(buildingType, isBuildTower);
    }
    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
