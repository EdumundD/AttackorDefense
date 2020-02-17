//
// @brief: 延迟执行的事件
// @version: 1.0.0
// @author lhy
// @date: 2020/2/11
// 
// 
//

public class DelayDo : BaseAction
{

    Fix64 m_fixPlanTime = Fix64.Zero;
    Fix64 m_fixElapseTime = Fix64.Zero;

    public override void updateLogic()
    {
        m_fixElapseTime = m_fixElapseTime + GameData.g_fixFrameLen;

        if (m_fixElapseTime >= m_fixPlanTime)
        {
            removeSelfFromManager();

            if (null != actionCallBackFunction)
            {
                actionCallBackFunction();
            }
        }
    }

    public void init(Fix64 time, ActionCallback cb)
    {
        name = "delaydo";
        m_fixPlanTime = time;
        actionCallBackFunction = cb;
    }
}
