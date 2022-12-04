using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextHandlerExample : MonoBehaviour
    {
        private TextFileHandler textFileHandler;
        private bool isTextFromBaselineFile = true;
        public Button choiceButton;
        public Button choiceButton2;
        public TextMeshProUGUI choiceText;

        private void Start()
        {
            textFileHandler = GetComponent<TextFileHandler>();
        }
        public void OnNextButtonClicked()
        {
            if (isTextFromBaselineFile)
            {
                if (textFileHandler.indexForListOfLines < (textFileHandler.GetMaxNumberOfLines()-1))
                {
                    textFileHandler.indexForListOfLines++;
                    textFileHandler.ChangeVisibleLineOfText(isTextFromBaselineFile);
                }
            }
            else
            {
                if (textFileHandler.indexForListOfBranchLines < (textFileHandler.GetMaxNumberOfLines()-1))
                {
                    textFileHandler.indexForListOfBranchLines++;
                    textFileHandler.ChangeVisibleLineOfText(isTextFromBaselineFile);
                }
            }
            
        }

        public void OnBackButtonClicked()
        {
            if (isTextFromBaselineFile)
            {
                if (textFileHandler.indexForListOfLines > 0)
                {
                    textFileHandler.indexForListOfLines--;
                    textFileHandler.ChangeVisibleLineOfText(isTextFromBaselineFile);
                }
            }
            else
            {
                if (textFileHandler.indexForListOfBranchLines > 0)
                {
                    textFileHandler.indexForListOfBranchLines--;
                    textFileHandler.ChangeVisibleLineOfText(isTextFromBaselineFile);
                }
            }
        }
        
        public void OnChoiceButtonClicked()
        {
            SetBaselineCheckingBoolToFalse();
            textFileHandler.indexForListOfBranchLines++;
            textFileHandler.ChangeVisibleLineOfText(isTextFromBaselineFile);
        }
        
        // gets invoked at the end of a branch
        public void ResetBaselineCheckingBool()
        {
            isTextFromBaselineFile = true;
        }

        public void SetBaselineCheckingBoolToFalse()
        {
            isTextFromBaselineFile = false;
        }
        
        public void ShowChoiceButtons()
        {
            choiceButton.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);
        }

        public void HideChoiceButtons()
        {
            choiceButton.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);
        }

        public void ChangeChoiceButtonText()
        {
            choiceText.text = textFileHandler.GetChoiceButtonText();
        }
    }

