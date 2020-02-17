//
// @brief: 物体高亮类
// @version: 1.0.0
// @author lhy
// @date: 2020/1/25
// 
// 
//

using UnityEngine;
using HighlightingSystem;

public class HighLighterController : MonoBehaviour
{
    private Highlighter m_highlighter;
    void Awake()
    {
        m_highlighter = GetComponent<Highlighter>();
        //Instantiate(Resources.Load("Monster/Polygonal Bugs Pack/Polygonal Metalon/Prefabs/Polygonal Metalon Red"), transform.position, transform.rotation);
        
    }
    void OnMouseEnter()
    {
        m_highlighter.FlashingParams(Color.yellow, Color.yellow, 0f);
        m_highlighter.FlashingOn();//调用Highlighte脚本的开始高亮函数 
    }
    void OnMouseExit()
    {
        m_highlighter.FlashingOff();//调用Highlighte脚本的结束高亮函数  
    }
}

        

