using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AttackOrDefenseServer.DAO;
using AttackOrDefenseServer.Model;

namespace AttackOrDefenseServer.DAO
{
    class BuildingDAO
    {
        public List<Building> QueryBuilding(MySqlConnection conn)
        {
            MySqlDataReader reader = null;
            List<Building> buildingList = new List<Building>();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from building", conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string buildingname = reader.GetString("buildingname");
                    int buildingatt = reader.GetInt32("buildingatt");
                    int buildingdef = reader.GetInt32("buildingdef");
                    int buildingats = reader.GetInt32("buildingats");
                    int buildingprice = reader.GetInt32("buildingprice");
                    Building building = new Building(id, buildingname, buildingatt, buildingdef, buildingats, buildingprice);
                    buildingList.Add(building);
                }
                return buildingList;
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }
    }
}
