using UnityEngine;
namespace WZK
{
    public class TransformButtonEditor : MonoBehaviour
    {
        public static void ShowButton(Transform tf)
        {
            if (GUILayout.Button("添加位置管理"))
            {
                if (tf.GetComponent<GameObjectPosition>() == null) tf.gameObject.AddComponent<GameObjectPosition>();
            }
        }
    }
}
