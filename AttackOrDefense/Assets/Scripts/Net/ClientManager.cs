using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;

/// <summary>
/// 用来管理跟服务器端的Socket连接
/// </summary>
public class ClientManager:BaseManager  {
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket;
    private Message msg = new Message();
    public ClientManager(GameFacade facade) : base(facade) { }


    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.GetData,msg.GetStartIndex,msg.GetRemainSize, SocketFlags.None, ReceiveCallBack, null);
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessDataCallBack);
            Start();
        }
        catch(Exception e)
        {
            Debug.LogWarning("接收数据的时候出现异常 " +e);
        }
    }
    private void OnProcessDataCallBack(ActionCode actionCode,string data)
    {
        //if (actionCode != ActionCode.LockStepLogic)
        //{
        //    Debug.Log("ActionCode: " + actionCode + " 解析出来一条消息: " + data);
        //}
        Debug.Log("ActionCode: " + actionCode + " 解析出来一条消息: " + data);
        facade.HandleReponse(actionCode, data);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        if (!clientSocket.Connected)
        {
            try
            {
                clientSocket.Connect(IP, PORT);
                Start();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
            clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch(Exception e)
        {
            Debug.LogWarning("无法关闭跟服务器的链接：" + e);
        }
    }
}
