/*
 * 1. Brynn McGovern and Charity Griffin
 *    2370579 and 2376898
 *    bmcgovern@chapman.edu and chagriffin@chapman.edu
 *    CPSC 245
 *    Final Project
 * 2. Menu class contains functions for showing the menu and accessing the stories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject firstStoryPanel;
    public GameObject secondStoryPanel;
   
    public GameObject menuPanel;

    public Outline firstStoryButton;
    public Outline secondStoryButton;

    //Hides the stories and shows the menu on start
    void Start()
    {
        menuPanel.SetActive(true);
        firstStoryPanel.SetActive(false);
        secondStoryPanel.SetActive(false);
    }

    //gets called when the first story button gets clicked. Show the corresponding story and hides all other panels
    public void FirstStory()
    {
        firstStoryPanel.SetActive(true);
        secondStoryPanel.SetActive(false);
        menuPanel.SetActive(false);
        firstStoryButton.enabled = true;
        secondStoryButton.enabled = false;
    }
    
    //gets called when the second story button gets clicked. Show the corresponding story and hides all other panels
    public void SecondStory()
    {
        firstStoryPanel.SetActive(false);
        secondStoryPanel.SetActive(true);
        menuPanel.SetActive(false);
        firstStoryButton.enabled = false;
        secondStoryButton.enabled = true;
    }
}
