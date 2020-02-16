//
// @brief: 甲壳虫
// @version: 1.0.0
// @author helin
// @date: 8/20/2018
// 
// 
//

using System.Collections;

public class Metalon : BaseSoldier
{

    public Metalon()
    {
        loadProperties();

        base.createFromPrefab("Monster/Metalon/Prefabs/Polygonal Metalon Red", this);

        //设置名称
        m_scName = "metalon";

        //showHP = m_gameObject.transform.Find("Canvas").Find("Slider").GetComponent<ShowHP>();
        //var slider = prefab.create("UIPanel/Slider").transform;
        showHP = GameFacade.Instance.CreateSlider().GetComponent<ShowHP>();
        showHP.maxValue = (float)hp;
        showHP.offsetPos = new UnityEngine.Vector2(0, 30);
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
        setHp((Fix64)200);
        speed = (Fix64)0.5f;
    }
}
