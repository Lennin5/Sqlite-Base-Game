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
        string language = "es";
        string image_path = "Sprites/Lawliett";

        // Call insert function and pass values
        InsertUser(name, age, level_id, language, image_path);
    }
    private void InsertUser(string name, int age, int level_id, string language, string image_path)
    {

        // Get database path       
        string filePath = InitializeDB.Instance.CurrentDatabasePath;

        // Open db connection
        conn = "URI=file:" + filePath;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Insert data
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("INSERT INTO users (name, age, level_id, language, image_path) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")", name, age, level_id, language, image_path);
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();

            // Call read users function and pass filepath
            ReadData.Instance.LoadReadUsers(filePath);

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
