using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

[CustomEditor(typeof(MySpreadSheet))]
public class MySpreadSheetEditor  : BaseEditor<MySpreadSheet>
{
	
	public override void OnEnable()
	{
		base.OnEnable();
		
		MySpreadSheet data = database as MySpreadSheet;
		
		databaseFields = ExposeProperties.GetProperties(data);
		
		foreach(MyData e in data.dataArray)
		{
		    dataFields = ExposeProperties.GetProperties(e);
			pInfoList.Add(dataFields);
		}
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		//DrawDefaultInspector();
		if (GUI.changed)
		{
			pInfoList.Clear();
			
			MySpreadSheet data = database as MySpreadSheet;
			foreach(MyData e in data.dataArray)
			{
				dataFields = ExposeProperties.GetProperties(e);
				pInfoList.Add(dataFields);
			}
			
			EditorUtility.SetDirty(target);
			Repaint();
		}
	}
	
	public override bool Load()
	{
		var client = new DatabaseClient(username, password);		
		var db = client.GetDatabase(database.SheetName) ?? client.CreateDatabase(database.SheetName);	
		var table = db.GetTable<MyData>(database.WorksheetName) ?? db.CreateTable<MyData>(database.WorksheetName);
		
		List<MyData> myDataList = new List<MyData>();
		
		var all = table.FindAll();						
		foreach(var elem in all)
		{
			MyData data = new MyData();
			
			data = Cloner.DeepCopy<MyData>(elem.Element);
			myDataList.Add(data);
		}
		
#if UNITY_EDITOR
		Debug.Log ("=== MyData ===");
		foreach(var e in myDataList)
		{
			Debug.Log ("Key: " + e.Key);
			Debug.Log ("Text: " + e.Text);
		}
#endif
		
		MySpreadSheet mySpreadSheet = (MySpreadSheet)base.database;
		mySpreadSheet.dataArray = myDataList.ToArray();
		
		EditorUtility.SetDirty(mySpreadSheet);
		AssetDatabase.SaveAssets();
		
		return true;
	}
}
