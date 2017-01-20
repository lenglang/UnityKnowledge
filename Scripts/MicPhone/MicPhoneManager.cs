using UnityEngine;
using System.Collections;

public class MicPhoneManager : MonoBehaviour
{
    private AudioSource audioSource;
    AudioClip clip;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRecord();

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopRecord();
        }
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord()
    {
        Microphone.End(null);
        clip = Microphone.Start(null, false, 20, 8000);
        Debug.Log("aaa");
    }
    /// <summary>
    /// 结束录音
    /// </summary>
    public void StopRecord()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}