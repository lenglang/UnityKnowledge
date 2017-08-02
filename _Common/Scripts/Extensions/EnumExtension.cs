using UnityEngine;

public static class EnumExtension
{

    public static string GetEnumDescription(this System.Enum enumValue)
    {
        string str = enumValue.ToString();
        System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
        object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        if (objs == null || objs.Length == 0) return str;
        System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
        return da.Description;
    }
    public static int GetLength(this System.Enum value)
    {
        return System.Enum.GetNames(value.GetType()).Length;
    }
}
