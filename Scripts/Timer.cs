using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    private int seconds = 10;
    public UnityEvent onTimerRunsOut;

    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Countdown()
    {
        while (seconds > 0)
        {
            timerText.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
            seconds--;
            //check if a choice button has been pressed 
            //if it hasn't then continue countdown
            //if it has then stop countdown
            //check at end if a button has been pressed
            //if not automatically select the left button 
            
        }
        
    }
    
    
}
