//
// @brief: 塔待机状态
// @version: 1.0.0
// @author helin
// @date: 8/20/2018
// 
// 
//
using System.Collections;

public class TowerStandState : BaseState
{
    public static Fix64 s_fixTestCount = (Fix64)0;

    public static string s_scTestContent = "";
    public TowerStandState()
    {
        init();
    }

    //- 初始化函数.
    // Some description, can be over several lines.
    // @return value description.
    void init()
    {
        m_scName = "towerstand";
    }

    //- 创建时进入的初始化函数
    // 
    // @param args 附加的创建信息
    // @return none
    public override void onInit(LiveObject args)
    {
        //UnityTools.Log("towerstand.onInit");
        m_unit = args;
    }



    //- 进入该状态时调用的函数
    // 
    // @param args 附加的调用信息
    // @return none
    public override void onEnter(Fix64 args)
    {
        //UnityTools.Log("towerstand.onEnter");
        //播放待机动画
        //m_unit.playAnimation("Stand");
    }


    //- 退出该状态时调用的函数
    // 
    // @return none
    public override void onExit()
    {
        //UnityTools.Log("towerstand.onExit");
    }

    //- 处于该状态时每帧调用的函数
    // 
    // @return none
    public override void updateLogic()
    {
        //UnityTools.Log("towerstand.updateLogic");
        for (int i = 0; i < GameData.g_listSoldier.Count; i++)
        {
            var soldier = GameData.g_listSoldier[i];
            if (!soldier.m_bKilled && !soldier.m_dying)
            {
                Fix64 distance = FixVector3.Distance(m_unit.m_fixv3LogicPosition, soldier.m_fixv3LogicPosition);
                s_scTestContent += distance.ToString() + ",";

                //如果进入攻击范围
                if (distance <= (Fix64)m_unit.attackRange)
                {
                    s_fixTestCount += distance;
                    m_unit.lockedAttackUnit = soldier;

                    m_unit.addAttackingObj(soldier);
                    soldier.addAttackMeObj(m_unit);

                    m_unit.changeState("towerattack");

                    break;
                }
            }
            
        }
    }
}
