using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Common;
using System.Linq;

public class GameFacade : MonoBehaviour
{

    //单例模式
    private static GameFacade _instance;
    private static object Singleton_Lock = new object();
    public static GameFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Singleton_Lock)
                {
                    if (_instance == null)
                    {
                        _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();
                    }
                }
            }
            return _instance;
        }
    }
    //InputMono
    public InputMono inputMono;

    private float timeScale = 1f;

    //所有Manager
    private UIManager _uiManager;
    private CameraManager _cameraManager;
    private AudioManager _audioManager;
    private RequestManager _requestManager;
    private ClientManager _clientManager;
    private BuildManager _buildManager;
    private BattleManager _battleManager;
    private UserData userData;

    //异步处理判断条件
    private bool isEnterPlaying = false;
    private bool isHandleReponse = false;
    private bool isFollowRole = false;
    private bool isBuildTower = false;
    private bool isCreatSolider = false;
    private bool isCalculateDelay = false;
    private ActionCode ac = ActionCode.None;
    private string str = null;
    private int GameLogicFrame = -1;

    //角色类型
    private RoleType uroleType = RoleType.None;
    public RoleType URoleType
    {
        get { return uroleType; }
        set { uroleType = value; }
    }

    //test
    private UnityEngine.UI.Text ShowLogicFrame;

    private void Awake()
    {
        Instance.InitManager();
    }
    void Update()
    {
        UpdateManage();

        //开始游戏
        if (isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
        //派发回应
        if (isHandleReponse)
        {
            HandleReponse(ac, str);
            isHandleReponse = false;
            ac = ActionCode.None;
            str = null;
        }
        //设置摄像机到固定位置
        if (isFollowRole)
        {
            FollowRole();
            isFollowRole = false;
        }
        //计算延迟
        if (isCalculateDelay)
        {
            var nowtime = Time.realtimeSinceStartup;
            _battleManager.m_lockStepLogic.m_delay = nowtime - _battleManager.m_lockStepLogic.m_calculateDelay;
            _battleManager.m_lockStepLogic.m_delay = 0.066f;
            _battleManager.m_lockStepLogic.m_isStartCalculateDelay = false;
        }
        //动态设置timeScale,减少卡顿和延迟
        if(_battleManager.m_lockStepLogic.m_fAccumilatedTime < 0.3f && _battleManager.m_lockStepLogic.m_fAccumilatedTime > 0)
        {
            timeScale = 0.3f;
        }
        else
        {
            if (_battleManager.m_lockStepLogic._maxServerFrameIdx != 0 && GameData.g_uGameLogicFrame != 0 && _battleManager.m_lockStepLogic._maxServerFrameIdx > GameData.g_uGameLogicFrame)
                timeScale = Mathf.Pow(_battleManager.m_lockStepLogic._maxServerFrameIdx - GameData.g_uGameLogicFrame, 0.5f) / 2;
        }
        UnityTools.setTimeScale(timeScale);

        //testUI
        ShowLogicFrame.text = "当前帧:" + GameData.g_uGameLogicFrame + "\n"
            + "服务器最大帧数:" + _battleManager.m_lockStepLogic._maxServerFrameIdx + "\n"
            + "延迟:" + _battleManager.m_lockStepLogic.m_delay * 1000 + "ms" + "\n"
            + "TimeScale:" + timeScale + "\n";
        //+ "接收到的数据数量:" + _battleManager.FrameInputs.Count + "\n"
        //+ "UserType:" + URoleType.ToString() + "\n"
        //+ "两帧之间的时间差:" + _battleManager.m_lockStepLogic.m_fInterpolation + "\n"
    }
    private void OnDestroy()
    {
        DestoryManager();
    }

    private void InitManager()
    {
        inputMono = gameObject.AddComponent<InputMono>();

        _uiManager = new UIManager(this);
        _audioManager = new AudioManager(this);
        _cameraManager = new CameraManager(this);
        _requestManager = new RequestManager(this);
        _clientManager = new ClientManager(this);
        _buildManager = new BuildManager(this);
        _battleManager = new BattleManager(this);


        _uiManager.OnInit();
        _audioManager.OnInit();
        _cameraManager.OnInit();
        _requestManager.OnInit();
        _clientManager.OnInit();
        _buildManager.OnInit();
        _battleManager.OnInit();

        //test
        ShowLogicFrame = GameObject.Find("Canvas/Text").GetComponent<UnityEngine.UI.Text>();
    }

    private void UpdateManage()
    {
        _uiManager.Update();
        _audioManager.Update();
        _cameraManager.Update();
        _requestManager.Update();
        _clientManager.Update();
        _buildManager.Update();
        _battleManager.Update();
    }

    private void DestoryManager()
    {
        _uiManager.OnDestroy();
        _audioManager.OnDestroy();
        _cameraManager.OnDestroy();
        _requestManager.OnDestroy();
        _clientManager.OnDestroy();
        _buildManager.OnDestroy();
        _battleManager.OnDestroy();
    }

    //- 添加请求类型
    //
    // @param actionCode 方法码
    // @param baseRequest 请求基类
    // @return none
    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        _requestManager.AddRequest(actionCode, baseRequest);
    }

    //- 移除请求类型
    //
    // @param actionCode 方法码
    // @return none
    public void RemoveRequest(ActionCode actionCode)
    {
        _requestManager.RemoveRequest(actionCode);
    }

    //- 发送请求
    //
    // @param actionCode 方法码
    // @return none
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        _clientManager.SendRequest(requestCode, actionCode, data);
    }

    //- 派发回应消息
    //
    // @param actionCode 方法码
    // @parm data 数据
    // @return none
    public void HandleReponse(ActionCode actionCode, string data)
    {
        _requestManager.HandleReponse(actionCode, data);
    }

    //- 显示消息框
    //
    // @param msg 消息内容
    // @return none
    public void ShowMessage(string msg)
    {
        _uiManager.ShowMessage(msg);
    }

    //- 播放背景音乐
    //
    // @param soundName 背景音乐名称
    // @return none
    public void PlayBgSound(string soundName)
    {
        _audioManager.PlayBgSound(soundName);
    }

    //- 播放音效
    //
    // @param soundName 音效名称
    // @return none
    public void PlayNormalSound(string soundName)
    {
        _audioManager.PlayNormalSound(soundName);
    }

    //- 异步调用开始战斗
    // 
    // @return none
    public void EnterPlayingSync(int playerCount)
    {
        _battleManager.m_playerCount = playerCount;
        isEnterPlaying = true;
    }

    //- 开始战斗
    // 
    // @return none
    private void EnterPlaying()
    {
        GameData.g_bRplayMode = false;
        _battleManager.startBattle();
        _uiManager.PushPanelSync(UIPanelType.TestBattle);
    }

    //- 设置用户信息
    //
    // @param userData 用户信息
    // @return none
    public void SetUserData(UserData userData)
    {
        this.userData = userData;
    }

    //- 获取用户信息
    //
    // @return none
    public UserData GetUserData()
    {
        return userData;
    }

    //- 开始建造模式
    //
    // @param buildingType 建筑类型
    // @return none
    public void StartBuilding(BuildingType buildingType,bool isBuildTower)
    {
        _buildManager.StartBuilding(buildingType,isBuildTower);
    }

    //- 跟随角色
    //
    // @return none
    public void FollowRole()
    {
        _cameraManager.FollowRole();
    }

    //- 异步跟随角色
    //
    // @return none
    public void FollowRoleSync()
    {
        isFollowRole = true;
    }


    public void EndBattle()
    {
        //_battleManager.m_bIsBattleEnd = true;
    }
    
    public void ReplayVideo()
    {
        //_battleManager.setBattleRecord(UnityTools.playerPrefsGetString("battleRecord"));
        //_battleManager.replayVideo();
    }

    //- 刷新输入缓存
    // 使得BattleManager不会发送已发送过的帧输入数据
    // @return none
    public void RefreshInput()
    {
        inputMono.isRefresh = true;
    }

    //- 提交输入缓存
    // 修改BattleManager等待发送的帧输入数据
    // @return none
    public void SendFrameInput(FrameInput frameInput)
    {
        _battleManager.frameInput = frameInput;
    }

    //- 添加待处理帧输入数据
    // 将服务器发送过来的当前帧房间内所有玩家的输入数据实例化，并添加到BattleManager的待处理输入帧数据列表中
    // @return none
    public void AddFrameInput(string data)
    {
        //43!1$0 / -1,-1 | -1 / -1,-1 | -1 / -1,-1 | -1 / -1 %#
        //datas: tick ! 一号玩家帧输入数据(输入数据.Count $ 输入数据1 % 输入数据2 % ..) # 二号玩家帧输入数据 # 三号玩家帧输入数据 # 四号玩家帧输入数据
        //输入数据 clientID + "/" +
        //buildTower.buildPosition.x + "," + buildTower.buildPosition.y + "|" + buildTower.towerId + "/" +
        //createBarrack.createPosition.x + "," + createBarrack.createPosition.y + "|" + createBarrack.barrackId + "/" +
        //createSoldier.createPosition.x + "," + createSoldier.createPosition.y + "|" + createSoldier.soldierId + "/" +
        //objectDeath.m_scId;
        List<FrameInput>[] frameInput = new List<FrameInput>[4];
        int tick = 0;
        try
        {
            tick = Int32.Parse(data.Split('!')[0]);

            //更新服务器最大帧数
            if (tick > _battleManager.m_lockStepLogic._maxServerFrameIdx) _battleManager.m_lockStepLogic._maxServerFrameIdx = tick;

            string allPlayerFrameInput = data.Split('!')[1];

            for (int m = 0; m < _battleManager.m_playerCount; m++)
            {
                string playerFrameInputs = allPlayerFrameInput.Split('#')[m];
                int frameInputCount = Int32.Parse(playerFrameInputs.Split('$')[0]);
                List<FrameInput> playerFrameinputList = new List<FrameInput>();
                for (int i = 0; i < frameInputCount; i++)
                {
                    playerFrameinputList.Add(new FrameInput(tick, playerFrameInputs.Split('$')[1].Split('%')[i]));
                }
                frameInput[m] = playerFrameinputList;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        ////计算延迟
        if (tick == _battleManager.m_lockStepLogic.m_sentTick)
        {
            
        }

        _battleManager.FrameInputs.Add(tick, frameInput);
    }
}
