//
// @brief: MechanicalGolem类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/10
// 
// 
//


public class MechanicalGolem : BaseSoldier
{
    public MechanicalGolem()
    {
        loadProperties();

        base.createFromPrefab("Monster/PolygonFantasyRivals/Prefabs/Characters/Character_BR_MechanicalGolem_01", this);

        //设置名称
        m_scName = "mechanicalgolem";

        //设置血条
        showHP = GameFacade.Instance.CreateSlider().GetComponent<ShowHP>();
        showHP.maxValue = (float)hp;
        showHP.offsetPos = new UnityEngine.Vector2(0, 110);
        showHP.target = m_gameObject.transform;
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

