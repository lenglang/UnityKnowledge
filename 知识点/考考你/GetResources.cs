//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.IO;
//using System;
//using Babybus.Framework;
//namespace KaoKaoNi
//{
//    /// <summary>
//    /// 获取持久化路径资源方法
//    /// </summary>
//    public class GetResources:MonoBehaviour
//    {
//        private static GetResources instance;
//        public static GetResources Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    GameObject obj = new GameObject("GetResources");
//                    instance = obj.AddComponent<GetResources>();
//                }
//                return instance;
//            }
//        }
//        private string _url=Application.persistentDataPath + "/考考你/";//持久化路径头
//        /// <summary>
//        /// 获取语音
//        /// </summary>
//        /// <param name="voiceName">声音名</param>
//        public AudioClip GetVoice(string voiceName)
//        {
//            AudioClip ac;
//            //先从初始声音获取
//            if (SoundControl.Instance._voiceDictionary.ContainsKey(voiceName))
//            {
//                ac = SoundControl.Instance._voiceDictionary[voiceName];
//                return ac;
//            }
//            string assetBundleVoiceName = NameChange(voiceName);
//            //从AssetBundle获取
//            AssetBundle assetBundle = Utility.CreateAssetBundleFromFile(_url + assetBundleVoiceName + ".mp3.unity3d");
//            ac = assetBundle.LoadAsset<AudioClip>(voiceName);
//            SoundControl.Instance._voiceDictionary.Add(voiceName, ac);
//            StartCoroutine(AudioClipReady(ac, assetBundle));
//            if (ac == null) Debug.LogError("不存在该语音，请检查初始语音和是否下载该语音至持久化路径下");
//            return ac;
//        }
//        /// <summary>
//        /// 音频是否准备好
//        /// 因为LoadAsset并不等于Load了完整的音频，它还需要依赖AssetBundle进行异步的音频硬件解码！
//        /// 经测试，当你把它Unload(false)后，它并不会报错，而是会一直无限期等待，
//        /// 且永远处于AudioDataLoadState.Loading状态，因为没有播放，也就没办法释放这个Asset资源，
//        /// 当你的音频加载过多后，就会造成内存泄漏！注：并不是所有音频都会播不出来，那种解码时间长的会播不出来，
//        /// 解码时间一瞬间完成的可以正常播放，经测试所有音频，在LoadAsset之后一到两帧可以解码完成
//        /// </summary>
//        /// <param name="audioClip"></param>
//        /// <param name="assetBundle"></param>
//        /// <returns></returns>
//        private IEnumerator AudioClipReady(AudioClip audioClip, AssetBundle assetBundle)
//        {
//            //while (audioClip.loadState == AudioDataLoadState.Loading||audioClip.loadState ==AudioDataLoadState.Unloaded)
//            //{
//            //    yield return null;
//            //}
//            while (audioClip.loadState != AudioDataLoadState.Loaded)
//            {
//                yield return new WaitForEndOfFrame();
//            }
//            if (assetBundle != null)
//            {
//                assetBundle.Unload(false);
//                assetBundle = null;
//            }
//        }
//        /// <summary>
//        /// 获取图片
//        /// </summary>
//        /// <param name="spriteName"></param>
//        /// <returns></returns>
//        public Sprite GetSprite(string spriteName)
//        {
//            Sprite sprite;
//            //先从初始图片获取
//            if (SpriteControl.Instance._spriteDictionary.ContainsKey(spriteName))
//            {
//                sprite = SpriteControl.Instance._spriteDictionary[spriteName];
//                return sprite;
//            }
//            string assetBundleSpriteName = NameChange(spriteName);
//            //从AssetBundle获取
//            AssetBundle assetBundle = Utility.CreateAssetBundleFromFile(_url + assetBundleSpriteName + ".png.unity3d");
//            sprite = assetBundle.LoadAsset<Sprite>(spriteName);
//            SpriteControl.Instance._spriteDictionary.Add(spriteName, sprite);
//            if (assetBundle != null)
//            {
//                assetBundle.Unload(false);
//                assetBundle = null;
//            }            
//            if (sprite == null) Debug.LogError("不存在该图片，请检查初始图片和是否下载该图片至持久化路径下");
//            return sprite;
//        }
//        /// <summary>
//        /// 资源是否存在
//        /// </summary>
//        /// <param name="fileName">文件全名包含扩展名</param>
//        /// <returns></returns>
//        public bool IsExist(string fileName)
//        {
//            string[] names = fileName.Split('.');
//            string n = names[0];
//            string t = names[1];
//            if (t == "mp3")
//            {
//                if (SoundControl.Instance._voiceDictionary.ContainsKey(n))
//                {
//                    return true;
//                }
//            }
//            else if (t == "png")
//            {
//                if (SpriteControl.Instance._spriteDictionary.ContainsKey(n))
//                {
//                    return true;
//                }
//            }
//            fileName = NameChange(n) + "." + t + ".unity3d";
//            return File.Exists(_url + fileName);
//        }
//        /// <summary>
//        /// 由于AssetBundle打包出的资源名都是小写，故这些名称资源需转化
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public string NameChange(string name)
//        {
//            switch (name)
//            {
//                case "A":
//                    return "a";
//                case "B":
//                    return "b";
//                case "C":
//                    return "c";
//                case "D":
//                    return "d";
//                case "E":
//                    return "e";
//                case "F":
//                    return "f";
//                case "G":
//                    return "g";
//                case "H":
//                    return "h";
//                case "I":
//                    return "i";
//                case "J":
//                    return "j";
//                case "K":
//                    return "k";
//                case "L":
//                    return "l";
//                case "M":
//                    return "m";
//                case "N":
//                    return "n";
//                case "O":
//                    return "o";
//                case "P":
//                    return "p";
//                case "Q":
//                    return "q";
//                case "R":
//                    return "r";
//                case "S":
//                    return "s";
//                case "T":
//                    return "t";
//                case "U":
//                    return "u";
//                case "V":
//                    return "v";
//                case "W":
//                    return "w";
//                case "X":
//                    return "x";
//                case "Y":
//                    return "y";
//                case "Z":
//                    return "z";
//            }
//            return name;
//        }
//    }
//}
