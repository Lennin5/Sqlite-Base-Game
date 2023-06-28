using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Net;
using UnityEngine.SceneManagement;
using System.Threading;

public class InitializeDB : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    string DatabaseName = "MySqliteDB.s3db";    

    // Start is called before the first frame update
    void Start()
    {
        // Initialize database with device type
        // Now only works for: Windows/Android
        InitializeSqlite("Windows");
        //InitializeSqlite("Android");
    }

    private void InitializeSqlite(string deviceType)
    {
        // Initialize database with device type
        switch (deviceType)
        {
            case "Windows":
                // Path to database
                string filepath = Application.dataPath + "/Plugins/" + DatabaseName;
                // If not exist database, create it with name DatabaseName
                if (!File.Exists(filepath))
                {
                    // If not found will create database
                    Debug.LogWarning("File \"" + DatabaseName + "\" doesn't exist. Creating new from \"" +
                                        Application.dataPath + "/Plugins/");

                    string url = Path.Combine(Application.streamingAssetsPath, DatabaseName);
                    UnityWebRequest loadDB = UnityWebRequest.Get(url);
                    loadDB.SendWebRequest();
                    Debug.Log("Database created successfully!");
                }                

                CreateTables(filepath, deviceType);
                break;
            case "Android":                
                string filepathAndroid = Application.persistentDataPath + "/" + DatabaseName;
                if (!File.Exists(filepathAndroid))
                {
                    // If not found on android will create Tables and database
                    Debug.LogWarning("File \"" + DatabaseName + "\" doesn't exist. Creating new from \"" +
                                     Application.persistentDataPath);

                    string url = Path.Combine(Application.streamingAssetsPath, DatabaseName);
                    UnityWebRequest loadDB = UnityWebRequest.Get(url);
                    loadDB.SendWebRequest();
                    Debug.Log("Database created successfully!");
                }                

                CreateTables(filepathAndroid, deviceType);
                break;
        }
    }

    private void CreateTables(string filepath, string deviceType)
    {
        // Open db connection
        conn = "URI=file:" + filepath;
        Debug.Log("Stablishing " + deviceType + " connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        // Create tables if not exist
        string userQuery, levelsQuery;
        levelsQuery = "CREATE TABLE IF NOT EXISTS levels (id INTEGER PRIMARY KEY AUTOINCREMENT, level_name varchar(50), score INT)";
        userQuery = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(100), age INT, level_id INT, FOREIGN KEY (level_id) REFERENCES levels(id))";
        // Create a list of commands to execute
        List<string> commands = new List<string>();
        commands.Add(userQuery);
        commands.Add(levelsQuery);
        try
        {
            foreach (string command in commands)
            {
                // Table name extracted from command query
                string tableName = command.Split(' ')[5];
                // Validates if table already exists on database
                dbcmd = dbconn.CreateCommand();
                dbcmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "'";
                reader = dbcmd.ExecuteReader();

                if (reader.Read())
                {
                    // If table already exists, do nothing
                    Debug.LogWarning("Table " + tableName + " already exists");
                }
                else
                {
                    // If table doesn't exist, create it
                    reader.Close();
                    dbcmd.CommandText = command;
                    reader = dbcmd.ExecuteReader();
                    Debug.Log("Table " + tableName + " created successfully!");
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error when creating table: " + e.Message);
        }

        // Close db connection
        //dbconn.Close();
        //Debug.Log("Closed connection to database.");
    }

}
