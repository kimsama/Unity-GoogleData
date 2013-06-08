using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MySpreadSheet : BaseDatabase 
{
	public MyData[] dataArray = new MyData[0];
	
	void OnEnable()
	{
#if UNITY_EDITOR
		//hideFlags = HideFlags.DontSave;
#endif
	}
	
	public MyData FindByKey(string key)
	{
		return Array.Find(dataArray, d => d.Key == key);
	}
}


[System.Serializable]
public class MyData
{
	[ExposeProperty]
	public string Key				{ get; set; }
	
	[ExposeProperty]
	public string Text	{ get; set; }
	
}