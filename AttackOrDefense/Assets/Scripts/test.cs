using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private Highlighter m_highlighter;
    void Awake()
    {
        m_highlighter = this.GetComponent<Highlighter>();
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

        

