//
// @brief: 公用数据类
// @version: 1.0.0
// @author lhy
// @date: 1/2/2020
// 
// 
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameData
{
    //所有兵营的队列
    public static List<BaseBarrack> g_listBarrack = new List<BaseBarrack>();

    //所有士兵的队列
    public static List<BaseSoldier> g_listSoldier = new List<BaseSoldier>();

    //所有塔的队列
    public static List<BaseTower> g_listTower = new List<BaseTower>();

    //所有子弹的队列
    public static List<BaseBullet> g_listBullet = new List<BaseBullet>();

    //所有操作事件的队列
    public static List<battleInfo> g_listUserControlEvent = new List<battleInfo>();

    //所有回放事件的队列
    public static List<battleInfo> g_listPlaybackEvent = new List<battleInfo>();

    //所有土地的队列
    public static List<LandData> g_listLand = new List<LandData>();
    
    //所有用于建造土地的二维数组
    public static int[,] g_buildmap = null;

    //所有道路的队列
    public static List<Point> g_listRoad = new List<Point>();

    //所有道路的二维数组
    public static int[,] g_map = null;

    //预定的每帧的时间长度
    public static Fix64 g_fixFrameLen = Fix64.FromRaw(273);

    //游戏的逻辑帧
    public static int g_uGameLogicFrame = 0;

    //是否为回放模式
    public static bool g_bRplayMode = false;

    //塔工厂
    public static TowerFactory g_towerFactory = new TowerFactory();

    //士兵工厂
    public static SoldierFactory g_soldierFactory = new SoldierFactory();

    //action主管理器(用于管理各liveobject内部的独立actionManager)
    public static ActionMainManager g_actionMainManager = new ActionMainManager();

    //子弹管理器
    public static BulletFactory g_bulletManager = new BulletFactory();

    //战斗是否结束
    public static bool g_bBattleEnd = false;

    //随机数对象
    public static SRandom g_srand = new SRandom(1000);

    public struct battleInfo
    {
        public int uGameFrame;
        public string sckeyEvent;
    }

    //- 获取地图
    // 用于AStar寻路算法使用
    // @return none
    public static int[,] GetMap()
    {
        if (null == g_map)
        {
            g_map = new int[58, 58];
            foreach (Point pi in g_listRoad)
            {
                g_map[pi.X, pi.Y] = 1;
            }
        }
        return g_map;
    }
    //- 获取建造地图地图
    // 用于BuildManager使用
    // @return none
    public static int[,] GetBuildMap()
    {
        if (null == g_buildmap)
        {
            g_buildmap = new int[58, 58];
            foreach (LandData ld in g_listLand)
            {
                if (ld.isForTower)
                    g_buildmap[(int)ld.localPosition.x, (int)ld.localPosition.z] = 1;
                if (ld.isForBarrack)
                    g_buildmap[(int)ld.localPosition.x, (int)ld.localPosition.z] = 2;
                if (ld.isBuild)
                    g_buildmap[(int)ld.localPosition.x, (int)ld.localPosition.z] = 3;
            }
        }
        return g_buildmap;
    }
    //- 释放资源
    // 
    // @return none
    public static void release()
    {
        g_listPlaybackEvent.Clear();

        g_listUserControlEvent.Clear();

        GameData.g_actionMainManager.release();
    }
}

