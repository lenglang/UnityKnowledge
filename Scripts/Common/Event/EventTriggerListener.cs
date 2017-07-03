using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
namespace WZK.Common
{
    /// <summary>
    /// 注意事项：1.场景需添加EventSystem；2.摄像机需挂载PhysicsRaycaster脚本；3.监听对象需要挂载BoxCollider组件
    /// </summary>
    public class EventTriggerListener :EventTrigger
    {
        public delegate void VoidDelegate(PointerEventData evenData, GameObject obj);
        public VoidDelegate _onClick;
        public VoidDelegate _onDown;
        public VoidDelegate _onEnter;
        public VoidDelegate _onExit;
        public VoidDelegate _onUp;
        public VoidDelegate _onBeginDrag;
        public VoidDelegate _onDrop;
        public VoidDelegate _onDrag;
        public VoidDelegate _onEndDrag;
        public VoidDelegate _onInitializePotentialDrag;
        public VoidDelegate _onScroll;
        public delegate void VoidDelegate2(BaseEventData evenData, GameObject obj);
        public VoidDelegate2 _onSelect;
        public VoidDelegate2 _onUpdateSelect;
        public VoidDelegate2 _onSubmit;
        public VoidDelegate2 _onCancel;
        public VoidDelegate2 _onDeselect;
        public delegate void VoidDelegate3(AxisEventData evenData, GameObject obj);
        public VoidDelegate3 _onMove;
        static public EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }
        /// <summary>
        /// 事件渗透
        /// 使用：PassEvent(evenData, ExecuteEvents.beginDragHandler);PassEvent(evenData, ExecuteEvents.dragHandler);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="function"></param>
        public static void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                }
            }
        }
        public override void OnSubmit(BaseEventData eventData)
        {
            if (_onSubmit != null) _onSubmit(eventData, gameObject);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_onClick != null) _onClick(eventData, gameObject);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_onDown != null) _onDown(eventData, gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (_onEnter != null) _onEnter(eventData, gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (_onExit != null) _onExit(eventData, gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (_onUp != null) _onUp(eventData, gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (_onSelect != null) _onSelect(eventData, gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (_onUpdateSelect != null) _onUpdateSelect(eventData, gameObject);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_onBeginDrag != null) _onBeginDrag(eventData, gameObject);
        }
        public override void OnCancel(BaseEventData eventData)
        {
            if (_onCancel != null) _onCancel(eventData, gameObject);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            if (_onDeselect != null) _onDeselect(eventData, gameObject);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (_onDrag != null) _onDrag(eventData, gameObject);
        }
        public override void OnDrop(PointerEventData eventData)
        {
            if (_onDrop != null) _onDrop(eventData, gameObject);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_onEndDrag != null) _onEndDrag(eventData, gameObject);
        }
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (_onInitializePotentialDrag != null) _onInitializePotentialDrag(eventData, gameObject);
        }
        public override void OnMove(AxisEventData eventData)
        {
            if (_onMove != null) _onMove(eventData, gameObject);
        }
        public override void OnScroll(PointerEventData eventData)
        {
            if (_onScroll != null) _onScroll(eventData, gameObject);
        }

    }
}