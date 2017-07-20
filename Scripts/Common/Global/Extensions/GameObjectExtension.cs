using UnityEngine;
public static class GameObjectExtension
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
    public static float ParticleSystemLength(GameObject obj)
    {
        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>();
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
    /// 从屏幕位置点发送射线是否碰撞到该物体-用于拖拽物体的碰撞检测
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="position">屏幕位置点</param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static bool IsRayHit(this GameObject obj, Vector2 position, Camera camera = null)
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
    /// <summary>
    /// 检测点是否在区域内
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layerMask">指定检测层范围</param>
    /// <param name="n">区域名</param>
    /// <param name="camera"></param>
    /// <returns></returns>
    private static bool IsPositionInArea(this Vector3 p, string layerMask, string n, Camera camera = null)
    {
        Ray ray;
        if (camera == null)
        {
            if (Camera.main == null)
            {
                Debug.LogError("场景中缺少照射的主摄像机，将照射相机Tag设置为MainCamera或给该类_camera属性赋值照射摄像机");
                return false;
            }
            ray = new Ray(Camera.main.transform.position, p - Camera.main.transform.position);
        }
        else
        {
            ray = new Ray(camera.transform.position, p - camera.transform.position);
        }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask(layerMask)))
        {
            if (hit.collider.name == n) return true;
        }
        return false;
    }
}
