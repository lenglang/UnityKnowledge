using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(ParticleSystem))]
public class ParticlePathEditor : Editor
{
    private ParticleSystem _particleSystem;
    private Assembly _assembly;
    private Type _particleSystemInspector;
    private MethodInfo _onInspectorGUI;
    private Editor _particleSystemEditor;
    private ParticlePath _particlePath;
    private int _currentCheckedPoint;

    private void OnEnable()
    {
        _particleSystem = target as ParticleSystem;
        //载入程序集
        _assembly = Assembly.GetAssembly(typeof(Editor));
        //获取ParticleSystemInspector类
        _particleSystemInspector = _assembly.GetTypes().Where(t => t.Name == "ParticleSystemInspector").FirstOrDefault();
        //获取OnInspectorGUI方法
        _onInspectorGUI = _particleSystemInspector.GetMethod("OnInspectorGUI", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        //创建ParticleSystemInspector的实例
        _particleSystemEditor = CreateEditor(target, _particleSystemInspector);
        _particlePath = _particleSystem.gameObject.GetComponent<ParticlePath>();
        _currentCheckedPoint = -1;

        if (!_particlePath)
        {
            _particlePath = _particleSystem.gameObject.AddComponent<ParticlePath>();
            _particlePath.IsApprove = true;
            _particlePath.hideFlags = HideFlags.HideInInspector;
            _particlePath.Waypoints = new List<Vector3>();
            _particlePath.IsHideInInspector = false;
            _particlePath.PS = _particleSystem;
        }
        if (!_particlePath.IsApprove)
        {
            DestroyImmediate(_particlePath);
            _particlePath = _particleSystem.gameObject.AddComponent<ParticlePath>();
            _particlePath.IsApprove = true;
            _particlePath.hideFlags = HideFlags.HideInInspector;
            _particlePath.Waypoints = new List<Vector3>();
            _particlePath.IsHideInInspector = false;
            _particlePath.PS = _particleSystem;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical("HelpBox");

        EditorGUILayout.BeginHorizontal();
        GUI.color = _particlePath.IsPath ? Color.white : Color.gray;
        _particlePath.IsPath = GUILayout.Toggle(_particlePath.IsPath, "", GUILayout.Width(25));
        if (GUILayout.Button("路径模式", "label"))
        {
            _particlePath.IsHideInInspector = !_particlePath.IsHideInInspector;
        }
        GUI.enabled = _particlePath.IsPath;
        EditorGUILayout.EndHorizontal();

        if (!_particlePath.IsHideInInspector)
        {
            for (int i = 0; i < _particlePath.Waypoints.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = _currentCheckedPoint == i ? Color.cyan : Color.white;
                if (GUILayout.Button("路径点" + (i + 1), "toolbarbutton"))
                {
                    _currentCheckedPoint = i;
                }
                if (i != 0)
                {
                    if (GUILayout.Button("↑", "toolbarbutton", GUILayout.Width(20)))
                    {
                        Vector3 vec = _particlePath.Waypoints[i];
                        _particlePath.Waypoints[i] = _particlePath.Waypoints[i - 1];
                        _particlePath.Waypoints[i - 1] = vec;
                    }
                }
                if (i != _particlePath.Waypoints.Count - 1)
                {
                    if (GUILayout.Button("↓", "toolbarbutton", GUILayout.Width(20)))
                    {
                        Vector3 vec = _particlePath.Waypoints[i];
                        _particlePath.Waypoints[i] = _particlePath.Waypoints[i + 1];
                        _particlePath.Waypoints[i + 1] = vec;
                    }
                }
                if (GUILayout.Button("-", "toolbarbutton", GUILayout.Width(20)))
                {
                    _particlePath.Waypoints.RemoveAt(i);
                    _currentCheckedPoint = -1;
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("", "OL Plus"))
            {
                if (_currentCheckedPoint != -1)
                {
                    _particlePath.Waypoints.Add(_particlePath.Waypoints[_currentCheckedPoint]);
                }
                else
                {
                    _particlePath.Waypoints.Add(_particlePath.transform.position);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Speed", "MiniLabel");
            _particlePath.Speed = EditorGUILayout.Slider(_particlePath.Speed, 10f, 0.5f);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        GUI.color = Color.white;
        GUI.enabled = true;

        _onInspectorGUI.Invoke(_particleSystemEditor, null);
    }

    private void OnSceneGUI()
    {
        if (_particlePath.IsPath)
        {
            Handles.color = Color.cyan;
            for (int i = 0; i < _particlePath.Waypoints.Count; i++)
            {
                if(i < _particlePath.Waypoints.Count - 1)
                {                    
                    Handles.DrawLine(_particlePath.Waypoints[i], _particlePath.Waypoints[i + 1]);
                }
            }

            if (_currentCheckedPoint != -1 && _currentCheckedPoint < _particlePath.Waypoints.Count)
            {
                Tools.current = Tool.None;
                Vector3 oldVec = _particlePath.Waypoints[_currentCheckedPoint];
                Vector3 newVec = Handles.PositionHandle(oldVec, Quaternion.identity);
                if (oldVec != newVec)
                {
                    _particlePath.Waypoints[_currentCheckedPoint] = newVec;
                }
                Handles.Label(newVec, "路径点" + (_currentCheckedPoint + 1));
            }
        }
    }
}
