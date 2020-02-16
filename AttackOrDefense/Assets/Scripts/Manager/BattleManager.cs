using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


public class BattleManager : BaseManager {
    public BattleManager(GameFacade facade) : base(facade) { }

    //当前玩家帧输入数据（待发送）
    public FrameInput frameInput = new FrameInput();

    //存放从游戏开始收到的所有玩家帧输入数据的字典   Key:tick   Value:DoFrameInputs
    public Dictionary<int, List<FrameInput>[]> FrameInputs = new Dictionary<int, List<FrameInput>[]>();

    //从FrameInputs中取出来的一个待处理帧输入数据
    public List<FrameInput>[] curFrameInput = new List<FrameInput>[] { };

    //是否暂停(进入结算界面时, 战斗逻辑就会暂停)
    public bool m_bIsBattlePause = true;
    
    //是否结束
    public bool m_bIsBattleEnd = false;

    //帧同步核心逻辑对象
    public LockStepLogic m_lockStepLogic = null;

    //战斗日志
    string battleRecord = "";

    //是否已开战
    private bool m_bFireWar = false;

    //玩家人数
    public int m_playerCount = 1;

    //- 初始化
    // 
    // @param mt 上下文句柄
    // @return 元表
    public override void OnInit()
    {
        //初始化帧同步逻辑对象
        m_lockStepLogic = new LockStepLogic();
        m_lockStepLogic.setCallUnit(this);

        //游戏运行速度
        UnityTools.setTimeScale(1);


        //战斗暂停
        m_bIsBattlePause = true;
    }

    //- 由GameFacade一直调用
    // 
    // @return none
    public override void Update()
    {
        updateLogic();
    }

    //- 主循环
    // 
    // @return none
    public void updateLogic()
    {
        //如果战斗逻辑暂停则不再运行
        if (m_bIsBattlePause)
        {
            return;
        }

        //调用帧同步逻辑
        m_lockStepLogic.updateLogic();
    }

    //- 战斗逻辑
    // 
    // @return none
    public void frameLockLogic()
    {
        //如果是回放模式
        if (GameData.g_bRplayMode)
        {
            //检测回放事件
            checkPlayBackEvent(GameData.g_uGameLogicFrame);
        }
        
        recordLastPos();


        try
        {
            for (int i = 0; i < m_playerCount; i++)
            {
                for(int j = 0; j < curFrameInput[i].Count; j++)
                {
                    if (curFrameInput[i][j].buildTower.towerId != -1)
                    {
                        //建造防御塔
                        createTowers(new FixVector3((Fix64)curFrameInput[i][j].buildTower.buildPosition.x, (Fix64)2,
                            (Fix64)curFrameInput[i][j].buildTower.buildPosition.y), curFrameInput[i][j].buildTower.towerId);

                        UnityTools.Log("生成防御塔:" + curFrameInput[i][j].buildTower.buildPosition.x + "," + 1 +
                            "," + curFrameInput[i][j].buildTower.buildPosition.y + " ID = " + curFrameInput[i][j].buildTower.towerId);
                    }
                    if (curFrameInput[i][j].createBarrack.barrackId != -1)
                    {
                        //生成兵营
                        createBarrack(new FixVector3((Fix64)curFrameInput[i][j].createBarrack.createPosition.x, (Fix64)1,
                            (Fix64)curFrameInput[i][j].createBarrack.createPosition.y), curFrameInput[i][j].createBarrack.barrackId, curFrameInput[i][j].clientID);

                        UnityTools.Log("生成兵营:" + curFrameInput[i][j].createBarrack.createPosition.x + "," + 1 +
                            "," + curFrameInput[i][j].createBarrack.createPosition.y + " ID = " + curFrameInput[i][j].createBarrack.barrackId);
                    }
                    if (curFrameInput[i][j].createSoldier.soldierId != -1)
                    {
                        //生成士兵
                        createSoldier(curFrameInput[i][j].createSoldier.path, curFrameInput[i][j].createSoldier.soldierId);

                        UnityTools.Log("生成士兵:" + curFrameInput[i][j].createSoldier.path[0].x + "," + 1 +
                            "," + curFrameInput[i][j].createSoldier.path[0].y + " id = " + curFrameInput[i][j].createSoldier.soldierId);
                    }
                }
            }
        }catch(Exception e)
        {
            UnityTools.Log(e);
        }

        //动作管理器update
        GameData.g_actionMainManager.updateLogic();

        //塔 update
        for (int i = 0; i < GameData.g_listTower.Count; i++)
        {
            GameData.g_listTower[i].updateLogic();
        }

        //子弹 update
        for (int i = 0; i < GameData.g_listBullet.Count; i++)
        {
            GameData.g_listBullet[i].updateLogic();
        }
        
        //兵营 update
        for (int i = 0; i < GameData.g_listBarrack.Count; i++)
        {
            GameData.g_listBarrack[i].updateLogic();
        }

        //士兵 update
        for (int i = 0; i < GameData.g_listSoldier.Count; i++)
        {
            GameData.g_listSoldier[i].updateLogic();
        }

        if (m_bFireWar && m_bIsBattleEnd)
        {
            stopBattle();
        }
    }

