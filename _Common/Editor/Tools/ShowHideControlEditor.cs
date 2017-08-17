using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[CustomEditor(typeof(ShowHideControl))]
public class ShowHideControlEditor : Editor
{
    private ShowHideControl _showHide;
    private ShowHideControl.GameObjectGroup _gameObjectGroup;
    private int _deleteGameObjectGroupIndex = -1;//删除组索引
    private int _choseAreaIndex = -1;
    private int _deleteGameObjectIndex = -1;//对象索引
    private ShowHideControl.GameObjectGroup.GameObjectInformation _gameObjectInformation;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _showHide = target as ShowHideControl;
        if (GUILayout.Button("添加管理"))
        {
            _showHide._list.Add(new ShowHideControl.GameObjectGroup());
        }
        GUILayout.Space(30);
        _deleteGameObjectGroupIndex = -1;
        for (int i = 0; i < _showHide._list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _gameObjectGroup = _showHide._list[i];
            _gameObjectGroup._desc = EditorGUILayout.TextField(_gameObjectGroup._desc);
            if (GUILayout.Button("正执行"))
            {
                for (int j = 0; j < _gameObjectGroup._list.Count; j++)
                {
                    _gameObjectGroup._list[j]._obj.SetActive(_gameObjectGroup._list[j]._show);
                }
            }
            if (GUILayout.Button("反执行"))
            {
                for (int j = 0; j < _gameObjectGroup._list.Count; j++)
                {
                    _gameObjectGroup._list[j]._obj.SetActive(!_gameObjectGroup._list[j]._show);
                }
            }
            if (GUILayout.Button("删除"))
            {
                if (EditorUtility.DisplayDialog("警告", "你确定要删除该组对象吗？", "确定", "取消"))
                {
                    _deleteGameObjectGroupIndex = i;
                }
            }
            EditorGUILayout.EndHorizontal();

            var dragArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
            GUI.Box(dragArea, new GUIContent("拖动对象到此区域"));
            if (dragArea.Contains(Event.current.mousePosition)&& Event.current.type == EventType.DragUpdated)
            {
                _choseAreaIndex = i;
            }
            else if(Event.current.type==EventType.DragUpdated)
            {
                _choseAreaIndex = -1;
            }
            _deleteGameObjectIndex = -1;
            for (int k = 0; k < _gameObjectGroup._list.Count; k++)
            {
                EditorGUILayout.BeginHorizontal();
                _gameObjectGroup._list[k]._obj = (GameObject)EditorGUILayout.ObjectField(_gameObjectGroup._list[k]._obj, typeof(GameObject), true);
                _gameObjectGroup._list[k]._show = EditorGUILayout.Toggle(_gameObjectGroup._list[k]._show);
                if (GUILayout.Button("删除"))
                {
                    _deleteGameObjectIndex = k;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (_deleteGameObjectIndex != -1) _gameObjectGroup._list.RemoveAt(_deleteGameObjectIndex);
            GUILayout.Space(50);
        }
        if (Event.current.type == EventType.DragExited && _choseAreaIndex != -1)
        {
            DragAndDrop.AcceptDrag();
            if (DragAndDrop.objectReferences.Length != 0)
            {
                for (int j = 0; j < DragAndDrop.objectReferences.Length; j++)
                {
                    if (DragAndDrop.objectReferences[j].GetType() == typeof(GameObject))
                    {
                        if (IsExistGameObject((GameObject)DragAndDrop.objectReferences[j]) == false)
                        {
                            _gameObjectInformation = new ShowHideControl.GameObjectGroup.GameObjectInformation();
                            _gameObjectInformation._obj = (GameObject)DragAndDrop.objectReferences[j];
                            _gameObjectGroup._list.Add(_gameObjectInformation);
                        }
                        else
                        {
                            Debug.LogError("列表已存在该对象");
                        }
                    }
                }
            }
        }
        if (_deleteGameObjectGroupIndex != -1) _showHide._list.RemoveAt(_deleteGameObjectGroupIndex);
        GUILayout.Space(1000);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_showHide);
        if (GUI.changed) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    /// <summary>
    /// 是否已经存在该对象
    /// </summary>
    /// <param name="obj"></param>
    public bool IsExistGameObject(GameObject obj)
    {
        for (int i = 0; i < _gameObjectGroup._list.Count; i++)
        {
            if (obj == _gameObjectGroup._list[i]._obj) return true;
        }
        return false;
    }
    [MenuItem("GameObject/自定义/创建显示隐藏管理", false, MenuItemConfig.显示隐藏管理)]
    private static void CreateSoundControlObject()
    {
        GameObject gameObject = new GameObject("显示隐藏管理");
        gameObject.AddComponent<ShowHideControl>();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gameObject;
        EditorGUIUtility.PingObject(Selection.activeObject);
        Undo.RegisterCreatedObjectUndo(gameObject, "Create GameObject");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
