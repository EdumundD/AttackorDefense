using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttackOrDefenseServer.Model
{
    public class FrameInput
    {
        // tick/clientID/buildPosition|towerId/createSoldier/objectDeath
        public FrameInput(int Tick, int ClientID, BuildTower BuildTower, CreateBarrack CreateBarrack, CreateSoldier CreateSoldier, ObjectDeath ObjectDeath)
        {
            tick = Tick;
            clientID = ClientID;
            buildTower = BuildTower;
            createBarrack = CreateBarrack;
            createSoldier = CreateSoldier;
            objectDeath = ObjectDeath;
        }
        public FrameInput(string data)
        {
            tick = Int32.Parse(data.Split('/')[0]);
            clientID = Int32.Parse(data.Split('/')[1]);
            buildTower = new BuildTower(data.Split('/')[2]);
            createBarrack = new CreateBarrack(data.Split('/')[3]);
            createSoldier = new CreateSoldier(data.Split('/')[4]);
            objectDeath = new ObjectDeath(data.Split('/')[5]);
        }
        //Default
        public FrameInput(int clientid)
        {
            tick = -1;
            clientID = clientid;
            buildTower = new BuildTower(new FixVector2(-1, -1), -1);
            createBarrack = new CreateBarrack(new FixVector2(-1, -1), -1);
            var path = new List<FixVector3>();
            path.Add(new FixVector3((Fix64)(-1), (Fix64)(-1), (Fix64)(-1)));
            createSoldier = new CreateSoldier(path, -1);
            objectDeath = new ObjectDeath(-1);
        }
        public string getString()
        {
            return 
                clientID + "/" +
                 buildTower.getString() + "/" +
                 createBarrack.getString() + "/" +
                 createSoldier.getString() + "/" +
                objectDeath.m_scId;
        }
        public int tick { get; set; }
        public int clientID { get; set; }
        public BuildTower buildTower { get; set; }
        public CreateBarrack createBarrack { get; set; }
        public CreateSoldier createSoldier { get; set; }
        public ObjectDeath objectDeath { get; set; }

    }
    public class BuildTower
    {
        public BuildTower(FixVector2 BuildPosition, int TowerId)
        {
            buildPosition = BuildPosition;
            towerId = TowerId;
        }
        public BuildTower(string data)
        {
            buildPosition = new FixVector2((Fix64)float.Parse(data.Split('|')[0].Split(',')[0]), (Fix64)float.Parse(data.Split('|')[0].Split(',')[1]));
            towerId = Int32.Parse(data.Split('|')[1]);
        }
        public string getString()
        {
            string data = "";
            data += buildPosition.x + "," + buildPosition.y + "|" + towerId;
            return data;
        }
        public FixVector2 buildPosition { get; set; }
        public int towerId { get; set; }
    }
    public class CreateBarrack
    {
        public CreateBarrack(FixVector2 CreatePosition, int BarrackId)
        {
            createPosition = CreatePosition;
            barrackId = BarrackId;
        }
        public CreateBarrack(string data)
        {
            createPosition = new FixVector2((Fix64)float.Parse(data.Split('|')[0].Split(',')[0]), (Fix64)float.Parse(data.Split('|')[0].Split(',')[1]));
            barrackId = Int32.Parse(data.Split('|')[1]);
        }
        public string getString()
        {
            string data = "";
            data += createPosition.x + "," + createPosition.y + "|" + barrackId;
            return data;
        }
        public FixVector2 createPosition { get; set; }
        public int barrackId { get; set; }
    }
    public class CreateSoldier
    {
        //point.x,point.y,point.z*point.x,point.y,point.z|soldierId
        public CreateSoldier(List<FixVector3> Path, int SoldierId)
        {
            path = Path;
            soldierId = SoldierId;
        }
        public CreateSoldier(string data)
        {
            List<FixVector3> points = new List<FixVector3>();
            for (int i = 0; i < data.Split('|')[0].Split('*').Length - 1; i++)
            {
                points.Add(new FixVector3(new Fix64(Int32.Parse(data.Split('|')[0].Split('*')[i].Split(',')[0])),
                    new Fix64(Int32.Parse(data.Split('|')[0].Split('*')[i].Split(',')[1])), new Fix64(Int32.Parse(data.Split('|')[0].Split('*')[i].Split(',')[2]))));
            }
            path = points;
            soldierId = Int32.Parse(data.Split('|')[1]);
        }
        public string getString()
        {
            string data = "";
            foreach (FixVector3 point in path)
            {
                data += point.x + "," + point.y + "," + point.z + "*";
            }
            data += "|" + soldierId;
            return data;
        }
        public List<FixVector3> path { get; set; }
        public int soldierId { get; set; }
    }
    public class ObjectDeath
    {
        public ObjectDeath(int sc_id)
        {
            m_scId = sc_id;
        }
        public ObjectDeath(string data)
        {
            m_scId = Int32.Parse(data);
        }
        public int m_scId { get; set; }
    }
}
