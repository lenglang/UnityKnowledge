using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace WZK.Common
{
    public class AudioClipHelper
    {
        public static AudioClip Combine(params AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
                return null;
            int channels = clips[0].channels;
            int frequency = clips[0].frequency;
            for (int i = 1; i < clips.Length; i++)
            {
                if (clips[i].channels != channels || clips[i].frequency != frequency)
                {
                    return null;
                }
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    if (clips[i] == null)
                        continue;

                    clips[i].LoadAudioData();
                    var buffer = clips[i].GetData();
                    memoryStream.Write(buffer, 0, buffer.Length);
                }
                var bytes = memoryStream.ToArray();
                var result = AudioClip.Create("Combine", bytes.Length / 4 / channels, channels, frequency, false);
                result.SetData(bytes);
                return result;
            }
        }
        public static AudioClip Combine(List<AudioClip> clips)
        {
            if (clips == null || clips.Count == 0)
                return null;
            int channels = clips[0].channels;
            int frequency = clips[0].frequency;
            for (int i = 1; i < clips.Count; i++)
            {
                if (clips[i].channels != channels || clips[i].frequency != frequency)
                {
                    return null;
                }
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                for (int i = 0; i < clips.Count; i++)
                {
                    if (clips[i] == null)
                        continue;

                    clips[i].LoadAudioData();
                    var buffer = clips[i].GetData();
                    memoryStream.Write(buffer, 0, buffer.Length);
                }
                var bytes = memoryStream.ToArray();
                var result = AudioClip.Create("Combine", bytes.Length / 4 / channels, channels, frequency, false);
                result.SetData(bytes);
                return result;
            }
        }
    }
}