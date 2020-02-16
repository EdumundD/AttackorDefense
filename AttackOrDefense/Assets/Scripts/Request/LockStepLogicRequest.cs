using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

