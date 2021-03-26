using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinusoide : Function
{
    float m_amplitude;
    float m_vOffSet;
    float m_pulsation;
    float m_phase;
    int m_pow;

    public Sinusoide(Rect dim, EInflexionType type, int pow)
        : base(dim, type)
    {
        int sign = (int)type * 2 - 1;
        int powOdd = pow % 2;
        
        m_pow = pow;
        m_amplitude = dim.height / (powOdd + 1);
        m_vOffSet = powOdd * m_amplitude + dim.yMin;
        m_pulsation = Mathf.PI / ((dim.width * 2) / (powOdd + 1));
        m_phase = (powOdd == 1 ? sign * Mathf.PI / 2 : (2 *  Mathf.PI) / (sign + 3)) - dim.xMin * m_pulsation;
    }
    
    public override float image(float x)
    {
        return m_vOffSet + m_amplitude * Mathf.Pow(Mathf.Sin(m_pulsation * x + m_phase), m_pow);
    }
    
    public override Vector2 tangeante(float x)
    {
        return Vector2.one;
    }
    
    public override Vector2 normal (float x)
    {
        return Vector2.one;
    }
    
    public override float derivative(float x, int n)
    {
        return 0.0f;
    }
}
