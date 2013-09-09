using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Google.GData.Client;
using Google.GData.Spreadsheets;

public class BaseEditor<T> : Editor where T : BaseDatabase
{
    // google data API
    protected GoogleSpreadsheet google;	
    protected string googleProjectName;
    protected bool useOAuth = false;
	
    protected string username;
    protected string password;
	
    protected bool waitingForAccessCode = false;
    protected string accessCode;
	
    protected bool authenticated = false;

    // custom data 
    protected BaseDatabase database; 
	
	// property draw
	protected PropertyField[] databaseFields;
	protected PropertyField[] dataFields;
	
	protected List<PropertyField[]> pInfoList = new List<PropertyField[]>();
		
	public virtual void OnEnable()
	{
		GoogleDataSettings settings = GoogleDataSettings.Instance;		
		if (settings != null)
		{
			googleProjectName = settings.GoogleProjectName;
			username = settings.Account;
			password = settings.Password;
		}
		else
		{
			Debug.LogError("Failed to get google data settings. See the google data setting if it has correct path.");
			return;
		}
				
		database = target as BaseDatabase;
		Debug.Log ("Target type: " + database.GetType().ToString());
	}

    public override void OnInspectorGUI()
    { 		
        ShowAuthenticastion();
		
		if (target == null)
			return;
		
		//this.DrawDefaultInspector();
		ExposeProperties.Expose(databaseFields);
 
		foreach(PropertyField[] p in pInfoList)
		{
			ExposeProperties.Expose( p );	
		}

    }
	
	/// 
	/// Should be reimplemented in derived class.
	/// 
	public virtual bool Load()
	{
		return false;
	}
	
  	protected List<int> SetArrayValue(string from)
  	{
		List<int> tmp = new List<int>();

    	CsvParser parser = new CsvParser(from);

    	foreach(string s in parser)
    	{
    		Debug.Log("parsed value: " + s);
    		tmp.Add(int.Parse(s));
    	}

    	return tmp;
  	}	

    void ShowAuthenticastion()
    {
        if (google == null)
        {
            // Get the name of the google project.
            googleProjectName = EditorGUILayout.TextField("Google Project Name", googleProjectName);
            
            // Use OAuth or not?
            useOAuth = EditorGUILayout.Toggle("Use OAuth?", useOAuth);
            
            // Display authentication options depending on authentication method.
            if(!useOAuth)
            {
                username = EditorGUILayout.TextField("Username", username);
				password = EditorGUILayout.PasswordField("Password", password);
            }
                
            if(GUILayout.Button("Connect"))
            {
				if (string.IsNullOrEmpty(database.sheetName))
				{
					Debug.LogError("Invalid Google Spreadsheet name which is null or empty.");
					return;
				}
                google = new GoogleSpreadsheet(googleProjectName, database.sheetName);
                
                if(useOAuth)
                {
                    google.AuthenticateOAuth2();
                    waitingForAccessCode = true;
                }
                else
                {
                    SecurityCertificatePolicy.Instate();
                    google.AuthenticateClientLogin(username, password);
                    authenticated = true;
                }
            }
        }
        else
        {
            if(waitingForAccessCode)
            {
                accessCode = EditorGUILayout.TextField("Access Code", accessCode);
                if(GUILayout.Button("Submit"))
                {
                    SecurityCertificatePolicy.Instate();
                    google.FinishOAuth2Authentication(accessCode);
                    waitingForAccessCode = false;
                    authenticated = true;
                }
            }
            
            if(GUILayout.Button("Disconnect"))
            {
                google = null;
                authenticated = false;
            }

           if (GUILayout.Button("Download"))
           {
				if (!Load ())
                    Debug.LogError("Failed to Load data from Google.");
           }

           if (GUILayout.Button("Upload"))
           {
				//TODO: upload database to google sprheadsheet.
            	;
           }
        }
    }
	
		
	/// 
	/// Retrieve workseet with the given worksheet name.
	/// 
	protected WorksheetEntry GetWorksheet()
	{		
		WorksheetEntry worksheet = null;
		if (!string.IsNullOrEmpty(database.worksheetName))
				worksheet = google.GetWorksheetByTitle(database.worksheetName);
		else
			Debug.LogError("Invalid Worksheet name: " + database.worksheetName);
		
		return worksheet;
	}
	
