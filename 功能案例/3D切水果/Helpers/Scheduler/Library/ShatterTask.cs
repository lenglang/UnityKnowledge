using System.Collections.Generic;
using UnityEngine;

public class ShatterTask : IShatterTask
{
	private GameObject gameObject;
	private Vector3 localPoint;
	
	public ShatterTask(GameObject gameObject, Vector3 point)
	{
		this.gameObject = gameObject;
		this.localPoint = gameObject.transform.InverseTransformPoint(point);
	}
	
	public void Run()
	{
		if (gameObject)
		{
			Vector3 point = gameObject.transform.TransformPoint(localPoint);
			
			gameObject.SendMessage("Shatter", point, SendMessageOptions.DontRequireReceiver);
		}
	}
}