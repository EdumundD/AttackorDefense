using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdataRoomRequest : BaseRequest {

    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        UserData ud1 = null;
        UserData ud2 = null;
        UserData ud3 = null;
        UserData ud4 = null;
        int clientCount = int.Parse(data.Split('%')[0]);
        string[] udStrArray = data.Split('%')[1].Split('|');
        if (clientCount > 0) ud1 = new UserData(udStrArray[0]);
        if (clientCount > 1) ud2 = new UserData(udStrArray[1]);
        if (clientCount > 2) ud3 = new UserData(udStrArray[2]);
        if (clientCount > 3) ud4 = new UserData(udStrArray[3]);

        roomPanel.SetAllPlayerResSync(ud1, ud2, ud3, ud4);
    }
}
