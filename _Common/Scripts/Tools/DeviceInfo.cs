using UnityEngine;
using System.Collections;
namespace WZK
{
    public class DeviceInfo
    {
        static System.Text.StringBuilder info = new System.Text.StringBuilder();
        public static string GetInfo()
        {
            info.AppendLine("设备与系统信息:");
            //设备的模型

            GetMessage("设备模型", SystemInfo.deviceModel);
            Debug.Log(SystemInfo.deviceModel.Contains("24A34"));
            //设备的名称

            GetMessage("设备名称", SystemInfo.deviceName);

            //设备的类型

            GetMessage("设备类型（PC电脑，掌上型）", SystemInfo.deviceType.ToString());

            //系统内存大小

            GetMessage("系统内存大小MB", SystemInfo.systemMemorySize.ToString());

            //操作系统

            GetMessage("操作系统", SystemInfo.operatingSystem);

            //设备的唯一标识符

            GetMessage("设备唯一标识符", SystemInfo.deviceUniqueIdentifier);

            //显卡设备标识ID

            GetMessage("显卡ID", SystemInfo.graphicsDeviceID.ToString());

            //显卡名称

            GetMessage("显卡名称", SystemInfo.graphicsDeviceName);

            //显卡类型

            GetMessage("显卡类型", SystemInfo.graphicsDeviceType.ToString());

            //显卡供应商

            GetMessage("显卡供应商", SystemInfo.graphicsDeviceVendor);

            //显卡供应唯一ID

            GetMessage("显卡供应唯一ID", SystemInfo.graphicsDeviceVendorID.ToString());

            //显卡版本号

            GetMessage("显卡版本号", SystemInfo.graphicsDeviceVersion);

            //显卡内存大小

            GetMessage("显存大小MB", SystemInfo.graphicsMemorySize.ToString());

            //显卡是否支持多线程渲染

            GetMessage("显卡是否支持多线程渲染", SystemInfo.graphicsMultiThreaded.ToString());

            //支持的渲染目标数量

            GetMessage("支持的渲染目标数量", SystemInfo.supportedRenderTargetCount.ToString());
            return info.ToString();
        }
        static void GetMessage(params string[] str)
        {
            if (str.Length == 2)
            {
                info.AppendLine(str[0] + ":" + str[1]);
            }
        }
    }
}
