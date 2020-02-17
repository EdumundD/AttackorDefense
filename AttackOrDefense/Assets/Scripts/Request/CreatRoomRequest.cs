//
// @brief: 创建房间请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/20
// 
// 
//

using Common;

public class CreatRoomRequest : BaseRequest {

    RoomListPanel roomlistPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        roomlistPanel = GetComponent<RoomListPanel>();
        base.Awake();
    } 
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        facade.URoleType = (RoleType)int.Parse(strs[1]);
        if(returnCode == ReturnCode.Success)
        {
            roomlistPanel.OnCreatRoomResponse();
        }
        else
        {
            facade.ShowMessage("创建房间失败！");
        }
    }
}
