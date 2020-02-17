//
// @brief: 帧同步请求类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/3
// 
// 
//

using Common;

public class LockStepLogicRequest:BaseRequest
{
    private ReturnCode returnCode;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.LockStepLogic;
        base.Awake();
    }
    private void Update()
    {
        
    }
    public override void OnResponse(string data)
    {
        facade.AddFrameInput(data);
    }
}

