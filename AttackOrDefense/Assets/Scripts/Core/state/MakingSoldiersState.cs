using System.Collections;


public class MakingSoldiersState : BaseState
{
    public MakingSoldiersState()
    {
        init();
    }
    //- 初始化函数.
    // Some description, can be over several lines.
    // @return value description.
    void init()
    {
        m_scName = "makingsoldiers";
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
        m_unit.isCooling = true;

        if(m_unit.m_scName == "mechanicalgolembarrack")
        {
            m_unit.delayDo((Fix64)1, delegate () {
                m_unit.createSoldier();
            }, "createSoldier");
        }
        else if(m_unit.m_scName == "metalonbarrack")
        {
            m_unit.delayDo((Fix64)1, delegate () {
                m_unit.createSoldier();
            }, "createSoldier");

            m_unit.delayDo((Fix64)2, delegate () {
                m_unit.createSoldier();
            }, "createSoldier");

            m_unit.delayDo((Fix64)3, delegate () {
                m_unit.createSoldier();
                m_unit.changeState("cooling", (Fix64)12);
            }, "createSoldier");
        }
    }


    //- 退出该状态时调用的函数
    // 
    // @return none
    public override void onExit()
    {
        m_unit.isCooling = false;
        m_unit.stopAction("createSoldier");
    }

    //- 处于该状态时每帧调用的函数
    // 
    // @return none
    public override void updateLogic()
    {

    }
}

