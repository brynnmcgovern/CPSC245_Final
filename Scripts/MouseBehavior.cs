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
