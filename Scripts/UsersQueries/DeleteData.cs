using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// References
using Mono.Data.Sqlite;
using TMPro;
using System.Data;
using System.Net;
using System.Xml.Linq;
using System;

public class DeleteData : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;

    public TMP_InputField txtBoxId;

    // Start is called before the first frame update
    void Start()
    {
        // Empty
    }

    public void DeleteUserButton()
    {
        // Set value to delete
        int id = int.Parse(txtBoxId.text);

        // Call delete function and pass the ID
        DeleteUser(id);
    }

    private void DeleteUser(int id)
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
            // Delete data
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("DELETE FROM users WHERE id = \"{0}\"", id);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();

            // Call read users function and pass filepath
            readDataScript.LoadReadUsers(filePathWindows);

            // Close db connection
            dbcmd.Dispose();
            dbconn.Close();
            Debug.Log("Data deleted successfully!");
        }
        catch (Exception e)
        {
            Debug.Log("Error when deleting data: " + e.Message);
        }
    }
}