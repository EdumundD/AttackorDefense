//
// @brief: MechanicalGolem兵营类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/10
// 
// 
//


class MechanicalGolemBarrack : BaseBarrack
{
    public MechanicalGolemBarrack()
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

        createFromPrefab("Build/MetalonBarrack/Prefabs/SM_Env_Ceiling_Stone_Flat_04 (1)", this);

        //调用父类的构造函数
        //self[BASETOWER]:init(self)

        //设置名字
        m_scName = "mechanicalgolembarrack";
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
        setSoldierType(2);
    }

    //- 设置士兵类型
    // 
    // @return none
    public void setSoldierType(int type)
    {
        soldierType = type;
    }
}


