// Shatter Toolkit
// Copyright 2012 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnCollision : MonoBehaviour
{
	public ShatterScheduler scheduler = null;
	
	public float requiredForce = 1.0f;
	
	public float cooldownTime = 0.5f;
	
	private float timeSinceInstantiated = 0.0f;
	
	public void Update()
	{
		timeSinceInstantiated += Time.deltaTime;
	}
	
	public void OnCollisionEnter(Collision collision)
	{
		if (timeSinceInstantiated >= cooldownTime)
		{
			if (collision.relativeVelocity.magnitude >= requiredForce)
			{
				// Find the new contact point
				foreach (ContactPoint contact in collision.contacts)
				{
					if (contact.otherCollider == collision.collider)
					{
						// Shatter at this contact point
						if (scheduler != null)
						{
							scheduler.AddTask(new ShatterTask(contact.thisCollider.gameObject, contact.point));
						}
						else
						{
							contact.thisCollider.SendMessage("Shatter", contact.point, SendMessageOptions.DontRequireReceiver);
						}
						
						break;
					}
				}
			}
		}
	}
}