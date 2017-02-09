using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 使用说明：直接添加脚本到物体上即可，然后设置物体旋转的方向，物体旋转的轴
/// </summary>
public class SlideGestures : MonoBehaviour
{
    Vector2 currentDragDir;
    Vector2 previousDragDir;
    Vector2 startDragDir;
    [Header("滑动方向")]
    public Direction direction = Direction.左;
    public Action onBeginDrag;
    public Action onEndDrag;
    public Action onComplete;
    private float _dx = 0;
    private float _dy = 0;
    [Header("滑动X距离算成功")]
    public float _disX = Screen.width / 6;
    [Header("滑动Y距离算成功")]
    public float _disY = Screen.height / 6;
    private float _dis = 0;
    void OnEnable()
    {
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
    }
    void OnDisable()
    {
        OnDestroy();
    }
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnBeginDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        currentDragDir = evenData.position;
        previousDragDir = currentDragDir;
        startDragDir = currentDragDir;
        if (onBeginDrag != null) onBeginDrag();
    }
    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    public void OnDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        currentDragDir = evenData.position;
        Direction dragDir = RotationDirection(currentDragDir, previousDragDir);
        if (dragDir == direction)
        {
            if (dragDir == Direction.上 || dragDir == Direction.下)
            {
                _dis =currentDragDir.y - startDragDir.y;
                if (Math.Abs(_dis) > _disY && ((dragDir == Direction.上 && _dis > 0) || (dragDir == Direction.下 && _dis < 0)))
                {
                    Debug.Log("成功！");
                    if (onComplete != null) onComplete();
                }
            }
            else
            {
                _dis = currentDragDir.x - startDragDir.x;
                if (Math.Abs(_dis) > _disX && ((dragDir == Direction.右 && _dis > 0) || (dragDir == Direction.左 && _dis < 0)))
                {
                    Debug.Log("成功！");
                    if (onComplete != null) onComplete();
                }
            }
        }
        previousDragDir = currentDragDir;
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnEndDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onEndDrag != null) onEndDrag();
    }
    /// <summary>
    /// 判断旋转顺逆时针
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="previousDir"></param>
    /// <returns></returns>
    public Direction RotationDirection(Vector2 currentDir, Vector2 previousDir)
    {
        _dx = currentDir.x - previousDir.x;
        _dy = currentDir.y - previousDir.y;
        if (Math.Abs(_dy) > Math.Abs(_dx))
        {
            if (_dy > 0)
            {
                return Direction.上;
            }
            else
            {
                return Direction.下;
            }
        }
        else
        {
            if (_dx > 0)
            {
                return Direction.右;
            }
            else
            {
                return Direction.左;
            }
        }
    }
    public enum Direction
    {
        上,
        下,
        左,
        右
    }
    void OnDestroy()
    {
        EventTriggerListener etl = gameObject.GetComponent<EventTriggerListener>();
        if (etl == null) return;
        EventTriggerListener.Get(gameObject).onBeginDrag = null;
        EventTriggerListener.Get(gameObject).onDrag = null;
        EventTriggerListener.Get(gameObject).onEndDrag = null;
        Destroy(etl);
    }
}
