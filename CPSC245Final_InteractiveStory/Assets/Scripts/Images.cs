/*
 * 1. Brynn McGovern and Charity Griffin
 *    2370579 and 2376898
 *    bmcgovern@chapman.edu and chagriffin@chapman.edu
 *    CPSC 245
 *    Final Project
 * 2. Images class contains functions for showing the current image, the next image, or the previous image
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Images : MonoBehaviour
{
    public Sprite[] images;
    public Image imageController;
    private int counter = -1;
    private Dialogue dialogue;

    //gets a reference to the Dialogue class at start
    private void Start()
    {
        dialogue = GetComponent<Dialogue>();
    }

    //increases the counter as along as it is less than the length of the images list, then sets the image
    public void NextImage()
    {
        if (counter < images.Length)
            counter++;
        //decreases the index to show the image(avoiding IndexOutOfRange error)
        //but increases it again for accuracy if the player chooses to go back 
        if (counter >= images.Length)
        {
            counter--;
            imageController.sprite = images[counter];
            counter++;
        }
        else
            imageController.sprite = images[counter];
    }

    //if the current line showing has an image switch prompt, sets the index back one then sets the image
    public void GoBack()
    {
        if (dialogue.currentLine.Contains("@"))
        {
            counter -= 1;
        }
        imageController.sprite = images[counter];
    }
    
    //only gets called if the previous line had an image switch prompt. Sets index back by one then sets image. 
    //If the current line also has an image switch prompt, sets index back by one more to compensate before setting the image.
    public void DoubleBack()
    {
        counter -= 1;
        if (dialogue.currentLine.Contains("@"))
        {
            counter -= 1;
        }
        imageController.sprite = images[counter];
    }
}
