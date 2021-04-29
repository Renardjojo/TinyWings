﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextSliderLink : MonoBehaviour
{
    private Text m_text;
    
    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    public void SetTextWithRoundFloat (float sliderValue)
    {
        m_text.text = System.Math.Round(sliderValue, 1).ToString();
    }
    
    public void SetTextWithFloat (float sliderValue)
    {
        m_text.text = sliderValue.ToString();
    }
}
