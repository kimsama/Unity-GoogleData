using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// 
/// My spread sheet.
/// 
[System.Serializable]
public class MySpreadSheet : ScriptableObject //BaseDatabase 
{
	
	[HideInInspector] [SerializeField] 
	public string sheetName = "";
	
	[HideInInspector] [SerializeField] 
	public string worksheetName = "";
	
	[ExposeProperty]
	public string SheetName 
	{
		get { return sheetName; }
		set { sheetName = value;}
	}
	
	[ExposeProperty]
	public string WorksheetName
	{
		get { return worksheetName; }
		set { worksheetName = value;}
	}
	
	
	public MyData[] dataArray;
	
	void OnEnable()
	{		
#if UNITY_EDITOR
		//hideFlags = HideFlags.DontSave;
#endif
		
		dataArray = new MyData[0];
	}
	
	public MyData FindByKey(string key)
	{
		return Array.Find(dataArray, d => d.Key == key);
	}
}


/// 
/// IMPORTANT:
/// 	Property cannot be serizialized, only non static member filed can be.
/// 
[System.Serializable]
public class MyData
{
	[SerializeField]
	string key;
	
	[SerializeField]
	string text;
	
	[ExposeProperty]
	public string Key	{ get {return key; } set { key = value;} }
	
	[ExposeProperty]
	public string Text	{ get { return text;} set { text = value;} }
	
}