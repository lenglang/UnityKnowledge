using UnityEngine;
using System;
using System.Collections;
using System.IO;
namespace WZK
{
    public static class MonoBehaviourExtension
    {
        public static Coroutine InvokeWaitForSeconds(this MonoBehaviour monoBehaviour, Action action, float time)
        {
            if (action == null)
                return null;

            return monoBehaviour.StartCoroutine(ActionCoroutine(action, new WaitForSeconds(time)));
        }
        public static Coroutine InvokeRepeatingWaitForSeconds(this MonoBehaviour monoBehaviour, Action action, float time, int count = 0xfffffff)
        {
            if (action == null)
                return null;

            return monoBehaviour.StartCoroutine(RepeatingActionCoroutine(action, new WaitForSeconds(time), count));
        }
        public static void InvokeWaitForEndOfFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            monoBehaviour.StartCoroutine(ActionCoroutine(action, new WaitForEndOfFrame()));
        }
        public static void InvokeWaitForFixedUpdate(this MonoBehaviour monoBehaviour, Action action)
        {
            monoBehaviour.StartCoroutine(ActionCoroutine(action, new WaitForFixedUpdate()));
        }
        public static void InvokeWaitForYieldInstruction(this MonoBehaviour monoBehaviour, Action action, YieldInstruction yieldInstruction)
        {
            monoBehaviour.StartCoroutine(ActionCoroutine(action, yieldInstruction));
        }
        public static void InvokeWaitForYieldInstruction<T>(this MonoBehaviour monoBehaviour, Action<T> action, YieldInstruction yieldInstruction) where T : YieldInstruction
        {
            monoBehaviour.StartCoroutine(ActionCoroutine(action, yieldInstruction));
        }
        private static IEnumerator ActionCoroutine(Action action, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;

            if (action != null)
                action();
        }
        private static IEnumerator ActionCoroutine<T>(Action<T> action, YieldInstruction yieldInstruction) where T : YieldInstruction
        {
            yield return yieldInstruction;

            if (action != null)
                action(yieldInstruction as T);
        }
        private static IEnumerator RepeatingActionCoroutine(Action action, WaitForSeconds waitForSeconds, int count)
        {
            while (count != 0)
            {
                yield return waitForSeconds;

                action();
                count--;
            }
        }
        public static void CreateAssetBundleFromFile(this MonoBehaviour monoBehaviour, string path, Action<AssetBundle> action)
        {
            if (!File.Exists(path))
                action(null);
            ThreadHelper.Instance.QueueOnThreadPool((state) =>
            {
                byte[] bytes = File.ReadAllBytes(path);
                ThreadHelper.Instance.QueueOnMainThread(delegate ()
                {
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bytes);
                    monoBehaviour.InvokeWaitForYieldInstruction(delegate ()
                    {
                        if (assetBundleCreateRequest == null)
                            action(null);
                        else
                            action(assetBundleCreateRequest.assetBundle);
                    }, assetBundleCreateRequest);
                });
            });
        }
    }
}

