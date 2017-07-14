using UnityEngine;
namespace WZK.Common
{
    public static class UtilityCommon
    {
        /// <summary>
        /// 设定层
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        public static void SetLayer(this GameObject obj, int layer)
        {
            obj.layer = layer;
            if (obj.transform.childCount == 0) return;
            foreach (Transform child in obj.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = layer;
            }
        }
        /// <summary>
        /// 获取粒子特效时长
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static float ParticleSystemLength(Transform transform)
        {
            ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.emission.enabled)
                {
                    if (ps.loop)
                    {
                        return -1f;
                    }
                    float dunration = 0f;
                    if (ps.emission.rate.constantMax <= 0)
                    {
                        dunration = ps.startDelay + ps.startLifetime;
                    }
                    else
                    {
                        dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
                    }
                    if (dunration > maxDuration)
                    {
                        maxDuration = dunration;
                    }
                }
            }
            return maxDuration;
        }
        /// <summary>
        /// 获取某段动画长度
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static float GetAnimationLenghtByName(this Animator animator, string name)
        {
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            AnimationClip[] clips = runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Equals(name))
                {
                    return clip.length;
                }
            }
            return 0;
        }
        /// <summary>
        /// 是否在播某个动画
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool IsPlayAnimation(this Animator animator, string animationName, int layerIndex = 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return stateInfo.IsName(animationName);
        }
        /// <summary>
        /// 发送射线是否碰撞到该物体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position">射线位置点</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsRayHit(this GameObject obj,Vector2 position,Camera camera=null)
        {
            Ray ray;
            if (camera == null)
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
                ray = camera.ScreenPointToRay(position);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                return hit.collider.gameObject == obj;
            }
            return false;
        }
        public static string GetEnumDescription(this System.Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }
    }
}
