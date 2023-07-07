using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using TMPro;

public class ReadData : MonoBehaviour
{
    public static ReadData Instance { get; private set; }
    private string conn;
    IDbConnection dbconn;    

    public TMP_Text dataResult;

    private void Awake()
    {
        // Assign 'this' when Instance method or variable is needed in another script
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // (Another way to get script from another GameObject)
        //// Get GameObject and InitializeDBScript
        //GameObject databaseManagerObject = GameObject.Find("MainCamera");
        //InitializeDB initializeDBScript = databaseManagerObject.GetComponent<InitializeDB>();

        //// Get database name
        //string DatabaseName = initializeDBScript.DatabaseName;

        //// Path to database        
        //string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        //string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        string filePath = InitializeDB.Instance.CurrentDatabasePath;

        // Call read function and pass filepath
        LoadReadUsers(filePath);
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

        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT id, name, age, level_id, language, image_path FROM users";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dataResult.text = "";
            while (reader.Read())
            {
                int IdRecord = reader.GetInt32(0);
                string NameRecord = reader.GetString(1);
                int AgeRecord = reader.GetInt32(2);
                int LevelIdRecord = reader.GetInt32(3);
                string LanguageRecord = reader.GetString(4);
                string ImagePathRecord = reader.GetString(5);
                dataResult.text += IdRecord + ": " + NameRecord + " - " + AgeRecord + " - " + LevelIdRecord + " - " + LanguageRecord + " - " + ImagePathRecord + "\n";
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
