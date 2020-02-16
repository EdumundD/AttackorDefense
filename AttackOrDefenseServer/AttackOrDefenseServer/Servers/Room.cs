using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Common;

namespace AttackOrDefenseServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        public List<Client> clientRoom = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
        private Server server;
        Thread updateStepLogic;

        public LockStepLogic lockStepLogic = null;

        public Room(Server server)
        {
            this.server = server;
            lockStepLogic = new LockStepLogic();
            lockStepLogic._room = this;
        }
        public void AddClient(Client client)
        {
            clientRoom.Add(client);
            client.Room = this;
            if (clientRoom.Count >= 4) state = RoomState.WaitingBattle;
        }
        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);
            if (clientRoom.Count >= 4) state = RoomState.WaitingBattle;
            else state = RoomState.WaitingJoin;
        }
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }
        public int GetRoomPlayerCount()
        {
            return clientRoom.Count;
        }
        public int GetId()
        {
            if (clientRoom.Count > 0) return clientRoom[0].GetUserId();
            return -1;
        }
        public string GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(clientRoom.Count + "%");
            foreach (Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
            foreach (Client c in clientRoom)
            {
                if (c != excludeClient)
                {
                    if (c != null)
                        server.SendResponse(c, actionCode, data);
                }
            }
        }
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }
        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null, ActionCode.StartPlay, GetRoomPlayerCount().ToString());
            updateStepLogic = new Thread(updateLogic);
            updateStepLogic.Start();
        }


        public void updateLogic() {
            lockStepLogic.Init();
            try
            {
                while (true)
                {
                    if (clientRoom.Count == 0)
                    {
                        StopUpdateStepLogic();
                    }
                    try
                    {
                        Thread.Sleep(15);
                        lockStepLogic.Update();
                        
                    }
                    catch (ThreadAbortException e)
                    {
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            catch (ThreadAbortException e)
            {
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void StopUpdateStepLogic()
        {
            updateStepLogic.Abort();
        }
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
                Close();
            else clientRoom.Remove(client);
        }
        public void Close()
        {
            StopUpdateStepLogic();
            foreach (Client client in clientRoom)
            {
                client.Room = null;
            }
            server.ReMoveRoom(this);
        }
        
    }
}

