using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class TextFileHandler : MonoBehaviour
    {
        [Tooltip("The object to display the text on screen.")]
        public TextMeshProUGUI textObject;
        [Tooltip("The container for the main text file.")]
        public TextAsset textFile;
        [Tooltip("The container for the text file for branches")]
        public TextAsset branchTextFile;
        [HideInInspector] public int indexForListOfLines = -1;
        [HideInInspector] public int indexForListOfBranchLines = 0;
        private string fullStringOfText;
        private List<string> allLinesOfText = new List<string>();
        private string branchFullString;
        private List<string> allLinesOfBranches = new List<string>();
        private string choiceButtonText;
        public UnityEvent onChoicePrompted;
        public UnityEvent onSwitchToMainText;


        //changes the on-screen text to a specific line of text retrieved from a list
        //takes a boolean as a parameter to check if the line of text should come from the main text file instead of the branch
        public void ChangeVisibleLineOfText(bool isFromMainTextFile)
        {
            textObject.text = GetLineToDisplay(isFromMainTextFile);
        }
    
        //returns the max number of lines from the list of lines in the text
        public int GetMaxNumberOfLines()
        {
            int maxLines = allLinesOfText.Count;
            return maxLines;
        }
        
        //returns the text for the choice button by pulling from the next line of the branching text file
        public string GetChoiceButtonText()
        {
            string line = GetLineToDisplay(false);
            SetChoiceButtonText(line);
            return choiceButtonText;
        }

        //creates lists of lines from the text files for the main text file and the file for branches at the start of the game
        private void Start()
        {
            CreatListOfLinesFromText(textFile, fullStringOfText, allLinesOfText);
            CreatListOfLinesFromText(branchTextFile, branchFullString, allLinesOfBranches);
        }
        
        //converts text file to string and separates the string into a list of lines
        private void CreatListOfLinesFromText(TextAsset text, string fullString, List<string> listOfLines)
        {
            fullString = GetTextFileAsString(text, fullString);
            SeparateStringIntoLines(fullString, listOfLines); 
        }
        
        //Gets the contents of a text file and returns it as a string
        private string GetTextFileAsString(TextAsset file, string text)
        {
            text = file.ToString();
            return text;
        }

        //reads through every line of a string and adds them individually to a list using the StringReader class
        private void SeparateStringIntoLines(string fullText, List<string> listOfLines)
        {
            StringReader stringReader = new StringReader(fullText);
            while (true)
            {
                var lineOfText = stringReader.ReadLine();
                if (lineOfText != null)
                    listOfLines.Add(lineOfText);
                else
                    break;
            }
            stringReader.Close();
        }
        
        //returns a string after checking a line of text for various details in it relevant for formatting
        //also checks if the line should be pulled from main text file or file for branches
        private string GetLineToDisplay(bool mainTextFile)
        {
            return mainTextFile ? CheckDetailsForMainText() : CheckDetailsForBranches();
        }
        
        // returns a string after checking it to see if there are indicators for character names or choice prompts
        private string CheckDetailsForMainText()
        {
            string line =
                CheckForCharacterNames(CheckForChoicePrompt(allLinesOfText[indexForListOfLines], indexForListOfLines),
                    indexForListOfLines);
            return line;
        }
        
        // returns a string after checking it for indicators for character names and the start or end of a branch
        private string CheckDetailsForBranches()
        {
            string line =
                CheckForCharacterNames(
                    CheckForStartOfBranch(CheckForEndOfBranch(allLinesOfBranches[indexForListOfBranchLines])), indexForListOfBranchLines);
            return line;
        }

        //checks the start of every line of text for the indicator for character names, //
        private string CheckForCharacterNames(string line, int lineIndex)
        {
            if (line.StartsWith("//"))
            {
                string[] namedDialogue = line.Split("//");
                line = namedDialogue[1] + "\n" + namedDialogue[2];
                //checks to see if the named dialogue is also a choice prompt
                if (namedDialogue[1].Contains("*"))
                {
                    line = CheckForChoicePrompt(line, lineIndex);
                }
            }
            return line;
        }

        //checks the start of every line of the text for the indicator for choice prompts, *
        private string CheckForChoicePrompt(string line, int lineIndex)
        {
            //if choice symbol in line
            if (line.StartsWith("*"))
            {
                line = line.TrimStart("*");
                //publish event for choice buttons to appear
                onChoicePrompted.Invoke();
            }
            return line;
        }
    
        //checks for the indicator for the start of a branch, aka the choice option that would go in a button, ::
        private string CheckForStartOfBranch(string line)
        {
            if (line.StartsWith("::"))
            {
                string[] choiceDialogue = line.Split("::");
                line = choiceDialogue[2];
            }
            return line;
        }

        //checks for the indicator for the end of the branch and switches back to the main text file, ;;
        private string CheckForEndOfBranch(string line)
        {
            if (line.StartsWith(";;"))
            {
                line = line.TrimStart(";;");
                onSwitchToMainText.Invoke();
            }
            return line;
        }
        
        //takes in a line as a parameter and sets the choiceButtonText variable to be that line
        private void SetChoiceButtonText(string line)
        {
            choiceButtonText = line;
        }
    }
