﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFloatLink : MonoBehaviour
{
    private Text m_text;
    
    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    public void SetTextWithRoundFloat (float value, int prescision = 1)
    {
        m_text.text = System.Math.Round(value, prescision).ToString();
    }
    
    public void SetTextWithFloat (float value)
    {
        m_text.text = value.ToString();
    }
}
