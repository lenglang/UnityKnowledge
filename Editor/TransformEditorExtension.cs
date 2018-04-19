using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
namespace WZK
{
    [CustomEditor(typeof(Transform))]
    public class TransformEditorExtension : Editor
    {
        private Transform _transform;
        private Assembly _assembly;
        private Type _transformInspector;
        private MethodInfo _onInspectorGUI;
        private Editor _transformEditor;

        private void OnEnable()
        {
            if (_assembly != null) return;
            _transform = target as Transform;
            //载入程序集
            _assembly = Assembly.GetAssembly(typeof(Editor));
            //获取TransformInspector类
            _transformInspector = _assembly.GetTypes().Where(t => t.Name == "TransformInspector").FirstOrDefault();
            //获取OnInspectorGUI方法
            _onInspectorGUI = _transformInspector.GetMethod("OnInspectorGUI", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            //创建TransformInspector的实例
            _transformEditor = CreateEditor(target, _transformInspector);
        }

        public override void OnInspectorGUI()
        {
            _onInspectorGUI.Invoke(_transformEditor, null);
            TransformButtonEditor.ShowButton(_transform);
        }
    }
}
