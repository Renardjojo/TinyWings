using UnityEngine;
using UnityEngine.Assertions;

public class HyperbolicTangeante : Function
{
    //float m_a; //A is same as K
    float m_b;
    float m_kprime;
    float m_k;
    float m_alpha;

    public HyperbolicTangeante(Rect dim, EInflexionType type)
        : base(dim, type)
    {
        //Only compute constants with rect base on origin. Avoid float imprecision with large xMin and yMin values and optimize computation
        //Also allow use to base shader on origin (avoid to send minX and minY)
       
        m_kprime = m_k = dim.height / 2f;
        //m_a = m_k; //instead of : m_k + dim.yMin
        m_b = dim.width / 2f; //instead of : dim.width / 2f + dim.xMin

        //Optimize alpha start and step in function of dimension ratio (hack, do not represente anything but it work)
        float step = 1 / dim.width * 1 / dim.height;
        m_alpha = step;
        
        const float espilone = 0.01f;
        while (Mathf.Abs(image(dim.xMin) - dim.yMax) > espilone) //tanh is pair, so optimize research only on one side
        {
            m_alpha += step; 
        }
        
        m_kprime += Mathf.Abs(image(dim.xMin) - dim.yMax); //Add epsilon to make sur that the curve respect xMin == yMin or xMin = yMax
        m_alpha *=  ((int) type * 2 - 1); //Add sign in function of desired inflexion
    }
    
    public override void sendDataToShader(Material mat)
    {
        base.sendDataToShader(mat);
        //mat.SetFloat("_A", m_a); //A is same as K
        mat.SetFloat("_B", m_b);
        mat.SetFloat("_Kp", m_kprime);
        mat.SetFloat("_K", m_k);
        mat.SetFloat("_Alpha", m_alpha);
    }
    
    public override float image(float x)
    {
        //Constants is based on rect located on the origin. Avoid float imprecision with large xMin and yMin values.
        //Also allow use to base shader on origin (avoid to send minX and minY)
        x -= m_dim.xMin;
        
        float exp =  Mathf.Exp(m_alpha * m_k * (x - m_b));
        //A is same as K for rect on origine
        return m_dim.yMin + m_k + m_kprime * (1 - exp) / (1 + exp);
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
                x -= m_dim.xMin;
                float expVal = Mathf.Exp(m_k * m_alpha * (x - m_b));
                return m_kprime * -2 * m_k * m_alpha * expVal / ((1 + expVal) * (1 + expVal));
            }
            case 2:
            {
                x -= m_dim.xMin;
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