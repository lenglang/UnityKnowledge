using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
namespace WZK.Common
{
    public enum SoundType
    {
        [Description("0210")]
        熊猫完成转身第一脚落下时播放,
        [Description("0210")]
        点击拿起衣服时,
        衣服落入洗衣篮时
    }
    public enum VoiceType
    {
        阳光真好我们来洗衣服吧,
        白色和彩色的衣服要分开放哦
    }
    public enum SoundID
    {
        [Description("背景")]
        背景,
        [Description("语音")]
        语音,
        [Description("音效")]
        音效
    }
    public class SoundControl : Singleton<SoundControl>
    {
        [Header("背景音乐")]
        public AudioClip _bgAudioClip;
        [Range(0, 1)]
        public float _bgSoundVolume = 0.6f;
        [Header("音效列表")]
        public List<AudioClip> _soundList = new List<AudioClip>();
        [Header("人声列表")]
        public List<AudioClip> _voiceList = new List<AudioClip>();
        private Dictionary<string, AudioClip> _soundDictionary = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _voiceDictionary = new Dictionary<string, AudioClip>();
        protected override void Awake()
        {
            base.Awake();
            foreach (AudioClip item in _soundList)
            {
                _soundDictionary.Add(item.name, item);
            }
            foreach (AudioClip item in _voiceList)
            {
                _voiceDictionary.Add(item.name, item);
            }
        }
        public void IsPlayBgSound(bool isPlay)
        {
            if (isPlay)
            {
                PlaySound(_bgAudioClip, "背景音乐", false).SetLoop(true).SetID((SoundID.背景).GetEnumDescription()).SetVolume(_bgSoundVolume);
            }
            else { DestroySound("背景音乐", false); }
        }
        public float GetSoundLength(VoiceType type)
        {
            string soundName = type.GetEnumDescription();
            return _voiceDictionary[soundName].length;
        }
        public float GetSoundLength(SoundType type)
        {
            string soundName = type.GetEnumDescription();
            return _soundDictionary[soundName].length;
        }
        /// <summary>
        /// 播放语音
        /// </summary>
        /// <param name="type">语音类型</param>
        /// <param name="isRepeatType">是否重复播放相同类，默认false</param>
        /// <param name="isRepeatSame">是否重复叠加播放该句</param>
        /// <returns></returns>
        public Sound Play(VoiceType type, bool isRepeatType = false, bool isRepeatSame = true)
        {
            if (isRepeatType == false) DestroyAllSound(SoundID.语音);
            string soundName = type.GetEnumDescription();
            if (soundName.Equals("")) return null;
            Sound newSound = PlaySound(_voiceDictionary[soundName], soundName, isRepeatSame);
            if (newSound != null)
                newSound.SetID((SoundID.语音).GetEnumDescription());
            return newSound;
        }
        public Sound Play(SoundType type, bool isRepeatSame = true)
        {
            string soundName = type.GetEnumDescription();
            if (soundName.Equals("")) return null;
            Sound sound = PlaySound(_soundDictionary[soundName], soundName, isRepeatSame);
            if (sound != null)
                sound.SetID((SoundID.音效).GetEnumDescription());
            return sound;
        }
        public Sound PlayFromResource(string path, string name, bool isRepeatSame = true)
        {
            AudioClip clip = Resources.Load<AudioClip>(path + name);
            return PlaySound(clip, name, isRepeatSame);
        }
        public Sound PlayFromResource(string path, Enum e, bool isRepeatSame = true)
        {
            string name = e.GetEnumDescription();
            AudioClip clip = Resources.Load<AudioClip>(path + name);
            return PlaySound(clip, name, isRepeatSame);
        }
        Sound PlaySound(AudioClip clip, string soundName, bool isRepeat)
        {
            if (!isRepeat)
                if (IsPlaying(soundName))
                    return null;
            return SoundManager.Instance.PlaySound(clip, soundName);
        }
        public bool IsPlaying(SoundType type)
        {
            string soundName = type.GetEnumDescription();
            if (IsPlaying(soundName))
                return true;
            return false;
        }
        public bool IsPlaying(VoiceType type)
        {
            string soundName = type.GetEnumDescription();
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
        List<Sound> GetSounds(string soundName)
        {
            return SoundManager.Instance.SearchSound(soundName);
        }
        Sound GetSound(string soundName)
        {
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds.Last();
            else
                return null;
        }
        public List<Sound> GetSounds(VoiceType type)
        {
            string soundName = type.GetEnumDescription();
            return SoundManager.Instance.SearchSound(soundName);
        }
        public List<Sound> GetSounds(SoundType type)
        {
            string soundName = type.GetEnumDescription();
            return SoundManager.Instance.SearchSound(soundName);
        }
        public Sound GetSound(VoiceType type)
        {
            string soundName = type.GetEnumDescription();
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds.Last();
            else
                return null;
        }
        public Sound GetSound(SoundType type)
        {
            string soundName = type.GetEnumDescription();
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds.Last();
            else
                return null;
        }
        public List<Sound> GetSoundsById(string ID)
        {
            return SoundManager.Instance.SearchSoundByID(ID);
        }
        public void DestroyAllSound(SoundID soundID)
        {
            List<Sound> sounds = GetSoundsById(soundID.GetEnumDescription());
            foreach (Sound sound in sounds)
            {
                DestroySound(sound.name, false);
            }
        }
        public void DestroySound(VoiceType type, bool isComplete = false)
        {
            string soundName = type.GetEnumDescription();
            if (soundName.Equals("")) return;
            DestroySound(soundName, isComplete);
        }
        public void DestroySound(SoundType type, bool isComplete = false)
        {
            string soundName = type.GetEnumDescription();
            if (soundName.Equals("")) return;
            DestroySound(soundName, isComplete);
        }
        void DestroySound(string soundName, bool isComplete)
        {
            if (isComplete)
                SoundManager.Instance.DestorySound(soundName);
            else
                SoundManager.Instance.DestorySoundNoAction(soundName);
        }
        public void DestroyAllSound()
        {
            SoundManager.Instance.ClearSound();
        }
        float GetSoundLeftTime(Sound sound)
        {
            return sound.audioSource.clip.length - sound.playTime;
        }
    }
}
