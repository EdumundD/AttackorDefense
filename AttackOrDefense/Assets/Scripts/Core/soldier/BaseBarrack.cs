//
// @brief: 兵营基类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/7
// 
// 
//

using System.Collections.Generic;

public class BaseBarrack : LiveObject
{
    List<FixVector3> path;
    protected int soldierType;
    public BaseBarrack()
    {
        init();
    }

    void init()
    {
        //设置类型为塔
        m_scType = "barrack";

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
        //状态机
        m_statemachine.updateLogic();

        //检测是否已经死亡
        checkIsDead();

        //检测事件
        checkEvent();
    }

    //- 设置位置
    // 
    // @param position 要设置到的位置
    // @return none
    public override void setPosition(FixVector3 position)
    {
        m_fixv3LogicPosition = position;
    }

    private List<FixVector3> GetPath()
    {
        if(null == path)
        {
            Astar maze = new Astar(GameData.GetMap());
            Point end = new Point((int)(m_fixv3LogicPosition.x - 0.5f) , (int)(m_fixv3LogicPosition.z - 0.5f));
            Point start = new Point(3, 3);
            var parent = maze.FindPath(start, end, false);
            path = new List<FixVector3>();
            while (parent != null)
            {
                path.Add(new FixVector3((Fix64)parent.X, (Fix64)1, (Fix64)parent.Y));
                parent = parent.ParentPoint;
            }
        }
        return path;
    }
    //- 生成士兵
    // 
    // @param position 要设置到的位置
    // @return none
    public override void createSoldier()
    {
        GameFacade.Instance.inputMono.frameInput.createSoldier = new CreateSoldier(GetPath(), soldierType);
        base.createSoldier();
    }
}