	/*
	public static void DrawDefaultInspectors<T>(GUIContent label, T target)
	   //where T : new()
	{
	    EditorGUILayout.Separator();

		Type type = typeof(T);		
		PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public);
		
		//foreach(PropertyInfo p in properties)
		//	Debug.Log (properties.);
	
	    EditorGUI.indentLevel++;
	 
	    foreach(PropertyInfo pInfo in properties)
	    {
			
	        //if(field.IsPublic)
	        //{
				
		         if(pInfo.GetType() == typeof(int))
		         {
		             //field.SetValue(target, EditorGUILayout.IntField(
		             //MakeLabel(field), (int) field.GetValue(target)));
					
					int result = EditorGUILayout.IntField ( MakeLabel(pInfo), (int)pInfo.GetGetMethod().Invoke(target, null));
					pInfo.SetValue(target, Convert.ChangeType (result, pInfo.PropertyType), null);
		         }   
		         else if(pInfo.GetType() == typeof(float))
		         {
		             //field.SetValue(target, EditorGUILayout.FloatField(MakeLabel(field), (float) field.GetValue(target)));
					float result = EditorGUILayout.FloatField ( MakeLabel(pInfo), (float)pInfo.GetGetMethod().Invoke(target, null));
					pInfo.SetValue(target, Convert.ChangeType (result, pInfo.PropertyType), null);					
		             
		         }
		 
		         ///etc. for other primitive types
		 
		         else if(pInfo.GetType().IsClass)
		         {
		             //Type[] parmTypes = new Type[]{ field.FieldType};
				Type[] parmTypes = new Type[]{ pInfo.PropertyType};
		 
		             string methodName = "DrawDefaultInspectors";
		 
		             MethodInfo drawMethod = 
		               typeof(CSEditorGUILayout).GetMethod(methodName);
		 
		             if(drawMethod == null)
		             {
		                 	Debug.LogError("No method found: " + methodName);
		             }
		 
		             bool foldOut = true;
		 
		             drawMethod.MakeGenericMethod(parmTypes).Invoke(null, 
		               	new object[]
		               	{
		                  	MakeLabel(pInfo),
							pInfo.GetGetMethod().Invoke(target, null)
		                  //field.GetValue(target)
		               	});
		         }      
		         else
		         {
		             Debug.LogError(
		               "DrawDefaultInspectors does not support fields of type " +
		               pInfo.PropertyType);//field.FieldType);
		         }
		    //}         
		}
		 
		EditorGUI.indentLevel--;
	}		
	
	//private static GUIContent MakeLabel(FieldInfo field)
	private static GUIContent MakeLabel(PropertyInfo pInfo)
	{
		GUIContent guiContent = new GUIContent();      
		//guiContent.text = field.Name.SplitCamelCase();
		//guiContent.text = 
		guiContent.text = SplitCamelCase(pInfo.Name);
		object[] descriptions = 
		      field.GetCustomAttributes(typeof(DescriptionAttribute), true);
		 
		if(descriptions.Length > 0)
		{
		    //just use the first one.
		    guiContent.tooltip = 
		         (descriptions[0] as DescriptionAttribute).Description;
		}
		 
		return guiContent;
	}
	*/
	
	/*
	static string[] SplitCamelCase(string stringToSplit)
    {
        if (!string.IsNullOrEmpty(stringToSplit))
        {
            List<string> words = new List<string>();

            string temp = string.Empty;
                
            foreach (char ch in stringToSplit)
            {
                if (ch >= 'a' && ch <= 'z')
                    temp = temp + ch;
                else
                {
                    words.Add(temp);
                    temp = string.Empty + ch;
                }
            }
            words.Add(temp);
            return words.ToArray();
        }
        else
            return null;
    }
    */
	
    public static string SplitCamelCase(string inputCamelCaseString)
    {
        string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
        return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
    }	
}
