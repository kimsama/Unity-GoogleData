Unity-GoogleData
================

Unity-GoogleData enables to use google spreadsheet data within Unity editor. With Unity-GoogleData, you can retrieve data from google spreadsheet or upload any changed data back to google spreadsheet. 


Features
--------
* It can create an empty google spreadsheet.
* It can retrieve data from google spreadsheet.
* It can upload data back to google spreadsheet.
* No need to parse any retrieved data, it automatically serializes retrieved data to Unity3D's ScriptableObject.


Usage
-----


### Setting a Google Project

### Creating a Google SpreadSheet

First, you need to create a google spreadsheet. Login google drive with your google account and create a new spreadsheet.


Change the title of the created spreadsheet as 'MySpreadSheet' like the following:

![Create a google spreadsheet](./images/gdata_title.png "Google Spreadsheet")

Next, create a new worksheet and rename it to whatever you want to as the following image shows:

![Create a worksheet](./images/gdata_worksheet.png "Google Worksheet")

Now, it needs to edit cells for spreadsheet. Insert 'Key' and 'Text' at the first row of the created worksheet as like that:

![Edit cells](./images/gdata_cells.png)

Note that the first row should not contain any values which are used for your class members.

### Setting GoogleDataSetting Unity3D asset

### Creating Unity3D ScriptableObject class



References
----------
* [GDataDB](https://github.com/mausch/GDataDB) is used to retrieve data from Google Spreadsheet.
* [ExposeProperties](http://wiki.unity3d.com/index.php/Expose_properties_in_inspector) is used to easily expose variables of spreadsheet on the Unity3D's inspector view and let [GDataDB](https://github.com/mausch/GDataDB) access through get/set accessors.


License
-------

This code is distributed under the terms and conditions of the MIT license.

Copyright (c) 2013 Kim, Hyoun Woo