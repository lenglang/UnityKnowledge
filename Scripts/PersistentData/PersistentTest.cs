using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistentTest : MonoBehaviour {


	/// <summary>
	/// 定义一个测试类
	/// </summary>
	public class TestClass
	{
		public string Name = "张三";
		public float Age = 23.0f;
		public int Sex = 1;

		public List<int> Ints = new List<int> ()
		{
			1,
			2,
			3
		};
	}

	void Start () 
	{
		//定义存档路径
		string dirpath = Application.persistentDataPath + "/Save";
		//创建存档文件夹
		IOHelper.CreateDirectory (dirpath);
		//定义存档文件路径
		string filename = dirpath + "/GameData.szv";
		TestClass t = new TestClass ();
		//保存数据
		IOHelper.SetData (filename,t);
		//读取数据
		TestClass t1 = (TestClass)IOHelper.GetData(filename,typeof(TestClass));

		Debug.Log(t1.Name);
		Debug.Log(t1.Age);
		Debug.Log(t1.Ints);
	}


}