﻿//
// @brief: 房间列表刷新请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/19
// 
// 
//

using System.Collections.Generic;
using Common;   

public class ListRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        List<UserData> udList = new List<UserData>();
        List<int> unList = new List<int>();
        if (data == "0")
        {
            roomListPanel.LoadRoomItemSync(udList, unList);
            return;
        }
        string[] udArray = data.Split('|');
        foreach (string ud in udArray)
        {
            string[] strss = ud.Split(';');
            unList.Add(int.Parse(strss[1]));
            string[] strs = strss[0].Split(',');
            udList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
        }
        roomListPanel.LoadRoomItemSync(udList, unList);
    }
}
