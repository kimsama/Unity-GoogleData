using UnityEngine;
using UnityEditor;

using System.Collections;

/// <summary>
/// Editor script class for GoogleDataSettings scriptable object to hide passsword of google account.
/// </summary>
[CustomEditor(typeof(GoogleDataSettings))]
public class GoogleDataSettingsEditor : Editor 
{
	
	GoogleDataSettings googleData;
	
	public void OnEnable()
	{
		googleData = target as GoogleDataSettings;
	}
	
	public override void OnInspectorGUI()
	{		
		GUILayout.Label("GoogleSpreadsheet Settings");
		
		// path and asset file name which contains a google account and password.
		GoogleDataSettings.kBuildSettingsPath = GUILayout.TextField(GoogleDataSettings.kBuildSettingsPath, 120);
		GoogleDataSettings.assetFileName = GUILayout.TextField(GoogleDataSettings.assetFileName, 120);
		
		// account and passwords setting, this should be specified before you're trying to connect a google spreadsheet.
		googleData.Account = GUILayout.TextField(googleData.Account, 100);
		googleData.Password = GUILayout.PasswordField (googleData.Password, "*"[0], 25);
	}
}
