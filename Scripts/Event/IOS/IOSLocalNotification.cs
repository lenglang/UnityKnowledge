using UnityEngine;
using System.Collections;

public class IOSLocalNotifition : MonoBehaviour
{
    //本地推送
    public static void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newDate = new System.DateTime(year, month, day, hour, 0, 0);
        NotificationMessage(message, newDate, isRepeatDay);
    }
    //本地推送 你可以传入一个固定的推送时间
    public static void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
        //推送时间需要大于当前时间
        if (newDate > System.DateTime.Now)
        {
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            if (isRepeatDay)
            {
                //是否每天定期循环
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }
    }

    void Awake()
    {
        //第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
        CleanNotification();
    }

    void OnApplicationPause(bool paused)
    {
        //程序进入后台时
        if (paused)
        {
            //10秒后发送
            NotificationMessage("雨松MOMO : 10秒后发送", System.DateTime.Now.AddSeconds(10), false);
            //每天中午12点推送
            NotificationMessage("雨松MOMO : 每天中午12点推送", 12, true);
        }
        else
        {
            //程序从后台进入前台时
            CleanNotification();
        }
    }

    //清空所有本地消息
    void CleanNotification()
    {
        UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification();
        l.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
    }
}