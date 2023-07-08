using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;

public class TranslationsSystem : MonoBehaviour
{
    // Translations instance, dictionary and current language
    public static TranslationsSystem Instance { get; private set; }
    public Dictionary<string, Dictionary<string, string>> translations;
    public string currentLanguage;

    // Database connection
    private string conn;
    IDbConnection dbconn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);                  
        }   
        else
        {
            Destroy(gameObject);
        }
    }
    
    // This isn't efficient, but it works
    //private void Update()
    //{
    //    LoadTranslations();
    //}

    private void Start()
    {
        RefreshAllTranslations();
    }

    public void RefreshAllTranslations()
    {
        LoadTranslations();
    }

    private void LoadTranslations()
    {
        string filePathJson = Path.Combine(Application.streamingAssetsPath, "Translations.json");
        //string filePathJson = Path.Combine(Application.dataPath + "/Resources/", "Translations2.json"); // Another way to load json file in "Resources" folder

        if (File.Exists(filePathJson))
        {
            string jsonText = File.ReadAllText(filePathJson);
            translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonText);

            // print greeting value from english language
            //Debug.Log("Greeting: " + translations["en"]["txtListUsers"]);

            string filePath = InitializeDB.Instance.CurrentDatabasePath;

            // Open db connection
            conn = "URI=file:" + filePath;
            dbconn = new SqliteConnection(conn);
            dbconn.Open();

            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open(); // Open connection to the database.
                IDbCommand dbcmd = dbconn.CreateCommand();
                string sqlQuery = "SELECT language FROM users where id=1";
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();

                if (reader.Read())
                {
                    string language = reader.GetString(0);
                    SetLanguage(language);
                }
                else
                {
                    Debug.LogWarning("No language data found in the database.");
                }

                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
            }
        }
        else
        {
            Debug.LogError("Translations file not found!");
        }
    }

    public void SetLanguage(string language)
    {
        if (translations.ContainsKey(language))
        {
            currentLanguage = language;
            // Aquí puedes notificar a otros objetos que el idioma ha cambiado, para que actualicen sus textos
            Debug.Log("<color=aqua>Language changed to: " + language + "</color>");
            // Add text to button
            TranslatedTerms.Instance.UpdateTranlatedTerms();
        }
        else
        {
            Debug.LogWarning("Language not found: " + language);
        }
    }

    public string GetTranslation(string key)
    {
        try
        {
            if (translations.ContainsKey(currentLanguage) && translations[currentLanguage].ContainsKey(key))
            {
                return translations[currentLanguage][key];
            }
            else
            {
                Debug.LogWarning("Translation not found for key: " + key);
                return string.Empty;
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Translation not found for key: " + key);
            throw;
        }
    }
}