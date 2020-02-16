using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandData : MonoBehaviour
{
    public FixVector3 localPosition;
    public bool isBuild = false;
    public bool isForTower = false;
    public bool isForBarrack = false;

    private void Start()
    {
        if (isForTower) gameObject.layer = 9;
        if (isForBarrack) gameObject.layer = 10;
        localPosition = new FixVector3((Fix64)transform.position.x, (Fix64)transform.position.y, (Fix64)transform.position.z);
        GameData.g_listLand.Add(this);
    }
}
