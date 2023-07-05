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

    private string conn;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;

    public string DatabaseName = "MySqliteDB.s3db";

    // Start is called before the first frame update
    void Start()
    {
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Initialize database with device type. Now only works for: Windows/Android
        InitializeSqlite("Windows", filePathWindows);
        //InitializeSqlite("Android", filePathAndroid);
    }

    private void InitializeSqlite(string deviceType, string filepath)
    {
        // If not exist database, create it with name DatabaseName
        if (!File.Exists(filepath))
        {
            // If not found will create database

            //Debug.LogWarning("File \"" + DatabaseName + "\" doesn't exist. " +
            //    "Creating new from \"" + filepath);

            string url = Path.Combine(Application.streamingAssetsPath, DatabaseName);
            UnityWebRequest loadDB = UnityWebRequest.Get(url);
            loadDB.SendWebRequest();
            Debug.Log("Database created successfully!");

        }

        CreateTables(filepath, deviceType);
    }

    private void CreateTables(string filepath, string deviceType)
    {
        // Open db connection
        conn = "URI=file:" + filepath;
        //Debug.Log("Stablishing " + deviceType + " connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        // Create tables if not exist
        string userQuery, levelsQuery;        
        levelsQuery = "CREATE TABLE IF NOT EXISTS levels (id INTEGER PRIMARY KEY AUTOINCREMENT, level_name varchar(50), score INT)";
        userQuery = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(100), age INT, level_id INT, profile_image TEXT, FOREIGN KEY (level_id) REFERENCES levels(id))";

        // Create a list of commands to execute
        List<string> commands = new List<string>();
        commands.Add(userQuery);
        commands.Add(levelsQuery);
        try
        {
            // Variable to control if insert default data or not when creating tables first time
            bool insertData = true; 
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
                    //Debug.LogWarning("Table " + tableName + " already exists");
                    insertData = false;
                }
                else
                {
                    // If table doesn't exist, create it
                    reader.Close();
                    dbcmd.CommandText = command;
                    reader = dbcmd.ExecuteReader();
                    Debug.Log("Table " + tableName + " created successfully!");

                    // If tableName is the last item in commands list, proceed to insert default data
                    if (tableName == commands[commands.Count - 1].Split(' ')[5])
                    {
                        if (insertData)
                        {
                            // Call method to insert default data
                            InsertDefaultData();
                            insertData = false;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error when creating table: " + e.Message);           
        }

        //Close db connection
        dbconn.Close();
        //Debug.Log("Closed connection to database.");
    }

    private void InsertDefaultData()
    {
        try
        {
            string insertLevelQuery, insertUserQuery;
            insertLevelQuery = "INSERT INTO levels (level_name, score) VALUES ('Level 1', 10), ('Level 2', 20), ('Level 3', 30), ('Level 4', 30), ('Level 5', 50)";
            insertUserQuery = "INSERT INTO users (name, age, level_id, profile_image) VALUES ('Lennin', 23, 1, 'Sprites/Lawliett')";
            // Create a list of commands to execute
            List<string> commandsToInsert = new List<string>();
            commandsToInsert.Add(insertLevelQuery);
            commandsToInsert.Add(insertUserQuery);

            foreach (string command in commandsToInsert)
            {
                // Table name extracted from command query
                string tableName = command.Split(' ')[2];
                // Execute command
                dbcmd = dbconn.CreateCommand();
                dbcmd.CommandText = command;
                reader = dbcmd.ExecuteReader();
                Debug.Log("Default data inserted on table " + tableName + " successfully!");
            }
        }
        catch (Exception)
        {
            Debug.LogWarning("Error when inserting default data");
        }

    }

}
