using System;
using System.Linq.Expressions;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

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

    public virtual void sendDataToShader(Material mat)
    {
        Assert.IsTrue(mat.HasProperty("_Dim"));
        mat.SetVector("_Dim", new Vector4(m_dim.xMin, m_dim.yMin, m_dim.xMax, m_dim.yMax));
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
    
    public int Factorial(int n)
    {
        if (n == 1)
            return 1;

        return n * Factorial(n - 1);
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
    
    public static float map(float v, float min1, float max1, float min2, float max2)
    {
        return min2 + (v- min1)*(max2-min2)/(max1 - min1);
    }
}