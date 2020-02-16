using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Common;
using AttackOrDefenseServer.Controller;

namespace AttackOrDefenseServer.Servers
{
    class Server
    {
        private static IPEndPoint iPEndPoint;//IP和端口号
        private static Socket serverSocket;//服务器socket    
        public static int connectedCount = 0;//当前连接客户端数 
        private List<Client> clientList = new List<Client>();//所有已连接Client的List
        private List<Room> roomList = new List<Room>();//所有房间列表
        private ControllerManager controllerManager;

        public Server() { }
        public Server(string ipStr,int port)//main函数入口，调用Server构造函数
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr, port);//设置ip和端口号
        }
        public void SetIpAndPort(string ipStr, int port)//设置ip和端口号
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(10);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult asyncResult)
        {
            Server.connectedCount += 1;
            Console.WriteLine("一个客户端正在连接,当前客户端连接数：" + Server.connectedCount);
            Socket clientSocket = serverSocket.EndAccept(asyncResult);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                connectedCount -= 1;
                clientList.Remove(client);
                Console.WriteLine(client.GetUserName() + " 退出了游戏。");
            }
        }
        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
            Console.WriteLine("当前房间数为:" + roomList.Count);
        }
        public void ReMoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }
        public List<Room> GetRoomList()
        {
            return roomList;
        }
        public Room GetRoomById(int id)
        {
            foreach (Room rm in roomList)
            {
                if (rm.GetId() == id) return rm;
            }
            return null;
        }
    }
}
