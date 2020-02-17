//
// @brief: 请求基类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/15
// 
// 
//

using UnityEngine;
using Common;
public class BaseRequest : MonoBehaviour {

    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected static GameFacade _facade;

    protected GameFacade facade
    {
        get
        {
            if (_facade == null)
                _facade = GameFacade.Instance;

            return _facade;
        }
    }
	public virtual void Awake () {
        facade.AddRequest(actionCode, this);
	}

    protected virtual void SendRequest(string data) {
        Debug.Log("发送了一条请求，RequestCode = " + requestCode + " ActionCode = " + actionCode + " Data = " + data);
        facade.SendRequest(requestCode, actionCode, data);
    }

    public virtual void SendRequest()
    {
    }

    public virtual void OnResponse(string data ) { }

    public virtual void OnDestroy()
    {
        //facade.RemoveRequest(actionCode);
    }
}
