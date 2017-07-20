using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class SceneEditor:Editor
{
    void OnSceneGUI()
    {
        //得到test脚本的对象
        Test test = (Test)target;

        //绘制文本框
        Handles.Label(test.transform.position + Vector3.up * 2,
                   test.transform.name + " : " + test.transform.position.ToString());

        //开始绘制GUI
        Handles.BeginGUI();

        //规定GUI显示区域
        GUILayout.BeginArea(new Rect(100, 100, 100, 100));

        //GUI绘制一个按钮
        if (GUILayout.Button("这是一个按钮!"))
        {
            Debug.Log("test");
        }
        //GUI绘制文本框
        GUILayout.Label("我在编辑Scene视图");

        GUILayout.EndArea();

        Handles.EndGUI();
    }
    }
