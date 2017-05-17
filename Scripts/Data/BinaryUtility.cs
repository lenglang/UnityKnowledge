using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class BinaryUtility
{
    public static T DeserializeBinary<T>(byte[] bytes)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return default(T);
    }

    public static T DeserializeBinary<T>(string path)
    {
        if (!File.Exists(path))
            return default(T);

        return DeserializeBinary<T>(File.ReadAllBytes(path));
    }

    public static void SerializeBinary(string path, object o)
    {
        if (string.IsNullOrEmpty(path))
            return;

        byte[] bytes = SerializeBinary(o);

        File.WriteAllBytes(path, bytes);
    }

    public static byte[] SerializeBinary(object o)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, o);

                byte[] bytes = stream.ToArray();

                return bytes;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return null;
    }
}

