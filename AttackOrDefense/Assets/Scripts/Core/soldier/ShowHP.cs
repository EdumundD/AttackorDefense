//
// @brief: 血量显示类
// @version: 1.0.0
// @author lhy
// @date: 2020/2/15
// 
// 
//

using UnityEngine;
using UnityEngine.UI;

public class ShowHP : MonoBehaviour
{
    public Slider hpSlider;
    private RectTransform rectTransform;

    public Transform target;    //目标对象
    public Vector2 offsetPos;   //偏移量
    public float value;         //当前血量
    public float maxValue;      //最大血量

    private void Start()
    {
        hpSlider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
        Init();
    }
    private void Init()
    {
        value = maxValue;
        hpSlider.value = value / maxValue;
    }
    private void Update()
    {
        if (null == target) return;
        if (value <= 0) Destroy(gameObject);
        hpSlider.value = value;
        Vector3 tarPos = target.transform.position;
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tarPos);
        rectTransform.position = pos + offsetPos;
    }
}
