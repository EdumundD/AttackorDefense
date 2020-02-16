using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  //可序列化的  内存内的数据存储到硬盘上
public class BuildingInfo:ISerializationCallbackReceiver
{
    [NonSerialized]
    public BuildingType buildingType;
    public string buildingTypeString;
    public string path;

    public void OnAfterDeserialize()
    {
        BuildingType type = (BuildingType)System.Enum.Parse(typeof(BuildingType), buildingTypeString);
        buildingType = type;
    }

    public void OnBeforeSerialize()
    {
    }
}
