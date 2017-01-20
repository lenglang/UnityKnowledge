using System.Collections.Generic;
/// <summary>
/// 工具类
/// </summary>
using UnityEngine;
public class WZKTool
{
    /// <summary>
    /// 将秒转成24小时格式
    /// </summary>
    /// <param name="s">多少秒</param>
    /// <returns>24小时格式</returns>
    public static string TimeFormat(int s)
    {
        int hour = s / 3600;
        int mm = 0;
        int ss = 0;
        if (hour > 0)
        {
            mm = (s % 3600) / 60;
            ss = s - hour * 3600 - mm * 60;
        }
        else
        {
            mm = s / 60;
            ss = s - hour * 3600 - mm * 60;
        }
        return timeCv(hour) + ":" + timeCv(mm) + ":" + timeCv(ss);
    }
    private static string timeCv(int value)
    {
        string str = "";
        if (value >= 0)
        {
            if (value < 10)
                str = "0" + value;
            else
                str = value.ToString();
        }
        else
        {
            str = "00";
        }
        return str;
    }
    /// <summary>  
    /// 对相机截图,相机Rendering Path设置成Deferred或Lagacy Deferred
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">Rect.截屏的区域</param>
    /// <param name="clipRect">Rect.通过Image显示图片的区域</param>  
    /// <param name="save">bool.是否保存到本地</param>  
    public static Sprite CaptureCamera(Camera camera, Rect rect, Rect clipRect, bool save = false)
    {
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();
        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);
        if(save)
        { // 最后将这些纹理数据，成一个png图片文件保存到本地 
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.dataPath + "/Screenshot.png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张照片: {0}", filename));
        }
        //Texture2D转换成Sprite
        return Sprite.Create(screenShot, clipRect, new Vector2(0, 0));
    }
    /// <summary>  
    /// Captures the screenshot2.  
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="rect">Rect.截图的区域，左下角为o点</param>  
    /// 截全屏CaptureScreenshot2( new Rect( Screen.width*0f, Screen.height*0f, Screen.width*1f, Screen.height*1f));
    public static Texture2D CaptureScreenshot2(Rect rect)
    {
        // 先创建一个的空纹理，大小可根据实现需要来设置  
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        // 读取屏幕像素信息并存储为纹理数据，  
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        // 然后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张图片: {0}", filename));
        // 最后，我返回这个Texture2d对象，这样我们直接，所这个截图图示在游戏中，当然这个根据自己的需求的。  
        return screenShot;
    }  
    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name=”timeStamp”></param>
    /// <returns></returns>
    public static System.DateTime GetTime(string timeStamp)
    {
        System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        System.TimeSpan toNow = new System.TimeSpan(lTime);
        return dtStart.Add(toNow);
    }
    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name=”time”></param>
    /// <returns></returns>
    public static int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    } 
}