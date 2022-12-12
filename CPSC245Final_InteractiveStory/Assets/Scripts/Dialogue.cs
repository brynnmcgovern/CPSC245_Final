using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.UI;

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
    private int previousBranch1Counter;
    private int previousBranch2Counter;
    public string currentLine;
    

    public TextMeshProUGUI storyText;
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI timerText;

    private int counter = 0;
    private int branch1Counter = 0;
    private int branch2Counter = 0;
    private int maxLines;
    private int choiceCounter = 0;
    private bool endOfBranch;
    private int seconds = 3;
    private bool rightButtonClicked;
    private bool leftButtonClicked;
    public bool isChoice = false;
    public bool isImageSwitch = false;
    
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public Outline leftOutline;
    private bool isFromMainStory = true;
    private int branchNum;


    private Images images;
    
    //to signal an image change, place @ symbol at front of line, before *, &, //, 
    // and ;;. But put it after #1 and ::1 (the symbols and numbers for choices and branches)

    // Start is called before the first frame update
    void Start()
    {
        CreateListOfLines(textFile, temp);
        backButton.SetActive(false);
        nextButton.SetActive(true);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        storyText.enabled = false;
        images = GetComponent<Images>();
        timerText.enabled = false;
        leftOutline.enabled = false;

        //hide choice buttons
        //set active next button

    }
    
    public void syncBranchCounters()
    {
        string temp;
        if (branchNum == 2)
        {
            previousBranch1Counter = branch1Counter;
            temp = branchOneStory[branch1Counter];
            while (!temp.Contains(";;"))
            {
                temp = branchOneStory[branch1Counter];
                branch1Counter++;
                if (temp.Contains(";;"))
                {
                    break;
                }
                //stop
            }
            //search thru branch2 list for ;;
            //increment branch2Counter
        }
        
        else if (branchNum == 1)
        {
            previousBranch2Counter = branch2Counter;
            temp = branchTwoStory[branch2Counter];
            while (!temp.Contains(";;"))
            {
                temp = branchTwoStory[branch2Counter];
                branch2Counter++;
                if (temp.Contains(";;"))
                {
                    break;
                }
                //stop
            }
            //search thru branch1 list for ;;
            //increment branch1Counter
        }
    }
    
    public void SyncBranchCountersBackward()
    {
        if (branchNum == 2)
        {
            branch1Counter = previousBranch1Counter;
        }
        
        else if (branchNum == 1)
        {
            branch2Counter = previousBranch2Counter;
        }
    }
    
    
    public void CheckIfRightButtonWasClicked()
    {
        rightButtonClicked = true;
    }

    public void CheckIfLeftButtonWasClicked()
    {
        leftButtonClicked = true;
    }

    public void StopTimer()
    {
        StopCoroutine("Countdown");
        leftOutline.enabled = false;
        timerText.enabled = false;
        seconds = 3;
    }

    public void Play()
    {
        storyText.enabled = true;
        if (isFromMainStory == true)
        {
            switchMainStoryText();
            CheckForEndOfStory();
        }
        else if(isFromMainStory == false)
        {
            if (branchNum == 1)
            {
                switchBranchOneText();
            }

            if (branchNum == 2)
            {
                switchBranchTwoText();
            }
        }
    }

    public void GoBack()
    {
        if (isChoice)
        {
            nextButton.SetActive(true);
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            choiceCounter--;
        }
        if (isFromMainStory == true)
        {
            counter -= 2;
            ParseText(previousStoryText, "MainStory");
        }
        else
        {
            if (branchNum == 1)
            {
                branch1Counter -= 2;
                ParseText(previousStoryText, "BranchOne");
            }

            if (branchNum == 2)
            {
                branch2Counter -= 2;
                ParseText(previousStoryText, "BranchTwo");
            }
        }

        if (leftButton.activeSelf)
        {
            choiceCounter -= 1;
            leftButtonText.text = leftButtonList[choiceCounter-1];
            rightButtonText.text = rightButtonList[choiceCounter-1];
            SyncBranchCountersBackward();
        }

        switch (CheckForImageSwitch(previousStoryText))
        {
            case true:
                images.DoubleBack();
                break;
            case false:
                images.GoBack();
                break;
        }
       
        backButton.SetActive(false);
    }

    private void ParseText(string line, string whichstory)
    {
        line = line.TrimStart("1");
        line = line.TrimStart("2");
        if (CheckForImageSwitch(line) == true)
        {
            line = line.TrimStart("@");
            images.NextImage();
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
            if (choiceCounter >= 0 && choiceCounter < leftButtonList.Count)
            {
                leftButtonText.text = leftButtonList[choiceCounter];
                rightButtonText.text = rightButtonList[choiceCounter];
            }
            line = line.TrimStart("*");

            if (CheckForTimedChoice(line) == true)
            {
                line = line.TrimStart("&");
                storyText.text = line;
                StartCoroutine("Countdown");
            }
            storyText.text = line;
            choiceCounter++;
        }
        if (CheckIfEndOfBranch(line) == true)
        {
            line = line.TrimStart(";;");
            storyText.text = line;
            switch (whichstory)
            {
                case "MainStory":
                    counter++;
                    break;
                case "BranchOne":
                    branch1Counter++;
                    break;
                case"BranchTwo":
                    branch2Counter++;
                    break;
            }
            isFromMainStory = true;
        }
        else
        {
            storyText.text = line;
            switch (whichstory)
            {
                case "MainStory":
                    counter++;
                    break;
                case "BranchOne":
                    branch1Counter++;
                    break;
                case"BranchTwo":
                    branch2Counter++;
                    break;
            }
        }
    }

    public void switchMainStoryText()
    {
        nextButton.SetActive(true);
        string line = mainStory[counter];
        currentLine = line;
        if (counter != 0)
        {   
            backButton.SetActive(true);
            if (mainStory[counter - 1].Contains(storyText.text))
                previousStoryText = mainStory[counter - 1];
            else
            {
                previousStoryText = storyText.text;
            }
        }
        ParseText(line, "MainStory");
    }

    //on leftButtonClick
    //once button is clicked switch story text with text from this branch
    public void switchBranchOneText()
    {
        branchNum = 1;
        isFromMainStory = false;
        isChoice = false;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        nextButton.SetActive(true);

        string line;
        line = branchOneStory[branch1Counter];
        currentLine = line;
        if (branch1Counter != 0)
        {
            if (branchOneStory[branch1Counter - 1].Contains(storyText.text))
                previousStoryText = branchOneStory[branch1Counter - 1];
            else
            {
                previousStoryText = mainStory[counter-1];
            }
        }

        if (branch1Counter == 0)
        {
            previousStoryText = mainStory[counter-1];
        }
        ParseText(line, "BranchOne");
    }
    public void switchBranchTwoText()
    {
        branchNum = 2;
        isFromMainStory = false;
        isChoice = false;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        nextButton.SetActive(true);

        string line;
        line = branchTwoStory[branch2Counter];
        currentLine = line;
        if (branch2Counter != 0)
        {
            if (branchTwoStory[branch2Counter - 1].Contains(storyText.text))
                previousStoryText = branchTwoStory[branch2Counter - 1];
            else
            {
                previousStoryText = mainStory[counter-1];
            }
        }

        if (branch2Counter == 0)
        {
            previousStoryText = mainStory[counter-1];
        }
        ParseText(line, "BranchTwo");
    }
    private void CheckForEndOfStory()
    {
        if (counter == maxLines)
        {
            nextButton.SetActive(false);
            backButton.SetActive(false);
        }
    }
    
    private bool CheckForImageSwitch(string line)
    {
        if (line.StartsWith("@"))
        {
            isImageSwitch = true;
            return true;
        }
        isImageSwitch = false;
        return false;
    }

    private bool CheckIfEndOfBranch(string line)
    {
        if (line.Contains(";;"))
        {
            return true;
        }
        return false;
    }

    private bool CheckForChoice(string line)
    {
        if (line.StartsWith("*"))
        {
            isChoice = true;
            return true;
        }
        isChoice = false;
        return false;
    }
    
    private bool CheckForTimedChoice(string line)
    {
        if (line.StartsWith("&"))
        {
            line = line.TrimStart("&");
            return true;
        }
        return false;
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
                seconds = 3;
                yield break;
            }
            else if (leftButtonClicked == true)
            {
                timerText.enabled = false;
                seconds = 3;
                yield break;
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
            seconds = 3;
            yield return new WaitForSeconds(1f);
            
            switchBranchOneText();
        }
    }

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
    
    private void CreateListOfLines(TextAsset file, List<string> list)
    {
        string line = GetTextFileAsLines(file);
        SeparateStringIntoLines(list, line);
        maxLines = temp.Count;
        SeparateLists();
        maxLines = mainStory.Count;
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
}
