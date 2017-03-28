using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 使用说明：直接添加脚本到物体
/// </summary>
public class SlideGestures : MonoBehaviour
{
    public enum Direction
    {
        从下到上,
        从上到下,
        从右到左,
        从左到右
    }
    Vector2 currentDragDir;
    Vector2 previousDragDir;
    Vector2 startDragDir;
    [Header("滑动方向")]
    public Direction direction = Direction.从右到左;
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
    void Awake()
    {
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
    }
    void OnEnable()
    {
        startDragDir = currentDragDir;
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl) etl.enabled = true;
    }
    void OnDisable()
    {
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl) etl.enabled = false;
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
            if (dragDir == Direction.从下到上 || dragDir == Direction.从上到下)
            {
                _dis =currentDragDir.y - startDragDir.y;
                if (Math.Abs(_dis) > _disY && ((dragDir == Direction.从下到上 && _dis > 0) || (dragDir == Direction.从上到下 && _dis < 0)))
                {
                    if (onComplete != null) onComplete();
                }
            }
            else
            {
                _dis = currentDragDir.x - startDragDir.x;
                if (Math.Abs(_dis) > _disX && ((dragDir == Direction.从左到右 && _dis > 0) || (dragDir == Direction.从右到左 && _dis < 0)))
                {
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
                return Direction.从下到上;
            }
            else
            {
                return Direction.从上到下;
            }
        }
        else
        {
            if (_dx > 0)
            {
                return Direction.从左到右;
            }
            else
            {
                return Direction.从右到左;
            }
        }
    }
    void OnDestroy()
    {
        EventTriggerListener etl = gameObject.GetComponent<EventTriggerListener>();
        if (etl == null) return;
        Destroy(etl);
    }
}
