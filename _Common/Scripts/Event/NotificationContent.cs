using UnityEngine;
namespace Common.MyEvent
{
    public class NotificationContent
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public GameObject _sender;
        public int _age;
        public string _name;
        public override string ToString()
        {
            return string.Format("sender={0},age={1},name={2}", _sender.name,_age, _name);
        }

    }
}