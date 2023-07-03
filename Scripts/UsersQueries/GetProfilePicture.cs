using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class GetProfilePicture : MonoBehaviour
{
    public Image profileImage;

    // Start is called before the first frame update
    void Start()
    {
        // Get InitializeDB script
        GameObject initializeDBManagerObject = GameObject.Find("MainCamera");
        InitializeDB initializeDBScript = initializeDBManagerObject.GetComponent<InitializeDB>();
        // Get database name and path
        string DatabaseName = initializeDBScript.DatabaseName;
        string filePathWindows = Application.dataPath + "/Plugins/" + DatabaseName;
        string filePathAndroid = Application.persistentDataPath + "/" + DatabaseName;

        // Open db connection
        string conn = "URI=file:" + filePathWindows;
        IDbConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        try
        {
            // Perform query
            IDbCommand dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = "SELECT profile_image FROM users WHERE id=1";
            IDataReader reader = dbcmd.ExecuteReader();

            if (reader.Read())
            {
                string imagePath = reader.GetString(0);

                // Load image from Resources
                Sprite loadedImage = Resources.Load<Sprite>(imagePath);

                if (loadedImage != null)
                {
                    // Set the loaded image to the profileImage component
                    profileImage.sprite = loadedImage;
                    Debug.Log("Profile picture loaded successfully!");
                }
                else
                {
                    Debug.LogWarning("Failed to load profile picture. Image not found: " + imagePath);
                }
            }
            else
            {
                Debug.LogWarning("No profile picture found for user with ID 1");
            }

            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error when querying profile picture: " + e.Message);
        }
    }
}