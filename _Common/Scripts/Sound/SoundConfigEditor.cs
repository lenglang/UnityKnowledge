using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
namespace Common.Sound
{
    [CustomEditor(typeof(SoundConfig))]
    public class SoundConfigEditor : Editor
    {
        private bool _isDelete;//是否删除
        private bool _isExist;//是否存在
        private string _directionPath;//文件夹路径
        private string _fileAssetPath;//文件工程目录
        private string[] _typeButtons = { "人声", "音效", "背景音乐" };//类型按钮切换
        private int _selectType = 0;//选择的类型
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SoundConfig soundConfig = target as SoundConfig;
            if (GUILayout.Button("生成枚举配置"))
            {
                string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
                if (string.IsNullOrEmpty(path))
                {
                    path = Application.dataPath;
                }
                else
                {
                    path = Application.dataPath.Substring(0, Application.dataPath.IndexOf("/")) + path.Substring(0, path.LastIndexOf("/") + 1);
                }
                path += "/" + soundConfig._fileName + ".cs";
                if (File.Exists(path) && EditorUtility.DisplayDialog("警告", "已存在该配置，是否覆盖：" + path, "是的", "取消")) //显示对话框  
                {
                    CreateCSConfig(soundConfig, path);
                }
                else
                {
                    CreateCSConfig(soundConfig, path);
                }
            }
            soundConfig._fileName = EditorGUILayout.TextField("脚本名", soundConfig._fileName);
            soundConfig._nameSpace = EditorGUILayout.TextField("命名空间", soundConfig._nameSpace);
            _selectType = GUILayout.Toolbar(_selectType, _typeButtons);
            List<SoundConfig.Config> scList;
            switch (_selectType)
            {
                case 0:
                    EditorGUILayout.LabelField("枚举名", soundConfig._voiceEnumType);
                    scList = soundConfig._voiceList;
                    break;
                case 1:
                    EditorGUILayout.LabelField("枚举名", soundConfig._soundEnumType);
                    scList = soundConfig._soundList;
                    break;
                case 2:
                    EditorGUILayout.LabelField("枚举名", soundConfig._musiceEnumType);
                    scList = soundConfig._musiceList;
                    break;
                default:
                    scList = soundConfig._musiceList;
                    break;
            }
            for (int i = 0; i < scList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                scList[i]._audioClip = EditorGUILayout.ObjectField("声音" + (i + 1), scList[i]._audioClip, typeof(AudioClip), true);
                _isDelete = false;
                if (GUILayout.Button("删除" + (i + 1)))
                {
                    _isDelete = true;
                }
                EditorGUILayout.EndHorizontal();
                scList[i]._desc = EditorGUILayout.TextField("描述" + (i + 1), scList[i]._desc);
                if (scList[i]._desc == "" && scList[i]._audioClip) scList[i]._desc = scList[i]._audioClip.name;
                if (_isDelete) scList.RemoveAt(i);
            }
            if (Event.current.type == EventType.DragExited)
            {
                if (DragAndDrop.objectReferences[0].GetType() == typeof(AudioClip))
                {
                    AddAudioClip(scList, (AudioClip)DragAndDrop.objectReferences[0]);
                }
                else
                {
                    _directionPath = Application.dataPath;
                    _directionPath = _directionPath.Substring(0, _directionPath.LastIndexOf("/") + 1) + DragAndDrop.paths[0];
                    string[] strs = _directionPath.Split('.');
                    if (strs.Length == 1 && Directory.Exists(_directionPath))
                    {
                        DirectoryInfo direction = new DirectoryInfo(_directionPath);
                        FileInfo[] files = direction.GetFiles("*.mp3", SearchOption.AllDirectories);
                        for (int i = 0; i < files.Length; i++)
                        {
                            _fileAssetPath = files[i].DirectoryName;
                            _fileAssetPath = _fileAssetPath.Substring(_fileAssetPath.IndexOf("Assets"));
                            AddAudioClip(scList, AssetDatabase.LoadAssetAtPath<AudioClip>(_fileAssetPath + "/" + files[i].Name));
                        }
                    }
                }
            }
            if (GUILayout.Button("清空") && scList.Count != 0)
            {
                if (EditorUtility.DisplayDialog("警告", "你确定要清空列表里的数据吗？", "是的", "取消"))
                {
                    scList.Clear();
                }
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(soundConfig);
        }
        /// <summary>
        /// 添加音频
        /// </summary>
        private void AddAudioClip(List<SoundConfig.Config> scList, AudioClip ac)
        {
            _isExist = false;
            for (int i = 0; i < scList.Count; i++)
            {
                if (scList[i]._audioClip == ac)
                {
                    _isExist = true;
                    Debug.LogError("配置表里已存在该音频");
                    break;
                }
            }
            if (_isExist == false) scList.Add(new SoundConfig.Config(ac));
        }
        /// <summary>
        /// 创建脚本配置
        /// </summary>
        /// <param name="soundConfig"></param>
        /// <param name="path"></param>
        private void CreateCSConfig(SoundConfig soundConfig, string path)
        {
            string audioConfig = "";
            CreateEnum(soundConfig._voiceList, ref audioConfig, soundConfig._voiceEnumType);
            audioConfig += "\n";
            CreateEnum(soundConfig._soundList, ref audioConfig, soundConfig._soundEnumType);
            audioConfig += "\n";
            CreateEnum(soundConfig._musiceList, ref audioConfig, soundConfig._musiceEnumType);
            string config = "using System.ComponentModel;" + "\n"
                + "namespace " + soundConfig._nameSpace + "\n"
                + "{" + "\n"
                + audioConfig
                + "\n"
                + "}";
            File.WriteAllText(path, config);
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 枚举创建
        /// </summary>
        /// <param name="scList"></param>
        /// <param name="audioConfig"></param>
        /// <param name="enumType"></param>
        private void CreateEnum(List<SoundConfig.Config> scList, ref string audioConfig, string enumType)
        {
            audioConfig += "public enum " + enumType + "\n"
                + "{" + "\n";
            for (int i = 0; i < scList.Count; i++)
            {
                audioConfig += "[Description(" + '"' + scList[i]._audioClip.name + '"' + ")]" + "\n";
                audioConfig += scList[i]._desc;
                if (i != scList.Count - 1)
                {
                    audioConfig += "," + "\n";
                }
            }
            audioConfig += "\n}";
        }
    }
}
