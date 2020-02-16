using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttackOrDefenseServer.Model;
using Common;

namespace AttackOrDefenseServer.Servers
{
    class LockStepLogic
    {
        public Room _room = null;

        //房间人数
        int playerCount = 1;

        //待发送字符串
        string datas = "";

        //累计运行的时间
        float m_fAccumilatedTime = 0;

        //下一个逻辑帧的时间
        float m_fNextGameTime = 0;

        //预定的每帧的时间长度
        float m_fFrameLen = (float)Fix64.FromRaw(273);

        //游戏的逻辑帧
        int g_uGameLogicFrame = 0;


        //保存从游戏开始以来所有的帧输入操作 
        //key:tick
        //List<FrameInput>[m]
        //m:行 表示有几个玩家 
        //List<FrameInput>:列 表示这一帧玩家的输入操作（如果网络有波动，导致上一帧的输入数据这一帧才到达，则和这一帧数据一起发送）
        private Dictionary<int, List<FrameInput>[]> _tickInputs = new Dictionary<int, List<FrameInput>[]>();

        private List<FrameInput> player1_tickInputs;
        private List<FrameInput> player2_tickInputs;
        private List<FrameInput> player3_tickInputs;
        private List<FrameInput> player4_tickInputs;

        public void Init()
        {
            playerCount = _room.GetRoomPlayerCount();
            m_fAccumilatedTime = 0;
            m_fNextGameTime = 0;
            m_fFrameLen = (float)Fix64.FromRaw(273);
            g_uGameLogicFrame = 0;
            if (playerCount > 0) player1_tickInputs = new List<FrameInput>();
            if (playerCount > 1) player2_tickInputs = new List<FrameInput>();
            if (playerCount > 2) player3_tickInputs = new List<FrameInput>();
            if (playerCount > 3) player4_tickInputs = new List<FrameInput>();
        }
        public void Update()
        {
            /**************以下是帧同步的核心逻辑*********************/
            m_fAccumilatedTime = m_fAccumilatedTime + 0.015f;
            //如果真实累计的时间超过游戏帧逻辑原本应有的时间,则循环执行逻辑,确保整个逻辑的运算不会因为帧间隔时间的波动而计算出不同的结果
            while (m_fAccumilatedTime > m_fNextGameTime)
            {
                DoUpdate();
                //计算下一个逻辑帧应有的时间
                m_fNextGameTime += m_fFrameLen;

            }
            /**************帧同步的核心逻辑完毕*********************/
        }
        public void DoUpdate()
        {
            CheckInput();
        }
        private void CheckInput()
        {
            Console.WriteLine("当前帧：" + g_uGameLogicFrame + " tickInputsCount: " + _tickInputs.Count + "_room.Count:" + _room.GetRoomPlayerCount());
            if (_tickInputs.TryGetValue(g_uGameLogicFrame, out var inputs))
            {
                Console.WriteLine("LockStepLogic 81 行出问题，不应该执行到这里");
            }
            else
            {
                List<FrameInput>[] frameInputs = new List<FrameInput>[playerCount];
                for(int i = 0; i < playerCount; i++)
                {
                    List<FrameInput> playerFrames = null;
                    if (i == 0) playerFrames = player1_tickInputs;
                    if (i == 1) playerFrames = player2_tickInputs;
                    if (i == 2) playerFrames = player3_tickInputs;
                    if (i == 3) playerFrames = player4_tickInputs;
                    List<FrameInput> frameInputList = new List<FrameInput>();
                    for(int j = playerFrames.Count - 1; j >= 0; j--)
                    {
                        if (playerFrames[j].tick <= g_uGameLogicFrame)
                        {
                            playerFrames[j].tick = g_uGameLogicFrame;
                            frameInputList.Add(playerFrames[j]);
                            playerFrames.Remove(playerFrames[j]);
                        }

                    }
                    //如果没有输入数据，设为默认数据
                    if (frameInputList.Count == 0)
                    {
                        frameInputList.Add(new FrameInput(i+1));
                    }
                    frameInputs[i] = frameInputList;
                }
                //在服务器端保存一份数据，方便断线重连
                _tickInputs.Add(g_uGameLogicFrame, frameInputs);

                //datas: tick ! 房间人数 @  一号玩家帧输入数据(输入数据.Count $ 输入数据1 % 输入数据2 % ..) # 二号玩家帧输入数据 # 三号玩家帧输入数据 # 四号玩家帧输入数据
                datas = "";
                datas += g_uGameLogicFrame + "!";
                for (int i = 0; i < playerCount; i++)
                {
                    datas += frameInputs[i].Count + "$";
                    foreach(FrameInput fi in frameInputs[i])
                    {
                        datas += fi.getString() + "%";
                    }
                    datas += "#";
                }
                Console.WriteLine("发送一条消息 : " + datas);
                _room?.BroadcastMessage(null, ActionCode.LockStepLogic, datas);
                //游戏逻辑帧自增
                g_uGameLogicFrame += 1;
            }
        }
        public void OnFrameInput(Client client,string data)
        {
            //Console.WriteLine(data);
            FrameInput input = new FrameInput(data);
            switch (input.clientID)
            {
                case 1: player1_tickInputs.Add(input);break;
                case 2: player2_tickInputs.Add(input); break;
                case 3: player3_tickInputs.Add(input); break;
                case 4: player4_tickInputs.Add(input); break;
            }
            //FrameInput[] inputs;
            //if(!_tickInputs.TryGetValue(input.tick,out inputs))
            //{
            //    inputs = new FrameInput[_room.GetRoomPlayerCount()];
            //    _tickInputs.Add(input.tick,inputs);
            //}
            //inputs[input.clientID - 1] = input;
        }
    }
}


