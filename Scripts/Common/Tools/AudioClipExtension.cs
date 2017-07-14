using UnityEngine;
using System;
namespace WZK.Common
{
    public static class AudioClipExtension
    {
        public static byte[] GetData(this AudioClip clip)
        {
            float[] data = new float[clip.samples * clip.channels];

            clip.GetData(data, 0);

            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static void SetData(this AudioClip clip, byte[] bytes)
        {
            float[] data = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);

            clip.SetData(data, 0);
        }
    }
}