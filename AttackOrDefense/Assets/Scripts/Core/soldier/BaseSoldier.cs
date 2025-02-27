﻿//
// @brief: 士兵类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/7
// 
// 
//

using System.Collections;

public class BaseSoldier : LiveObject
{
    public BaseSoldier()
    {
        init();
    }

    void init()
    {
        //设置类型
        m_scType = "soldier";

        //状态机对象
        m_statemachine = new StateMachine();

        //设置起作用的单元主体
        m_statemachine.setUnit(this);
    }

    //- 每帧循环
    // 
    // @return none
    virtual public void updateLogic()
    {
        m_statemachine.updateLogic();

        checkIsDead();
        checkEvent();
    }

    //- 检查状态
    // 在冷却状态结束后检测一下当前状态,以便根据当前状态刷新逻辑
    // @return none
    public override void checkStatue()
    {

    }
}
