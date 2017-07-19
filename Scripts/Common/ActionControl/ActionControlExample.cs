using UnityEngine;
using System.Collections;
namespace WZK.Common
{
    public class ActionControlExample : MonoBehaviour
    {
        public enum WaitActionType
        {
            播放成功动画,
            播放失败动画
        }
        public enum LoopActionType
        {
            操作提示1,
            操作提示2
        }
        // Use this for initialization
        void Start()
        {
            //等待动作使用方法
            WaitActionControl<int>.Instance.AddWaitAction(delegate { Debug.Log("方法1"); }, 1);

            WaitActionControl<string>.Instance.AddWaitAction(delegate { Debug.Log("方法2"); }, 2);
            WaitActionControl<string>.Instance.AddWaitAction(delegate { Debug.Log("方法3"); }, 2,"3");
            WaitActionControl<string>.Instance.RemoveWaitAction("3");

            WaitActionControl<WaitActionType>.Instance.AddWaitAction(delegate { Debug.Log("播放成功动画"); }, 3);
            WaitActionControl<WaitActionType>.Instance.AddWaitAction(delegate { Debug.Log("播放失败动画"); }, 1, WaitActionType.播放失败动画);
            if (Random.value < 0.5f) WaitActionControl<WaitActionType>.Instance.RemoveWaitAction(WaitActionType.播放失败动画);

            //循环动作使用方法
            LoopActionControl<int>.Instance.AddLoopAction(delegate { Debug.Log("循环动作1"); }, 1);

            LoopActionControl<string>.Instance.AddLoopAction(delegate { Debug.Log("循环动作2"); }, 2);
            LoopActionControl<string>.Instance.AddLoopAction(delegate { Debug.Log("循环动作3"); }, 2, "3");
            LoopActionControl<string>.Instance.RemoveLoopAction("3");

            LoopActionControl<LoopActionType>.Instance.AddLoopAction(delegate { Debug.Log("循环动作4"); },5,LoopActionType.操作提示1, false,1);

            LoopActionControl<LoopActionType>.Instance.AddLoopAction(delegate { Debug.Log("循环动作5"); }, 5, LoopActionType.操作提示1, false, 1,2);

        }
        private void FixedUpdate()
        {
            WaitActionControl<int>.Instance.FixedUpdate();
            WaitActionControl<string>.Instance.FixedUpdate();
            WaitActionControl<WaitActionType>.Instance.FixedUpdate();

            LoopActionControl<int>.Instance.FixedUpdate();
            LoopActionControl<string>.Instance.FixedUpdate();
            LoopActionControl<LoopActionType>.Instance.FixedUpdate();
        }
        private void OnDestroy()
        {
            //清空
        }
    }
}
