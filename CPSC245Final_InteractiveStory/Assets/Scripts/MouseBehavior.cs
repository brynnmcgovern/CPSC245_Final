/*
 * 1. Brynn McGovern and Charity Griffin
 *    2370579 and 2376898
 *    bmcgovern@chapman.edu and chagriffin@chapman.edu
 *    CPSC 245
 *    Final Project
 * 2. MouseBehavior class contains functions for controlling the mouse and changing its color
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class MouseBehavior : MonoBehaviour
{
    public Color activeColor;
    public Color notActiveColor;

    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick()
    {
        Activate();
    }

    public void OnPointerEnterAndDown()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Activate();
        }
    }

    public void OnPointerExit()
    {
        image.color = notActiveColor;
    }

    private void Activate()
    {
        image.color = activeColor;
    }
}