    //- 记录最后的位置
    // 生成的时候防止插值运算导致的抖动
    // @return none.
    void recordLastPos()
    {
        //子弹
        for (int i = 0; i < GameData.g_listBullet.Count; i++)
        {
            GameData.g_listBullet[i].recordLastPos();
        }

        //士兵
        for (int i = 0; i < GameData.g_listSoldier.Count; i++)
        {
            GameData.g_listSoldier[i].recordLastPos();
        }
    }


    //- 更新各种对象绘制的位置
    // 包括怪,子弹等等,因为塔和兵营的位置是固定的,所以不需要实时刷新,提升效率
    // @return none
    public void updateRenderPosition(float interpolation)
    {
        //子弹
        for (int i = 0; i < GameData.g_listBullet.Count; i++)
        {
            GameData.g_listBullet[i].updateRenderPosition(interpolation);
        }

        //士兵
        for (int i = 0; i < GameData.g_listSoldier.Count; i++)
        {
            GameData.g_listSoldier[i].updateRenderPosition(interpolation);
        }
    }
    
    


    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <returns></returns>
    public void startBattle()
    {
        GameFacade.Instance.inputMono.isStartBattle = true;

        GameFacade.Instance.RefreshInput();

        //重置随机数
        GameData.g_srand = new SRandom(1000);

        m_bIsBattlePause = false;
        m_lockStepLogic.init();

        //读取玩家操作数据,为回放做准备
        if (GameData.g_bRplayMode)
        {
            loadUserCtrlInfo();
        }

        GameData.g_uGameLogicFrame = 0;

    }

    /// <summary>
    /// 停止战斗
    /// </summary>
    /// <returns></returns>
    public void stopBattle()
    {
        UnityTools.Log("end logic frame: " + GameData.g_uGameLogicFrame);
        UnityTools.Log("s_fixTestCount: " + TowerStandState.s_fixTestCount);
        UnityTools.Log("s_scTestContent: " + TowerStandState.s_scTestContent);

        m_bFireWar = false;

        //记录关键事件
        if (!GameData.g_bRplayMode)
        {
            GameData.battleInfo info = new GameData.battleInfo();
            info.uGameFrame = GameData.g_uGameLogicFrame;
            info.sckeyEvent = "stopBattle";
            GameData.g_listUserControlEvent.Add(info);
        }

        gameEnd();
    }

    /// <summary>
    /// 回放战斗录像
    /// </summary>
    /// <returns></returns>
    public void replayVideo()
    {
        GameData.g_bRplayMode = true;
        GameData.g_uGameLogicFrame = 0;
        startBattle();
    }

    /// <summary>
    /// 创建塔
    /// </summary>
    /// <returns></returns>
    public void createTowers(FixVector3 createPosition, int towerID)
    {
        var tower = GameData.g_towerFactory.createTower(towerID);
        tower.m_fixv3LogicPosition = createPosition;
        tower.updateRenderPosition(0f);
    }
    
    /// <summary>
     /// 创建兵营
     /// </summary>
     /// <returns></returns>
    public void createBarrack(FixVector3 createPosition, int barrackID, int clientID)
    {
        var barrack = GameData.g_soldierFactory.createBarrack(barrackID,clientID);
        barrack.m_fixv3LogicPosition = createPosition;
        barrack.updateRenderPosition(0f);
    }

