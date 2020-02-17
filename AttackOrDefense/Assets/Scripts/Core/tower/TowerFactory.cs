//
// @brief: 塔工厂类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/6
// 
// 
//

using System.Collections.Generic;

public class TowerFactory
{
    public List<TowerData> towerDataList;

    public TowerFactory()
    {
        towerDataList = new List<TowerData>();
        towerDataList.Add(new TowerData("1,ArrowTower,10,0,1,100"));
        towerDataList.Add(new TowerData("2,Metalon,10,0,1,100"));
        towerDataList.Add(new TowerData("3,Metalon,10,0,1,100"));
    }
    //- 创建塔
    // 
    // @param name 名字
    // @param pos 在地图中的位置(注意是地图块的位置, 不是坐标)
    // @return 创建出的士兵.
    public BaseTower createTower(int towerID)
    {
        BaseTower tower = new BaseTower();
        if (towerID == 1)
        {
            tower = new ArrowTower();
        }
        if (towerID == 2)
        {
            tower = new ArrowTower();
        }
        if (towerID == 3)
        {
            tower = new ArrowTower();
        }

        tower.changeState("towerstand");

        GameData.g_listTower.Add(tower);

        return tower;
    }
}
