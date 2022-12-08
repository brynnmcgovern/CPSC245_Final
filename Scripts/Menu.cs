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
    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        firstStoryPanel.SetActive(false);
        secondStoryPanel.SetActive(false);
    }

    private void Awake()
    {
        //hide button outlines
        firstStoryButton.enabled = false;
        secondStoryButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstStory()
    {
        firstStoryPanel.SetActive(true);
        secondStoryPanel.SetActive(false);
        menuPanel.SetActive(false);
        

        //show button outline
        //hide button outline
    }

    public void SecondStory()
    {
        firstStoryPanel.SetActive(false);
        secondStoryPanel.SetActive(true);
        menuPanel.SetActive(false);
       
        //show button outline
        //hide button outline
    }
}
