using UnityEngine;
using System;
namespace Common.Sound
{
    public class Sound
    {
        public string _name;
        public AudioClip _clip;
        public bool _loop;
        public Action _competeAction;
        public SoundControl _soundControl;
        public AudioSource _audioSource;
        public float _playTime;
        public string _id;
        public bool _isScaleTime = false;
        public bool isFinish
        {
            get
            {
                return _playTime >= _clip.length;
            }
        }
        public Sound(SoundControl soundControl)
        {
            _loop = false;
            _competeAction = null;
            _soundControl = soundControl;
            _playTime = 0;
            _id = string.Empty;
        }
        public float process
        {
            get
            {
                return ((float)_audioSource.timeSamples) / ((float)_clip.samples);
            }
        }
        public void Finish()
        {
            if (_competeAction != null)
                _competeAction();
            _soundControl.RemoveSound(this);
        }
        public void FinishNoComplete()
        {
            _soundControl.RemoveSound(this);
        }
        public Sound OnComplete(Action complete)
        {
            this._competeAction = complete;
            return this;
        }
        public Sound SetLoop(bool loop = true)
        {
            _loop = loop;
            _audioSource.loop = loop;
            return this;
        }
        public AudioClip GetClip()
        {
            return this._clip;
        }
        public Sound SetVolume(float value = 1.0f)
        {
            _audioSource.volume = value;
            return this;
        }
        public Sound SetID(string id)
        {
            _id = id;
            return this;
        }
    }
}

