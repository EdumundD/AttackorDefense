//
// @brief: 建筑物高亮类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/18
// 
// 
//

using UnityEngine;
using HighlightingSystem;

public class BuildingHighLighter : MonoBehaviour
{
    private Highlighter m_highlighter;
    void Awake()
    {
        m_highlighter = GetComponent<Highlighter>();
        
    }
    void OnMouseEnter()
    {
        m_highlighter.FlashingParams(Color.yellow, Color.yellow, 0f);
        m_highlighter.FlashingOn();//调用Highlighte脚本的开始高亮函数 
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach(BaseTower tower in GameData.g_listTower)
            {
                if (tower.m_gameObject.transform == gameObject.transform)
                    GameFacade.Instance.ShowBuildingInfoPanel(tower);
            }
        }
    }
    void OnMouseExit()
    {
        m_highlighter.FlashingOff();//调用Highlighte脚本的结束高亮函数  
    }
}

        

