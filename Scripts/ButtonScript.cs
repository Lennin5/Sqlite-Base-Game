using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public TMP_Text textComponent;

    private void Start()
    {
        UpdateText();
    }

    public void OnButtonClick()
    {
        // Get InitializeDB script
        GameObject localizationManagerObject = GameObject.Find("LocalizationManager");
        LocalizationManager locationManagerScript = localizationManagerObject.GetComponent<LocalizationManager>();
        string currentLanguage = locationManagerScript.currentLanguage;
        Debug.Log("Current language: " + currentLanguage);

        if (currentLanguage == "en")
        {
            LocalizationManager.Instance.SetLanguage("es"); // Cambia el idioma a español
        }
        else
        {
            LocalizationManager.Instance.SetLanguage("en"); // Cambia el idioma a inglés
        }

        UpdateText();
    }

    private void UpdateText()
    {
        string buttonText = LocalizationManager.Instance.GetTranslation("greeting");
        textComponent.text = buttonText;
    }
}
