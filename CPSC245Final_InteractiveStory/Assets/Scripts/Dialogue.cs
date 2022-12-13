/*
 * 1. Brynn McGovern and Charity Griffin
 *    2370579 and 2376898
 *    bmcgovern@chapman.edu and chagriffin@chapman.edu
 *    CPSC 245
 *    Final Project
 * 2. Dialogue class contains functions for tracking, organizing, and showing dialogue
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //The text file containing the full text of the story
    public TextAsset textFile;
    
    //Text objects to show the story,the choice buttons, and the timer for timed choices
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI timerText;
    
    //Game Objects for the the next and back buttons, the choice buttons, and the outline for the left choice button
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public Outline leftOutline;
    
    //A string to keep track of the currently visible line of text from the story
    [HideInInspector]public string currentLine;
    
    //A list for containing the lines of the text file before they are split into smaller lists, a list for the main story,
    //and lists for the branches and choices once they are separated
    private List<string> temp = new List<string>();
    private List<string> mainStory = new List<string>();
    private List<string> branchOneStory = new List<string>();
    private List<string> leftButtonList = new List<string>();
    private List<string> branchTwoStory = new List<string>();
    private List<string> rightButtonList = new List<string>();
    
    //Strings for tracking previous text to go back to
    private string previousStoryText;
    private string previousRightButtonText;
    private string previousLeftButtonText;
    
    //Integers for tracking previous list indexes, current indexes, the max number of lines in the story,
    //Which branch is being accessed, and how many seconds long the timed choice should be
    private int previousBranch1Counter;
    private int previousBranch2Counter;
    private int counter = 0;
    private int branch1Counter = 0;
    private int branch2Counter = 0;
    private int choiceCounter = 0;
    private int maxLines;
    private int branchNum;
    private int seconds = 3;
    
    //booleans for checking for the end of a branch, whether a choice button has been clicked,
    //and if the current text is a choice prompt or if it is from the main story
    private bool endOfBranch;
    private bool rightButtonClicked;
    private bool leftButtonClicked;
    private bool isChoice = false;
    private bool isFromMainStory = true;
    
    //A reference to the images class
    private Images images;
    
    //to signal an image change, place @ symbol at front of line, before *, &, //, 
    // and ;;. But put it after #1 and ::1 (the symbols and numbers for choices and branches)

    //Hides the choice and back buttons, the timer text and the outline on the left button, and shows the next button.
    //Also parses the text file to create two lists of lines
    private void Start()
    {
        images = GetComponent<Images>();
        CreateListOfLines(textFile, temp);
        HideBackButton();
        ShowNextButton();
        HideChoiceButtons();
        storyText.enabled = false;
        timerText.enabled = false;
        leftOutline.enabled = false;
    }

    //After one branch has been chosen, this method iterates through the other branch until it finds the end of it (signified by ";;")
    //So that the lines of the branches do not desync while one is being played through
    public void SyncBranchCounters()
    {
        string temp;
        if (branchNum == 2)
        {
            //saves the current index before iterating through the branch so that it can be referenced later
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
            }
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
            }
        }
    }
    
    //uses the previously saved index to keep branches synced as they are set back to what they were before
    private void SyncBranchCountersBackward()
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
    
    //Called with the Next button. Displays the story text and checks if it should be from the main story or one of the branches.
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
                SwitchBranchOneText();
            }

            if (branchNum == 2)
            {
                switchBranchTwoText();
            }
        }
    }
    
    //Called with the Back Button. Resets dialogue, buttons, and images to set the story back to the previous line
    public void GoBack()
    {
        //if the current line is a choice, undoes the setup for the choice prompt (hides choices, shows next button, sets back choice index)
        if (isChoice)
        {
            ShowNextButton();
            HideChoiceButtons();
            choiceCounter--;
        }
        //if current line is from the main story, parses through the previous line of text to display it
        if (isFromMainStory == true)
        {
            //counter must be subtracted by two to properly be set back because it will be incremented during the parsing of the previous line
            counter -= 2;
            ParseText(previousStoryText, "MainStory");
        }
        //if the current line is from one of the branches, parses through the corresponding previous line of text to display it
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
        
        //Displaying the previous line may have triggered an old choice prompt. This is checked by seeing if there is now a choice button active.
        //If so, the buttons and corresponding branches are set back to sync with the prompt
        if (leftButton.activeSelf)
        {
            choiceCounter -= 1;
            leftButtonText.text = leftButtonList[choiceCounter-1];
            rightButtonText.text = rightButtonList[choiceCounter-1];
            SyncBranchCountersBackward();
        }
        
        //Checks to see if the previous line had a prompt for an image switch.
        //If false, it calls GoBack() in the images class. If true, it will need to be set back farther to compensate, calling DoubleBack() instead.
        switch (CheckForImageSwitch(previousStoryText))
        {
            case true:
                images.DoubleBack();
                break;
            case false:
                images.GoBack();
                break;
        }
        HideBackButton();
    }
    
    //Handles parsing and displaying a line of the main story text. Also saves a reference to the line before it
    public void switchMainStoryText()
    {
        ShowNextButton();
        string line = mainStory[counter];
        currentLine = line;
        if (counter != 0)
        {   
            ShowBackButton();
            //checks to see if the previous line displayed is the same as the previous line stored in the main story list and sets it if true.
            //if false, it is from the branch, and the previous text is set as the visible text on screen
            //setting the previous text directly from the list saves the prompts (*, &, @, etc) and is useful for when the back button is used
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
    //once the left choice button is clicked, switches story text with text from this branch.
    public void SwitchBranchOneText()
    {
        branchNum = 1;
        isFromMainStory = false;
        isChoice = false;
        HideChoiceButtons();
        ShowNextButton();

        string line;
        line = branchOneStory[branch1Counter];
        currentLine = line;
        if (branch1Counter != 0)
        {
            if (branchOneStory[branch1Counter - 1].Contains(storyText.text))
            {
                HideBackButton();
                previousStoryText = branchOneStory[branch1Counter - 1];
            }
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
    
    //Called with Right Choice Button. Once the left button is clicked, it switches from main story text to the text of this branch.
    //The next button (aka Play()) will pull text from this branch until it ends and needs to switch back to the main story
    public void switchBranchTwoText()
    {
        branchNum = 2;
        isFromMainStory = false;
        isChoice = false;
        HideChoiceButtons();
        ShowNextButton();

        string line;
        line = branchTwoStory[branch2Counter];
        currentLine = line;
        if (branch2Counter != 0)
        {
            if (branchTwoStory[branch2Counter - 1].Contains(storyText.text))
            {
                previousStoryText = branchTwoStory[branch2Counter - 1];
                HideBackButton();
            }
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
    
    //Parses through the text, checking for all possible prompts/indicators in a line and trimming them
    //Once the line has been parsed, the index of the list it came from is incremented so the next line can be properly parsed
     private void ParseText(string line, string whichstory)
    {
        line = line.TrimStart("1");
        line = line.TrimStart("2");
        if (CheckForImageSwitch(line) == true)
        {
            line = line.TrimStart("@");
            images.NextImage();
        }
        
        //the name indicators surround the name so that it can be separated from the rest of the text
        //rather than just indicating it at the beginning of the line
        if (line.StartsWith("//"))
        {
            string[] namedDialogue = line.Split("//");
            line = namedDialogue[1] + "\n" + namedDialogue[2];
        }

        if (CheckForChoice(line) == true)
        {
            HideNextButton();
            ShowChoiceButtons();
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
    
     //Called when the right choice button is clicked
    public void CheckIfRightButtonWasClicked()
    {
        rightButtonClicked = true;
    }

    //Called when the left button is clicked
    public void CheckIfLeftButtonWasClicked()
    {
        leftButtonClicked = true;
    }
    
    //stops the coroutine for the timed choice, hides the outline of the choice that would have been auto-selected, and resets the timer
    public void StopTimer()
    {
        StopCoroutine("Countdown");
        leftOutline.enabled = false;
        ResetTimer();
    }
    
    
    //hides the back and next buttons once the last line of the story has been reached
    private void CheckForEndOfStory()
    {
        if (counter == maxLines)
        {
            HideNextButton();
            HideBackButton();
        }
    }
    
    //checks the start of the line for the indicator to change the image
    private bool CheckForImageSwitch(string line)
    {
        if (line.StartsWith("@"))
        {
            return true;
        }
        return false;
    }
    
    
    //checks the start of the line for the indicator of the last line of the branch
    private bool CheckIfEndOfBranch(string line)
    {
        if (line.Contains(";;"))
        {
            return true;
        }
        return false;
    }
    
    //checks the start of the line for the indicator for a choice prompt
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
    
    //checks the start of the line for the indicator for a timed choice
    private bool CheckForTimedChoice(string line)
    {
        if (line.StartsWith("&"))
        {
            line = line.TrimStart("&");
            return true;
        }
        return false;
    }
    
    //starts the timer for the timed choice and checks if a choice button has been pressed 
    //if it hasn't then continue countdown, if it has then stop countdown
    //check at end if a button has been pressed, if not automatically select the left button 
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
                ResetTimer();
                yield break;
            }
            else if (leftButtonClicked == true)
            {
                ResetTimer();
                yield break;
            }
        }

        if (seconds == 0)
        {
            leftOutline.enabled = true;
            ResetTimer();
            yield return new WaitForSeconds(1f);
            SwitchBranchOneText();
            SyncBranchCounters();
            leftOutline.enabled = false;
        }
    }
    
    //resets the timer by hiding it and setting the number of seconds back to 3 
    private void ResetTimer()
    {
        timerText.enabled = false;
        seconds = 3;
    }

    //gets a file as a string, separates that string into a list of lines, then separates that lists into smaller lists.
    private void CreateListOfLines(TextAsset file, List<string> list)
    {
        string line = GetTextFileAsLines(file);
        SeparateStringIntoLines(list, line);
        maxLines = temp.Count;
        SeparateLists();
        maxLines = mainStory.Count;
    }
    
    //converts the text file to a string
    private string GetTextFileAsLines(TextAsset file)
    {
        string line = file.ToString();
        return line;
    }
    
    //separates the string into a list of a lines via the StringReader class
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

    //separates list into smaller lists using symbols and numbers as indicators of whether they are branches or choices or part of the main story
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
    
    //shows the choice buttons by enabling them
    private void ShowChoiceButtons()
    {
        leftButton.SetActive(true);
        rightButton.SetActive(true);
    }

    //Hides the choice buttons by disabling them
    private void HideChoiceButtons()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);
    }
    
    //hides the back button by disabling it
    private void HideBackButton()
    {
        backButton.SetActive(false);
    }
    
    //shows the back button by enabling it
    private void ShowBackButton()
    {
        backButton.SetActive(true);
    }

    //hides the next button by disabling it
    private void HideNextButton()
    {
        nextButton.SetActive(false);
    }
    
    //shows the next button by enabling it
    private void ShowNextButton()
    {
        nextButton.SetActive(true);
    }
}
