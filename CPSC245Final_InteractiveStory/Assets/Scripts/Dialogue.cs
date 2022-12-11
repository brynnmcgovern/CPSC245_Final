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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void syncBranchCounters()
    {
        string temp;
        if (branchNum == 2)
        {
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
            temp = branchTwoStory[branch2Counter];
            while (!temp.Contains(";;"))
            {
                temp = branchTwoStory[branch2Counter];
                branch2Counter++;
                
                if (temp.Contains(";;"))
                {
                    break;
                }
                //break
            }
            
            //search thru branch1 list for ;;
            //increment branch1Counter
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
                syncBranchCounters();
                
            }

            if (branchNum == 2)
            {
                switchBranchTwoText();
                syncBranchCounters();
                
            }
        }
    }
    private void CreateListOfLines(TextAsset file, List<string> list)
    {
        string line = GetTextFileAsLines(file);
        SeparateStringIntoLines(list, line);
        maxLines = temp.Count;
        SeparateLists();
        maxLines = mainStory.Count;
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
            //branchCounter -= 1; fix this
            counter -= 1;
        }
        
    }

    public void CheckForEndOfStory()
    {
        if (counter == maxLines)
        {
            storyText.text = "End of Story";
            nextButton.SetActive(false);
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
        if (CheckForImageSwitch(line) == true)
        {
            line = line.TrimStart("@");
            images.NextImage();
        }

        if (line.StartsWith("//"))
        {
            line = line.TrimStart("//");
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
        line = branchOneStory[branch1Counter];
        line = line.TrimStart("1");
        if (branch1Counter != 0)
        {
            previousStoryText = branchOneStory[branch1Counter - 1];
            //previousLeftButtonText = leftButton[choiceCounter - 2];
            //previousRightButtontText = rightButton[choiceCounter - 2];
        }

        if (branch1Counter == 0)
        {
            previousStoryText = mainStory[counter];
        }

        if (CheckForImageSwitch(line) == true)
        {
            line = line.TrimStart("@");
            images.NextImage();
        }

        if (CheckIfEndOfBranch(line) == true)
        {
            line = line.TrimStart(";;");
            storyText.text = line;
            branch1Counter++;
            isFromMainStory = true;
            //onSwitchToMainText.Invoke();
            //go back to main story
        }
        else
        {
            storyText.text = line;
            branch1Counter++;
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
        line = branchTwoStory[branch2Counter];
        line = line.TrimStart("2");
        if (branch2Counter != 0)
        {
            previousStoryText = branchTwoStory[branch2Counter - 1];
            //previousLeftButtonText = leftButton[choiceCounter - 2];
            //previousRightButtontText = rightButton[choiceCounter - 2];
        }

        if (branch2Counter == 0)
        {
            previousStoryText = mainStory[counter];
        }

        if (CheckForImageSwitch(line) == true)
        {
            line = line.TrimStart("@");
            images.NextImage();
        }

        if (CheckIfEndOfBranch(line) == true)
        {
            line = line.TrimStart(";;");
            storyText.text = line;
            branch2Counter++;
            isFromMainStory = true;
            //onSwitchToMainText.Invoke();
            //go back to main story
        }
        else
        {
            storyText.text = line;
            branch2Counter++;
        }
        
        


    }

    public bool CheckForImageSwitch(string line)
    {
        if (line.StartsWith("@"))
        {
            return true;
        }

        return false;
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
