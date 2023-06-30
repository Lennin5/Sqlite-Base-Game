using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using TMPro;
using System.Data;
using System.Net;
using System.Xml.Linq;
using System;

public class UpdateData : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;

    public TMP_InputField txtBoxId, txtBoxName;

    // Start is called before the first frame update
    void Start()
    {
        // Empty
    }

    public void UpdateUserButton()
    {
        // Set values to insert
        int id = int.Parse(txtBoxId.text);
        string name = txtBoxName.text;

        // Call insert function and pass values
        UpdateUser(id,name);
    }
    private void UpdateUser(int id, string name)
    {
        // Get InitializeDB script
        GameObject initializeDBManagerObject = GameObject.Find("MainCamera");
        InitializeDB initializeDBScript = initializeDBManagerObject.GetComponent<InitializeDB>();
        // Get database name and path
        string DatabaseName = initializeDBScript.DatabaseName;
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Get ReadData script
        GameObject readDataManagerObject = GameObject.Find("ScrollViewData");
        ReadData readDataScript = readDataManagerObject.GetComponent<ReadData>();

        // Open db connection
        conn = "URI=file:" + filePathWindows;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Update data
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("UPDATE users SET name = \"{0}\" WHERE id = \"{1}\"", name, id);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();

            // Call read users function and pass filepath
            readDataScript.LoadReadUsers(filePathWindows);

            // Close db connection
            dbcmd.Dispose();
            dbconn.Close();
            Debug.Log("Data updated successfully!");
        }
        catch (Exception e)
        {
            Debug.Log("Error when updating data: " + e.Message);
        }
    }
}
