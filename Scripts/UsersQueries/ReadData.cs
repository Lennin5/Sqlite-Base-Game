using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using TMPro;

public class ReadData : MonoBehaviour
{
    private string conn;
    IDbConnection dbconn;    

    public TMP_Text dataResult;


    // Start is called before the first frame update
    void Start()
    {
        // Get GameObject and InitializeDBScript
        GameObject databaseManagerObject = GameObject.Find("MainCamera");
        InitializeDB initializeDBScript = databaseManagerObject.GetComponent<InitializeDB>();

        // Get database name
        string DatabaseName = initializeDBScript.DatabaseName;

        // Path to database        
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Call read function and pass filepath
        LoadReadUsers(filePathWindows);
    }

    public void LoadReadUsers(string filePath)
    {
        ReadUsers(filePath);
    }

    private void ReadUsers(string filepath)
    {
        // Open db connection
        
        conn = "URI=file:" + filepath;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        // int idreaders ;
        string NameRecord;
        int IdRecord, AgeRecord, LevelIdRecord;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT id, name, age, level_id FROM users";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dataResult.text = "";
            while (reader.Read())
            {
                IdRecord = reader.GetInt32(0);
                NameRecord = reader.GetString(1);
                AgeRecord = reader.GetInt32(2);
                LevelIdRecord = reader.GetInt32(3);                
                dataResult.text += IdRecord + ": " + NameRecord + " - " + AgeRecord + " - " + LevelIdRecord + "\n";
                //Debug.Log(" name =" + NameRecord + "Address=" + AgeRecord);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
        }
    }
}
