//
// @brief: 土地高亮类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/18
// 
// 
//

using HighlightingSystem;
using UnityEngine;

public class LandHighLighter : MonoBehaviour
{
    private Highlighter m_highlighter;
    void Awake()
    {
        m_highlighter = GetComponent<Highlighter>();
        m_highlighter.FlashingParams(Color.blue, Color.red, 3f);

    }
    public void FlashingOn()
    {
        m_highlighter.FlashingOn();//调用Highlighte脚本的开始高亮函数 
    }
    public void FlashingOff()
    {
        m_highlighter.FlashingOff();//调用Highlighte脚本的结束高亮函数  
    }
}
