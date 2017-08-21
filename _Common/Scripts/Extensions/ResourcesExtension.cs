using UnityEngine;
using UnityEngine.Events;
namespace WZK
{
    public static class ResourcesExtension
    {
        public static void LoadAssetAsync<T>(string path, System.Action<ResourceRequest> action) where T : Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
            Debug.Log(resourceRequest);
            //GlobalGameObject.Instance.InvokeWaitForYieldInstruction(action, resourceRequest);
        }
    }
}
