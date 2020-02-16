using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackOrDefenseServer.Model
{
    class Building
    {
        public Building(int id, string buildingname, int buildingatt, int buildingdef, int buildingats, int buildingprice)
        {
            this.Id = id;
            this.BuildingName = buildingname;
            this.BuildingAtt = buildingatt;
            this.BuildingDef = buildingdef;
            this.BuildingAts = buildingats;
            this.BuildingPrice = buildingprice;
        }
        public int Id { get; set; }//ID    
        public string BuildingName { get; private set; }//建筑名称
        public int BuildingAtt { get; set; }//建筑攻击力
        public int BuildingDef { get; set; }//建筑防御力
        public int BuildingAts { get; set; }//建筑攻击速度
        public int BuildingPrice { get; set; }//建筑价格
    }
}
