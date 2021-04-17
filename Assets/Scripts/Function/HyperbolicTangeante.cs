using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HyperbolicTangeante : Function
{
    float m_a;
    float m_b;
    float m_kprime;
    float m_k;
    float m_alpha;

    public HyperbolicTangeante(Rect dim, EInflexionType type)
        : base(dim, type)
    {
        const float espilone = 0.01f;
        m_kprime = m_k = dim.height / 2f;
        m_a = m_k + dim.yMin;
        m_b = dim.width / 2f + dim.xMin;

        float step = 1/dim.width * 1/dim.height;
        
        m_alpha = step;
        
        while (Mathf.Abs(image(dim.xMin) - dim.yMax) > espilone) //tanh is pair
        {
            m_alpha += step; 
        }
        
        m_kprime += Mathf.Abs(image(dim.xMin) - dim.yMax);
        m_alpha *= ((int) type * 2 - 1);
    }
    
    public override float image(float x)
    {
        float exp =  Mathf.Exp(m_alpha * m_k * (x - m_b));
        return m_a + m_kprime * (1 - exp) / (1 + exp);
    }

    public override Vector2 normal (float x)
    {
        return Vector2.one;
    }

    public override float derivative(float x, int n)
    {
        return 0f;
    }
}