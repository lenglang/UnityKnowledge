using UnityEngine;
using System.Collections;

public class NavigationExample : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {
        
	}
    /// <summary>
    /// 获取导航随机点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int t = Random.Range(0, navMeshData.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);
        return point;
    }
}
