using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
/// <summary>
/// 默认注册按下事件，按下后需手动添加取消按下事件，添加移动相关事件和放开事件，放开后若有需要继续添加按下事件
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
    public Action onUp;
    [HideInInspector]
    public  Vector3 _localPosition;
    private bool _isDown = false;//是否按下
    private bool _isDrag = false;//是否拖拽
    public PointerEventData _evenData;
	// Use this for initialization
    void Awake()
    {
        EventTriggerListener.Get(gameObject).onDown = OnDown;
	}
    public void OnDown(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
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
        if (onDown != null) onDown();
    }
    public void CancelOnDown()
    {
        EventTriggerListener.Get(gameObject).onDown = null;
    }
    public void AddOnDown()
    {
        EventTriggerListener.Get(gameObject).onDown = OnDown;
    }
    public void CancelOnUp()
    {
        EventTriggerListener.Get(gameObject).onUp = null;
    }
    public void AddOnUp()
    {
        EventTriggerListener.Get(gameObject).onUp = OnUp;
    }
    public void CancelOnDrag()
    {
        EventTriggerListener.Get(gameObject).onBeginDrag = null;
        EventTriggerListener.Get(gameObject).onDrag = null;
        EventTriggerListener.Get(gameObject).onEndDrag = null;
    }
    public void AddOnDrag()
    {
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
    }
    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        _isDrag = true;
        if (onBeginDrag != null) onBeginDrag();
        UpdatePosition(evenData);
    }
    public void OnDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onDrag != null) onDrag();
        UpdatePosition(evenData);
    }
    public void UpdatePosition(UnityEngine.EventSystems.PointerEventData evenData)
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
    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        //有拖动才有结束
        if (onEndDrag != null) onEndDrag();
        CancelOnDrag();
    }
    public void OnUp(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        _isDown = false;
        if (onUp != null) onUp();
        if (_isDrag==false)
        {
            CancelOnDrag();
        }
        CancelOnUp();
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
        Destroy(this.GetComponent<EventTriggerListener>());
    }
    void OnEnable()
    {
        _localPosition = this.transform.localPosition;
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl) etl.enabled = true;
    }
    void OnDisable()
    {
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl)
        {
            etl.enabled = false;
            CancelOnDrag();
        } 
    }
}
