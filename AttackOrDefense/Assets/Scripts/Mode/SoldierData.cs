//
// @brief: 士兵信息类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/4
// 
// 
//


public class SoldierData
{
    public SoldierData(string userData)
    {
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);
        this.SoldierName = strs[1];
        this.SoldierAtt = int.Parse(strs[2]);
        this.SoldierHP = int.Parse(strs[3]);
        this.SoldierSpeed = int.Parse(strs[4]);
        this.SoldierPrice = int.Parse(strs[5]);
        this.SoldierType = (BuildingType)(int.Parse(strs[0]) + GameData.g_towerFactory.towerDataList.Count);
    }
    public int Id { get; set; }
    public string SoldierName { get; private set; }
    public int SoldierAtt { get; set; }
    public int SoldierHP { get; set; }
    public int SoldierSpeed { get; set; }
    public int SoldierPrice { get; set; }
    public BuildingType SoldierType { get; set; }

}

