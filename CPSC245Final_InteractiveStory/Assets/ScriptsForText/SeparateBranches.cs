using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateBranches : TextFileHandler
{
    
    private void SeparateBranchesIntoLists()
    {
        foreach (string line in allLinesOfBranches)
        {
            if (line.StartsWith("::"))
            {
                //make new list
                //add lines to list until reaching a line with ";;" at the start
            }
        }
    }
    
    /*
     -every branch = self-contained list
     -instead of checking full branch file when calling CheckForBranchDetails, somehow pass in the smaller lists for the choices
     -change "StartsWith" to "Contains" (for branches only)
     -keep track of branches by numbering their lines in the text file
    */
    
}