    /// <summary>
    /// 创建士兵
    /// </summary>
    /// <returns></returns>
    public void createSoldier(List<FixVector3> createPosition, int soldierID)
    {
        m_bFireWar = true;

        var soldier = GameData.g_soldierFactory.createSoldier(soldierID);
        soldier.m_fixv3LogicPosition = createPosition[0];
        soldier.updateRenderPosition(0f);

        soldier.moveTo(createPosition);

        //记录关键事件
        if (!GameData.g_bRplayMode)
        {
            GameData.battleInfo info = new GameData.battleInfo();
            info.uGameFrame = GameData.g_uGameLogicFrame;
            info.sckeyEvent = "createSoldier";
            GameData.g_listUserControlEvent.Add(info);
        }
    }

    //- 读取玩家的操作信息
    // 在战斗开始时，如果是回放模式，则读取操作信息，添加到GameData中的回放事件列表中
    // @return none
    void loadUserCtrlInfo()
    {
        GameData.g_listPlaybackEvent.Clear();

        string content = battleRecord;

        string[] contents = content.Split('$');

        for (int i = 0; i < contents.Length - 1; i++)
        {
            string[] battleInfo = contents[i].Split(',');

            GameData.battleInfo info = new GameData.battleInfo();

            info.uGameFrame = int.Parse(battleInfo[0]);
            info.sckeyEvent = battleInfo[1];

            GameData.g_listPlaybackEvent.Add(info);
        }
    }

    //- 检测回放事件
    // 如果GameDate中回放事件列表有回放事件则进行回放
    // @param gameFrame 当前的游戏帧
    // @return none
    void checkPlayBackEvent(int gameFrame)
    {
        if (GameData.g_listPlaybackEvent.Count > 0)
        {
            for (int i = 0; i < GameData.g_listPlaybackEvent.Count; i++)
            {
                GameData.battleInfo v = GameData.g_listPlaybackEvent[i];

                if (gameFrame == v.uGameFrame)
                {
                    if (v.sckeyEvent == "createSoldier")
                    {
                        //createSoldier(new FixVector3((Fix64)9, (Fix64)1, (Fix64)1),1);
                    }
                }
            }
        }
    }

    //- 暂停战斗逻辑
    // 游戏结束时调用
    // @return none.
    void pauseBattleLogic()
    {
        m_bIsBattlePause = true;
    }

    public void gameEnd()
    {
        if (!m_bIsBattlePause)
        {
            //UnityTools.setTimeScale(1);

            //销毁战场上的所有对象
            //塔
            for (int i = GameData.g_listTower.Count - 1; i >= 0; i--)
            {
                GameData.g_listTower[i].killSelf();
            }

            //子弹
            for (int i = GameData.g_listBullet.Count - 1; i >= 0; i--)
            {
                GameData.g_listBullet[i].killSelf();
            }

            //士兵
            for (int i = GameData.g_listSoldier.Count - 1; i >= 0; i--)
            {
                GameData.g_listSoldier[i].killSelf();
            }

            if (!GameData.g_bRplayMode)
            {
                recordBattleInfo();
#if _CLIENTLOGIC_
                //SimpleSocket socket = new SimpleSocket();
                //socket.Init();
                //socket.sendBattleRecordToServer(UnityTools.playerPrefsGetString("battleRecord"));
#endif
            }

            pauseBattleLogic();

            GameData.g_bRplayMode = false;

            GameData.release();
        }
    }

    //- 记录战斗信息(回放时使用)
    // 
    // @return none
    void recordBattleInfo()
    {
        if (false == GameData.g_bRplayMode)
        {

            //记录战斗数据
            string content = "";
            for (int i = 0; i < GameData.g_listUserControlEvent.Count; i++)
            {
                GameData.battleInfo v = GameData.g_listUserControlEvent[i];
                //出兵
                if (v.sckeyEvent == "createSoldier")
                {
                    content += v.uGameFrame + "," + v.sckeyEvent + "$";
                }
            }

            UnityTools.playerPrefsSetString("battleRecord", content);
            GameData.g_listUserControlEvent.Clear();
        }
    }

    public void setBattleRecord(string record)
    {
        battleRecord = record;
    }

    //- 释放资源
    // 
    // @return none
    void release()
    {

    }
}

