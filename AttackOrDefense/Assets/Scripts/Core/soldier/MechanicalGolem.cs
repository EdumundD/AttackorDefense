using System.Collections;

public class MechanicalGolem : BaseSoldier
{
    public MechanicalGolem()
    {
        loadProperties();

        base.createFromPrefab("Monster/PolygonFantasyRivals/Prefabs/Characters/Character_BR_MechanicalGolem_01", this);

        //设置名称
        m_scName = "mechanicalgolem";

        showHP = m_gameObject.GetComponent<ShowHP>();
        showHP.maxValue = (float)hp;
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
        setHp((Fix64)600);
        speed = (Fix64)1f;
    }
}

