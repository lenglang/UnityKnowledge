using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class PassEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    //监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        Next(eventData, ExecuteEvents.pointerDownHandler);
    }
    //监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        Next(eventData, ExecuteEvents.pointerUpHandler);
    }

    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        Next(eventData, ExecuteEvents.submitHandler);
        Next(eventData, ExecuteEvents.pointerClickHandler);
    }
    //把事件透下去
    public void Next<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
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
                break;
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
            }
        }
    }
}
