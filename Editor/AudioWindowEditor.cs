using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

/// <summary>
/// 音效管理面板
/// </summary>
public class AudioWindowEditor : EditorWindow {
    [MenuItem("自定义窗口/AudioManager")]
    static void CreateWindow()
    {
        //Rect rect = new Rect(400, 400, 300, 400);
        //AudioWindowEditor window = EditorWindow.GetWindowWithRect(typeof(AudioWindowEditor), rect) as AudioWindowEditor;
        AudioWindowEditor window = EditorWindow.GetWindow<AudioWindowEditor>("音效管理");
        window.Show();
    }

    private string audioName;
    private string audioPath;
    private Dictionary<string, string> audioDict = new Dictionary<string, string>();

    void Awake()
    {
        //Debug.Log(EditorApplication.applicationPath);
        //Debug.Log(EditorApplication.applicationContentsPath);
        //Debug.Log(Application.dataPath);
        LoadAudioList();
    }
    void OnGUI()
    {
        //EditorGUILayout.TextField("输入文字1", text);
        //GUILayout.TextField("输入文字2");
        GUILayout.BeginHorizontal();
        GUILayout.Label("音效名称");
        GUILayout.Label("音效路径");
        GUILayout.Label("操作");
        GUILayout.EndHorizontal();
        foreach (string key in audioDict.Keys)
        {
            string value;
            audioDict.TryGetValue(key, out value);
            GUILayout.BeginHorizontal();
            GUILayout.Label(key);
            GUILayout.Label(value);
            if (GUILayout.Button("删除"))
            {
                audioDict.Remove(key);
                SaveAudioList();
                return;
            }
            GUILayout.EndHorizontal();
        }

        audioName = EditorGUILayout.TextField("音效名字", audioName);
        audioPath = EditorGUILayout.TextField("音效路径", audioPath);
        if (GUILayout.Button("添加音效")) 
        { 
            object o = Resources.Load(audioPath);
            if (o == null)
            {
                Debug.LogWarning("音效不存在于" + audioPath + " 添加不成功");
                audioPath = "";
            }
            else {
                if (audioDict.ContainsKey(audioName))
                {
                    Debug.LogWarning("名字已经存在，请修改");
                }
                else
                {
                    audioDict.Add(audioName, audioPath);
                    SaveAudioList();
                }
            }
        }
    }

    //窗口面板被更新的时候调用
    void OnInspectorUpdate()
    {
        //Debug.Log("Update");
        LoadAudioList();
    }

    

    private void SaveAudioList()
    {
        StringBuilder sb = new StringBuilder();
         
        foreach (string key in audioDict.Keys)
        {
            string value;
            audioDict.TryGetValue(key, out value);
            sb.Append(key + "," + value + "\n");
        } 
        File.WriteAllText(AudioManager.AudioTextPath, sb.ToString());
        //File.AppendAllText(savePath, sb.ToString());
    }
    private void LoadAudioList()
    {
        audioDict = new Dictionary<string, string>();
        if (File.Exists(AudioManager.AudioTextPath) == false) return;
        string[] lines = File.ReadAllLines(AudioManager.AudioTextPath);
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            string[] keyvalue = line.Split(',');
            audioDict.Add(keyvalue[0], keyvalue[1]);
        }
    }
}