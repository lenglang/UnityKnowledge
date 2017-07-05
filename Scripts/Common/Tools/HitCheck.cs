using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace WZK.Common
{
    /// <summary>
    /// 作者-wzk
    /// 功能-检测鼠标(手指)是否在物体上
    /// 使用说明-传入判断物体，配合拖拽功能，实现拖动的物体是否拖到指定位置处
    /// </summary>
    public class HitCheck : MonoBehaviour
    {
        [HideInInspector]
        public Camera _camera;//摄像机
        private static HitCheck _instance;
        public static HitCheck Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("HitCheck");
                    _instance = obj.AddComponent<HitCheck>();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 检测碰撞体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool CheckHitObject(GameObject obj,Vector3 position)
        {
            Ray ray;
            if (_camera == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("场景中缺少照射的主摄像机，将照射相机Tag设置为MainCamera或给该类_camera属性赋值照射摄像机");
                    return false;
                }
                ray = Camera.main.ScreenPointToRay(position);
            }
            else
            {
                ray = _camera.ScreenPointToRay(position);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                return hit.collider.gameObject == obj;
            }
            return false;
        }
        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
