using System.Collections;
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

