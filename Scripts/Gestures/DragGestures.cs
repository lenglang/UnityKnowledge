using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class DragGestures : MonoBehaviour {
    public Camera _camera;
    private Vector3 _screenSpace;
    public Vector3 _offset;
    public Action onDown;
    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;
    public Action onUp;
    public  Vector3 _position;
    public bool _isDown = false;
    public PointerEventData _evenData;
	// Use this for initialization
    void Awake()
    {
        EventTriggerListener.Get(gameObject).onDown = OnDown;
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
        EventTriggerListener.Get(gameObject).onUp = OnUp;
	}
    public void OnDown(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        _isDown = true;
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
    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
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
    }
    public void OnUp(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        _isDown = false;
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
        Destroy(this.GetComponent<EventTriggerListener>());
    }
    void OnEnable()
    {
        _position = this.transform.position;
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl) etl.enabled = true;
    }
    void OnDisable()
    {
        EventTriggerListener etl = this.GetComponent<EventTriggerListener>();
        if (etl) etl.enabled = false;
    }
}
