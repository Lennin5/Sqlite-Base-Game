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
        GameObject databaseManagerObject = GameObject.Find("Main Camera");
        InitializeDB initializeDBScript = databaseManagerObject.GetComponent<InitializeDB>();

        string DatabaseName = initializeDBScript.DatabaseName;

        // Path to database        
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Call read function and pass filepath
        ReadUsers(filePathWindows);
    }

    private void ReadUsers(string filepath)
    {
        // Open db connection
        
        conn = "URI=file:" + filepath;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        // int idreaders ;
        string NameRecord;
        int AgeRecord, LevelIdRecord;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT name, age, level_id FROM users";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dataResult.text = "";
            while (reader.Read())
            {
                // idreaders = reader.GetString(1);
                NameRecord = reader.GetString(0);
                AgeRecord = reader.GetInt32(1);
                LevelIdRecord = reader.GetInt32(2);                
                dataResult.text += NameRecord + " - " + AgeRecord + " - " + LevelIdRecord + "\n";
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
