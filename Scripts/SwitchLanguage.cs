using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//References
using Mono.Data.Sqlite;
using System.Data;
using System;

public class SwitchLanguage : MonoBehaviour
{
    private string conn, sqlQuery;
    private IDbConnection dbconn;
    private IDbCommand dbcmd;

    public void UpdateLanguageButton()
    {
        // Get InitializeDB script
        GameObject initializeDBManagerObject = GameObject.Find("MainCamera");
        InitializeDB initializeDBScript = initializeDBManagerObject.GetComponent<InitializeDB>();
        // Get database name and path
        string DatabaseName = initializeDBScript.DatabaseName;
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;

        // Open db connection
        conn = "URI=file:" + filePathWindows;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Update language value
            dbcmd = dbconn.CreateCommand();
            sqlQuery = "UPDATE users SET language = CASE WHEN language = 'es' THEN 'en' ELSE 'es' END";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            dbcmd.Dispose();
            dbconn.Close();
            Debug.Log("Language Updated");

            // Get TranslationsSystem script
            GameObject translationsSystemManagerObject = GameObject.Find("TranslationsSystem");
            TranslationsSystem translationsSystemScript = translationsSystemManagerObject.GetComponent<TranslationsSystem>();

            translationsSystemScript.RefreshAll();
        }
        catch (Exception e)
        {
            Debug.Log("Error when updating language: " + e.Message);
        }
    }
}
