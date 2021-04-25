using UnityEngine;

public class Elliptical : Function
{
    float vOffSet = 0f;
    float k;
    float sqr3;

    public Elliptical(Rect dim, EInflexionType type)
        : base(dim, type)
    {
        k = dim.xMax - dim.xMin;
        sqr3 = Mathf.Sqrt(3);
    }

    public override void sendDataToShader(Material mat)
    {
        //base.sendDataToShader(mat);
        mat.SetVector("_Dim", new Vector4(m_dim.xMin, m_dim.yMin, m_dim.xMax, m_dim.yMax));
        mat.SetFloat("_XMin", m_dim.xMin);
        mat.SetFloat("_XMax", m_dim.xMax);
        mat.SetFloat("_YMin", m_dim.yMin);
        mat.SetFloat("_YMax", m_dim.yMax);
        mat.SetFloat("_IsDesc", m_type == EInflexionType.DESCANDANTE ? 1 : 0);
    }

    public override float derivative(float x, int n)
    {
        if (n == 0)
            return image(x);
        else if (n == 1)
        {
            return ComputeFirstDerivate(x);
            /*//first part of the curve
            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
            {
                return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMin) /
                    (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
            }
            else
            {
                return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMax) /
                (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMax) / (m_dim.xMax - m_dim.xMin), 2))));
            }*/
        }
        else if (n == 2)
        {
            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
            {
                return 3 * (m_dim.yMax - m_dim.yMin) / (m_dim.xMax - m_dim.xMin) * (m_dim.xMax - m_dim.xMin) * (1 - Mathf.Pow(sqr3 * (x - m_dim.xMin) / k, 2)) * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMin) / k, 2));
            }
            else
            {
                return 3 * (m_dim.yMax - m_dim.yMin) / (m_dim.xMin - m_dim.xMax) * (m_dim.xMin - m_dim.xMax) * (1 - Mathf.Pow(sqr3 * (x - m_dim.xMax) / k, 2)) * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMax) / k, 2));
            }
        }

        return 0f;
    }

    public float ComputeFirstDerivate(float x)
    {
        if (m_type == EInflexionType.DESCANDANTE)
        {
            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
            {
                return 3 * (m_dim.yMin - m_dim.yMax) * (x - m_dim.xMin) /
                (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
            }
            else
            {
                return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMax) /
                (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMax) / (m_dim.xMax - m_dim.xMin), 2))));
            }
        }
        else
        {
            //first part of the curve
            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
            {
                return 3 * (m_dim.yMax - m_dim.yMin) * (x - m_dim.xMin) /
                    (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMin) / (m_dim.xMax - m_dim.xMin), 2))));
            }
            else
            {
                //return 0f;
                return 3 * (m_dim.yMin - m_dim.yMax) * (x - m_dim.xMax) /
                    (Mathf.Pow(m_dim.xMax - m_dim.xMin, 2) * Mathf.Sqrt(1 - (3 * Mathf.Pow((x - m_dim.xMax) / (m_dim.xMax - m_dim.xMin), 2))));
            }
        }

    }

    public override float image(float x)
    {
        if(m_type == EInflexionType.DESCANDANTE)
        {
            vOffSet = m_dim.yMax - m_dim.yMin;

            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
                return m_dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMin) / k, 2));
            else
                return m_dim.yMax - vOffSet * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMax) / k, 2));
        }
        else
        {
            if (x < (m_dim.xMax - m_dim.xMin) / 2f + m_dim.xMin)
            {
                vOffSet = m_dim.yMin - m_dim.yMax;
                return m_dim.yMax + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMin) / k, 2));
            }
            else
            {
                vOffSet = m_dim.yMax - m_dim.yMin;
                return m_dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow(sqr3 * (x - m_dim.xMax) / k, 2));
            }
        }
    }
}
