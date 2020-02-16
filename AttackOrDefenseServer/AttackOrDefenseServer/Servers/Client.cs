using System;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Common;
using AttackOrDefenseServer.Model;

namespace AttackOrDefenseServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private MySqlConnection mysqlConn;
        private User user = null;
        private Result result;
        private Room room;
        private Message msg = new Message();

        public MySqlConnection MySQLConn
        {
            get { return mysqlConn; }
        }
        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }
        public string GetUserData()
        {
            return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
        }
        public string GetUserName()
        {
            if (user == null)
            {
                return "无效用户";
            }
            return user.Username;
        }
        public int GetUserId() { return user.Id; }
        public bool IsHouseOwner()
        {
            return room.IsHouseOwner(this);
        }
        public Room Room { set { room = value; } get { return room; } }
        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mysqlConn = ConnHelper.Connect();
        }
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.GetData,msg.GetStartIndex,msg.GetRemainSize, SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(asyncResult);
                if (count == 0) Close();
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);  
                Close();
            }
        }
        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }
        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket?.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }
        private void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);
            if(clientSocket != null)
            {
                clientSocket.Close();
            }
            if (room != null) room.QuitRoom(this);
            server.RemoveClient(this);
        }
    }
}
