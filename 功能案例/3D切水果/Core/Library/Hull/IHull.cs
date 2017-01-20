// Shatter Toolkit
// Copyright 2012 Gustav Olsson
using UnityEngine;

public interface IHull
{
	bool IsEmpty
	{
		get;
	}
	
	Mesh GetMesh();
	
	void Split(Vector3 localPointOnPlane, Vector3 localPlaneNormal, bool fillCut, UvMapper uvMapper, out IHull resultA, out IHull resultB);
}