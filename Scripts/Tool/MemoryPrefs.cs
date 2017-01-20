using System;
using System.Collections.Generic;
using System.Linq;

public sealed class MemoryPrefs
{
    public static List<string> Keys
    {
        get
        {
            return dictionary.Keys.ToList();
        }
    }

    private static Dictionary<string, object> dictionary = new Dictionary<string, object>();

    public static void DeleteAll()
    {
        dictionary.Clear();
    }

    public static void DeleteKey(string key)
    {
        if (!HasKey(key))
            return;

        dictionary.Remove(key);
    }

    public static bool HasKey(string key)
    {
        return dictionary.ContainsKey(key);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        if (!HasKey(key))
            return defaultValue;

        return (bool)dictionary[key];
    }

    public static byte GetByte(string key, byte defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (byte)dictionary[key];
    }

    public static byte[] GetBytes(string key, byte[] defaultValue = null)
    {
        if (!HasKey(key))
            return defaultValue;

        return (byte[])dictionary[key];
    }

    public static char GetChar(string key, char defaultValue = '\0')
    {
        if (!HasKey(key))
            return defaultValue;

        return (char)dictionary[key];
    }

    public static decimal GetDecimal(string key, decimal defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (decimal)dictionary[key];
    }

    public static double GetDouble(string key, double defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (double)dictionary[key];
    }

    public static short GetInt16(string key, short defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (short)dictionary[key];
    }

    public static int GetInt32(string key, int defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (int)dictionary[key];
    }

    public static long GetInt64(string key, long defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (long)dictionary[key];
    }

    public static sbyte GetSByte(string key, sbyte defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (sbyte)dictionary[key];
    }

    public static float GetFloat(string key, float defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (float)dictionary[key];
    }

    public static string GetString(string key, string defaultValue = "")
    {
        if (!HasKey(key))
            return defaultValue;

        return (string)dictionary[key];
    }

    public static ushort GetUInt16(string key, ushort defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (ushort)dictionary[key];
    }

    public static uint GetUInt32(string key, uint defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (uint)dictionary[key];
    }

    public static ulong GetUInt64(string key, ulong defaultValue = 0)
    {
        if (!HasKey(key))
            return defaultValue;

        return (ulong)dictionary[key];
    }

    public static T GetObject<T>(string key, T defaultValue = default(T))
    {
        if (!HasKey(key))
            return defaultValue;

        return (T)dictionary[key];
    }

    public static void SetBool(string key, bool value)
    {
        dictionary[key] = value;
    }

    public static void SetByte(string key, byte value)
    {
        dictionary[key] = value;
    }

    public static void SetBytes(string key, byte[] value)
    {
        dictionary[key] = value;
    }

    public static void SetChar(string key, char value)
    {
        dictionary[key] = value;
    }

    public static void SetDecimal(string key, decimal value)
    {
        dictionary[key] = value;
    }

    public static void SetDouble(string key, double value)
    {
        dictionary[key] = value;
    }

    public static void SetInt16(string key, short value)
    {
        dictionary[key] = value;
    }

    public static void SetInt32(string key, int value)
    {
        dictionary[key] = value;
    }

    public static void SetInt64(string key, long value)
    {
        dictionary[key] = value;
    }

    public static void SetSByte(string key, sbyte value)
    {
        dictionary[key] = value;
    }

    public static void SetFloat(string key, float value)
    {
        dictionary[key] = value;
    }

    public static void SetString(string key, string value)
    {
        dictionary[key] = value;
    }

    public static void SetUInt16(string key, ushort value)
    {
        dictionary[key] = value;
    }

    public static void SetUInt32(string key, uint value)
    {
        dictionary[key] = value;
    }

    public static void SetUInt64(string key, ulong value)
    {
        dictionary[key] = value;
    }

    public static void SetObject(string key, object value)
    {
        dictionary[key] = value;
    }
    public static DateTime GetDateTime(string key, DateTime defaultValue = default(DateTime))
    {
        if (!HasKey(key))
            return defaultValue;

        string timeStr = dictionary[key] as string;

        DateTime.TryParse(timeStr, out defaultValue);
        return defaultValue;
    }

    public static void SetDateTime(string key, DateTime value)
    {
        dictionary[key] = value.ToString();
    }

}