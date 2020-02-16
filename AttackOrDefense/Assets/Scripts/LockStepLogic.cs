//
// @brief: 帧同步核心逻辑
// @version: 1.0.0
// @author lhy
// @date: 1/20/2020
// 
// 
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

public class LockStepLogic {
    //累计运行的时间
    public float m_fAccumilatedTime = 0;

    //下一个逻辑帧的时间
    float m_fNextGameTime = 0;

    //预定的每帧的时间长度
    float m_fFrameLen;

    //挂载的逻辑对象
    BattleManager m_callUnit = null;

    //两帧之间的时间差
    public float m_fInterpolation = 0;

    //用于计算延迟 判断是否开始计时
    public bool m_isStartCalculateDelay = false;

    //用于计算延迟 记录发送时的帧数
    public int m_sentTick = 0;

    //用于计算延迟 记录发送时的时间
    public float m_calculateDelay = 0;

    //延迟
    public float m_delay = 0;

    //预测帧数
    int predictTickCount = 2;

    //当前发送帧数
    int inputTick = 0;

    //服务器最大帧数
    public int _maxServerFrameIdx = 0;

    public LockStepLogic()
    {
        init();
    }
    public void init()
    {
        m_fFrameLen = (float)GameData.g_fixFrameLen;

        m_fAccumilatedTime = 0;

        m_fNextGameTime = 0;

        m_fInterpolation = 0;
    }

    public void updateLogic()
    {
        float deltaTime = 0;
#if _CLIENTLOGIC_
        deltaTime = UnityEngine.Time.deltaTime;
#else
        deltaTime = 0.1f;
#endif
        /**************以下是帧同步的核心逻辑*********************/
        m_fAccumilatedTime = m_fAccumilatedTime + deltaTime;
        //如果真实累计的时间超过游戏帧逻辑原本应有的时间,则循环执行逻辑,确保整个逻辑的运算不会因为帧间隔时间的波动而计算出不同的结果
        while (m_fAccumilatedTime > m_fNextGameTime)
        {
            if (!GameData.g_bRplayMode)
            {
                SendInput();
            }

            if (GetFrame(GameData.g_uGameLogicFrame) == null)
            {
                UnityTools.Log("GetFrame(GameData.g_uGameLogicFrame) == nulls");

            }
            else
            {
                //更新BattleManager中的待处理帧输入数据
                m_callUnit.curFrameInput = GetFrame(GameData.g_uGameLogicFrame);

                //运行与游戏相关的具体逻辑
                m_callUnit.frameLockLogic();

                //计算下一个逻辑帧应有的时间
                m_fNextGameTime += m_fFrameLen;

                //游戏逻辑帧自增
                GameData.g_uGameLogicFrame += 1;
            }
        }

        //计算真实两帧的时间差,用于运行补间动画
        m_fInterpolation = (m_fAccumilatedTime + m_fFrameLen - m_fNextGameTime) / m_fFrameLen;
        //UnityTools.Log("m_fInterpolation:" + m_fInterpolation + " m_fAccumilatedTime:" + m_fAccumilatedTime + " m_fNextGameTime:" + m_fNextGameTime + " m_fFrameLen：" + m_fFrameLen);
        //更新绘制位置
        m_callUnit.updateRenderPosition(m_fInterpolation);
        /**************帧同步的核心逻辑完毕*********************/
    }

    //- 设置调用的Manager
    // 
    // @param unit 调用的Manager
    // @return none
    public void setCallUnit(BattleManager unit)
    {
        m_callUnit = unit;
    }

    //- 发送帧输入数据给服务器
    // 每个逻辑帧发送一次，当发送的数据帧数大于缓存帧数+服务器最大帧数，则暂不发送
    // @return none
    public void SendInput() {

        //计算延迟
        if(m_isStartCalculateDelay == false)
        {
            //m_calculateDelay = UnityEngine.Time.realtimeSinceStartup;
            m_calculateDelay = m_fAccumilatedTime;
            m_isStartCalculateDelay = true;
            m_sentTick = inputTick;
        }  

        predictTickCount = 2; //Mathf.Clamp(Mathf.CeilToInt(pingVal / 30), 1, 20);
        if (inputTick > predictTickCount + _maxServerFrameIdx)
        {
            return;
        }
        m_callUnit.frameInput.tick = inputTick;
        m_callUnit.frameInput.clientID = (int)GameFacade.Instance.URoleType;
        GameFacade.Instance.SendRequest(RequestCode.Game, ActionCode.LockStepLogic, m_callUnit.frameInput.getString());
        //UnityTools.Log(frameInput.getString() + "|" + facade.URoleType + "|" + (int)facade.URoleType);
        GameFacade.Instance.RefreshInput();
        inputTick++;
    }

    //- 获取BattleManager中对应的帧输入数据
    // 
    // @param tick 
    // @return none
    public List<FrameInput>[] GetFrame(int tick)
    {
        if (m_callUnit.FrameInputs.Count >= tick)
        {
            if (m_callUnit.FrameInputs.TryGetValue(tick,out var frames))
            {
                return frames;
            }
        }

        return null;
    }
}

