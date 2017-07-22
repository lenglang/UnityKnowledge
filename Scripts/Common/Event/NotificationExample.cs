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
            NotificationControl<int>.Instance.AddEventListener(1, OnComplete5);
            NotificationControl<int>.Instance.AddEventListener(1, OnComplete5);
            NotificationControl<int>.Instance.AddEventListener(1, OnComplete5);
            NotificationControl<string>.Instance.AddEventListener("2", OnComplete5);
            NotificationControl<EventType>.Instance.AddEventListener(EventType.学生, OnComplete5);

            NotificationControl<int>.Instance.DispatchEvent(1);
            NotificationControl<string>.Instance.DispatchEvent("2");
            NotificationControl<EventType>.Instance.DispatchEvent(EventType.学生);

            NotificationControl<int, int>.Instance.AddEventListener(1, OnComplete2);
            NotificationControl<string, string>.Instance.AddEventListener("2", OnComplete3);
            NotificationControl<EventType, NotificationContent>.Instance.AddEventListener(EventType.学生, OnComplete4);
            NotificationControl<EventType, NotificationContent>.Instance.AddEventListener(EventType.学生, OnComplete4);

            NotificationControl<int, int>.Instance.DispatchEvent(1, 1);
            NotificationControl<string, string>.Instance.DispatchEvent("2", "2");
            NotificationContent nc = new NotificationContent();
            NotificationControl<EventType, NotificationContent>.Instance.DispatchEvent(EventType.学生, nc);
        }

        private void OnComplete5()
        {
            Debug.Log("通知方法1");
        }

        private void OnComplete2(int data)
        {
            Debug.Log("通知方法2:" + data);
        }

        private void OnComplete3(string data)
        {
            Debug.Log("通知方法2:" + data);
        }

        private void OnComplete4(NotificationContent data)
        {
            Debug.Log("通知方法2:" + data._name.Contains("王"));
        }
        private void OnDestroy()
        {
            //事件移除
            NotificationControl<int>.Instance.RemoveEvent(1);
            NotificationControl<string>.Instance.RemoveEvent("2");
            //不可使用NotificationControl<string>.Instance.RemoveAllEvent();
            NotificationControl<EventType>.Instance.RemoveAllEvent();
        }
    }
}
