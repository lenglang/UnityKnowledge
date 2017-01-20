// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class MouseSplit : MonoBehaviour
{
	public ShatterScheduler scheduler = null;
	
	public int raycastCount = 5;
	
	private bool started = false;
	private Vector3 start, end;
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			start = Input.mousePosition;
			
			started = true;
		}
		
		if (Input.GetMouseButtonUp(0) && started)
		{
			end = Input.mousePosition;
			
			// Calculate the world-space line
			Camera mainCamera = Camera.main;
			
			float near = mainCamera.nearClipPlane;
			
			Vector3 line = mainCamera.ScreenToWorldPoint(new Vector3(end.x, end.y, near)) - mainCamera.ScreenToWorldPoint(new Vector3(start.x, start.y, near));
			
			// Find game objects to split by raycasting at points along the line
			for (int i = 0; i < raycastCount; i++)
			{
				Ray ray = mainCamera.ScreenPointToRay(Vector3.Lerp(start, end, (float)i / raycastCount));
				
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit))
				{
                    if (!hit.transform.tag.Equals("Split")) continue;
					Plane splitPlane = new Plane(Vector3.Normalize(Vector3.Cross(line, ray.direction)), hit.point);
					
					if (scheduler != null)
					{
						scheduler.AddTask(new SplitTask(hit.collider.gameObject, new Plane[] { splitPlane }));
					}
					else
					{
                        GameObject[] objs = hit.transform.GetComponent<ShatterTool>().Split(new Plane[] { splitPlane });
                        if (objs.Length == 2)
                        {
                            MoveAway(objs[0], objs[1], end - start);
                        }
						//hit.collider.SendMessage("Split", new Plane[] { splitPlane }, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			
			started = false;
		}
	}
    private void MoveAway(GameObject objOne, GameObject objTwo, Vector3 from)
    {
        float distance = Random.Range(0.01f, 0.02f);
        if (Mathf.Abs(from.x) - Mathf.Abs(from.y) < 0)
        {

            //Êú
            int dir = 0;
            if (from.y > 0)
                dir = -1;
            else
                dir = 1;
            Debug.Log(dir);
            //objOne.transform.DOMoveX(distance * dir, 0).SetRelative();
            //objTwo.transform.DOMoveX(-distance * dir, 0).SetRelative();
        }
        else
        {
            //ºá
            int dir = 0;
            if (from.x > 0)
                dir = 1;
            else
                dir = -1;
            Debug.Log(dir);
        }
        Debug.Log(distance);


    }
    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
            started = false;
    }
}