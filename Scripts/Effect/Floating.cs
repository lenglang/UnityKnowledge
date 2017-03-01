using UnityEngine;
using System.Collections;
/// <summary>
/// 主界面的开始按钮使用该脚本，控制上下来回浮动
/// </summary>
public class Floating : MonoBehaviour
{
    float radian = 0;//弧度
    float perRadian = 0.03f;//每次变灰的弧度
    float radius = 0.8f;//半径
    Vector3 oldPos;//开始时候的坐标
    void Start()
    {
        oldPos = transform.position;
    }
    void Update()
    {
        radian += perRadian;
        float dy = Mathf.Cos(radian) * radius;
        transform.position = oldPos + new Vector3(0, dy, 0);
    }
}