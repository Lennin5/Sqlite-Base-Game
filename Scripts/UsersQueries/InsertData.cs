using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using TMPro;
using System.Data;
using System.Net;
using System.Xml.Linq;
using System;

public class InsertData : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;

    // Start is called before the first frame update
    void Start()
    {
        // Empty
    }

    public void InsertUserButton()
    {
        // Set values to insert
        string name = "Helena";
        int age = 25;
        int level_id = 1;

        // Call insert function and pass values
        InsertUser(name, age, level_id);
    }
    private void InsertUser(string name, int age, int level_id)
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
            // Insert data
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("INSERT INTO users (name, age, level_id) values (\"{0}\",\"{1}\",\"{2}\")", name, age, level_id);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();

            // Call read users function and pass filepath
            readDataScript.LoadReadUsers(filePathWindows);

            // Close db connection
            dbcmd.Dispose();
            dbconn.Close();
            Debug.Log("Insert Done");
        }
        catch (Exception e)
        {
            Debug.Log("Error when inserting data: " + e.Message);
        }
    }
}
