using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Google.GData.Client;
using Google.GData.Spreadsheets;


public class WorkSheetData
{
	public string title;
	public uint numRow;
	public uint numCol;
}

public class GoogleSpreadsheet 
{	
	// The project name (from Google Console).
	private string projectName;
	
	// The name of the sheet.
	private string spreadsheetName;
	
	// The spreadsheet service.
	public SpreadsheetsService service;
	
	// The spreadsheet being worked on.
	public SpreadsheetEntry sheet;
	
	//*******************************************
	//* OAUTH VARIABLES
	//*******************************************
	private OAuth2Parameters oAuthParams;
	
	// OAuth2.0 info.
	/*
	private const string CLIENT_ID = "xxxxxxxxxxxxxxxx.apps.googleusercontent.com";
	private const string CLIENT_SECRET = "PUT_SECRET_HERE";
	private const string REDIRECT_URI = "PUT_REDIRECT_HERE";
	*/
	private const string CLIENT_ID = "300118700711.apps.googleusercontent.com";
	private const string CLIENT_SECRET = "JKxzGDSjXm7ZjQGmOoZ0UoDC";
	private const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";	
	private const string SCOPE = "https://spreadsheets.google.com/feeds/ https://docs.google.com/feeds/";
	

	public List<WorkSheetData> wsDataList = new List<WorkSheetData>();
	
	//*******************************************
	//* CONSTRUCTOR
	//*******************************************
	public GoogleSpreadsheet(string apiProjectName, string sheetName) 
	{
		projectName = apiProjectName;
		spreadsheetName = sheetName;
	}
	
	//*******************************************
	//* PUBLIC METHODS
	//*******************************************
	public void AuthenticateClientLogin(string pUsername, string pPassword) 
	{
		// Create the service.
		service = new SpreadsheetsService(projectName);
		
		// Set the user's credentials.
		service.setUserCredentials(pUsername, pPassword);
		
		// Get the spreadsheet.
		Connect();
	}
	
	public void AuthenticateOAuth2() 
	{
		// Create the service.
		service = new SpreadsheetsService(projectName);
		
		// Create OAuth2 Parameters.
		oAuthParams = new OAuth2Parameters();
		oAuthParams.ClientId = CLIENT_ID;
		oAuthParams.ClientSecret = CLIENT_SECRET;
		oAuthParams.RedirectUri = REDIRECT_URI;
		oAuthParams.Scope = SCOPE;
		
		// Generate a authentication URL.
		string authUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(oAuthParams);
		
		// Open the URL in the browser.
		Application.OpenURL(authUrl);
	}
	
	public void FinishOAuth2Authentication(string accessCode) 
	{
		// Save access code.
		oAuthParams.AccessCode = accessCode;
		
		// Get access token.
		OAuthUtil.GetAccessToken(oAuthParams);
		
		// Save data to make authorized requests.
		GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, projectName, oAuthParams);
		service.RequestFactory = requestFactory;
		
		// Connect to the spreadsheet.
		Connect();
	}
	
	public string GetCell(int x, int y, string defaultValue)
	{
		// Assume working with first worksheet.
		WorksheetEntry worksheet = (WorksheetEntry)sheet.Worksheets.Entries[0];
		
		// Create a query for the requested cell.
		CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
		cellQuery.MinimumColumn = (uint)x;
		cellQuery.MaximumColumn = (uint)x;
		cellQuery.MinimumRow = (uint)y;
		cellQuery.MaximumRow = (uint)y;
		
		// Get cells meeting the query.
		CellFeed cellFeed = service.Query(cellQuery);
		
		if(cellFeed.Entries.Count > 0)
		{
			return (cellFeed.Entries[0] as CellEntry).InputValue;
		}
		
		return defaultValue;
	}
	
	public string GetCell(WorksheetEntry worksheet, int x, int y, string defaultValue)
	{
		// Assume working with first worksheet.
		//WorksheetEntry worksheet = (WorksheetEntry)sheet.Worksheets.Entries[0];
		
		// Create a query for the requested cell.
		CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
		cellQuery.MinimumColumn = (uint)x;
		cellQuery.MaximumColumn = (uint)x;
		cellQuery.MinimumRow = (uint)y;
		cellQuery.MaximumRow = (uint)y;
		
		// Get cells meeting the query.
		CellFeed cellFeed = service.Query(cellQuery);
		
		if(cellFeed.Entries.Count > 0)
		{
			return (cellFeed.Entries[0] as CellEntry).InputValue;
		}
		
		return defaultValue;
	}	
	
	public void SetCell(int x, int y, string stringValue)
	{
		// Assume working with first worksheet.
		WorksheetEntry worksheet = (WorksheetEntry)sheet.Worksheets.Entries[0];
		
		// Create a query for the requested cell.
		CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
		cellQuery.MinimumColumn = (uint)x;
		cellQuery.MaximumColumn = (uint)x;
		cellQuery.MinimumRow = (uint)y;
		cellQuery.MaximumRow = (uint)y;
		
		// Get cells meeting the query.
		CellFeed cellFeed = service.Query(cellQuery);
		
		// Update the cell.
		foreach(CellEntry cellEntry in cellFeed.Entries)
		{
			Debug.Log("Updating cell " + cellEntry.Title.Text);
			cellEntry.InputValue = stringValue;
			cellEntry.Update();
		}
	}
	
	//*******************************************
	//* PRIVATE METHODS
	//*******************************************
	private bool Connect() 
	{
		// Create query to get all spreadsheets.
		SpreadsheetQuery getSpreadsheetsQuery = new SpreadsheetQuery();
		
		// Call to API to get all spreadsheets.
		SpreadsheetFeed spreadsheets = service.Query(getSpreadsheetsQuery);
		
		// Iterate through returned spreadsheets and find sheet of interest.
		foreach(SpreadsheetEntry spreadsheet in spreadsheets.Entries)
		{ 
			if(string.Compare(spreadsheet.Title.Text, spreadsheetName) == 0)
			{
				sheet = spreadsheet;

				QueryAllWorksheets(sheet);
				
				return true;
			}
		}
		
		// Sheet not found.
		Debug.LogError("Could not find spreadsheet named " + spreadsheetName);
		return false;
	}

	private void QueryAllWorksheets(SpreadsheetEntry sheet)
	{
		WorksheetFeed  wsFeed = sheet.Worksheets;

		foreach(WorksheetEntry entry in wsFeed.Entries)
		{
			WorkSheetData wsData = new WorkSheetData();
			wsData.title = entry.Title.Text;
			wsData.numRow = entry.Rows;
			wsData.numCol = entry.Cols;

			wsDataList.Add(wsData);
		}
	}
	
	public WorksheetEntry GetWorksheetByTitle(string title)
	{		
		if (sheet == null || service == null)
			return null;
		
		AtomLink link = this.sheet.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
		
		WorksheetQuery query = new WorksheetQuery(link.HRef.ToString());
		WorksheetFeed feed = this.service.Query(query);
		
		foreach(WorksheetEntry worksheet in feed.Entries)
		{		
			if (worksheet.Title.Text == title)
				return worksheet;
		}
				
		return null;
	}
	
}
