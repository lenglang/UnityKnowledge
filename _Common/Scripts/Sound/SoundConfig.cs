using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace Common.Sound
{
    [CreateAssetMenu(fileName = "xxx声音配置", menuName = "自定义/创建声音配置", order = 0)]
    public class SoundConfig : ScriptableObject
    {
        public string _nameSpace = "";//命名空间
        public string _fileName = "";//.cs配置文件名
        public List<Config> _voiceList = new List<Config>();//人声列表
        public string _voiceEnumType = "VoiceType";//人声枚举类型
        public List<Config> _soundList = new List<Config>();//音效列表
        public string _soundEnumType = "SoundType";//音效枚举类型
        public List<Config> _musiceList = new List<Config>();//背景音乐列表
        public string _musiceEnumType = "MusiceType";//背景音乐枚举类型
        [System.Serializable]
        public class Config
        {
            public Object _audioClip;//声音源
            public string _desc;//描述-用于枚举
            public Config(AudioClip audioClip = null, string desc = "")
            {
                _audioClip = audioClip;
                _desc = desc;
            }
        }
        [MenuItem("GameObject/自定义/创建声音管理对象", false, 16)]
        private static void CreateSoundControlObject()
        {
            Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Common/Prefabs/Sound/声音管理.prefab")).name = "声音管理";
        }
    }
}
