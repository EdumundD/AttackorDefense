﻿//
// @brief: UI面板序列化类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/10
// 
// 
//

using UnityEngine;
using System;

[Serializable]
public class UIPanelInfo :ISerializationCallbackReceiver {
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;
    //{
    //    get
    //    {
    //        return panelType.ToString();S
    //    }
    //    set
    //    {
    //        UIPanelType type =(UIPanelType)System.Enum.Parse(typeof(UIPanelType), value);
    //        panelType = type;
    //    }
    //}
    public string path;

    // 反序列化   从文本信息 到对象
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
