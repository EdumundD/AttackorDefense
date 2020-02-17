//
// @brief: 士兵移动状态
// @version: 1.0.0
// @author lhy
// @date: 2020/2/13
// 
// 
//


public class SoldierMoveState : BaseState
{
    //- 初始化函数.
    // Some description, can be over several lines.
    // @return value description.
    void init()
    {
        m_scName = "soldiermove";
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
        m_unit.playAnimation("Walk Forward");
    }


    //- 退出该状态时调用的函数tgfdsdfgdfsgfgggggggg
    // 
    // @return none
    public override void onExit()
    {
        m_unit.stopAnimation("Walk Forward");
    }

    //- 处于该状态时每帧调用的函数
    // 
    // @return none
    public override void updateLogic()
    {

    }
}

