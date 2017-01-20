using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class TxtManager {
    private static Dictionary<string, string> audioDict = new Dictionary<string, string>();
    public TxtManager()
    {
        //audioClipDict.TryGetValue(name, out ac);
    }
    public static void SaveTxt()
    {
        audioDict.Add("1", "321.mp3");
        audioDict.Add("2", "3121.mp3");
        audioDict.Add("2sdaf", "3121.mp3");
        StringBuilder sb = new StringBuilder();
        foreach (string key in audioDict.Keys)
        {
            string value;
            audioDict.TryGetValue(key, out value);
            sb.Append(key + "," + value + "\n");
        }
        try
        {
            File.WriteAllText(Application.dataPath + "/Resources/test.txt", sb.ToString());
        }
        catch (System.Exception)
        {
            Debug.LogError("不存在Resources文件夹");
        } 
    }
    public static void ReadTxt()
    {
        //Dictionary<string,AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
        TextAsset ta = Resources.Load<TextAsset>("test");
        string[] lines = ta.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            string[] keyvalue = line.Split(',');
            string key = keyvalue[0];
            Debug.Log(key);
            //AudioClip value = Resources.Load<AudioClip>(keyvalue[1]);
            //audioClipDict.Add(key, value);
        }
        //扩展
        //if (File.Exists(AudioManager.AudioTextPath) == false) return;
        string[] liness = File.ReadAllLines(Application.dataPath + "/Resources/test.txt");
        foreach (string line in liness)
        {
            if (string.IsNullOrEmpty(line)) continue;
            string[] keyvalue = line.Split(',');
            string key = keyvalue[0];
            Debug.Log(key + "," + keyvalue[1]);
        }

    }
}
