using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.UI;

//timed choices
//pub/sub call left button function
public class Dialogue : MonoBehaviour
{
    public TextAsset textFile;
    private List<string> mainStory = new List<string>();
    private List<string> temp = new List<string>();
    private List<string> branchOneStory = new List<string>();
    private List<string> leftButtonList = new List<string>();
    private List<string> branchTwoStory = new List<string>();
    private List<string> rightButtonList = new List<string>();
    private string previousStoryText;
    private string previousRightButtontText;
    private string previousLeftButtonText;

    public TextMeshProUGUI storyText;
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI timerText;

    private int counter = 0;
    private int branchCounter = 0;
    private int maxLines;
    private int choiceCounter = 0;
    private bool endOfBranch;
    private int seconds = 10;
    private bool rightButtonClicked;
    private bool leftButtonClicked;
    
    //public UnityEvent onChoicePrompted;

    public GameObject nextButton;
    public GameObject backButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public Outline leftOutline;
    private bool isFromMainStory = true;
    private int branchNum;

    private Images images;
    
    //make a method to check which button was pressed
    //so on click it would return true
    //check those methods in timer class
    //move timer countdown to this class

    // Start is called before the first frame update
    void Start()
    {
        CreateListOfLines(textFile, temp);
        backButton.SetActive(false);
        nextButton.SetActive(true);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        storyText.text = "Title Of Story";
        images = GetComponent<Images>();
        timerText.enabled = false;
        leftOutline.enabled = false;
        

        //hide choice buttons
        //set active next button

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfRightButtonWasClicked()
    {
        rightButtonClicked = true;
    }

    public void CheckIfLeftButtonWasClicked()
    {
        leftButtonClicked = true;
    }
    
    private IEnumerator Countdown()
    {
        rightButtonClicked = false;
        leftButtonClicked = false;
        timerText.enabled = true;
        while (seconds > 0)
        {
            timerText.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
            seconds--;

            if (rightButtonClicked == true)
            {
                timerText.enabled = false;
                break;
            }
            else if (leftButtonClicked == true)
            {
                timerText.enabled = false;
                break;
            }
            //check if a choice button has been pressed 
            //if it hasn't then continue countdown
            //if it has then stop countdown
            //check at end if a button has been pressed
            //if not automatically select the left button 
            
        }

        if (seconds == 0)
        {
            timerText.enabled = false;
            leftOutline.enabled = true;
            yield return new WaitForSeconds(1f);
            
            switchBranchOneText();
        }
        
    }

    public void Play()
    {
        if (isFromMainStory == true)
        {
            switchMainStoryText();
            images.NextImage();
        }
        else if(isFromMainStory == false)
        {
            if (branchNum == 1)
            {
                switchBranchOneText();
                images.NextImage();
            }

            if (branchNum == 2)
            {
                switchBranchTwoText();
                images.NextImage();
            }
        }
    }
    private void CreateListOfLines(TextAsset file, List<string> list)
    {
        string line = GetTextFileAsLines(file);
        SeparateStringIntoLines(list, line);
        maxLines = temp.Count;
        SeparateLists();
    }

    public void GoBack()
    {
        storyText.text = previousStoryText;
        leftButtonText.text = previousLeftButtonText;
        rightButtonText.text = previousRightButtontText;
        images.GoBack();
        backButton.SetActive(false);

        
        if (isFromMainStory == true)
        {
            counter -= 1;
        }
        else
        {
            branchCounter -= 1;
        }
        
    }
    private void SeparateLists()
    {

        string line;
        foreach(string tempLine in temp)
        {
            
            if (tempLine.StartsWith("#"))
            {
                
                line = tempLine.TrimStart("#");
                if (line.StartsWith("1"))
                {
                    branchOneStory.Add(line);
                }
                else if (line.StartsWith("2"))
                {
                    branchTwoStory.Add(line);
                }

            }
            else if (tempLine.StartsWith("::"))
            {
                line = tempLine.TrimStart("::");
                if (line.StartsWith("1"))
                {
                    line = line.TrimStart("1");
                    leftButtonList.Add(line);
                }

                else if (line.StartsWith("2"))
                {
                    line = line.TrimStart("2");
                    rightButtonList.Add(line);
                }
            }
            else
            {
                mainStory.Add(tempLine);
            }
            
        }
    }

    public void switchMainStoryText()
    {
        nextButton.SetActive(true);
        string line = mainStory[counter];
        if (counter != 0)
        {   
            backButton.SetActive(true);
            previousStoryText = mainStory[counter - 1];
        }

        if (line.StartsWith("//"))
        {
             string[] namedDialogue = line.Split("//");
             line = namedDialogue[1] + "\n" + namedDialogue[2];
        }

        if (CheckForChoice(line) == true)
        {
            //set buttons active
            //hide next button
            
            nextButton.SetActive(false);
            leftButton.SetActive(true);
            rightButton.SetActive(true);
            leftButtonText.text = leftButtonList[choiceCounter];
            rightButtonText.text = rightButtonList[choiceCounter];
            line = line.TrimStart("*");
            
            if (CheckForTimedChoice(line) == true)
            {
                line = line.TrimStart("&");
                storyText.text = line;
                StartCoroutine(Countdown());
            }
            storyText.text = line;
            counter++;
            choiceCounter++;
            
            //onChoicePrompted.Invoke();
        }
        else
        {
            storyText.text = line;
            counter++;
        }
        
    }

    public bool CheckForTimedChoice(string line)
    {
        if (line.StartsWith("&"))
        {
            line = line.TrimStart("&");
            return true;
        }

        return false;
    }

    
    

    //on leftButtonClick
    //once button is clicked switch story text with text from this branch
    public void switchBranchOneText()
    {
        branchNum = 1;
        isFromMainStory = false;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        nextButton.SetActive(true);
        images.NextImage();
        
        string line;
        line = branchOneStory[branchCounter];
        line = line.TrimStart("1");
        if (branchCounter != 0)
        {
            previousStoryText = branchOneStory[branchCounter - 1];
            //previousLeftButtonText = leftButton[choiceCounter - 2];
            //previousRightButtontText = rightButton[choiceCounter - 2];
        }

        if (branchCounter == 0)
        {
            previousStoryText = mainStory[counter];
        }

        if (CheckIfEndOfBranch(line) == true)
        {
            line = line.TrimStart(";;");
            storyText.text = line;
            branchCounter++;
            isFromMainStory = true;
            //onSwitchToMainText.Invoke();
            //go back to main story
        }
        else
        {
            storyText.text = line;
            branchCounter++;
        }
        
        
        


    }
    public void switchBranchTwoText()
    {
        branchNum = 2;
        isFromMainStory = false;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        nextButton.SetActive(true);
        images.NextImage();
        
        string line;
        line = branchTwoStory[branchCounter];
        line = line.TrimStart("2");
        if (branchCounter != 0)
        {
            previousStoryText = branchTwoStory[branchCounter - 1];
            //previousLeftButtonText = leftButton[choiceCounter - 2];
            //previousRightButtontText = rightButton[choiceCounter - 2];
        }

        if (branchCounter == 0)
        {
            previousStoryText = mainStory[counter];
        }

        if (CheckIfEndOfBranch(line) == true)
        {
            line = line.TrimStart(";;");
            storyText.text = line;
            branchCounter++;
            isFromMainStory = true;
            //onSwitchToMainText.Invoke();
            //go back to main story
        }
        else
        {
            storyText.text = line;
            branchCounter++;
        }
        
        


    }
    

    public bool CheckIfEndOfBranch(string line)
    {
        if (line.Contains(";;"))
        {
            return true;
        }

        return false;
    }

    public bool CheckForChoice(string line)
    {
        if (line.StartsWith("*"))
        {
            return true;
        }

        return false;
    }
    //  
    private string GetTextFileAsLines(TextAsset file)
    {
        string line = file.ToString();
        return line;
    }

    private void SeparateStringIntoLines(List<string> list, string line)
    {
        
        StringReader stringReader = new StringReader(line);
        while (true)
        {
            var lineOfText = stringReader.ReadLine();
            if (lineOfText != null)
            {
                list.Add(lineOfText);
            }
            else
            {
                break;
            }
        }
        stringReader.Close();
    }
    
}

