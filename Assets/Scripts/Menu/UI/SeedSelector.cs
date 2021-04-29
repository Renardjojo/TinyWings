using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SeedSelector : MonoBehaviour
{
    [SerializeField] protected Button m_minusButton;
    [SerializeField] protected Text m_label;
    [SerializeField] protected Button m_plusButton;
    protected int m_index = 0;
    
    protected virtual void Start()
    {
        m_minusButton.onClick.AddListener(DecrementLabel);
        m_plusButton.onClick.AddListener(IncrementLabel);
        
        m_index = PlayerPrefs.GetInt("Seed", 0);
        m_label.text = m_index.ToString();
    }
    
    public void IncrementLabel()
    {
        ++m_index;
        PlayerPrefs.SetInt("Seed", m_index);
        m_label.text = m_index.ToString();
    }

    public void DecrementLabel()
    {
        if (--m_index < 0)
        {
            m_index = 0;
        }

        PlayerPrefs.SetInt("Seed", m_index);
        m_label.text = m_index.ToString();
    }
}
