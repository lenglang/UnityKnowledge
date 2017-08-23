using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace WZK
{
    /// <summary>
    /// 位置信息编辑器扩展
    /// </summary>
    [CustomEditor(typeof(MyPositionManager))]
    public class MyPositionManagerEditor : Editor
    {
        private MyPositionManager _myPositionManager;
        private MyPositionManager.PositionObject _positionObject;
        private TransformInformation _information;
        private string _flodOutName;
        private int _deleteObjectIndex = -1;
        private int _deletePositionIndex = -1;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _myPositionManager = target as MyPositionManager;
            EditorGUILayout.LabelField("拖动场景对象到下面任意区域");
            GUILayout.Space(30);
            _deleteObjectIndex = -1;
            _deletePositionIndex = -1;
            for (int i = 0; i < _myPositionManager._list.Count; i++)
            {
                _positionObject = _myPositionManager._list[i];
                _flodOutName = "待赋值";
                if (_positionObject._obj != null) _flodOutName = _positionObject._obj.name;
                EditorGUILayout.BeginHorizontal();
                _positionObject._show = EditorGUILayout.Foldout(_positionObject._show, _flodOutName);
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("警告", "你确定要删除该对象吗？", "确定", "取消"))
                    {
                        _deleteObjectIndex = i;
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (_positionObject._show)
                {
                    EditorGUILayout.BeginHorizontal();
                    _myPositionManager._list[i]._obj = (GameObject)EditorGUILayout.ObjectField(_myPositionManager._list[i]._obj, typeof(GameObject), true);
                    if (GUILayout.Button("添加位置"))
                    {
                        _positionObject._list.Add(new TransformInformation());
                        SavePosition(_positionObject._list[_positionObject._list.Count - 1], _positionObject._obj.GetComponent<Transform>());
                    }
                    EditorGUILayout.EndHorizontal();
                    for (int j = 0; j < _positionObject._list.Count; j++)
                    {
                        _information = _positionObject._list[j];
                        EditorGUILayout.BeginHorizontal();
                        _information._desc = EditorGUILayout.TextField(_information._desc);
                        if (GUILayout.Button("切换到该位置"))
                        {
                            Transform transform = _positionObject._obj.GetComponent<Transform>();
                            transform.localPosition = _information._localPosition;
                            transform.rotation = _information._rotation;
                            transform.localScale = _information._localScale;
                        }
                        if (GUILayout.Button("保存当前位置"))
                        {
                            SavePosition(_information, _positionObject._obj.GetComponent<Transform>());
                        }
                        if (GUILayout.Button("删除"))
                        {
                            if (EditorUtility.DisplayDialog("警告", "你确定要删除当前保存的位置信息吗？", "确定", "取消"))
                            {
                                _deletePositionIndex = j;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(20);
                    }
                    if (_deletePositionIndex != -1) _positionObject._list.RemoveAt(_deletePositionIndex);
                }
                GUILayout.Space(50);
            }
            if (_deleteObjectIndex != -1) _myPositionManager._list.RemoveAt(_deleteObjectIndex);
            if (Event.current.type == EventType.DragExited)
            {
                if (DragAndDrop.objectReferences.Length != 0)
                {
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        if (DragAndDrop.objectReferences[i].GetType() == typeof(GameObject))
                        {
                            if (IsExistGameObject(_myPositionManager, (GameObject)DragAndDrop.objectReferences[i]) == false)
                            {
                                _positionObject = new MyPositionManager.PositionObject();
                                _positionObject._obj = (GameObject)DragAndDrop.objectReferences[i];
                                _myPositionManager._list.Add(_positionObject);
                            }
                            else
                            {
                                Debug.LogError("列表已存在该对象");
                            }
                        }
                    }
                }
            }
            GUILayout.Space(1000);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_myPositionManager);
            if (GUI.changed) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        /// <summary>
        /// 是否存在该对象
        /// </summary>
        /// <param name="myPosition"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsExistGameObject(MyPositionManager myPosition, GameObject obj)
        {
            for (int i = 0; i < myPosition._list.Count; i++)
            {
                if (myPosition._list[i]._obj == obj)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 保存位置
        /// </summary>
        /// <param name="information"></param>
        /// <param name="transform"></param>
        private void SavePosition(TransformInformation information, Transform transform)
        {
            information._localPosition = transform.localPosition;
            information._position = transform.position;
            information._rotation = transform.rotation;
            information._localScale = transform.localScale;
        }
        [MenuItem("GameObject/自定义/创建位置信息管理对象", false, MenuItemConfig.位置信息管理)]
        private static void CreateSoundControlObject()
        {
            GameObject gameObject = new GameObject("位置信息管理");
            gameObject.AddComponent<MyPositionManager>();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = gameObject;
            EditorGUIUtility.PingObject(Selection.activeObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Create GameObject");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
