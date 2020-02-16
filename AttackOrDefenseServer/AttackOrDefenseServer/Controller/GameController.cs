using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using AttackOrDefenseServer.Servers;
using AttackOrDefenseServer.Model;
using AttackOrDefenseServer.DAO;

namespace AttackOrDefenseServer.Controller
{
    class GameController:BaseController
    {
        private BuildingDAO buildingDAO = new BuildingDAO();
        public GameController()
        {
            requestCode = RequestCode.Game;
        }
        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsHouseOwner())
            {
                Room room = client.Room;
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                room.StartTimer();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
        public void LockStepLogic(string data, Client client, Server server)
        {
            client?.Room.lockStepLogic.OnFrameInput(client, data);
        }
        public string ListBuilding(string data , Client client,Server server)
        {
            StringBuilder sb = new StringBuilder();
            List<Building> buildingList = new List<Building>();
            buildingList = buildingDAO.QueryBuilding(client.MySQLConn);
            foreach (Building building in buildingList)
            {
                sb.Append(building.Id + "," + building.BuildingName + "," + building.BuildingAtt + "," + building.BuildingDef + "," + building.BuildingAts + "," + building.BuildingPrice + "|");
                
            }
            if (sb.Length == 0) { sb.Append("0"); } else { sb.Remove(sb.Length - 1, 1); }
            return sb.ToString();
        }
        public string ListSoldierFactory(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            List<Building> buildingList = new List<Building>();
            buildingList = buildingDAO.QueryBuilding(client.MySQLConn);
            foreach (Building building in buildingList)
            {
                sb.Append(building.Id + "," + building.BuildingName + "," + building.BuildingAtt + "," + building.BuildingDef + "," + building.BuildingAts + "," + building.BuildingPrice + "|");

            }
            if (sb.Length == 0) { sb.Append("0"); } else { sb.Remove(sb.Length - 1, 1); }
            return sb.ToString();
        }

        public void BuildTower(string data, Client client, Server server)
        {
            Room room = client.Room;
            room.BroadcastMessage(client, ActionCode.BuildTower, ((int)ReturnCode.Success).ToString());
        }
        public void CreateSoldier(string data, Client client, Server server)
        {
            if (data.Split('|')[1] == "Create")
            {
                Console.WriteLine(client.GetUserName() + "| Soldier Create:" + data.Split('|')[0]);
                StringBuilder sb = new StringBuilder();
                int logicFrame = Int32.Parse(data.Split('|')[0]) + 2;
                sb.Append(((int)ReturnCode.Success).ToString() + "|" + logicFrame);
                Room room = client.Room;
                room.BroadcastMessage(null, ActionCode.CreateSoldier, sb.ToString());

            }
            if (data.Split('|')[1] == "Die")
            {
                Console.WriteLine(client.GetUserName() + "| Soldier Die:" + data.Split('|')[0]);
            }
        }
        
        public void EndBattle(string data, Client client, Server server)
        {
            Room room = client.Room;
            room.BroadcastMessage(client, ActionCode.EndBattle, ((int)ReturnCode.Success).ToString());
        }
    }
}
