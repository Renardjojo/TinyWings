using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elliptical : Function
{
    float vOffSet = 0f;
    float k;
    float s;

    public Elliptical(Rect dim, EInflexionType type)
        : base(dim, type)
    {
        k = dim.xMax - dim.xMin;
        s = Mathf.Sqrt(3);
    }

    public override float derivative(float x, int n)
    {
        return 0f;
    }

    public override float image(float x)
    {
        Debug.Log(x);
        if (m_type == EInflexionType.DESCANDANTE)
        {
            vOffSet = m_dim.yMax - m_dim.yMin;
            
            if (x < (m_dim.xMin + m_dim.xMax) / 2)
                return m_dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(s * (x - m_dim.xMin) / k, 2));
            else
                return m_dim.yMax - vOffSet * Mathf.Sqrt(1 - Mathf.Pow(s * (x - m_dim.xMax) / k, 2));
        }
        else
        {
            if (x < (m_dim.xMin + m_dim.xMax) / 2)
            {
                vOffSet = m_dim.yMin - m_dim.yMax;
                return m_dim.yMax + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(s * (x - m_dim.xMin) / k, 2));
            }
            else
            {
                vOffSet = m_dim.yMax - m_dim.yMin;
                return m_dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(s * (x - m_dim.xMax) / k, 2));
            }
        }
    }

    public override Vector2 normal(float x)
    {
        return Vector2.one;
    }
}
