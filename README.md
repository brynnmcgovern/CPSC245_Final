# CPSC245_Final

Names: Charity Griffin and Brynn McGovern
Student IDs: 2376898 and 2370579
Emails: chagriffin@chapman.edu and bmcgovern@chapman.edu
Course: CPSC 245
Assignment: Final Project

Source Files:
Dialogue
Images
Menu
Application

References:
Unity Scripting API
Microsoft Learn
Unity Support
Storybook project from Visual Programming
Thaleah Pixel Font (for art, not code)

Known errors:
No known Errors

Build Instructions:
Upon opening the game there should be a title screen with two buttons, 
"The Legend Of Sandie" on the left and "A Pirate's Tale" on the right.
Clicking the left button should load "The Legend Of Sandie" story with
a title image, an empty text box, and a next button. Clicking the next 
button should change the image and display text in the text box. Clicking
next again should cause a back button to appear. Clicking the back button
should set the image and the dialogue to what they were one line previously.
The back button should disappear after being clicked, but will reappear once 
the next button is clicked again. (The story's images should not change every 
time the next button is clicked, but the text will).

The next button should eventually lead to two choice buttons appearing below 
the text in the dialogue box. (The back button can be pressed whenever it is 
on screen to set the story back one line during this time). Once the choice 
options appear, the next button should disappear. Either of the choice buttons 
can be clicked and should both give different dialogue options. Back can be 
pressed immediately after making the choice, but the back button will disappear
after committing to the branch and will not reappear until the branch ends. 
Once the branch ends, the back button will reappear. Pressing the back button 
at this point should display the last line of the chosen branch. The game can 
also continue as normal by continuing to press the next button. 

Upon reaching a timed choice, a short timer will appear in the top right corner 
of the screen. If the timer runs out, it will auto-select the left choice button, 
giving it a yellow outline before progressing with its corresponding branch. 
Pressing back at this point will reset the choice and the timer. Clicking a choice
before the timer runs out will function as the previous choices have, proceeding 
with its corresponding branch.

Upon reaching the end of the story, the back and next buttons will disappear. 
Pressing "R" on the keyboard should reset the game back to the main menu. "R" can
be pressed at any point in the game to reset to the main menu. "Q" can also be 
pressed at any time to quit the application entirely. 

Once back at the main menu, either of the story buttons can be pressed to play
through the corresponding story from the beginning. The right button, "A Pirate's
Tale," has the same core functionality as "The Legend Of Sandie" but has a 
different configuration (less choices, longer branches, more timed choices, etc), 
in order to show how the system can add the story functionality to any correctly
formatted text file.