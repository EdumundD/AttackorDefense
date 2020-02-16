//
// @brief: 移动到指定位置的动作事件
// @version: 1.0.0
// @author helin
// @date: 8/20/2018
// 
// 
//
using System.Collections;
using System.Collections.Generic;

public class MoveTo : BaseAction
{
    int m_nowCount = 0;

    List<FixVector3> m_fixAllPositions = new List<FixVector3>();

    FixVector3 m_fixv3MoveDistance = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

    Fix64 m_fixMoveTime = Fix64.Zero;

    Fix64 m_fixMoveElpaseTime = Fix64.Zero;

    FixVector3 m_fixMoveStartPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

    FixVector3 m_fixEndPosition = new FixVector3(Fix64.Zero, Fix64.Zero, Fix64.Zero);

    public override void updateLogic()
    {
        bool actionOver = false;

        unit.m_gameObject.transform.LookAt(m_fixEndPosition.ToVector3());

        m_fixMoveTime = unit.speed;

        m_fixMoveElpaseTime += GameData.g_fixFrameLen;

        Fix64 timeScale = m_fixMoveElpaseTime / m_fixMoveTime;

        if (timeScale >= (Fix64)1)
        {
            timeScale = Fix64.Zero;
            m_fixMoveElpaseTime = Fix64.Zero;
            m_nowCount += 1;
            if (m_nowCount < m_fixAllPositions.Count - 1)
            {
                unit.m_fixv3LogicPosition = m_fixAllPositions[m_nowCount];
                m_fixMoveStartPosition = m_fixAllPositions[m_nowCount];
                m_fixEndPosition = m_fixAllPositions[m_nowCount + 1];
                m_fixv3MoveDistance = new FixVector3(m_fixEndPosition.x - m_fixMoveStartPosition.x, m_fixEndPosition.y - m_fixMoveStartPosition.y,
                    m_fixEndPosition.z - m_fixMoveStartPosition.z);
                //UnityTools.Log(m_nowCount + "|" + m_fixMoveStartPosition.x + "," + m_fixMoveStartPosition.z);
            }
            else
            {
                timeScale = (Fix64)1;
                actionOver = true;
            }
        }

        FixVector3 elpaseDistance = new FixVector3(m_fixv3MoveDistance.x * timeScale, m_fixv3MoveDistance.y * timeScale, m_fixv3MoveDistance.z * timeScale);
        FixVector3 newPosition = new FixVector3(m_fixMoveStartPosition.x + elpaseDistance.x, m_fixMoveStartPosition.y + elpaseDistance.y,
            m_fixMoveStartPosition.z + elpaseDistance.z);
        unit.m_fixv3LogicPosition = newPosition;

        if (actionOver)
        {
            removeSelfFromManager();

            if (null != actionCallBackFunction)
            {
                actionCallBackFunction();
            }
        }
    }

    public void init(BaseObject unitbody,List<FixVector3> positions, Fix64 time, ActionCallback cb)
    {
        name = "moveto";

        unit = unitbody;
        m_fixAllPositions = positions;
        unit.m_fixv3LogicPosition = positions[0];
        m_fixMoveStartPosition = positions[0];
        m_fixEndPosition = positions[1];
        m_fixMoveTime = time;
        if (m_fixMoveTime == Fix64.Zero)
        {
            m_fixMoveTime = (Fix64)0.1f;
        }

        actionCallBackFunction = cb;
        m_fixv3MoveDistance = new FixVector3(m_fixEndPosition.x - m_fixMoveStartPosition.x, m_fixEndPosition.y - m_fixMoveStartPosition.y,
            m_fixEndPosition.z - m_fixMoveStartPosition.z);
    }
}
