using UnityEngine;
using UnityEditor;
using System.IO;

public static class GoogleDataAssetUtility
{
	[MenuItem("Assets/Create/GoogleData/MySpreadSheet")]
	public static void CreateGoogleDataTestSphreadSheetAsset()
	{
		CustomAssetUtility.CreateAsset<MySpreadSheet>();
	}
	
}