using UnityEngine;
using UnityEngine.Assertions;

public class Polynome : Function
{
    float m_a;
    float m_b;
    float m_c;
    float m_d;

    public Polynome(Rect dim, EInflexionType type)
        : base(dim, type)
    {
        Matrix4x4 m = Matrix4x4.identity;

        //Only compute matrix with rect base on origin. Avoid float imprecision with large xMin and yMin values
        //Also allow use to base shader on origin (avoid to send minX and minY)
        m.SetRow(0, new Vector4(0f, 0f, 0f, 1f)); // instead of Mathf.Pow(dim.xMin,3), Mathf.Pow(dim.xMin,2), dim.xMin, 1f)
        m.SetRow(1, new Vector4(dim.width * dim.width * dim.width, dim.width * dim.width, dim.width, 1f)); // instead of Mathf.Pow(dim.xMax,3), Mathf.Pow(dim.xMax,2), dim.xMax, 1f)
        m.SetRow(2, new Vector4(0f, 0f, 1f, 0f)); // instead of 3f * Mathf.Pow(dim.xMin,2), 2f * dim.xMin, 1f, 0f)
        m.SetRow(3, new Vector4(3f * dim.width * dim.width, 2f * dim.width, 1f, 0f)); // instead of 3f * Mathf.Pow(dim.xMax,2), 2f * dim.xMax, 1f, 0f)

        // Two solution to inverse : Change constante or inverse sign. I think compute constante if better (only once and not each call)
        Vector4 x = new Vector4(type == EInflexionType.ASCENDANTE ? 0f : dim.height,
                                type == EInflexionType.ASCENDANTE ? dim.height : 0f, 0f, 0f);

        Vector4 b = m.inverse * x;
        Assert.IsFalse(b.sqrMagnitude == 0f);
        
        m_a = b.x;
        m_b = b.y;
        m_c = b.z;
        m_d = b.w;
    }
    
    public override void sendDataToShader(Material mat)
    {
        base.sendDataToShader(mat);
        mat.SetFloat("_A", m_a);
        mat.SetFloat("_B", m_b);
        mat.SetFloat("_C", m_c);
        mat.SetFloat("_D", m_d);
    }
    
    public override float image(float x)
    {
        //Constants is based on rect located on the origin. Avoid float imprecision with large xMin and yMin values.
        //Also allow use to base shader on origin (avoid to send minX and minY)
        x -= m_dim.xMin;
        //Use horner method : https://en.wikipedia.org/wiki/Horner%27s_method. That allow fast image computation
        return m_dim.yMin + ((m_a * x + m_b) * x + m_c) * x + m_d;
        
        // Version without (Cost of power is not negligeable)
        // return m_a * Mathf.Pow(x,3) + m_b * Mathf.Pow(x, 2) + m_c * x + m_d;
    }

    public override float derivative(float x, int n)
    {
        Assert.IsTrue(n > 0 && n < 4);

        switch (n)
        {
            case 0:
                return image(x);
            case 1:
                x -= m_dim.xMin;
                return (3f * m_a * x + 2f * m_b) * x + m_c;
            case 2:
                x -= m_dim.xMin;
                return 6f * m_a * x + 2f * m_b;
            case 3:
                return 6f * m_a;
            default:
                return 0;
        }
    }
}
