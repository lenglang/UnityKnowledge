using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class GUIExample : MonoBehaviour
{
    private string _setLevel = "0";
    void OnGUI()
    {
        GUILayout.Space(Screen.height / 6);
        if (GUILayout.Button("测试1", GUILayout.Height(Screen.height / 10)))
        {

        }
        if (GUILayout.Button("测试2", GUILayout.Height(Screen.height / 10)))
        {

        }
        _setLevel = GUILayout.TextField(_setLevel, 80);
        _setLevel = Regex.Replace(_setLevel, "[^0-9]", "");
        if (GUILayout.Button("设置关卡", GUILayout.Height(Screen.height / 10)))
        {
            Debug.Log((int)float.Parse(_setLevel));
        }

    }
}
