
using System.Collections.Generic;
public enum VoiceType
    {
        把相同的东西找出来吧,
    }
    public enum SoundType
    {
        出场可爱的嚎叫的声音,
    }
    public enum SoundID
    {
        背景,
        语音,
        音效,
    }
    public class SoundController : Singleton<SoundController>
    {
        public string BackGoundSound = "春节背景";
        public float SoundVolume = 0.6f;
        public void IsPlayBackGroundSound(bool isPlay)
        {
            //if (isPlay)
            // PlaySound(BackGoundSound, false).SetLoop(true).SetID(GetSoundID(SoundID.背景)).SetVolume(0.6f);
            //else
            //DestroySound(BackGoundSound, false);
        }
        string GetSoundName(VoiceType type)
        {
            switch (type)
            {
                case VoiceType.把相同的东西找出来吧:
                    return "01001把相同的东西找出来吧~";
            }
            return "";
        }
        string GetSoundName(SoundType type)
        {
            switch (type)
            {
                case SoundType.出场可爱的嚎叫的声音:
                    return "出场可爱的嚎叫的声音";
            }
            return "";
        }
        public string GetSoundID(SoundID id)
        {
            switch (id)
            {
                case SoundID.背景:
                    return "背景";
                case SoundID.语音:
                    return "语音";
                case SoundID.音效:
                    return "音效";
                default:
                    return "";
            }
        }
        public Sound Play(VoiceType type, bool isRepeat = true)
        {
            string soundName = GetSoundName(type);
            if (soundName.Equals("")) return null;

            Sound newSound = PlaySound(soundName, isRepeat).SetVolume(0.8f);
            if (newSound != null)
                newSound.SetID(GetSoundID(SoundID.语音));
            return newSound;
        }
        public Sound Play(SoundType type, bool isRepeat = true)
        {
            string soundName = GetSoundName(type);
            if (soundName.Equals("")) return null;
            Sound sound = PlaySound(soundName, isRepeat);
            if (sound != null)
                sound.SetID(GetSoundID(SoundID.音效));
            return sound;
        }
        Sound PlaySound(string soundName, bool isRepeat)
        {
            if (!isRepeat)
                if (IsPlaying(soundName))
                    return null;
            return SoundManager.Instance.PlayFromAssetbundle(soundName);
        }
        public bool IsPlaying(SoundType type)
        {
            string soundName = GetSoundName(type);
            if (IsPlaying(soundName))
                return true;
            return false;
        }
        public bool IsPlaying(VoiceType type)
        {
            string soundName = GetSoundName(type);
            if (IsPlaying(soundName))
                return true;
            return false;
        }
        bool IsPlaying(string soundName)
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
                return sounds[0];
            else
                return null;
        }
        public List<Sound> GetSounds(VoiceType type)
        {
            string soundName = GetSoundName(type);
            return SoundManager.Instance.SearchSound(soundName);
        }
        public List<Sound> GetSounds(SoundType type)
        {
            string soundName = GetSoundName(type);
            return SoundManager.Instance.SearchSound(soundName);
        }
        public Sound GetSound(VoiceType type)
        {
            string soundName = GetSoundName(type);
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds[0];
            else
                return null;
        }
        public Sound GetSound(SoundType type)
        {
            string soundName = GetSoundName(type);
            List<Sound> sounds = GetSounds(soundName);
            if (sounds.Count != 0)
                return sounds[0];
            else
                return null;
        }
        public List<Sound> GetSoundsById(string ID)
        {
            return SoundManager.Instance.SearchSoundByID(ID);
        }
        public void DestroyAllSound(SoundID soundID)
        {
            List<Sound> sounds = GetSoundsById(GetSoundID(soundID));
            foreach (Sound sound in sounds)
            {
                DestroySound(sound.name, false);
            }
        }
        public void DestroySound(VoiceType type, bool isComplete = false)
        {
            string soundName = GetSoundName(type);
            if (soundName.Equals("")) return;
            DestroySound(soundName, isComplete);
        }
        public void DestroySound(SoundType type, bool isComplete = false)
        {
            string soundName = GetSoundName(type);
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