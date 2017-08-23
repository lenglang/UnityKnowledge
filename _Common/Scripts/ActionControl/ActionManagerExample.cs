using UnityEngine;
namespace WZK
{
    public class ActionManagerExample : MonoBehaviour
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
            WaitActionManager<int>.Instance.AddWaitAction(delegate { Debug.Log("方法1"); }, 1);

            WaitActionManager<string>.Instance.AddWaitAction(delegate { Debug.Log("方法2"); }, 2);
            WaitActionManager<string>.Instance.AddWaitAction(delegate { Debug.Log("方法3"); }, 2, "3");
            WaitActionManager<string>.Instance.RemoveWaitAction("3");

            WaitActionManager<WaitActionType>.Instance.AddWaitAction(delegate { Debug.Log("播放成功动画"); }, 3);
            WaitActionManager<WaitActionType>.Instance.AddWaitAction(delegate { Debug.Log("播放失败动画"); }, 1, WaitActionType.播放失败动画);
            if (Random.value < 0.5f) WaitActionManager<WaitActionType>.Instance.RemoveWaitAction(WaitActionType.播放失败动画);

            //循环动作使用方法
            LoopActionManager<int>.Instance.AddLoopAction(delegate { Debug.Log("循环动作1"); }, 1);

            LoopActionManager<string>.Instance.AddLoopAction(delegate { Debug.Log("循环动作2"); }, 2);
            LoopActionManager<string>.Instance.AddLoopAction(delegate { Debug.Log("循环动作3"); }, 2, "3");
            LoopActionManager<string>.Instance.RemoveLoopAction("3");

            LoopActionManager<LoopActionType>.Instance.AddLoopAction(delegate { Debug.Log("循环动作4"); }, 5, LoopActionType.操作提示1, false, 1);

            LoopActionManager<LoopActionType>.Instance.AddLoopAction(delegate { Debug.Log("循环动作5"); }, 5, LoopActionType.操作提示1, false, 1, 2);

        }
        private void FixedUpdate()
        {
            WaitActionManager<int>.Instance.FixedUpdate();
            WaitActionManager<string>.Instance.FixedUpdate();
            WaitActionManager<WaitActionType>.Instance.FixedUpdate();

            LoopActionManager<int>.Instance.FixedUpdate();
            LoopActionManager<string>.Instance.FixedUpdate();
            LoopActionManager<LoopActionType>.Instance.FixedUpdate();
        }
        private void OnDestroy()
        {
            //清空
            WaitActionManager<int>.Instance.RemoveAllWaitAction();
            WaitActionManager<string>.Instance.RemoveAllWaitAction();
            WaitActionManager<WaitActionType>.Instance.RemoveAllWaitAction();

            LoopActionManager<int>.Instance.RemoveAllLoopAction();
            LoopActionManager<string>.Instance.RemoveAllLoopAction();
            LoopActionManager<LoopActionType>.Instance.RemoveAllLoopAction();
        }
    }
}
