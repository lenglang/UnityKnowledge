using System.Collections.Generic;
using UnityEngine;

public class ShatterScheduler : MonoBehaviour
{
	[SerializeField]
	private int frameCooldown = 1;
	
	private List<IShatterTask> tasks = new List<IShatterTask>();
	
	private int framesSinceLastTask;
	
	/// <summary>
	/// Adds a shatter task to the queue of tasks to be run.
	/// Each of the tasks will be run in the order they were added
	/// </summary>
	/// <param name="task">
	/// The shatter task to add
	/// </param>
	public void AddTask(IShatterTask task)
	{
		tasks.Add(task);
	}
	
	public void Update()
	{
		framesSinceLastTask++;
		
		if (frameCooldown == 0)
		{
			// Run all tasks at once and reset the timer
			foreach (IShatterTask task in tasks)
			{
				task.Run();
			}
			
			tasks.Clear();
			
			framesSinceLastTask = 0;
		}
		else if (framesSinceLastTask >= frameCooldown)
		{
			// Run the oldest task and reset the timer
			if (tasks.Count >= 1)
			{
				tasks[0].Run();
				
				tasks.RemoveAt(0);
				
				framesSinceLastTask = 0;
			}
		}
	}
}