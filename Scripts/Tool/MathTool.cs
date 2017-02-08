using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathTool
{
    /// <summary>
    /// 三维:获取b点相对于a点的角度，也就是说a点加上这角度就会指向b点。
    /// </summary>
    /// <returns>The angle.</returns>
    /// <param name="a">The alpha component.</param>
    /// <param name="b">The blue component.</param>
    public static float GetAngle3(Vector3 a, Vector3 b)
    {
        b.x -= a.x;
        b.z -= a.z;
        float deltaAngle = 0;
        if (b.x == 0 && b.z == 0)
        {
            return 0;
        }
        else if (b.x > 0 && b.z > 0)
        {
            deltaAngle = 0;
        }
        else if (b.x > 0 && b.z == 0)
        {
            return 90;
        }
        else if (b.x > 0 && b.z < 0)
        {
            deltaAngle = 180;
        }
        else if (b.x == 0 && b.z < 0)
        {
            return 180;
        }
        else if (b.x < 0 && b.z < 0)
        {
            deltaAngle = -180;
        }
        else if (b.x < 0 && b.z == 0)
        {
            return -90;
        }
        else if (b.x < 0 && b.z > 0)
        {
            deltaAngle = 0;
        }
        float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
        return angle;
    }
    /// <summary>
    /// 获取二维两点坐标角度
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float GetAngle2(Vector2 p1, Vector2 p2)
    {
        float vx = p2.x - p1.x;
        float vy = p2.y - p1.y;
        float hyp = Mathf.Sqrt(Mathf.Pow(vx, 2) + Mathf.Pow(vy, 2));
        float rad = Mathf.Acos(vx / hyp);
        float angle = 180 / (Mathf.PI / rad);
        if (vy < 0)
        {
            angle = (-angle);
        }
        else if ((vy == 0) && vx < 0)
        {
            angle = 180;
        }
        return angle;
    }
    /// <summary>
    /// 对点集围着中心顺序排序
    /// </summary>
    /// <param name="points"></param>
    public static void SortPoints(List<Vector2> points)
    {
        Vector2 center = GetCenter(points);
        bool exist = true;
        while (exist)
        {
            exist = false;
            for (int i = 0; i < points.Count-1; i++)
            {
                if (GetAngle2(center, points[i]) > GetAngle2(center, points[i + 1]))
                {
                    Vector2 temp = points[i];
                    points[i] = points[i + 1];
                    points[i + 1] = temp;
                    exist = true;
                }
            }
        }
    }
    /// <summary>
    /// 获取点集合的中心
    /// </summary>
    /// <param name="_list"></param>
    /// <returns></returns>
    public static Vector2 GetCenter(List<Vector2> points)
    {
        Vector2 center = new Vector2();
        foreach (var item in points)
        {
            center.x += item.x;
            center.y += item.y;
        }
        center.x = center.x / points.Count;
        center.y = center.y / points.Count;
        return center;
    }
    /// <summary>
    /// 获取线段与折线交点
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="_angle">折线角度</param>
    /// <param name="_clickPoint">点击点</param>
    /// <param name="_movePoint">移动点</param>
    /// <param name="_K">折线斜率</param>
    /// <param name="_b">折线b值</param>
    /// <returns></returns>
    public static Vector2 GetFocus(Vector2 p1, Vector2 p2, float _angle, Vector2 _clickPoint, Vector2 _movePoint, float _K, float _b)
    {
        float angle = GetAngle2(p1, p2);
        float radian = angle * Mathf.PI / 180;
        float x1, x2, y1, y2;
        Vector2 point = new Vector2();
        float k2 = Mathf.Tan(radian);
        float b2 = p2.y - (k2 * p2.x);
        if (_angle == 0 || _angle == 180)
        {
            if (p1.x == p2.x)
            {
                //用来做无效处理判断
                point.x = 10000;
                return point;
            }
            if (p1.y == p2.y)
            {
                y1 = y2 = p1.y;
                x1 = x2 = (_clickPoint.x + _movePoint.x) / 2;
            }
            else
            {
                x1 = x2 = (_clickPoint.x + _movePoint.x) / 2;
                y1 = y2 = k2 * x1 + b2;
            }
            point.x = x1;
            point.y = y1;
        }
        else if (_angle == 90 || _angle == -90)
        {
            if (p1.y == p2.y)
            {
                //用来做无效处理判断
                point.x = 10000;
                return point;
            }
            if (p1.x == p2.x)
            {
                x1 = x2 = p1.x;
                y1 = y2 = (_clickPoint.y + _movePoint.y) / 2;
            }
            else
            {
                y1 = y2 = (_clickPoint.y + _movePoint.y) / 2;
                x1 = x2 = (y1 - b2) / k2;
            }
            point.x = x1;
            point.y = y1;
        }
        else
        {
            if (p1.x == p2.x)
            {
                //垂直
                x1 = x2 = p1.x;
                y1 = y2 = x1 * _K + _b;
            }
            else if (p1.y == p2.y)
            {
                //水平
                y1 = y2 = p1.y;
                x1 = x2 = (y1 - _b) / _K;
            }
            else
            {
                x1 = x2 = point.x = (_b - b2) / (k2 - _K);
                y1 = _K * point.x + _b;
                y2 = k2 * point.x + b2;
            }
            point.x = x1;
            point.y = y1;
        }
        if (Mathf.Round(x1) == Mathf.Round(x2) && Mathf.Round(y1) == Mathf.Round(y2) && Mathf.Round(point.x) >= Mathf.Round(Mathf.Min(p1.x, p2.x)) && Mathf.Round(point.x) <= Mathf.Round(Mathf.Max(p1.x, p2.x)) && Mathf.Round(point.y) <= Mathf.Round(Mathf.Max(p1.y, p2.y)) && Mathf.Round(point.y) >= Mathf.Round(Mathf.Min(p1.y, p2.y)))
        {
            return point;
        }
        //用来做无效处理判断
        point.x = 10000;
        return point;
    }
    /// <summary>
    /// 获取两点之间的距离
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float GetDistance(Vector2 p1, Vector2 p2)
    {
        return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
    }
    /// <summary>
    /// 获取对称点
    /// </summary>
    /// <param name="p"></param>
    /// <param name="_clickPoint"></param>
    /// <param name="_movePoint"></param>
    /// <param name="_angle"></param>
    /// <param name="_K"></param>
    /// <param name="_b"></param>
    /// <returns></returns>
    public static Vector2 GetSymmetry(Vector2 p, float _angle, Vector2 _clickPoint, Vector2 _movePoint, float _K, float _b)
    {
        Vector2 point = new Vector2();
        point.x = (2 * p.y * _K - 2 * _b * _K + p.x - _K * _K * p.x) / (1 + _K * _K);
        point.y = p.y - (point.x - p.x) / _K;
        if (_angle == 0 || _angle == 180)
        {
            if (_clickPoint.x < _movePoint.x)
            {
                //右
                point.x = p.x + ((_movePoint.x + _clickPoint.x) / 2 - p.x) * 2;
            }
            else
            {
                //左
                point.x = p.x - (p.x - (_movePoint.x + _clickPoint.x) / 2) * 2;
            }
        }
        else if (_angle == 90)
        {
            //下移
            point.y = p.y + ((_movePoint.y + _clickPoint.y) / 2 - p.y) * 2;
        }
        else if (_angle == -90)
        {
            //上移
            point.y = p.y - (p.y - (_movePoint.y + _clickPoint.y) / 2) * 2;
        }
        return point;
    }
    /// <summary>
    /// 射线法判断点是否在多边形内部
    /// </summary>
    /// <param name="p"></param>
    /// <param name="poly"></param>
    /// <returns></returns>
    public string RayCasting(Vector2 p, List<Vector2> poly)
    {
        float px = p.x;
        float py = p.y;
        bool flag = false;
        for (int i = 0, l = poly.Count, j = l - 1; i < l; j = i, i++)
        {
            float sx = poly[i].x;
            float sy = poly[i].y;
            float tx = poly[j].x;
            float ty = poly[j].y;
            // 点与多边形顶点重合
            if ((sx == px && sy == py) || (tx == px && ty == py))
            {
                return "out";
                //return "on";
            }
            // 判断线段两端点是否在射线两侧
            if ((sy < py && ty >= py) || (sy >= py && ty < py))
            {
                // 线段上与射线 Y 坐标相同的点的 X 坐标
                var x = sx + (py - sy) * (tx - sx) / (ty - sy);
                // 点在多边形的边上
                if (x == px)
                {
                    return "in";
                    //return "on";
                }
                // 射线穿过多边形的边界
                if (x > px)
                {
                    flag = !flag;
                }
            }
        }
        // 射线穿过多边形边界的次数为奇数时点在多边形内
        return flag ? "in" : "out";
    }
    public static void CountDraw(List<Vector2> points, float _angle, Vector2 _clickPoint, Vector2 _movePoint, float _K, float _b, List<List<Vector2>> _newPoints)
    {
        List<Vector2> symmetryPoints = new List<Vector2>();
        List<Vector2> lessPoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++)
        {
            if (_angle == 0 || _angle == 180)
            {
                //右移
                if (_clickPoint.x < _movePoint.x && points[i].x < (_clickPoint.x + _movePoint.x) / 2)
                {

                    symmetryPoints.Add(GetSymmetry(points[i], _angle, _clickPoint, _movePoint, _K, _b));
                }
                //左移
                else if (_clickPoint.x > _movePoint.x && points[i].x > (_clickPoint.x + _movePoint.x) / 2)
                { symmetryPoints.Add(GetSymmetry(points[i], _angle, _clickPoint, _movePoint, _K, _b)); }
                else { lessPoints.Add(points[i]); }
                continue;
            }
            if (_clickPoint.y < _movePoint.y)
            {
                //下移
                if (points[i].y > points[i].x * _K + _b)
                {
                    lessPoints.Add(points[i]);
                }
                else
                {
                    //_firstBoolean = true;
                    symmetryPoints.Add(GetSymmetry(points[i], _angle, _clickPoint, _movePoint, _K, _b));
                }
            }
            else
            {
                //上移
                if (points[i].y < points[i].x * _K + _b)
                {
                    lessPoints.Add(points[i]);
                }
                else
                {
                    //_firstBoolean = true;
                    symmetryPoints.Add(GetSymmetry(points[i], _angle, _clickPoint, _movePoint, _K, _b));
                }
            }
        }
        //是否有交点
        List<Vector2> focusPoints = new List<Vector2>();
        for (int n = 0; n < points.Count; n++)
        {
            Vector2 focus=new Vector2(10000,0);
            if (n == points.Count - 1)
            {
                focus = GetFocus(points[n], points[0], _angle, _clickPoint, _movePoint, _K, _b);
            }
            else
            {
                focus = GetFocus(points[n], points[n + 1], _angle, _clickPoint, _movePoint, _K, _b);
            }
            if (focus.x!=10000)
            {
                focusPoints.Add(focus);
            }
        }
        List<Vector2> arr1 = new List<Vector2>();
        foreach (var item in lessPoints)
        {
            arr1.Add(item);
        }
        foreach (var item in focusPoints)
        {
            arr1.Add(item);
        }
        SortPoints(arr1);
        if (arr1.Count != 0)
        {
            _newPoints.Insert(0, arr1);
        }
        List<Vector2> arr2 = new List<Vector2>();
        foreach (var item in focusPoints)
        {
            arr2.Add(item);
        }
        foreach (var item in symmetryPoints)
        {
            arr2.Add(item);
        }
        SortPoints(arr2);
        if (arr2.Count != 0)
        {
            _newPoints.Insert(_newPoints.Count, arr2);
        }
    }
    /// <summary>
    /// 转换flash二维坐标
    /// </summary>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(Vector2 v2)
    {
        v2.y = Screen.height - v2.y;
        return v2;
    }
}
