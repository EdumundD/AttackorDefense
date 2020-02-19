//
// @brief: 箭塔
// @version: 1.0.0
// @author lhy
// @date: 2020/2/6
// 
// 
//

public class ArrowTower : BaseTower
{
    public ArrowTower()
    {
        init();
    }

    //- 初始化
    // 
    // @param self 
    // @return none
    void init()
    {
        //每个塔加载的资源不同,所以单独处理
        createFromPrefab("Build/Tower/ArrowTower/ArrowTowerLv1", this);

        //调用父类的构造函数
        //self[BASETOWER]:init(self)

        //设置名字为魔法塔
        m_scName = "arrowtower";

        //设置子弹发射位置偏移量
        createBulletOffset = new FixVector3((Fix64)0, (Fix64)1, (Fix64)0);
    }

    //- 每帧循环
    // 
    // @return none
    public override void updateLogic()
    {
        //调用父类Update
        base.updateLogic();
    }

    //- 加载属性
    // 
    // @return none
    public override void loadProperties()
    {
        setDamageValue(TowerData.BuildingAtt);
        attackRange = TowerData.BuildingAttackRange;
        attackSpeed = TowerData.BuildingCooling;
    }
}
