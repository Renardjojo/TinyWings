using UnityEngine;
using UnityEngine.Assertions;

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
        
        //Only compute constants with rect base on origin. Avoid float imprecision with large xMin and yMin values and optimize computation
        //Also allow use to base shader on origin (avoid to send minX and minY)
        m_pow = pow;
        m_amplitude = dim.height / (powOdd + 1);
        m_vOffSet = powOdd * m_amplitude; // instead of : powOdd * m_amplitude + m_dim.yMin
        m_pulsation = Mathf.PI / ((dim.width * 2) / (powOdd + 1));
        m_phase = (powOdd == 1 ? sign * Mathf.PI / 2 : (2 *  Mathf.PI) / (sign + 3)); // instead of (powOdd == 1 ? sign * Mathf.PI / 2 : (2 *  Mathf.PI) / (sign + 3)) - dim.xMin * m_pulsation;
    }

    public override void sendDataToShader(Material mat)
    {
        base.sendDataToShader(mat);
        Assert.IsTrue(mat.HasProperty("_Amplitude")); 
        Assert.IsTrue(mat.HasProperty("_VOffset")); 
        Assert.IsTrue(mat.HasProperty("_Pulsation")); 
        Assert.IsTrue(mat.HasProperty("_Phase")); 
        Assert.IsTrue(mat.HasProperty("_Pow")); 
        
        mat.SetFloat("_Amplitude", m_amplitude);
        mat.SetFloat("_VOffset", m_vOffSet);
        mat.SetFloat("_Pulsation", m_pulsation);
        mat.SetFloat("_Phase", m_phase);
        mat.SetFloat("_Pow", m_pow);
    }
    
    public override float image(float x)
    {
        //Constants is based on rect located on the origin. Avoid float imprecision with large xMin and yMin values.
        //Also allow use to base shader on origin (avoid to send minX and minY)
        x -= m_dim.xMin;
        return m_dim.yMin + m_vOffSet + m_amplitude * Mathf.Pow(Mathf.Sin(m_pulsation * x + m_phase), m_pow);
    }

    public override float derivative(float x, int n)
    {
        float sum = 0f;
        float coef = m_amplitude / Mathf.Pow(2, m_pow - 1f);
        x -= m_dim.xMin;
        
        if (m_pow % 2 == 0)
        {
            for (int k = 0; k <= (m_pow - 2) / 2f; ++k)
            {
                sum += Mathf.Pow(-1, k) * Cbin(m_pow, k) * Mathf.Pow(m_pulsation * (m_pow - 2 * k), n) *
                       Mathf.Cos((m_pow - 2 * k) * (x * m_pulsation + m_phase) + n * Mathf.PI / 2f);
            }

            coef *= Mathf.Pow(-1, m_pow / 2f);
        }
        else
        {
            for (int k = 0; k <= (m_pow - 1) / 2f; ++k)
            {
                sum += Mathf.Pow(-1, k) * Cbin(m_pow, k) * Mathf.Pow(m_pulsation * (m_pow - 2 * k), n) *
                       Mathf.Sin((m_pow - 2 * k) * (x * m_pulsation + m_phase) + n * Mathf.PI / 2f);
            }

            coef *= Mathf.Pow(-1, (m_pow - 1) / 2f);
        }

        return coef * sum;
    }
}
