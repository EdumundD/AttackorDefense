//
// @brief: Road类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/15
// 
// 
//

using UnityEngine;
public class Road :MonoBehaviour
{
    Point point;
    private void Awake()
    {
        point = new Point((int)transform.position.x, (int)transform.position.z);
        GameData.g_listRoad.Add(point);
    }
}

