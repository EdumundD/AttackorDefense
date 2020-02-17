//
// @brief: 请求管理类(统管所有的请求类，分发回应)
// @version: 1.0.0
// @author lhy
// @date: 2019/11/20
// 
// 
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RequestManager : BaseManager {

    public RequestManager(GameFacade facade) : base(facade) { }

    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode,BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode); 
    }

    public void HandleReponse(ActionCode actionCode, string data)
    {
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if(request == null)
        {
            Debug.Log("无法得到ActionCode[" + actionCode + "]对应的Request类"); return;
        }
        request.OnResponse(data);
    }
}
