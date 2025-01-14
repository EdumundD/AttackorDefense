﻿//
// @brief: 更新战绩请求类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/27
// 
// 
//

using Common;

public class UpdateResultRequest : BaseRequest {

    private RoomListPanel roomListPanel;
    private bool isUpdateResult = false;
    private int totalCount;
    private int winCount;
    public override void Awake()
    {
        actionCode = ActionCode.UpdateResult;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    private void Update()
    {
        if(isUpdateResult)
        {
            roomListPanel.OnUpdateResultResponse(totalCount, winCount);
            isUpdateResult = false;
        }
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        int totalCount = int.Parse(strs[0]);
        int winCount = int.Parse(strs[1]);
        isUpdateResult = true;
    }
}
