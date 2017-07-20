using UnityEngine;
using System.Collections;

public static class AnimatorExtension
{
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
}
