using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class MyPosition:MonoBehaviour
{
    private static MyPosition _instance = null;
    public static MyPosition Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (new GameObject("位置信息管理")).AddComponent<MyPosition>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public List<PositionObject> _list = new List<PositionObject>();
    [System.Serializable]
    public class PositionObject
    {
        public bool _show = true;
        public GameObject _obj;
        public List<TransformInformation> _list = new List<TransformInformation>();
    }
    /// <summary>
    /// 获取信息
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation GetInformation(GameObject obj, string desc)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i]._obj == obj)
            {
                for (int j = 0; j < _list[i]._list.Count; j++)
                {
                    if (_list[i]._list[j]._desc == desc) return _list[i]._list[j];
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 获取局部位置坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public Vector3 GetLocalPosition(GameObject obj, string desc)
    {
        return GetInformation(obj, desc)._localPosition;
      
    }
    /// <summary>
    /// 获取世界位置坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public Vector3 GetPosition(GameObject obj, string desc)
    {
        return GetInformation(obj, desc)._position;

    }
    /// <summary>
    /// 获取旋转角度
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public Quaternion GetRotation(GameObject obj, string desc)
    {
        return GetInformation(obj, desc)._rotation;

    }
    /// <summary>
    /// 获取缩放大小
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public Vector3 GetScale(GameObject obj, string desc)
    {
        return GetInformation(obj, desc)._localScale;

    }
    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation SetInformation(GameObject obj, string desc)
    {
        TransformInformation information = GetInformation(obj, desc);
        obj.GetComponent<Transform>().localPosition = information._localPosition;
        obj.GetComponent<Transform>().rotation = information._rotation;
        obj.GetComponent<Transform>().localScale = information._localScale;
        return information;
    }
    /// <summary>
    /// 设置局部位置坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation SetLocalPosition(GameObject obj, string desc)
    {
        TransformInformation information = GetInformation(obj, desc);
        obj.GetComponent<Transform>().localPosition = information._localPosition;
        return information;
    }
    /// <summary>
    /// 设置世界位置坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation SetPosition(GameObject obj, string desc)
    {
        TransformInformation information = GetInformation(obj, desc);
        obj.GetComponent<Transform>().position = information._position;
        return information;
    }
    /// <summary>
    /// 设置旋转角度
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation SetRotation(GameObject obj, string desc)
    {
        TransformInformation information = GetInformation(obj, desc);
        obj.GetComponent<Transform>().rotation = information._rotation;
        return information;
    }
    /// <summary>
    /// 设置缩放大小
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    public TransformInformation SetScale(GameObject obj, string desc)
    {
        TransformInformation information = GetInformation(obj, desc);
        obj.GetComponent<Transform>().localScale = information._localScale;
        return information;
    }
}
[System.Serializable]
public class TransformInformation
{
    public string _desc = "描述";
    public bool _show = false;
    public Vector3 _localPosition;
    public Vector3 _position;
    public Quaternion _rotation;
    public Vector3 _localScale;
    public Vector3 _scale;
}
