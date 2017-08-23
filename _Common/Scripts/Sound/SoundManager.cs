using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
namespace WZK
{
    [DisallowMultipleComponent]
    public class SoundManager : MonoBehaviour
    {
        public enum SoundID
        {
            背景音乐,
            人声,
            音效
        }
        [Header("背景音乐声音大小")]
        public float _bgSoundVolume = 0.6f;
        private List<Sound> _playingSoundList = new List<Sound>();
        private float _deltaTime;
        private Dictionary<string, AudioClip> _voiceDictionary = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _soundDictionary = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _musicDictionary = new Dictionary<string, AudioClip>();
        private static SoundManager _instance = null;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (new GameObject("声音管理")).AddComponent<SoundManager>();
                }
                return _instance;
            }
        }
        void Awake()
        {
            _instance = this;
            foreach (Config item in _voiceList)
            {
                _voiceDictionary.Add(item._desc, item._audioClip);
            }
            foreach (Config item in _soundList)
            {
                _soundDictionary.Add(item._desc, item._audioClip);
            }
            foreach (Config item in _musiceList)
            {
                _musicDictionary.Add(item._desc, item._audioClip);
            }
            Debug.LogWarning("接入宝宝巴士框架，这里需给语言赋值，得到当前手机语言");
            if (string.IsNullOrEmpty(_testLanguage) == false && SystemData.IsDebugBuild) SystemData.LANGUAGE = _testLanguage;
            if (SystemData.LANGUAGE != "zh")
            {
                List<AudioClip> audioClipList = new List<AudioClip>();
                audioClipList = GetLanguageAudioClip(SystemData.LANGUAGE);
                if (audioClipList != null)
                {
                    ReplaceVoiceAudioClip(audioClipList);
                }
                else if (SystemData.LANGUAGE != "zht")
                {
                    audioClipList = GetLanguageAudioClip("en");
                    if (audioClipList != null) ReplaceVoiceAudioClip(audioClipList);
                }
            }
            Debug.Log("当前语言:" + SystemData.LANGUAGE);
        }
        /// <summary>
        /// 替换人声
        /// </summary>
        /// <param name="audioClipList"></param>
        void ReplaceVoiceAudioClip(List<AudioClip> audioClipList)
        {
            if (SystemData.LANGUAGE != "zht" && _voiceDictionary.Count != audioClipList.Count) Debug.LogError("替换的人声和中文的人声个数不对等，请检查！！！");
            for (int i = 0; i < audioClipList.Count; i++)
            {
                foreach (KeyValuePair<string, AudioClip> item in _voiceDictionary)
                {
                    if (item.Value.name == audioClipList[i].name)
                    {
                        _voiceDictionary[item.Key] = audioClipList[i];
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 获取语言音频
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        List<AudioClip> GetLanguageAudioClip(string language)
        {
            for (int i = 0; i < _languageAudioClipList.Count; i++)
            {
                if (_languageAudioClipList[i]._language == language) return _languageAudioClipList[i]._audioClipList;
            }
            return null;
        }
        /// <summary>
        /// 播放人声
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isRepeatType">同类型的音频，是否叠加播放</param>
        /// <param name="isRepeatSame">该音频正在播，是否叠加播放</param>
        /// <returns></returns>
        public Sound PlayVoice(Enum type, bool isRepeatType = false, bool isRepeatSame = true)
        {
            if (SwitchSoundID(type) != SoundID.人声) Debug.LogError("播放类型不对，应用VoiceType！");
            return Play(_voiceDictionary, type, SoundID.人声, isRepeatType, isRepeatSame);
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isRepeatType">同类型的音频，是否叠加播放</param>
        /// <param name="isRepeatSame">该音频正在播，是否叠加播放</param>
        /// <returns></returns>
        public Sound PlaySound(Enum type, bool isRepeatType = true, bool isRepeatSame = true)
        {
            if (SwitchSoundID(type) != SoundID.音效) Debug.LogError("播放类型不对，应用SoundType！");
            return Play(_soundDictionary, type, SoundID.音效, isRepeatType, isRepeatSame);
        }
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isRepeatType">同类型的音频，是否叠加播放</param>
        /// <param name="isRepeatSame">该音频正在播，是否叠加播放</param>
        /// <returns></returns>
        public Sound PlayMusic(Enum type)
        {
            if (SwitchSoundID(type) != SoundID.音效) Debug.LogError("播放类型不对，应用MusiceType！");
            Sound sound = Play(_musicDictionary, type, SoundID.背景音乐, false, true);
            if (sound != null) sound.SetVolume(_bgSoundVolume).SetLoop();
            return sound;
        }
        /// <summary>
        /// 播放Resources文件夹下音乐
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isRepeatSame"></param>
        /// <returns></returns>
        public Sound PlayFromResource(Enum type, bool isRepeatSame = true)
        {
            string name = type.GetEnumDescription();
            AudioClip clip = Resources.Load<AudioClip>(name);
            return PlaySound(clip, name, isRepeatSame).SetID(SwitchSoundID(type).ToString());
        }
        /// <summary>
        /// 播放Resources文件夹下加载场景时不销毁音效-用于点击音效
        /// </summary>
        /// <param name="type"></param>
        public static void PlayDontDestroyOnLoad(Enum type)
        {
            string name = type.GetEnumDescription();
            GameObject obj = new GameObject(name);
            DontDestroyOnLoad(obj);
            AudioClip clip = Resources.Load<AudioClip>(name);
            AudioSource audioSource = obj.AddComponent<AudioSource>();
            obj.AddComponent<AutoDestroy>()._time = clip.length;
            audioSource.clip = clip;
            audioSource.Play();
        }
        private Sound Play(Dictionary<string, AudioClip> dictionary, Enum type, SoundID id, bool isRepeatType, bool isRepeatSame)
        {
            if (isRepeatType == false) DestroyAllSound(id);
            string soundName = type.ToString();
            if (soundName.Equals("")) return null;
            Sound newSound = PlaySound(dictionary[soundName], soundName, isRepeatSame);
            if (newSound != null)
                newSound.SetID(id.ToString());
            return newSound;
        }
        private Sound PlaySound(AudioClip clip, string soundName, bool isRepeatSame)
        {
            if (!isRepeatSame)
                if (IsPlaying(soundName))
                    return null;
            Sound sound = new Sound(this);
            sound._clip = clip;
            sound._name = soundName;
            sound._audioSource = gameObject.AddComponent<AudioSource>();
            sound._audioSource.clip = clip;
            sound._audioSource.Play();
            _playingSoundList.Add(sound);
            return sound;
        }
        public void RemoveSound(Sound sound)
        {
            Destroy(sound._audioSource);
            _playingSoundList.Remove(sound);
            sound = null;
        }
        public void PauseAllSound()
        {
            for (int i = 0; i < _playingSoundList.Count; i++)
            {
                _playingSoundList[i]._audioSource.Pause();
            }
        }
        public void ResumeAllSound()
        {
            for (int i = 0; i < _playingSoundList.Count; i++)
            {
                _playingSoundList[i]._audioSource.UnPause();
            }
        }
        public bool IsPlaying(Enum type)
        {
            string soundName = type.ToString();
            if (IsPlaying(soundName))
                return true;
            return false;
        }
        public bool IsPlaying(string soundName)
        {
            if (GetSound(soundName) != null)
                return true;
            return false;
        }
        public List<Sound> GetSounds(string soundName)
        {
            return SearchSound(soundName);
        }
        public List<Sound> GetSounds(Enum type)
        {
            string soundName = type.ToString();
            return SearchSound(soundName);
        }
        public Sound GetSound(string soundName)
        {
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds.Last();
            else
                return null;
        }
        public Sound GetSound(Enum type)
        {
            string soundName = type.ToString();
            return GetSound(soundName);
        }
        public List<Sound> GetSoundsById(string ID)
        {
            List<Sound> sound = new List<Sound>(this._playingSoundList.Where(s => s._id.Equals(ID)));
            return sound;
        }
        public void DestroyAllSound(SoundID soundID)
        {
            List<Sound> sounds = GetSoundsById(soundID.ToString());
            foreach (Sound sound in sounds)
            {
                DestroySound(sound._name, false);
            }
        }
        public void DestroyAllSound()
        {
            _playingSoundList.ToList().ForEach(x => x.FinishNoComplete());
        }
        public void DestroySound(Enum type, bool isComplete = false)
        {
            string soundName = type.ToString();
            if (soundName.Equals("")) return;
            DestroySound(soundName, isComplete);
        }
        public void DestroySound(string soundName, bool isComplete)
        {
            List<Sound> destoryList = SearchSound(name);
            if (isComplete)
            {
                destoryList.ToList().ForEach(x => x.Finish());
            }
            else
            {
                destoryList.ToList().ForEach(x => x.FinishNoComplete());
            }
        }
        private List<Sound> SearchSound(string soundName)
        {
            List<Sound> sound = new List<Sound>(this._playingSoundList.Where(s => s._name.Contains(soundName)));
            return sound;
        }
        float GetSoundLeftTime(Sound sound)
        {
            return sound._audioSource.clip.length - sound._playTime;
        }
        public float GetSoundLength(Enum type)
        {
            string soundName = type.ToString();
            return SwitchDictionary(type)[soundName].length;
        }
        private Dictionary<string, AudioClip> SwitchDictionary(Enum type)
        {
            switch (type.GetType().Name)
            {
                case "VoiceType":
                    return _voiceDictionary;
                case "SoundType":
                    return _soundDictionary;
                default:
                    return _musicDictionary;
            }
        }
        private SoundID SwitchSoundID(Enum type)
        {
            switch (type.GetType().Name)
            {
                case "VoiceType":
                    return SoundID.人声;
                case "SoundType":
                    return SoundID.音效;
                default:
                    return SoundID.背景音乐;
            }
        }
        void Update()
        {
            _playingSoundList.ToList().ForEach(x =>
            {
                if (x._isScaleTime == false)
                {
                //Time.unscaledDeltaTime 不考虑timescale时候与deltaTime相同，若timescale被设置，则无效。
                _deltaTime = Time.unscaledDeltaTime;
                }
                else
                {
                    _deltaTime = Time.deltaTime;
                }
                x._playTime += _deltaTime;
                if (!x._audioSource.isPlaying && x.isFinish)
                {
                    if (!(x._loop))
                        x.Finish();
                }
            });
        }
        void OnDestroy()
        {
            DestroyAllSound();
            _instance = null;
        }
        #region 声音配置
        public string _savePath = "";//存储路径
        public string _nameSpace = "";//命名空间
        public string _fileName = "";//.cs配置文件名
        public bool _isResources = false;//是否Resources下资源
        public List<Config> _voiceList = new List<Config>();//人声列表
        public string _voiceEnumType = "VoiceType";//人声枚举类型
        public List<Config> _soundList = new List<Config>();//音效列表
        public string _soundEnumType = "SoundType";//音效枚举类型
        public List<Config> _musiceList = new List<Config>();//背景音乐列表
        public string _musiceEnumType = "MusiceType";//背景音乐枚举类型
        public int _choseLanguage = 0;//选择的国际化语言
        public string _testLanguage = "";//测试语言
        public List<string> _languageTypeList = new List<string>() { "zht", "en" };//其他语言类型
        public List<LanguageAudioClip> _languageAudioClipList = new List<LanguageAudioClip>();//国际化音频
        [System.Serializable]
        public class Config
        {
            public AudioClip _audioClip;//声音源
            public string _desc;//描述-用于枚举
            public string _resourcesPath;//Resources下路径
            public Config(AudioClip audioClip = null, string resourcesPath = "", string desc = "")
            {
                _audioClip = audioClip;
                _resourcesPath = resourcesPath;
                _desc = desc;
            }
        }
        [System.Serializable]
        public class LanguageAudioClip
        {
            public string _language = "";
            public List<AudioClip> _audioClipList = new List<AudioClip>();
            public LanguageAudioClip(string language)
            {
                _language = language;
            }
        }
        #endregion
    }
}

