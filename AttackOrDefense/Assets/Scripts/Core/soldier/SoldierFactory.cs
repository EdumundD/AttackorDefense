//
// @brief: 士兵工厂类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/6
// 
// 
//
using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;


public class SoldierFactory
{
    public List<SoldierData> soldierDataList;

    public SoldierFactory()
    {
        soldierDataList = new List<SoldierData>();
        soldierDataList.Add(new SoldierData("1,Metalon,10,200,5,100"));
        soldierDataList.Add(new SoldierData("2,MechanicalGolem,10,200,10,100"));
    }

    //- 创建兵营
    // 
    // @return 创建出的兵营.
    public BaseBarrack createBarrack(int barrackID,int clientID)
    {
        BaseBarrack barrack = new BaseBarrack();

        if (barrackID == 1)
        {
            barrack = new MetalonBarrack();

            //开启HighLighter脚本
            barrack.m_gameObject.GetComponent<Highlighter>().enabled = true;
        }
        if (barrackID == 2)
        {
            barrack = new MechanicalGolemBarrack();
            
            //开启HighLighter脚本
            barrack.m_gameObject.GetComponent<Highlighter>().enabled = true;
        }
        if (barrackID == 3)
        {

        }
        if (barrackID == 4)
        {

        }
        if(clientID == (int)GameFacade.Instance.URoleType)
            barrack.changeState("makingsoldiers", (Fix64)3);

        GameData.g_listBarrack.Add(barrack);


        //立即记录最后的位置,否则通过vector3.lerp来进行移动动画时会出现画面抖动的bug
        barrack.recordLastPos();

        return barrack;
    }

    //- 创建士兵
    // 
    // @return 创建出的士兵.
    public BaseSoldier createSoldier(int soldierID)
    {
        BaseSoldier soldier = new BaseSoldier();

        if (soldierID == 1)
        {
            soldier = new Metalon();
        }
        if (soldierID == 2)
        {
            soldier = new MechanicalGolem();
        }
        if (soldierID == 3)
        {

        }
        if (soldierID == 4)
        {

        }

        soldier.changeState("soldiermove");

        GameData.g_listSoldier.Add(soldier);

        //立即记录最后的位置,否则通过vector3.lerp来进行移动动画时会出现画面抖动的bug
        soldier.recordLastPos();

        return soldier;
    }
}
