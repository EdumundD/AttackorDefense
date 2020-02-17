//
// @brief: 普通弓箭
// @version: 1.0.0
// @author lhy
// @date: 2020/2/13
// 
// 
//

using System.Collections.Generic;

public class NormalArrow : BaseBullet
{
    Fix64 m_fixMoveTime = Fix64.Zero;

    //- 每帧循环
    // 
    // @return none
    public override void updateLogic()
    {
        //调用父类Update
        base.updateLogic();
    }

    //- 初始化数据
    // 
    // @param src 发射源
    // @param dest 射击目标
    // @param poOri 发射的起始位置
    // @param poDst 发射的目标位置
    // @return none.
    public override void initData(LiveObject src, LiveObject dest, FixVector3 poOri, FixVector3 poDst)
    {
        base.initData(src, dest, poOri, poDst);

        Fix64 distance = FixVector3.Distance(poOri, poDst);

        speed = distance / speed;

    }

    //- 射击
    // 
    // @return none.
    public override void shoot()
    {
        m_fixv3LogicPosition = m_fixv3SrcPosition;
        List<FixVector3> list = new List<FixVector3>();
        list.Add(m_fixv3SrcPosition);
        list.Add(m_fixv3DestPosition);
        moveTo(list, delegate ()
        {
            doShootDest();
        });
    }

    //- 根据名字加载预制体
    // 
    // @param name 子弹的名字
    // @return none
    public override void createBody(string nameValue)
    {
        //加载子弹主体
        createFromPrefab("Bullet/SM_Arrow_01", this);

        //名字
        m_scName = nameValue;
    }

    //- 加载属性
    // 
    // @return none
    public override void loadProperties()
    {
        speed = (Fix64)15;
    }
}