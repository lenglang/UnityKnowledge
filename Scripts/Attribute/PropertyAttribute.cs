using UnityEngine;  
using System.Collections;  
/// <summary>  
/// 脚本位置：要求放在Editor文件夹下，其实不放也可以运行  
/// 脚本功能：实现一个在Inspector面板中可以用滑动条来控制变量的大小  
/// 创建事件：2015.07.26  
/// </summary>  
public class MyRangeAttribute : PropertyAttribute
{
    // 这3个变量是在Inspector面板中显示的  
    public float min;    // 定义一个float类型的最大  
    public float max;    // 定义一个float类型的最大  
    public string label; // 显示标签  

    // 在脚本（1）ValueRangeExample定义[MyRangeAttribute(0,100,"玩家魔法值")]  
    // 就可以使用这个功能了，但是为什么这里的string label = ""呢  
    // 因为给了一个初值的话，在上面定义[MyRangeAttribute(0,100)]就不会报错了，不会提示没有2个参数的方法  
    public MyRangeAttribute(float min, float max, string label = "")
    {
        this.min = min;
        this.max = max;
        this.label = label;
    }
}