using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public MySpreadSheet spreadSheet;
	
	// Use this for initialization
	void Start () {
	
		Debug.Log ("Start to get data from MySpreadSheet.");
		Debug.Log ("Num Items: " + spreadSheet.dataArray.Length.ToString());
		
		foreach(MyData s in spreadSheet.dataArray)
		{
			Debug.Log ("Key: " + s.Key);
			Debug.Log ("Text:  " + s.Text);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		
	}
}
