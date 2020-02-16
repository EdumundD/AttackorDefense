using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ListBuildingsRequest : BaseRequest
{
    private BuildingListPanel buildingListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ListBuilding;
        buildingListPanel = GetComponent<BuildingListPanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        List<TowerData> bdList = new List<TowerData>();
        if (data == "0")
        {
            buildingListPanel.LoadBuildingItemSync(bdList);
            return;
        }

        string[] bdArray = data.Split('|');
        foreach (string bd in bdArray)
        {
            bdList.Add(new TowerData(bd));
        }
        buildingListPanel.LoadBuildingItemSync(bdList);
    }
}
