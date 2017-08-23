using UnityEngine;
namespace WZK
{
    public class NotificationExample : MonoBehaviour
    {
        public enum EventType
        {
            分数
        }
        // Use this for initialization
        void Start()
        {
            NotificationManager<EventType>.Instance.AddEventListener(EventType.分数, delegate { Debug.Log("666"); });
            NotificationManager<EventType>.Instance.AddEventListener(EventType.分数, OnComplete1);
            NotificationManager<EventType>.Instance.DispatchEvent(EventType.分数);

            NotificationManager<EventType, NotificationContent>.Instance.AddEventListener(EventType.分数, OnComplete2);
            NotificationContent nc = new NotificationContent();
            nc._sender = this.gameObject;
            nc._age = 20;
            nc._name = "宝宝";
            NotificationManager<EventType, NotificationContent>.Instance.DispatchEvent(EventType.分数, nc);
        }
        private void OnComplete1()
        {
            Debug.Log("分数更新");
        }
        private void OnComplete2(NotificationContent data)
        {
            Debug.Log("获取数据:" + data.ToString());
        }
        private void OnDestroy()
        {
            //内部使用最好将事件移除
            NotificationManager<EventType>.Instance.RemoveAllEvent();
        }
    }
}
