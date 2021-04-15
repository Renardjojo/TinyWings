using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        m.SetRow(0, new Vector4(Mathf.Pow(dim.xMin,3), Mathf.Pow(dim.xMin,2), dim.xMin, 1f));
        m.SetRow(1, new Vector4(Mathf.Pow(dim.xMax,3), Mathf.Pow(dim.xMax,2), dim.xMax, 1f));
        m.SetRow(2, new Vector4(3f * Mathf.Pow(dim.xMin,2), 2f * dim.xMin, 1f, 0f));
        m.SetRow(3, new Vector4(3f * Mathf.Pow(dim.xMax,2), 2f * dim.xMax, 1f, 0f));

        // Two solution to inverse : Change constante or inverse sign. I think compute constante if better (only once and not each call)
        Vector4 x = new Vector4(type == EInflexionType.ASCENDANTE ? dim.yMin : dim.yMax,
                                type == EInflexionType.ASCENDANTE ? dim.yMax : dim.yMin, 0f, 0f);

        Vector4 b = m.inverse * x;
        m_a = b.x;
        m_b = b.y;
        m_c = b.z;
        m_d = b.w;
    }
    
    public override float image(float x)
    {
        return m_a * Mathf.Pow(x,3) + m_b * Mathf.Pow(x, 2) + m_c * x + m_d;
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
