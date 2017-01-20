using UnityEngine;  
using System.Collections;  
using UnityEditor; // 引入Editor命名空间  
// 使用绘制器，如果使用了[MyRangeAttribute（0,100,"lable"）]这种自定义的属性抽屉  
// 就执行下面代码对MyRangeAttribute进行补充  
[CustomPropertyDrawer(typeof(MyRangeAttribute))]
/// <summary>  
/// 脚本位置：要求放在Editor文件夹下，其实不放也可以运行  
/// 脚本功能：对MyRangeAttribute脚本的功能实现  
/// 创建事件：2015.07.26  
/// </summary>  
// 一定要继承绘制器类 PropertyDrawer  
public class MyRangeAttributeDrawer : PropertyDrawer
{
    // 重写OnGUI的方法（坐标，SerializedProperty 序列化属性，显示的文字）  
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent lable)
    {
        // attribute 是PropertyAttribute类中的一个属性  
        // 调用MyRangeAttribute中的最大和最小值还有文字信息，用于绘制时候的显示  
        MyRangeAttribute range = attribute as MyRangeAttribute;
        // 判断传进来的值类型  
        if (property.propertyType == SerializedPropertyType.Float)
        {
            EditorGUI.Slider(position, property, range.min, range.max, range.label);
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, range.label);
        }
    }
}