using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 注意事项：1.场景需添加EventSystem；2.摄像机需挂载PhysicsRaycaster脚本；3.监听对象需要挂载BoxCollider组件
/// </summary>
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(PointerEventData evenData, GameObject obj);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onDrop;
    public VoidDelegate onDrag;
    public VoidDelegate onEndDrag;
    public VoidDelegate onInitializePotentialDrag;
    public VoidDelegate onScroll;
    public delegate void VoidDelegate2(BaseEventData evenData, GameObject obj);
    public VoidDelegate2 onSelect;
    public VoidDelegate2 onUpdateSelect;
    public VoidDelegate2 onSubmit;
    public VoidDelegate2 onCancel;
    public VoidDelegate2 onDeselect;
    public delegate void VoidDelegate3(AxisEventData evenData, GameObject obj);
    public VoidDelegate3 onMove;
    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(eventData, gameObject);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(eventData, gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(eventData, gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(eventData, gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(eventData, gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(eventData, gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(eventData, gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(eventData, gameObject);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(eventData, gameObject);
    }
    public override void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(eventData, gameObject);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null) onDeselect(eventData, gameObject);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData, gameObject);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(eventData, gameObject);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(eventData, gameObject);
    }
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(eventData, gameObject);
    }
    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(eventData, gameObject);
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(eventData, gameObject);
    }
    
}