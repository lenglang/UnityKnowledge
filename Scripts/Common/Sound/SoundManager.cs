using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace WZK.Common
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }
                return instance;
            }
        }
        private List<Sound> soundList = new List<Sound>();
        private float _deltaTime;
        void Update()
        {
            soundList.ToList().ForEach(x =>
            {
                if (x.isScaleTime == false)
                {
                //Time.unscaledDeltaTime 不考虑timescale时候与deltaTime相同，若timescale被设置，则无效。
                _deltaTime = Time.unscaledDeltaTime;
                }
                else
                {
                    _deltaTime = Time.deltaTime;
                }
                x.playTime += _deltaTime;
                if (!x.audioSource.isPlaying && x.isFinish)
                {
                    if (!(x.loop))
                        x.Finish();
                }
            });
        }
        private void OnDestroy()
        {
            instance = null;
        }
        //public Sound PlayFromAssetbundle(string name, bool isScaleTime = false)
        //{
        //    AudioClip clip = AssetBundleLoader.Load<AudioClip>(name);
        //    return PlaySound(clip, name, isScaleTime);
        //}
        //public Sound PlayFromAssetbundle(Enum e, bool isScaleTime = false)
        //{
        //    string name = e.GetEnumDescription();
        //    AudioClip clip = AssetBundleLoader.Load<AudioClip>(name);
        //    return PlaySound(clip, name, isScaleTime);
        //}
        //public Sound PlayFromResource(string path, string name,bool isScaleTime=false)
        //{
        //    AudioClip clip = Resources.Load<AudioClip>(path + name);
        //    return PlaySound(clip, name,isScaleTime);
        //}
        //public Sound PlayFromResource(string path, Enum e,bool isScaleTime = false)
        //{
        //    string name = e.GetEnumDescription();
        //    AudioClip clip = Resources.Load<AudioClip>(path + name);
        //    return PlaySound(clip, name,isScaleTime);
        //}
        public Sound PlaySound(AudioClip clip, string soundName, bool isScaleTime = false)
        {
            Sound sound = new Sound(this);
            sound.clip = clip;
            sound.isScaleTime = isScaleTime;
            sound.name = soundName;
            sound.audioSource = this.gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = clip;
            sound.audioSource.Play();
            soundList.Add(sound);
            return sound;
        }
        public void DestorySound(string name)
        {
            List<Sound> destoryList = SearchSound(name);
            destoryList.ToList().ForEach(x => x.Finish());
        }

        public void DestorySoundByID(string ID)
        {
            List<Sound> destoryList = SearchSoundByID(ID);
            destoryList.ToList().ForEach(x => x.Finish());
        }
        public void DestorySoundNoAction(string name)
        {
            List<Sound> destoryList = SearchSound(name);
            destoryList.ToList().ForEach(x => x.FinishNoComplete());
        }

        public List<Sound> SearchSound(string soundName)
        {
            List<Sound> sound = new List<Sound>(this.soundList.Where(s => s.name.Contains(soundName)));
            return sound;
        }
        public List<Sound> SearchSoundByID(string ID)
        {
            List<Sound> sound = new List<Sound>(this.soundList.Where(s => s.ID.Equals(ID)));
            return sound;
        }
        public void RemoveSound(Sound sound)
        {
            Destroy(sound.audioSource);
            soundList.Remove(sound);
            sound = null;
        }
        public void PauseAllSound()
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                soundList[i].audioSource.Pause();
            }
        }
        public void ResumeAllSound()
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                soundList[i].audioSource.UnPause();
            }
        }
        public void ClearSound()
        {
            soundList.ToList().ForEach(x => x.FinishNoComplete());
        }
        float GetSoundLeftTime(string soundName)
        {
            List<Sound> sounds = SoundManager.Instance.SearchSound(soundName);
            return sounds.Last().audioSource.clip.length - sounds.Last().playTime;
        }
        bool IsPlaying(string soundName)
        {
            List<Sound> sounds = SoundManager.Instance.SearchSound(soundName);
            if (sounds.Count != 0)
                return true;
            return false;
        }
    }
}
