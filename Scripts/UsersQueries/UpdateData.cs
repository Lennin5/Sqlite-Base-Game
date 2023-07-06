using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using TMPro;

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
        // Get database paths
        string filePath = InitializeDB.Instance.CurrentDatabasePath;

        // Open db connection
        conn = "URI=file:" + filePath;
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
            ReadData.Instance.LoadReadUsers(filePath);

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
