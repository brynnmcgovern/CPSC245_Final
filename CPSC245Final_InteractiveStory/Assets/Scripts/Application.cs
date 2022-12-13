/*
 * 1. Brynn McGovern and Charity Griffin
 *    2370579 and 2376898
 *    bmcgovern@chapman.edu and chagriffin@chapman.edu
 *    CPSC 245
 *    Final Project
 * 2. Application class contains functions for reloading or quitting the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Application : MonoBehaviour
{
    //Update checks for input to reload the scene or quit the game
    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            UnityEngine.Application.Quit();
        }
    }
}
