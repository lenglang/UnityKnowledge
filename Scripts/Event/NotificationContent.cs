using System;
using UnityEngine;
    public class NotificationContent
    {
        /// <summary>
        /// 通知发送者
        /// </summary>
        public GameObject sender;
        /// <summary>
        /// 通知内容
        /// 备注：在发送消息时需要装箱、解析消息时需要拆箱
        /// 所以这是一个糟糕的设计，需要注意。
        /// </summary>
        public object param;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sender">通知发送者</param>
        /// <param name="param">通知内容</param>
        public NotificationContent(GameObject sender, object param)
        {
            this.sender = sender;
            this.param = param;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="param"></param>
        public NotificationContent(object param)
        {
            this.sender = null;
            this.param = param;
        }
        /// <summary>
        /// 实现ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("sender={0},param={1}", this.sender, this.param);
        }
    }