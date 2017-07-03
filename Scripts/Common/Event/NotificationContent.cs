using System;
using UnityEngine;
public class NotificationContent
{
	/// <summary>
	/// 通知发送者
	/// </summary>
	public GameObject sender;
	public int _age = 58;
	public string _name="小王";
	public override string ToString()
	{
		return string.Format("age={0},name={1}", this._age, this._name);
	}

}