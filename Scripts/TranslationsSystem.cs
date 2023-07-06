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
    public string currentLanguage = "en";

    public TMP_Text txtBtnLanguage;

    // Database connection
    private string conn;
    IDbConnection dbconn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTranslations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Update()
    //{
    //    LoadTranslations();
    //}

    public void RefreshAll()
    {
        LoadTranslations();
    }

    private void LoadTranslations()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "translations.json");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonText);

            // print greeting value from english language
            //Debug.Log("Greeting: " + translations["en"]["greeting"]);

            // Get GameObject and InitializeDBScript
            GameObject databaseManagerObject = GameObject.Find("MainCamera");
            InitializeDB initializeDBScript = databaseManagerObject.GetComponent<InitializeDB>();

            // Get database name
            string DatabaseName = initializeDBScript.DatabaseName;

            // Path to database        
            string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
            string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

            // Open db connection
            conn = "URI=file:" + filePathWindows;
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
            Debug.Log("Language changed to: " + language);
            // Add text to button
            txtBtnLanguage.text = translations[currentLanguage]["txtButtonLanguage"];
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