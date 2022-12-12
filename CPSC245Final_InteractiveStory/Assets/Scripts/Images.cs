using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Images : MonoBehaviour
{
    public Sprite[] images;
    private int counter = -1;
    public Image imageController;
    public Dialogue dialogue;
    public int previousImageCounter = -1;

    
    // Start is called before the first frame update

    private void Start()
    {
        
    }

    public void SetImage()
    {
        imageController.sprite = images[counter];
    }

    public void NextImage()
    {
        
        if (counter <= images.Length)
            counter++;
        imageController.sprite = images[counter];
        print(counter);
    }

    public void GoBack()
    {
        if (dialogue.currentLine.Contains("@"))
        {
            counter -= 1;
        }
        imageController.sprite = images[counter];
        print(counter);
    }
    public void DoubleBack()
    {
        counter -= 1;
        if (dialogue.currentLine.Contains("@"))
        {
            counter -= 1;
        }
        imageController.sprite = images[counter];
        print(counter);
    }
}
