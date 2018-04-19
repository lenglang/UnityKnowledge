using UnityEditor;
using UnityEngine;
namespace WZK
{
    [CustomEditor(typeof(RectTransform))]
    public class RectTransformEditorExtension : DecoratorEditor
    {
        public RectTransformEditorExtension() : base("RectTransformEditor") { }
        public Transform _transform;
        private void OnEnable()
        {
            _transform = target as Transform;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TransformButtonEditor.ShowButton(_transform);
        }
    }
}