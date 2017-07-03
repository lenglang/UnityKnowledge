using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using WZK.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(TurnGestures3D))]
[AddComponentMenu("Attribute/AttributeTest")]
public class AttributeTest : MonoBehaviour
{
    [Header("生命值")]
    public float _life = 10;
    [MyRange(0, 500, "魔法值")]
    public float _magic = 0;
    [Range(0, 300)]
    public float _attack = 20;
    [TextArea]
    public string _text;
    [Header("类序列化")]
    public SerializableExample _se;
    public int nospace1 = 0;
    public int nospace2 = 0;
    [Space(30)]
    public int space = 0;
    public int nospace3 = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("Do Something")]
    void DoSomething()
    {
        Debug.Log("Perform operation");
    }
    [ContextMenuItem("Reset", "ResetName")]
    public string _name = "修改名字后，右键重置名字";
    void ResetName()
    {
        _name = "Default";
    }
    //[RuntimeInitializeOnLoadMethod]
    //static void OnRuntimeMethodLoad()
    //{
        //Debug.Log("在游戏启动时，会自动调用添加了该属性的方法。该类无需AddComponent到面板");
    //}
#if UNITY_EDITOR
    [MenuItem("Attribute/DebugMessage")]
#endif
    public static void CreateGameObject()
    {
        Debug.Log("DebugMessage");
    }
    [System.Serializable]
    public class SerializableExample
    {
        public int _time;
        public int _coin;
    }
}
