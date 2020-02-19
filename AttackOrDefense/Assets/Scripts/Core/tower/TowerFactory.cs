//
// @brief: 塔工厂类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/6
// 
// 
//

using HighlightingSystem;
using System.Collections.Generic;

public class TowerFactory
{
    public Dictionary<BuildingType ,TowerData > towerDataList;

    public TowerFactory()
    {
        towerDataList = new Dictionary<BuildingType, TowerData>();
        towerDataList.Add(BuildingType.ArrowTowerLv1, new TowerData(1, "弩箭塔", 10, 1f, 20, 15, BuildingType.ArrowTowerLv1, BuildingType.ArrowTowerLv2));
        towerDataList.Add(BuildingType.ArrowTowerLv2, new TowerData(2, "弩箭塔", 10, 1f, 20, 15, BuildingType.ArrowTowerLv2, BuildingType.ArrowTowerLv3));
        towerDataList.Add(BuildingType.ArrowTowerLv3, new TowerData(3, "弩箭塔", 10, 1f, 20, 15, BuildingType.ArrowTowerLv3, BuildingType.ArrowTowerLv4));
        towerDataList.Add(BuildingType.ArrowTowerLv4, new TowerData(4, "弩箭塔", 10, 1f, 20, 15, BuildingType.ArrowTowerLv4, BuildingType.None));
        GameData.g_towerdatas = towerDataList;
    }
    //- 创建塔
    // 
    // @param name 名字
    // @param pos 在地图中的位置(注意是地图块的位置, 不是坐标)
    // @return 创建出的塔.
    public BaseTower createTower(int towerID)
    {
        BaseTower tower = new BaseTower();
        if (towerID == 1)
        {
            tower = new ArrowTower();
            towerDataList.TryGetValue(BuildingType.ArrowTowerLv1, out var towerdata);
            tower.TowerData = towerdata;

            //开启HighLighter脚本
            tower.m_gameObject.GetComponent<Highlighter>().enabled = true;
        }
        if (towerID == 2)
        {
            tower = new ArrowTower();
        }
        if (towerID == 3)
        {
            tower = new ArrowTower();
        }

        tower.loadProperties();

        tower.changeState("towerstand");

        GameData.g_listTower.Add(tower);

        return tower;
    }
}
