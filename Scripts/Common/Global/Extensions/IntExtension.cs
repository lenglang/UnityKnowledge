using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class IntExtension
{
    private static string ToChineseWithinTenThousand(this int value)
    {
        if (value == 0)
            return "零";

        if (value < 0 || value >= 10000)
            return "";

        string result = "";

        string[] numbers = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        string[] units = new string[] { "", "十", "百", "千" };

        int cursor = 0;

        while (value > 0)
        {
            int remainder = value % 10;

            if(remainder != 0)
                result = numbers[remainder] + units[cursor] + result;
            else
                result = numbers[remainder] + result;

            cursor += 1;

            value /= 10;
        }

        result = Regex.Replace(result, "零*零", "零");
        result = Regex.Replace(result, "零$", "");
        result = Regex.Replace(result, "^一十", "十");

        return result;
    }

    public static string ToChinese(this int value)
    {
        if (value == 0)
            return "零";

        bool negative = value < 0;
        if (negative)
            value = -value;

        string result = "";

        string[] units = new string[] { "", "万", "亿" };

        int cursor = 0;

        while (value > 0)
        {
            int remainder = value % 10000;

            if (remainder != 0)
            {
                result = ToChineseWithinTenThousand(remainder) + units[cursor] + result;
                if (remainder < 1000 && remainder != value)
                    result = "零" + result;
            }

            cursor += 1;

            value /= 10000;
        }

        if (negative)
            result = "负" + result;

        return result;
    }
}
