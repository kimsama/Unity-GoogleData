using UnityEngine;
using UnityEditor;
using System.Collections;

//[InitializeOnLoad]
public class GoogleDataSettings : ScriptableObject 
{
	[SerializeField]
	public string kBuildSettingsPath = "Assets/Script/Data/Editor/";
	[SerializeField]
	public string assetFileName = "GoogleDataSettings.asset";
	
	public static string filePath;
	
	[SerializeField]
	string googleProjectName = "Google Project Name";
	//[SerializeField]
	//string sheetName = "SpreadSheet Name";
	[SerializeField]
	string account = "account@gmail.com"; // your google acccount.
	[SerializeField]
	string password = "";	
	
	void OnEnable()
	{
		filePath = kBuildSettingsPath + assetFileName;
	}
	
	static GoogleDataSettings s_Instance;	
	public static GoogleDataSettings Instance
	{
		get
		{
			if (s_Instance == null)
			{
				
				s_Instance = (GoogleDataSettings)AssetDatabase.LoadAssetAtPath (filePath, typeof (GoogleDataSettings));
				
				/* if crashes at the startup of Unity editor.
				if (s_Instance == null)
				{
					s_Instance = CreateInstance<GoogleDataSettings> ();

					AssetDatabase.CreateAsset (s_Instance, kBuildSettingsPath);

					Selection.activeObject = s_Instance;
					EditorUtility.DisplayDialog (
						"Validate Settings",
						"Default google dasa settings have been created for accessing Google project page. You should validate these before proceeding.",
						"OK"
					);
				}
				*/
			}

			return s_Instance;
		}
	}
	
	[MenuItem("Assets/Create/GoogleDataSetting")]
	public static void CreateGoogleDataSetting()
	{		
		GoogleDataSettings.Create();
	}
	
	public static GoogleDataSettings Create()
	{
		s_Instance = (GoogleDataSettings)AssetDatabase.LoadAssetAtPath (filePath, typeof (GoogleDataSettings));
						
		if (s_Instance == null)
		{
			s_Instance = CreateInstance<GoogleDataSettings> ();

			AssetDatabase.CreateAsset (s_Instance, filePath);

			Selection.activeObject = s_Instance;
			
			EditorUtility.DisplayDialog (
				"Validate Settings",
				"Default google dasa settings have been created for accessing Google project page. You should validate these before proceeding.",
				"OK"
			);
		}
		
		return s_Instance;
	}
	
	/*
	static GoogleDataSettings ()
	{
		if (Instance == null)
		{
			Debug.LogError ("Failed to create google data settings");
		}
	}
	*/	
		
	[MenuItem ("Edit/Project Settings/Google Data Settings")]
	public static void Edit ()
	{
		Selection.activeObject = Instance;
	}

	public  string GoogleProjectName
	{
		get 
		{
			return Instance.googleProjectName;
		}
		set
		{
			if (Instance.googleProjectName != value)
			{
				Instance.googleProjectName = value;
				EditorUtility.SetDirty(Instance);
			}
		}
	}
	
	/*
	public static string SheetName
	{
		get 
		{
			return Instance.sheetName;
		}
		set
		{
			if (Instance.sheetName != value)
			{
				Instance.sheetName = value;
				EditorUtility.SetDirty(Instance);
			}
		}
	}
	*/
	
	public  string Account
	{
		get 
		{
			return Instance.account;
		}
		set
		{
			if (Instance.account != value)
			{
				Instance.account = value;
				EditorUtility.SetDirty(Instance);
			}
		}
	}	

	public  string Password
	{
		get 
		{
			return Instance.password;
		}
		set
		{
			if (Instance.password != value)
			{
				Instance.password = value;
				EditorUtility.SetDirty(Instance);
			}
		}
	}	
}
