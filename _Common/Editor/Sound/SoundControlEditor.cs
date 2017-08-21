using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
namespace WZK
{
    /// <summary>
    /// 声音管理编辑器扩展
    /// </summary>
    [CustomEditor(typeof(SoundControl))]
    public class SoundControlEditor : Editor
    {
        private bool _isDelete;//是否删除
        private bool _isExist;//是否存在
        private string _directionPath;//文件夹路径
        private string _fileAssetPath;//文件工程目录
        private string[] _typeButtons = { "人声", "音效", "背景音乐", "国际化语言" };//类型按钮切换
        private int _selectType = 0;//选择的类型
        private bool _isOtherLanguage = false;//是否其他语言
        private SoundControl soundControl;//声音控制
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            soundControl = target as SoundControl;
            soundControl._testLanguage = EditorGUILayout.TextField("测试语言-Debug模式下才生效", soundControl._testLanguage);
            soundControl._bgSoundVolume = EditorGUILayout.FloatField("背景音乐音量", soundControl._bgSoundVolume);
            soundControl._savePath = EditorGUILayout.TextField("生成的枚举类存放的Assets下文件夹路径", soundControl._savePath);
            if (GUILayout.Button("生成枚举配置"))
            {
                if (string.IsNullOrEmpty(soundControl._fileName) || string.IsNullOrEmpty(soundControl._nameSpace))
                {
                    EditorUtility.DisplayDialog("警告", "脚本名或命名空间未填写", "知道了");
                    return;
                }
                string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
                if (string.IsNullOrEmpty(soundControl._savePath))
                {
                    path = Application.dataPath + path.Substring(path.IndexOf("/"));
                    path = Path.GetDirectoryName(path);
                }
                else
                {
                    path = Application.dataPath + "/" + soundControl._savePath;
                }
                path += "/" + soundControl._fileName + ".cs";
                if (File.Exists(path) && EditorUtility.DisplayDialog("警告", "已存在该配置，是否覆盖：" + path, "是的", "取消")) //显示对话框  
                {
                    CreateCSConfig(soundControl, path);
                }
                else
                {
                    CreateCSConfig(soundControl, path);
                }
            }
            soundControl._fileName = EditorGUILayout.TextField("脚本名", soundControl._fileName);
            soundControl._nameSpace = EditorGUILayout.TextField("命名空间", soundControl._nameSpace);
            _selectType = GUILayout.Toolbar(_selectType, _typeButtons);
            List<SoundControl.Config> scList;
            _isOtherLanguage = false;
            if (_selectType == 3) _isOtherLanguage = true;
            switch (_selectType)
            {
                case 0:
                    EditorGUILayout.LabelField("枚举名", soundControl._voiceEnumType);
                    scList = soundControl._voiceList;
                    break;
                case 1:
                    EditorGUILayout.LabelField("枚举名", soundControl._soundEnumType);
                    scList = soundControl._soundList;
                    break;
                case 2:
                    EditorGUILayout.LabelField("枚举名", soundControl._musiceEnumType);
                    scList = soundControl._musiceList;
                    break;
                default:
                    scList = soundControl._musiceList;
                    break;
            }
            if (_isOtherLanguage)
            {
                OhterLanguage(soundControl);
            }
            else
            {
                ChineseLanguage(scList);
            }
            GUILayout.Space(1000);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(soundControl);
            if (GUI.changed)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        /// <summary>
        /// 创建脚本配置
        /// </summary>
        /// <param name="soundControl"></param>
        /// <param name="path"></param>
        private void CreateCSConfig(SoundControl soundControl, string path)
        {
            string audioConfig = "";
            CreateEnum(soundControl._voiceList, ref audioConfig, soundControl._voiceEnumType);
            audioConfig += "\n";
            CreateEnum(soundControl._soundList, ref audioConfig, soundControl._soundEnumType);
            audioConfig += "\n";
            CreateEnum(soundControl._musiceList, ref audioConfig, soundControl._musiceEnumType);
            string config = "using System.ComponentModel;" + "\n"
                + "namespace " + soundControl._nameSpace + "\n"
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
        private void CreateEnum(List<SoundControl.Config> scList, ref string audioConfig, string enumType)
        {
            audioConfig += "public enum " + enumType + "\n"
                + "{" + "\n";
            for (int i = 0; i < scList.Count; i++)
            {
                audioConfig += "[Description(" + '"' + scList[i]._resourcesPath + '"' + ")]" + "\n";
                audioConfig += scList[i]._desc;
                if (i != scList.Count - 1)
                {
                    audioConfig += "," + "\n";
                }
            }
            audioConfig += "\n}";
        }
        /// <summary>
        /// 中文语言
        /// </summary>
        private void ChineseLanguage(List<SoundControl.Config> scList)
        {
            for (int i = 0; i < scList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                scList[i]._audioClip = (AudioClip)EditorGUILayout.ObjectField("声音" + (i + 1), scList[i]._audioClip, typeof(AudioClip), false);
                _isDelete = false;
                if (GUILayout.Button("删除" + (i + 1)))
                {
                    _isDelete = true;
                }
                EditorGUILayout.EndHorizontal();
                scList[i]._desc = EditorGUILayout.TextField("描述" + (i + 1), scList[i]._desc);
                GUILayout.Space(10);
                if (scList[i]._desc == "" && scList[i]._audioClip) scList[i]._desc = scList[i]._audioClip.name;
                if (_isDelete) scList.RemoveAt(i);
            }
            if (Event.current.type == EventType.DragExited)
            {
                if (DragAndDrop.objectReferences[0].GetType() == typeof(AudioClip))
                {
                    AddAudioClip(scList, (AudioClip)DragAndDrop.objectReferences[0], DragAndDrop.paths[0]);
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
                            _fileAssetPath = _fileAssetPath.Substring(_fileAssetPath.IndexOf("Assets")) + "/" + files[i].Name;
                            AddAudioClip(scList, AssetDatabase.LoadAssetAtPath<AudioClip>(_fileAssetPath), _fileAssetPath);
                        }
                    }
                }
            }
            GUILayout.Space(30);
            if (GUILayout.Button("清空") && scList.Count != 0)
            {
                if (EditorUtility.DisplayDialog("警告", "你确定要清空列表里的数据吗？", "确定", "取消"))
                {
                    scList.Clear();
                }
            }
        }
        /// <summary>
        /// 添加音频
        /// </summary>
        private void AddAudioClip(List<SoundControl.Config> scList, AudioClip ac, string resourcesPath)
        {
            resourcesPath = resourcesPath.Replace("\\", "/");
            if (resourcesPath.Contains("Resources")) resourcesPath = resourcesPath.Substring(resourcesPath.IndexOf("Resources/") + 10);
            resourcesPath = Path.GetDirectoryName(resourcesPath) + "/" + Path.GetFileNameWithoutExtension(resourcesPath);
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
            if (_isExist == false) scList.Add(new SoundControl.Config(ac, resourcesPath));
        }
        /// <summary>
        /// 其他语言
        /// </summary>
        public string _addLanguageType = "";
        private void OhterLanguage(SoundControl soundControl)
        {
            EditorGUILayout.LabelField("说明：其他语言的音频文件名需要跟中文音频的文件名相同,这样才会在游戏开始时，根据当前语言将中文对应的音频替换掉。");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("添加国际语言");
            if (soundControl._languageAudioClipList.Count < soundControl._languageTypeList.Count)
            {
                soundControl._languageAudioClipList.Clear();
                for (int i = 0; i < soundControl._languageTypeList.Count; i++)
                {
                    soundControl._languageAudioClipList.Add(new SoundControl.LanguageAudioClip(soundControl._languageTypeList[i]));
                }
            }
            _addLanguageType = EditorGUILayout.TextField(_addLanguageType);
            if (GUILayout.Button("添加"))
            {
                if (_addLanguageType != "" && soundControl._languageTypeList.IndexOf(_addLanguageType) == -1)
                {
                    soundControl._languageTypeList.Add(_addLanguageType);
                    soundControl._languageAudioClipList.Add(new SoundControl.LanguageAudioClip(_addLanguageType));
                }
            }
            EditorGUILayout.EndHorizontal();
            string[] languageButtons;
            languageButtons = new string[soundControl._languageTypeList.Count];
            for (int i = 0; i < soundControl._languageTypeList.Count; i++)
            {
                languageButtons[i] = soundControl._languageTypeList[i];
            }
            soundControl._choseLanguage = GUILayout.Toolbar(soundControl._choseLanguage, languageButtons);//删除时索引会超出的地方，已做了修复
            if (languageButtons.Length != 0)
            {
                //绘制需放在未添加前绘制，否则会报错
                List<AudioClip> audioClip = soundControl._languageAudioClipList[soundControl._choseLanguage]._audioClipList;
                for (int i = 0; i < audioClip.Count; i++)
                {
                    GUILayout.Space(10);
                    audioClip[i] = (AudioClip)EditorGUILayout.ObjectField("声音" + (i + 1), audioClip[i], typeof(AudioClip), false);
                }
                if (Event.current.type == EventType.DragExited)
                {
                    if (DragAndDrop.objectReferences[0].GetType() == typeof(AudioClip))
                    {
                        AddOtherLanguageAudioClip(soundControl, soundControl._choseLanguage, (AudioClip)DragAndDrop.objectReferences[0]);
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
                                _fileAssetPath = _fileAssetPath.Substring(_fileAssetPath.IndexOf("Assets")) + "/" + files[i].Name;
                                AddOtherLanguageAudioClip(soundControl, soundControl._choseLanguage, AssetDatabase.LoadAssetAtPath<AudioClip>(_fileAssetPath));
                            }
                        }
                    }
                }
            }
            GUILayout.Space(30);
            if (GUILayout.Button("删除该类型语言") && languageButtons.Length != 0)
            {
                soundControl._languageAudioClipList[soundControl._choseLanguage]._audioClipList.Clear();
                soundControl._languageAudioClipList.RemoveAt(soundControl._choseLanguage);
                soundControl._languageTypeList.RemoveAt(soundControl._choseLanguage);
                if (soundControl._choseLanguage > 0) soundControl._choseLanguage--;//只有在选择的时候会改变，删除数组它不会自动减少，导致索引超出
            }
        }
        /// <summary>
        /// 添加其他语言音频
        /// </summary>
        private void AddOtherLanguageAudioClip(SoundControl soundControl, int choseLanguage, AudioClip ac)
        {
            List<AudioClip> audioClipList = new List<AudioClip>();
            audioClipList = soundControl._languageAudioClipList[choseLanguage]._audioClipList;
            _isExist = false;
            for (int i = 0; i < audioClipList.Count; i++)
            {
                if (audioClipList[i] == ac)
                {
                    _isExist = true;
                    Debug.LogError("配置表里已存在该音频");
                    break;
                }
            }
            if (_isExist == false) audioClipList.Add(ac);
        }
        [MenuItem("GameObject/自定义/创建声音管理对象", false, MenuItemConfig.声音管理)]
        private static void CreateSoundControlObject()
        {
            GameObject gameObject = new GameObject("声音管理");
            gameObject.AddComponent<SoundControl>();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = gameObject;
            EditorGUIUtility.PingObject(Selection.activeObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Create GameObject");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            //GameObject obj = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Common/Prefabs/Sound/声音管理.prefab")).name = "声音管理";
        }
    }
}
