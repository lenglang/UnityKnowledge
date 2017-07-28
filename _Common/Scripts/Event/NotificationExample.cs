using UnityEngine;
namespace WZK.Common
{
    public class NotificationExample : MonoBehaviour
    {
        public enum EventType
        {
            学生
        }
        // Use this for initialization
        void Start()
        {
            NotificationControl<EventType>.Instance.AddEventListener(EventType.学生, delegate { Debug.Log("666"); });
            NotificationControl<EventType>.Instance.AddEventListener(EventType.学生, OnComplete1);
            NotificationControl<EventType>.Instance.AddEventListener(EventType.学生, OnComplete3);
            NotificationControl<EventType>.Instance.DispatchEvent(EventType.学生);

            NotificationControl<EventType, NotificationContent>.Instance.AddEventListener(EventType.学生, OnComplete2);
            NotificationContent nc = new NotificationContent();
            NotificationControl<EventType, NotificationContent>.Instance.DispatchEvent(EventType.学生, nc);
        }

        private void OnComplete1()
        {
            Debug.Log("通知方法1");
        }
        private void OnComplete3()
        {
            Debug.Log("通知方法1");
        }
        private void OnComplete2(NotificationContent data)
        {
            Debug.Log("通知方法2:" + data._name.Contains("王"));
        }
        private void OnDestroy()
        {
            //内部使用最好将事件移除
            NotificationControl<EventType>.Instance.RemoveAllEvent();
        }
    }
}
