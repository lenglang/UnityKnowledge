using UnityEngine;
using System.Collections;

public class ThreadTest : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Person p = new Person(1, "刘备");
        ThreadHelper.QueueOnThreadPool(RunWorkerThread,p);
	}
    void RunWorkerThread(object obj)
    {
        Person p = obj as Person;
        Debug.Log(p.Id+","+p.Name);
        for (int i = 0; i < 1000; i++)
        {
            Debug.Log("输出************");
        }
        ThreadHelper.QueueOnMainThread(MainThread);
    }
    void MainThread()
    {
        Debug.Log("回到主线程啦");
    }
}
public class Person
{
    public Person(int id, string name) { Id = id; Name = name; }
    public int Id { get; set; }
    public string Name { get; set; }
}
