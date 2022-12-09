using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Images : MonoBehaviour
{
    public Sprite[] images;
    private int counter = 0;
    public Image imageController;
    private Sprite previousImage;

    
    // Start is called before the first frame update

    private void Start()
    {
        
    }

    public void NextImage()
    {
        imageController.sprite = images[counter];
        if (counter != 0)
        {
            previousImage = images[counter - 1];
        }
        
        counter++;
        
    }

    public void GoBack()
    {
        imageController.sprite = previousImage;
    }
}
