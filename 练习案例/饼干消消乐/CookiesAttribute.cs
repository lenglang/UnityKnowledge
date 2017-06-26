using UnityEngine;
using System.Collections;

public enum PropType
{
    None,
    列消除,
    横消除,
    炸弹,
    时间
}
public class CookiesAttribute : MonoBehaviour
{
    public int _c = 0;//列
    public int _r = 0;//横
    public int _move = 0;//移动间隔
    public PropType _propType = PropType.None;//道具
}
