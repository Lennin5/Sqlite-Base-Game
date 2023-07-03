using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{
    public Image imageComponent;

    void Start()
    {
        // Recordar que antes de usar este código convertir el Testure Type a Sprite (2D and UI)

        // Cargar la imagen desde Assets/Sprites/Lawliett.jpg 
        string imagePath = "Sprites/Lawliett"; // This is the reference on Assets/Resources/Sprites/Lawliett.jpg
        //string imagePath = Application.dataPath + "/Sprites/Lennin"; // This ir the reference on Assets/Resources/Sprites/Lawliett.jpg (NOT WORKING)

        // get image from local storage
        Sprite loadedImage = Resources.Load<Sprite>(imagePath);

        try
        {            
            imageComponent.name = "Lawliett change name";
            //imageComponent.color = Color.red;

            // Add to Source Image in Image component
            imageComponent.sprite = loadedImage;

            Debug.Log("Imagen cargada correctamente: "+ loadedImage);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar la imagen: " + e.Message);
        }
    }
}
