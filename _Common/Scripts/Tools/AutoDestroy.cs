using UnityEngine;
public class AutoDestroy : MonoBehaviour
{
    [Header("销毁时间")]
    public float _time = 1;
    void Start()
    {
        Invoke("DestroyGameObject", _time);
    }
    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

}
