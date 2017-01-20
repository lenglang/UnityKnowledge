using UnityEngine;
using System.Collections;
using System;
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;
    public Action competeAction;
    public SoundManager soundManager;
    public AudioSource audioSource;
    public float playTime;
    public string ID;
    public bool isScaleTime;
    public bool isFinish
    {
        get
        {
            return playTime >= clip.length;
        }
    }
    // Use this for initialization
    public Sound(SoundManager soundManager)
    {
        loop = false;
        competeAction = null;
        this.soundManager = soundManager;
        playTime = 0;
        this.ID = string.Empty;
    }

    public float process
    {
        get
        {
            return ((float)audioSource.timeSamples) / ((float)clip.samples);
        }
    }

    public void Finish()
    {
        if (competeAction != null)
            competeAction();

        soundManager.RemoveSound(this);
    }
    public void FinishNoComplete()
    {

        soundManager.RemoveSound(this);
    }
    public Sound OnComplete(System.Action complete)
    {
        this.competeAction = complete;
        return this;
    }
    public Sound SetLoop(bool loop = true)
    {
        this.loop = loop;
        this.audioSource.loop = loop;
        return this;
    }

    public Sound GetClip(out AudioClip clip)
    {
        clip = this.clip;
        return this;
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="value">0.0-1.0</param>
    /// <returns></returns>
    public Sound SetVolume(float value = 1.0f)
    {
        this.audioSource.volume = value;
        return this;
    }

    public Sound SetID(string ID)
    {
        this.ID = ID;
        return this;
    }
}
