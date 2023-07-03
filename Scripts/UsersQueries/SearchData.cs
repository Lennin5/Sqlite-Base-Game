using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// References
using Mono.Data.Sqlite;
using TMPro;

public class SearchData : MonoBehaviour
{
    private string conn;
    IDbConnection dbconn;
    public TMP_InputField txtBoxName;
    public TMP_Text dataResult;

    // Start is called before the first frame update
    void Start()
    {
        // Empty
    }

    public void SearchUserButton()
    {
        // Get input name
        string name = txtBoxName.text;

        // Call search function and pass the name
        SearchUser(name);
    }

    private void SearchUser(string name)
    {
        // Get InitializeDB script
        GameObject initializeDBManagerObject = GameObject.Find("MainCamera");
        InitializeDB initializeDBScript = initializeDBManagerObject.GetComponent<InitializeDB>();
        // Get database name and path
        string DatabaseName = initializeDBScript.DatabaseName;
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Open db connection
        conn = "URI=file:" + filePathWindows;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Search data
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = string.Format("SELECT id, name, age, level_id FROM users WHERE name LIKE '%{0}%'", name);
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dataResult.text = "";
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string userName = reader.GetString(1);
                int age = reader.GetInt32(2);
                int levelId = reader.GetInt32(3);
                dataResult.text += id + ": " + userName + " - " + age + " - " + levelId + "\n";
            }
            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();

            Debug.Log("Data search completed!");
        }
        catch (System.Exception e)
        {
            Debug.Log("Error when searching data: " + e.Message);
        }
    }
}
