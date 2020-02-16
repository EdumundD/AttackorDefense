//
// @brief: 箭塔
// @version: 1.0.0
// @author helin
// @date: 8/20/2018
// 
// 
//
using System.Collections;

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
        loadProperties();

        //每个塔加载的资源不同,所以单独处理
        createFromPrefab("Build/Buildings_I3", this);

        //调用父类的构造函数
        //self[BASETOWER]:init(self)

        //设置名字为魔法塔
        m_scName = "magicstand";
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
        setDamageValue((Fix64)50);
        attackRange = (Fix64)6;
        attackSpeed = (Fix64)1;
    }
}
