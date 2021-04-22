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

        if (m_type == EInflexionType.DESCANDANTE)
            vOffSet = m_dim.yMax - m_dim.yMin;
    }

    public override float derivative(float x, int n)
    {
        if(n == 1)
        {
            if (m_type == EInflexionType.DESCANDANTE)
            {
                //first part of the curve
                if(x < (m_dim.xMin + m_dim.xMax) / 2)
                {
                    return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMin) /
                        (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
                }
                else
                {
                    return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMin) /
                        (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
                }
            }
            else
            {
                if (x < (m_dim.xMin + m_dim.xMax) / 2)
                {
                    return 3 * (m_dim.yMin - m_dim.yMax) * (x - m_dim.xMin) /
                    (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
                }
                else
                {
                    return 3 * (m_dim.yMax - m_dim.yMax) * (x - m_dim.xMax) /
                    (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMax) / (m_dim.xMax - m_dim.xMin), 2))));
                }
            }
        }

        return 0f;
    }

    public override float image(float x)
    {
        if (m_type == EInflexionType.DESCANDANTE)
        {
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
