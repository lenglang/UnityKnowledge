using UnityEngine;
using System.Collections;

public static class AnimatorUtility
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
    
} 
