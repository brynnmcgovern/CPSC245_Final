using UnityEngine;


public class TextHandlerExample : MonoBehaviour
    {
        private TextFileHandler textFileHandler;

        private void Start()
        {
            textFileHandler = GetComponent<TextFileHandler>();
        }
        public void OnNextButtonClicked()
        {
            if (textFileHandler.indexForListOfLines < (textFileHandler.GetMaxNumberOfLines()-1))
            {
                textFileHandler.indexForListOfLines++;
                textFileHandler.ChangeVisibleLineOfText();
            }
        }

        public void OnBackButtonClicked()
        {
            if (textFileHandler.indexForListOfLines > 0)
            {
                textFileHandler.indexForListOfLines--;
                textFileHandler.ChangeVisibleLineOfText();
            }
        }
    }

