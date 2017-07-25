using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

namespace babybus.NavMeshEx
{
[CustomEditor(typeof(NavMeshObject))]
public class NavMeshObjectEditor : Editor
{
    private NavMeshObject navMeshObject
    {
        get
        {
            return target as NavMeshObject;
        }
    }

    private Tool last;

    void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
    }

    void OnUndoRedoPerformed()
    {
        navMeshObject.UpdateMesh();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Current Triangles: " + navMeshObject.Triangles.Count / 3);


        if (navMeshObject.Editable)
            GUI.color = Color.yellow;

        if (navMeshObject.Editable && GUILayout.Button("Edit Mode: On") || !navMeshObject.Editable && GUILayout.Button("Edit Mode: Off"))
        {
            Undo.RecordObject(navMeshObject, "editable");

            navMeshObject.Editable = !navMeshObject.Editable;

            if (navMeshObject.Editable)
            {
                last = Tools.current;
                Tools.current = Tool.None;
            }
            else
                Tools.current = last;
        }


        GUI.color = Color.white;

        if (GUILayout.Button("Bake NavMesh"))
        {
            GameObjectUtility.SetStaticEditorFlags(navMeshObject.gameObject, StaticEditorFlags.NavigationStatic);
            NavMeshBuilder.BuildNavMesh();
        }

        if (GUILayout.Button("Save NavMeshObject Mesh"))
        {
            
            string path = SceneManager.GetActiveScene().path;
            path=Path.GetDirectoryName(path)+ "/Assets/";
                if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                AssetDatabase.CreateAsset(Instantiate(navMeshObject.NavMesh), path + navMeshObject.gameObject.name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    public void OnSceneGUI()
    {
        List<Vector3> points = navMeshObject.TransformPoints();

        #region 当前处于可编辑状态
        if (navMeshObject.Editable)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));//编辑时独占模式

            Event e = Event.current;

            List<int> selecteds = navMeshObject.Selecteds;

            #region 移动选中点
            for (int i = 0; i < selecteds.Count; i++)
            {
                int selected = selecteds[i];

                Handles.color = navMeshObject.selectedColor;

                Vector3 position = points[selected];

                Handles.DrawSolidDisc(position, Vector3.up, HandleUtility.GetHandleSize(position) * 0.1f);

                Vector3 delta = Handles.PositionHandle(position, Quaternion.identity) - position;

                GUIUtility.GetControlID(FocusType.Passive);
                if (e.type == EventType.used)
                {
                    Undo.RecordObject(navMeshObject, "PositionHandleAt");

                    navMeshObject.PositionHandleAt(selected, delta);

                    EditorUtility.SetDirty(navMeshObject);
                    break;
                }
            }
            #endregion

            #region 鼠标右键按下
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                Undo.RecordObject(navMeshObject, "selecteds");
                selecteds.Clear();
            }
            #endregion

            #region 鼠标左键按下
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                int index = FindClosest(points, Event.current.mousePosition);

                if (index != -1)
                {
                    #region 鼠标点击到已有的顶点
                    Undo.RecordObject(navMeshObject, "selecteds");

                    if (e.control)
                    {
                        selecteds.Add(index);

                        if (selecteds.Count == 3)
                        {
                            navMeshObject.AddTriangle(new int[] { selecteds[0], selecteds[1], selecteds[2] });
                            EditorUtility.SetDirty(navMeshObject);

                            selecteds.Clear();
                        }
                    }
                    else
                    {
                        selecteds.Clear();
                        selecteds.Add(index);
                    }
                    #endregion
                }
                else
                {
                    #region 鼠标没有点击到已有的顶点
                    Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                    RaycastHit hitInfo;
                    if (Physics.Raycast(worldRay, out hitInfo))
                    {
                        Undo.RecordObject(navMeshObject, "AddPoint");

                        Vector3 point = hitInfo.point;
                        point.y += 0.001f;

                        navMeshObject.AddPoint(point);

                        points.Add(point);

                        if (e.control)
                        {
                            selecteds.Add(points.Count - 1);

                            if (selecteds.Count == 3)
                            {
                                navMeshObject.AddTriangle(new int[] { selecteds[0], selecteds[1], selecteds[2] });
                                EditorUtility.SetDirty(navMeshObject);

                                selecteds.Clear();
                            }
                        }
                        else
                        {
                            if (points.Count >= 3)
                            {
                                switch (selecteds.Count)
                                {
                                case 0:
                                    navMeshObject.AddTriangle(new int[] { 0, points.Count - 2, points.Count - 1 });
                                    break;

                                case 1:
                                    if (selecteds[0] == 0 || selecteds[0] == points.Count - 2)
                                        navMeshObject.AddTriangle(new int[] { 0, points.Count - 2, points.Count - 1 });
                                    else
                                        navMeshObject.AddTriangle(new int[] { selecteds[0], points.Count - 2, points.Count - 1 });
                                    break;

                                case 2:
                                    navMeshObject.AddTriangle(new int[] { selecteds[0], selecteds[1], points.Count - 1 });
                                    break;
                                }

                                selecteds.Clear();
                                selecteds.Add(points.Count - 1);
                            }
                        }

                        EditorUtility.SetDirty(navMeshObject);
                    }
                    #endregion
                }
            }
            #endregion

            #region 删除选中点
            if (selecteds.Count == 1)
            {
                int selected = selecteds[0];

                if (e.keyCode == KeyCode.Backspace)
                {
                    Undo.RecordObject(navMeshObject, "RemovePointAt");

                    navMeshObject.RemovePointAt(selected);
                    points.RemoveAt(selected);

                    selecteds.Clear();

                    EditorUtility.SetDirty(navMeshObject);
                }
            }
            #endregion
        }
        #endregion

        Handles.color = navMeshObject.outlineColor;

        List<int> triangles = navMeshObject.Triangles;
        for (int i = 0; i < triangles.Count / 3; i++)
            Handles.DrawPolyLine(points[triangles[i * 3]], points[triangles[i * 3 + 1]], points[triangles[i * 3 + 2]]);

        Handles.color = navMeshObject.normalColor;

        for (int i = 0; i < points.Count; i++)
            Handles.CircleCap(0, points[i], Quaternion.LookRotation(Vector3.up), HandleUtility.GetHandleSize(points[i]) * 0.1f);

        HandleUtility.Repaint();
    }

    /// <summary>返回距离鼠标点击点最近的顶点索引</summary>
    private int FindClosest(List<Vector3> points, Vector2 mousePosition)
    {
        List<int> closest = new List<int>();

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 position = HandleUtility.WorldToGUIPoint(points[i]);
            if (Vector2.Distance(position, mousePosition) < 10)//因为之前绘制时大小为HandleUtility.GetHandleSize(points[i]) * 0.1f
                closest.Add(i);
        }

        if (closest.Count == 0)
            return -1;
        else if (closest.Count == 1)
            return closest[0];
        else
        {
            Vector3 cameraPosition = Camera.current.transform.position;
            float nearDist = float.MaxValue;
            int near = -1;

            //遍历所有点，找到距离摄像机最近的点
            for (int i = 0; i < closest.Count; i++)
            {
                float dist = Vector3.Distance(points[closest[i]], cameraPosition);
                if (dist < nearDist)
                {
                    nearDist = dist;
                    near = closest[i];
                }
            }

            return near;
        }
    }
}
}