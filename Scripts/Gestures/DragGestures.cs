using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class DragGestures : MonoBehaviour {
    public Camera _camera;
    private Vector3 _screenSpace;
    private Vector3 _offset;
    public Action onDown;
    public Action onBeginDrag;
    public Action onDrag;
    public Action onEndDrag;
    public Action onUp;
    public  Vector3 _position;
	// Use this for initialization
	void Start () {
        EventTriggerListener.Get(gameObject).onDown = OnDown;
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
        EventTriggerListener.Get(gameObject).onUp = OnUp;
        _position = this.transform.position;
	}
    private void OnDown(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onDown != null) onDown();
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
    }
    private void OnBeginDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onBeginDrag != null) onBeginDrag();
        UpdatePosition(evenData);
    }
    private void OnDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onDrag != null) onDrag();
        UpdatePosition(evenData);
    }
    private void UpdatePosition(UnityEngine.EventSystems.PointerEventData evenData)
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
    private void OnEndDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        //有拖动才有结束
        if (onEndDrag != null) onEndDrag();
    }
    private void OnUp(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onUp != null) onUp();
    }
    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //游戏暂停-缩到桌面的时候触发
            if (onUp != null) onUp();
        }
        else
        {
            //游戏开始-回到游戏的时候触发
        }
    }
}
