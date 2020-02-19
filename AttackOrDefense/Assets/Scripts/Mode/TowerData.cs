//
// @brief: 防御塔信息类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/20
// 
// 
//

using UnityEngine;

public class TowerData
{
    public TowerData(int id, string name, int att, float cooling, int attackRenge, int price, BuildingType buildingType,BuildingType nextBuildingType)
    {
        this.Id = (Fix64)id;
        this.BuildingName = name;
        this.BuildingAtt = (Fix64)att;
        this.BuildingCooling = (Fix64)cooling;
        this.BuildingAttackRange = (Fix64)attackRenge;
        this.BuildingPrice = (Fix64)price;
        this.BuildingType = buildingType;
        this.NextBuildingType = nextBuildingType;
    }
    public Fix64 Id { get; set; }                         //ID
    public string BuildingName { get; private set; }      //名称
    public Fix64 BuildingAtt { get; set; }                //攻击力
    public Fix64 BuildingCooling { get; set; }            //冷却
    public Fix64 BuildingAttackRange { get; set; }        //射程
    public Fix64 BuildingPrice { get; set; }              //价格
    public BuildingType BuildingType { get; set; }        //当前类型  
    public BuildingType NextBuildingType { get; set; }   //下一级类型
}

