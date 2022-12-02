using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


public class TextFileHandler : MonoBehaviour
    {
        [Tooltip("The container for the text file.")]
        public TextAsset textFile;
        [Tooltip("The object to display the text on screen.")]
        public TextMeshProUGUI textObject;
        [HideInInspector] public int indexForListOfLines = -1;
        private string fullStringOfText;
        private List<string> allLinesOfText = new List<string>();

        //changes the on-screen text to a specific line of text retrieved from a list
        public void ChangeVisibleLineOfText()
        {
            textObject.text = allLinesOfText[indexForListOfLines];
        }
    
        //returns the max number of lines from the list of lines in the text
        public int GetMaxNumberOfLines()
        {
            int maxLines = allLinesOfText.Count;
            return maxLines;
        }
    
        //Gets the text files as a string and then separates that string into individual lines at start of game
        private void Start()
        {
            GetTextFileAsString(textFile);
            SeparateStringIntoLines(fullStringOfText);
        }

        //Gets the contents of a text file and stores it as a string
        private string GetTextFileAsString(TextAsset file)
        {
            fullStringOfText = file.ToString();
            return fullStringOfText;
        }

        //reads through every line of the string and adds them individually to a list using the StringReader class
        private void SeparateStringIntoLines(string fullText)
        {
            StringReader stringReader = new StringReader(fullText);
            while (true)
            {
                var lineOfText = stringReader.ReadLine();
                if (lineOfText != null)
                    allLinesOfText.Add(lineOfText);
                else
                    break;
            }
            stringReader.Close();
        }
    }
