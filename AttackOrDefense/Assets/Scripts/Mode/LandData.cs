//
// @brief: 土地类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/20
// 
// 
//

using HighlightingSystem;
using UnityEngine;

public class LandData : MonoBehaviour
{
    private Highlighter m_highlighter;
    public FixVector3 localPosition;
    public bool isBuild = false;
    public bool isForTower = false;
    public bool isForBarrack = false;

    private void Start()
    {
        if (isForTower)
        {
            gameObject.layer = 9;
            m_highlighter = GetComponent<Highlighter>();
            m_highlighter.FlashingParams(Color.green, Color.white, 0f);
        }
        if (isForBarrack)
        {
            gameObject.layer = 10;
            m_highlighter = GetComponent<Highlighter>();
            m_highlighter.FlashingParams(Color.green, Color.white, 0f);
        }
        localPosition = new FixVector3((Fix64)transform.position.x, (Fix64)transform.position.y, (Fix64)transform.position.z);

        
        GameData.g_listLand.Add(this);
    }

    public void Flashing(bool isOn)
    {
        if (isOn)
            m_highlighter.FlashingOn();//调用Highlighte脚本的开始高亮函数 
        else
            m_highlighter.FlashingOff();//调用Highlighte脚本的结束高亮函数 
    }
}
