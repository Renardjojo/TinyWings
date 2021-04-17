using System;
using System.Linq.Expressions;
using UnityEngine;

[SerializeField]
public enum EInflexionType
{
    ASCENDANTE = 0,
    DESCANDANTE = 1,
    COUNT = 2
}

public abstract class Function
{
    protected Rect m_dim;
    protected EInflexionType m_type;

    protected Function(Rect dim, EInflexionType type)
    {
        m_dim = dim;
        m_type = type;
    }

    public abstract float image(float x);

    public Vector2 tangeante(float x)
    {
        Vector2 normVec = normal(x);
        return new Vector2(normVec.y, normVec.x); //3pi/2 rotation
    }
    
    public abstract Vector2 normal (float x);
    public abstract float derivative(float x, int n);
    
    /*
     * @brief : Return area bellow function thanks to median rectangles method
     */
    public static float[] IntervalRectMed(Func<float, float> funct, float from, float to, int step)
    {
        float[] rst = new float[step];
        float h = (to - from) / step;

        float xi = from;
        for (int i = 0; i < step; ++i)
        {
            rst[i] = funct(xi + h / 2f) * h;
            xi += h;
        }

        return rst;
    }
    
    public static float map(float v, float min1, float max1, float min2, float max2)
    {
        return min2 + (v- min1)*(max2-min2)/(max1 - min1);
    }
}