using UnityEngine;
using System;
using UnityEngine.EventSystems;
/// <summary>
/// 默认注册按下事件，按下后注册拖拽和弹起事件
/// </summary>
public class DragGestures2D : MonoBehaviour {
    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    public Action onDown;
    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;
    public Action onUp;
    private  Vector3 _localPosition;
    private bool _isDown = false;//是否按下
    private bool _isDrag = false;//是否拖拽
    private EventTriggerListener _eventTriggerListener;
	// Use this for initialization
    void Awake()
    {
        _eventTriggerListener = EventTriggerListener.Get(gameObject);
        _eventTriggerListener.onDown = OnDown;
    }
    public void OnDown(PointerEventData evenData, GameObject obj)
    {
        _localPosition = this.transform.localPosition;
        _isDown = true;
        _isDrag = false;
        _startPosition = evenData.position;
        CancelOnDown();
        AddOnDrag();
        AddOnUp();
        if (onDown != null) onDown();
    }
    /// <summary>
    /// 取消按下监听
    /// </summary>
    public void CancelOnDown()
    {
        _eventTriggerListener.onDown = null;
    }
    /// <summary>
    /// 添加按下监听
    /// </summary>
    public void AddOnDown()
    {
        _eventTriggerListener.onDown = OnDown;
    }
    /// <summary>
    /// 取消弹起监听
    /// </summary>
    public void CancelOnUp()
    {
        _eventTriggerListener.onUp = null;
    }
    /// <summary>
    /// 添加弹起监听
    /// </summary>
    public void AddOnUp()
    {
        _eventTriggerListener.onUp = OnUp;
    }
    /// <summary>
    /// 添加拖拽监听相关事件
    /// </summary>
    public void CancelOnDrag()
    {
        _eventTriggerListener.onBeginDrag = null;
        _eventTriggerListener.onDrag = null;
        _eventTriggerListener.onEndDrag = null;
    }
    /// <summary>
    /// 取消拖拽监听相关事件
    /// </summary>
    public void AddOnDrag()
    {
        _eventTriggerListener.onBeginDrag = OnBeginDrag;
        _eventTriggerListener.onDrag = OnDrag;
        _eventTriggerListener.onEndDrag = OnEndDrag;
    }
    public void OnBeginDrag(PointerEventData evenData, GameObject obj)
    {
        _isDrag = true;
        UpdatePosition(evenData);
        if (onBeginDrag != null) onBeginDrag();
    }
    public void OnDrag(PointerEventData evenData, GameObject obj)
    {
        UpdatePosition(evenData);
        if (onDrag != null) onDrag();
    }
    public void UpdatePosition(PointerEventData evenData)
    {
        _currentPosition = evenData.position;
        this.transform.localPosition = new Vector2(_localPosition.x+_currentPosition.x - _startPosition.x, _localPosition.y + _currentPosition.y - _startPosition.y);
    }
    public void OnEndDrag(PointerEventData evenData, GameObject obj)
    {
        CancelOnDrag();
        //有拖动才有结束
        if (onEndDrag != null) onEndDrag();
    }
    public void OnUp(PointerEventData evenData, GameObject obj)
    {
        _isDown = false;
        if (_isDrag == false)
        {
            CancelOnDrag();
        }
        CancelOnUp();
        if (onUp != null) onUp();
    }
    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //游戏暂停-缩到桌面的时候触发
            if (onUp != null&&_isDown) onUp();
        }
        else
        {
            //游戏开始-回到游戏的时候触发
        }
    }
    void OnDestroy()
    {
        Destroy(_eventTriggerListener);
    }
    void OnEnable()
    {
        _eventTriggerListener.enabled = true;
    }
    void OnDisable()
    {
        _eventTriggerListener.enabled = false;
    }
}
