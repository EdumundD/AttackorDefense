//
// @brief: 士兵死亡状态
// @version: 1.0.0
// @author lhy
// @date: 2020/2/13
// 
// 
//

using UnityEngine;

public class SoldierDeath : BaseState
{
    public SoldierDeath()
    {
        init();
    }

    //- 初始化函数.
    // Some description, can be over several lines.
    // @return value description.
    void init()
    {
        m_scName = "soldierdeath";
    }

    //- 创建时进入的初始化函数
    // 
    // @param args 附加的创建信息
    // @return none
    public override void onInit(LiveObject args)
    {
        m_unit = args;
    }



    //- 进入该状态时调用的函数
    // 
    // @param args 附加的调用信息
    // @return none
    public override void onEnter(Fix64 args)
    {
        m_unit.stopAllAction();
        m_unit.playAnimation("Die");
    }


    //- 退出该状态时调用的函数tgfdsdfgdfsgfgggggggg
    // 
    // @return none
    public override void onExit()
    {

    }

    //- 处于该状态时每帧调用的函数
    // 
    // @return none
    public override void updateLogic()
    {
        if (m_unit.m_gameObject.transform.GetComponent<Animator>().GetFloat("death") > 1)
        {
            m_unit.m_dying = false;
            m_unit.m_bKilled = true;
        }
    }
}
