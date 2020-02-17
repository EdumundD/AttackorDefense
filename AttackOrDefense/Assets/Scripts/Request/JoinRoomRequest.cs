//
// @brief: 加入房间请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/22
// 
// 
//

using Common;

public class JoinRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-');
        string[] strs2 = strs[0].Split(',');
        RoleType roleType = RoleType.None;
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        UserData ud1 = null;
        UserData ud2 = null;
        UserData ud3 = null;
        UserData ud4 = null;

        if (returnCode == ReturnCode.Success)
        {
            int roomPlayCount = int.Parse(strs[1].Split('%')[0]);
            string[] udStrArray = strs[1].Split('%')[1].Split('|');
            if(roomPlayCount >0)
            ud1 = new UserData(udStrArray[0]);
            if(roomPlayCount >1)
            ud2 = new UserData(udStrArray[1]);
            if(roomPlayCount >2)
            ud3 = new UserData(udStrArray[2]);
            if(roomPlayCount >3)
            ud4 = new UserData(udStrArray[3]);

            roleType = (RoleType)int.Parse(strs2[1]);
        }
        roomListPanel.OnJoinResponse(returnCode, ud1, ud2, ud3, ud4);
        facade.URoleType = roleType;
    }
}
