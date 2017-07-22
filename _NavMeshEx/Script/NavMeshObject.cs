using UnityEngine;
using System.Collections.Generic;


namespace babybus.NavMeshEx
{
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class NavMeshObject : MonoBehaviour
{
    public bool Editable
    {
        get
        {
            return editable;
        }
        set
        {
            editable = value;
        }
    }

    [SerializeField, HideInInspector]
    private bool editable;

    //顶点的本地坐标
    public List<Vector3> Vertices
    {
        get
        {
            return vertices;
        }
    }

    [SerializeField]
    private List<Vector3> vertices = new List<Vector3>();

    //三角形顶点索引
    public List<int> Triangles
    {
        get
        {
            return triangles;
        }
    }

    [SerializeField, HideInInspector]
    private List<int> triangles = new List<int>();

    //选中点索引，最多选中三个
    public List<int> Selecteds
    {
        get
        {
            return selecteds;
        }
    }

    [SerializeField, HideInInspector]
    private List<int> selecteds = new List<int>();

    public Mesh NavMesh
    {
        get;
        private set;
    }

    public Color normalColor = Color.black, selectedColor = Color.white, outlineColor = Color.black;

    public void Awake()
    {
        if (NavMesh == null)
        {
            NavMesh = new Mesh();
            NavMesh.name = "NavMeshObject Mesh";
            GetComponent<MeshFilter>().mesh = NavMesh;
        }
    }

    public void OnDestroy()
    {
        GetComponent<MeshFilter>().mesh = null;
    }

    /// <summary>添加世界坐标点</summary>
    public void AddPoint(Vector3 point)
    {
        Vertices.Add(transform.InverseTransformPoint(point));
        UpdateMesh();
    }

    /// <summary>根据索引移除点</summary>
    public void RemovePointAt(int index)
    {
        Vertices.RemoveAt(index);

        for (int i = 0; i < Triangles.Count; i++)
        {
            //移除使用该点的三角形
            if (Triangles[i] == index)
            {
                Triangles.RemoveRange(i - i % 3, 3);
                i = i - i % 3 - 1;
            }
        }

        for (int i = 0; i < Triangles.Count; i++)
        {
            if (Triangles[i] > index)
                Triangles[i]--;
        }

        UpdateMesh();
    }

    /// <summary>移动顶点</summary>
    public void PositionHandleAt(int index, Vector3 delta)
    {
        Vertices[index] += delta;
        UpdateMesh();
    }

    /// <summary>转换为世界坐标</summary>
    public List<Vector3> TransformPoints()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < Vertices.Count; i++)
            points.Add(transform.TransformPoint(Vertices[i]));

        return points;
    }

    /// <summary>indices是组成改三角形的顶点数组索引</summary>
    public void AddTriangle(int[] indices)
    {
        for (int i = 0; i < Triangles.Count / 3; i++)
        {
            if (Triangles[i * 3] == indices[0] && Triangles[i * 3 + 1] == indices[1] && Triangles[i * 3 + 2] == indices[2])//三角形已经添加过
                return;
        }

        //正反面都要添加
        Triangles.Add(indices[0]);
        Triangles.Add(indices[1]);
        Triangles.Add(indices[2]);

        Triangles.Add(indices[1]);
        Triangles.Add(indices[0]);
        Triangles.Add(indices[2]);

        UpdateMesh();
    }

    public void UpdateMesh()
    {
        NavMesh.triangles = null;

        NavMesh.vertices = Vertices.ToArray();
        NavMesh.triangles = Triangles.ToArray();
    }
}
}