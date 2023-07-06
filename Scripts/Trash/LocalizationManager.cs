using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public string currentLanguage;
    // create a dictionary to store the translations
    public Dictionary<string, Dictionary<string, string>> translations;

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

    private void LoadTranslations()
    {

        string filePath = Path.Combine(Application.streamingAssetsPath, "translations.json");


        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonText);


            // print greeting value from english language
            Debug.Log("Greeting: " + translations["en"]["greeting"]);

            currentLanguage = "es";            
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
