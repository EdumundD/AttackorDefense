using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TowerData
{
    public TowerData(string userData)
    {
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);
        this.BuildingName = strs[1];
        this.BuildingAtt = int.Parse(strs[2]);
        this.BuildingDef = int.Parse(strs[3]);
        this.BuildingAts = int.Parse(strs[4]);
        this.BuildingPrice = int.Parse(strs[5]);
        this.BuildingType = (BuildingType)(int.Parse(strs[0]));
    }
    public int Id { get; set; }
    public string BuildingName { get; private set; }
    public int BuildingAtt { get; set; }
    public int BuildingDef { get; set; }
    public int BuildingAts { get; set; }
    public int BuildingPrice { get; set; }
    public BuildingType BuildingType { get; set; }
}

