using System.Collections.Generic;
using UnityEngine;

public class SplitTask : IShatterTask
{
	private GameObject gameObject;
	private Vector3[] localPoints;
	private Vector3[] localNormals;
	
	public SplitTask(GameObject gameObject, Plane[] planes)
	{
		this.gameObject = gameObject;
		this.localPoints = new Vector3[planes.Length];
		this.localNormals = new Vector3[planes.Length];
		
		for (int i = 0; i < planes.Length; i++)
		{
			Plane plane = planes[i];
			
			this.localPoints[i] = gameObject.transform.InverseTransformPoint(plane.normal * -plane.distance);
			this.localNormals[i] = gameObject.transform.InverseTransformDirection(plane.normal);
		}
	}
	
	public void Run()
	{
		if (gameObject)
		{
			Plane[] planes = new Plane[localPoints.Length];
			
			for (int i = 0; i < planes.Length; i++)
			{
				Vector3 point = gameObject.transform.TransformPoint(localPoints[i]);
				Vector3 normal = gameObject.transform.TransformDirection(localNormals[i]);
				
				planes[i] = new Plane(normal, point);
			}
			
			gameObject.SendMessage("Split", planes, SendMessageOptions.DontRequireReceiver);
		}
	}
}