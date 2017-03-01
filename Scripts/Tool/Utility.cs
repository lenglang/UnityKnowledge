using UnityEngine;
using System.Collections;

public static class Utility
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
}
