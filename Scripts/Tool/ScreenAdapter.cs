using UnityEngine;
using System.Collections;

public class ScreenAdapter
{
    public enum ScreenType
    {
        Null,
        Screen4X3,
        Screen16X9,
        Screen2X1
    }
    public static ScreenType _type = ScreenType.Null;
    public static ScreenType Type
    {
        get
        {
            if (_type == ScreenType.Null)
            {
                if (Mathf.Abs(Screen.width * 1.0f / Screen.height - 16.0f / 9.0f) < 0.2f)
                    _type = ScreenType.Screen16X9;
                else if (Mathf.Abs(Screen.width * 1.0f / Screen.height - 2.0f) < 0.1f)
                    _type = ScreenType.Screen2X1;
                else
                    _type = ScreenType.Screen4X3;
            }
            return _type;
        }
    }
    /// <summary>
    /// 拖拽距离根据不同分辨率屏幕进行转化(屏幕1024*768与高进行缩放)
    /// distance两个点evenData.position.x差值;
    /// </summary>
    public static float DragDistanceChange(float distance)
    {
        if (ScreenType.Screen4X3 == Type)
        {
            distance = distance * 1024 / Screen.width;
        }
        else if (ScreenType.Screen16X9 == Type)
        {
            distance = distance * 1024 * 4 / (Screen.width * 3);
        }
        else
        {
            distance = distance * 1024 * 6 / (Screen.width * 4);
        }
        return distance;
    }
}
