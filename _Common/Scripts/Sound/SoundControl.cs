using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Common.Sound;
[DisallowMultipleComponent]
public class SoundControl:MonoBehaviour
{
    public enum SoundID
    {
        背景音乐,
        人声,
        音效
    }
    [Header("声音配置")]
    public SoundConfig _soundConfig=null;
    [Header("背景音乐声音大小")]
    public float _bgSoundVolume = 0.6f;
    private List<Sound> _soundList = new List<Sound>();
    private float _deltaTime;
    private Dictionary<string, AudioClip> _voiceDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _soundDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _musicDictionary = new Dictionary<string, AudioClip>();
    private static SoundControl _instance = null;
    public static SoundControl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (new GameObject("声音管理")).AddComponent<SoundControl>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        if (_soundConfig == null) return;
        foreach (SoundConfig.Config item in _soundConfig._voiceList)
        {
            _voiceDictionary.Add(item._desc, (AudioClip)item._audioClip);
        }
        foreach (SoundConfig.Config item in _soundConfig._soundList)
        {
            _soundDictionary.Add(item._desc, (AudioClip)item._audioClip);
        }
        foreach (SoundConfig.Config item in _soundConfig._musiceList)
        {
            _musicDictionary.Add(item._desc, (AudioClip)item._audioClip);
        }
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
        Sound sound= Play(_musicDictionary, type, SoundID.背景音乐, false, false);
        if (sound != null) sound.SetVolume(_bgSoundVolume);
        return sound;
    }
    /// <summary>
    /// path所在文件夹路径 name文件名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="isRepeatSame"></param>
    /// <returns></returns>
    public Sound PlayFromResource(string path, string name, SoundID id,bool isRepeatSame = true)
    {
        AudioClip clip = Resources.Load<AudioClip>(path + name);
        Sound sound= PlaySound(clip, name, isRepeatSame);
        if (sound != null) sound.SetID(id.ToString());
        return sound;
    }
    /// <summary>
    /// path所在文件夹路径 type枚举名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type">不能为SoundID</param>
    /// <param name="isRepeatSame"></param>
    /// <returns></returns>
    public Sound PlayFromResource(string path, Enum type, bool isRepeatSame = true)
    {
        string name = type.ToString();
        AudioClip clip = Resources.Load<AudioClip>(path + name);
        return PlaySound(clip, name, isRepeatSame).SetID(SwitchSoundID(type).ToString());
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
        _soundList.Add(sound);
        return sound;
    }
    public void RemoveSound(Sound sound)
    {
        Destroy(sound._audioSource);
        _soundList.Remove(sound);
        sound = null;
    }
    public void PauseAllSound()
    {
        for (int i = 0; i < _soundList.Count; i++)
        {
            _soundList[i]._audioSource.Pause();
        }
    }
    public void ResumeAllSound()
    {
        for (int i = 0; i < _soundList.Count; i++)
        {
            _soundList[i]._audioSource.UnPause();
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
        List<Sound> sound = new List<Sound>(this._soundList.Where(s => s._id.Equals(ID)));
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
        _soundList.ToList().ForEach(x => x.FinishNoComplete());
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
        List<Sound> sound = new List<Sound>(this._soundList.Where(s => s._name.Contains(soundName)));
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
        _soundList.ToList().ForEach(x =>
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
}

