using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager  {
    //static 静态  const是常量
    private static string audioTextPathPrefix = Application.dataPath + "/UnityKnowledge/Resources/";
    private const string audioTextPathMiddle = "audiolist";
    private const string audioTextPathPostfix = ".txt";

    public static string AudioTextPath
    {
        get
        {
            return audioTextPathPrefix +audioTextPathMiddle+ audioTextPathPostfix;
        }
    }

    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

    public bool isMute=false;

    //public AudioManager()
    //{
    //    LoadAudioClip();
    //}
    public void Init()
    {
        LoadAudioClip();
    }
    private void LoadAudioClip()
    { 
        audioClipDict = new Dictionary<string, AudioClip>();
        TextAsset ta = Resources.Load<TextAsset>(audioTextPathMiddle);
        string[] lines = ta.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            string[] keyvalue = line.Split(',');
            string key = keyvalue[0]; 
            AudioClip value = Resources.Load<AudioClip>(keyvalue[1]);
            audioClipDict.Add(key, value);
        }
    }

    public void Play(string name)
    {
        if(isMute)return;
        AudioClip ac;
        audioClipDict.TryGetValue(name, out ac);
        if (ac != null)
        {
            AudioSource.PlayClipAtPoint(ac, Vector3.zero);
        }
    }
    public void Play(string name, Vector3 position)
    {
        if(isMute)return;
        AudioClip ac;
        audioClipDict.TryGetValue(name, out ac);
        if (ac != null)
        {
            AudioSource.PlayClipAtPoint(ac, position);
        }
    }
}
