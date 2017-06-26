using UnityEngine;
using System;
using UnityEngine.EventSystems;
/// <summary>
/// 默认注册按下事件，按下后注册拖拽和弹起事件
/// </summary>
public class DragGestures : MonoBehaviour {
    [HideInInspector]
    public Camera _camera;
    private Vector3 _screenSpace;
    [HideInInspector]
    public Vector3 _offset;
    public Action onDown;
    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;
    private bool _isDown = false;//是否按下
    private bool _isDrag = false;//是否拖拽
    [HideInInspector]
    public PointerEventData _evenData;
    private EventTriggerListener _eventTriggerListener;
    // Use this for initialization
    void Awake()
    {
        _eventTriggerListener = EventTriggerListener.Get(gameObject);
        _eventTriggerListener.onDown = OnDown;
	}
    public void OnDown(PointerEventData evenData, GameObject obj)
    {
        _isDown = true;
        _isDrag = false;
        if (_camera == null)
        {
            _screenSpace = Camera.main.WorldToScreenPoint(this.transform.position);
            _offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(evenData.position.x, evenData.position.y, _screenSpace.z));
        }
        else
        {
            _screenSpace = _camera.WorldToScreenPoint(this.transform.position);
            _offset = this.transform.position - _camera.ScreenToWorldPoint(new Vector3(evenData.position.x, evenData.position.y, _screenSpace.z));
        }

        _evenData = evenData;
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
        Vector3 curScreenSpace = new Vector3(evenData.position.x, evenData.position.y, _screenSpace.z);
        if (_camera == null)
        {
            Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + _offset;
            this.transform.position = CurPosition;
        }
        else
        {
            Vector3 CurPosition = _camera.ScreenToWorldPoint(curScreenSpace) + _offset;
            this.transform.position = CurPosition;
        }
    }
    public void OnEndDrag(PointerEventData evenData, GameObject obj)
    {
        //有拖动才有结束
        CancelOnDrag();
        if (onEndDrag != null) onEndDrag();
        
    }
    public void OnUp(PointerEventData evenData, GameObject obj)
    {
        _isDown = false;
        CancelOnUp();
        if (_isDrag == false)
        {
            CancelOnDrag();
            if (onEndDrag != null) onEndDrag();
        }
    }
    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //游戏暂停-缩到桌面的时候触发
        }
        else
        {
            //游戏开始-回到游戏的时候触发
            if (onEndDrag != null && _isDown) onEndDrag();
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
