using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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
        m_alpha *=  ((int) type * 2 - 1);
    }
    
    public override void sendDataToShader(Material mat)
    {
        base.sendDataToShader(mat);
        mat.SetFloat("_A", m_a);
        mat.SetFloat("_B", m_b);
        mat.SetFloat("_Kp", m_kprime);
        mat.SetFloat("_K", m_k);
        mat.SetFloat("_Alpha", m_alpha);
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
        Assert.IsTrue(n > 0 && n < 3);

        switch (n)
        {
            case 0:
                return image(x);
            case 1:
            {
                float expVal = Mathf.Exp(m_k * m_alpha * (x - m_b));
                return m_kprime * -2 * m_k * m_alpha * expVal / ((1 + expVal) * (1 + expVal));
            }
            case 2:
            {
                //d for derivative. Expression bollow with b / a^2
                float expVal = Mathf.Exp(m_k * m_alpha * (x - m_b));
                float a = 1 + expVal;
                float da = m_k * m_alpha * expVal;
                float sqrA = a * a;
                float dSqrA = 2 * a * da;
                float b = -2 * m_k * m_alpha * expVal;
                float db = -2 * m_k * m_k * m_alpha * m_alpha * expVal;
                return m_kprime * (sqrA * db - b * dSqrA) / (sqrA * sqrA);
            }
            default:
                return 0f;
        }
    }
}