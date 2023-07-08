using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslatedTerms : MonoBehaviour
{
    public static TranslatedTerms Instance { get; private set; }
    public TMP_Text txtListUsers;
    public TMP_Text txtButtonLanguage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Empty...
    }    

    public void UpdateTranlatedTerms()
    {
        txtListUsers.text = TranslationsSystem.Instance.GetTranslation("txtListUsers");
        txtButtonLanguage.text = TranslationsSystem.Instance.GetTranslation("txtButtonLanguage");
    }
}
