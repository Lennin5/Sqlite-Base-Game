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
        // Get database path
        string filePath = InitializeDB.Instance.CurrentDatabasePath;

        // Open db connection
        conn = "URI=file:" + filePath;
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Update language value
            dbcmd = dbconn.CreateCommand();
            sqlQuery = "UPDATE users SET language = CASE WHEN language = 'es' THEN 'en' ELSE 'es' END WHERE id = 1";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

            dbcmd.Dispose();
            dbconn.Close();

            // Get TranslationsSystem script
            GameObject translationsSystemManagerObject = GameObject.Find("TranslationsSystem");
            TranslationsSystem translationsSystemScript = translationsSystemManagerObject.GetComponent<TranslationsSystem>();

            translationsSystemScript.RefreshAllTranslations();
        }
        catch (Exception e)
        {
            Debug.Log("Error when updating language: " + e.Message);
        }
    }
}
