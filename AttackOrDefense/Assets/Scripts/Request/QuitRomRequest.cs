//
// @brief: 退出房间请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/23
// 
// 
//

using Common;

public class QuitRomRequest : BaseRequest {

    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.QuitRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if(returnCode == ReturnCode.Success)
        {
            roomPanel.OnExitResponse();
        }
    }
}
