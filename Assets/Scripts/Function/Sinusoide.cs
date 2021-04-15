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

    public override Vector2 normal (float x)
    {
        return Vector2.one;
    }

    public int Cbin(int n, int k)
    {
        int res = 1;
        for (int i = n - k + 1; i <= n; ++i)
            res *= i;
        for (int i = 2; i <= k; ++i)
            res /= i;
        return res;
    }

    public override float derivative(float x, int n)
    {
        float sum = 0f;
        float coef = 0f;

        if (m_pow % 2 == 0)
        {
            for (int k = 0; k < (m_pow - 2) / 2; ++k)
            {
                sum = Mathf.Pow((-1), k) * Cbin(m_pow, k) * Mathf.Pow((n - 2 * k), n) * Mathf.Cos((n - 2) * x) +
                      n * Mathf.PI / 2;
            }

            coef = (-1) ^ (m_pow / 2) / 2 ^ (m_pow - 1);
        }
        else
        {
            for (int k = 0; k < (m_pow - 1) / 2; ++k)
            {
                sum = Mathf.Pow((-1), k) * Cbin(m_pow, k) * Mathf.Pow((n - 2 * k), n) * Mathf.Sin((n - 2) * x) +
                      n * Mathf.PI / 2;
            }

            coef = (-1) ^ ((m_pow - 1) / 2) / 2 ^ (m_pow - 1);
        }

        return coef * sum;
    }
}
