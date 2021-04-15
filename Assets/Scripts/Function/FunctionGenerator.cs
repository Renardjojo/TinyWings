using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionGenerator
{
    public static Vector2[] DescSinusoide(Rect dim)
    {
        float amplitude = dim.height / 2;
        float vOffSet = amplitude + dim.yMin;
        float pulsation = Mathf.PI / dim.width;
        float phase = Mathf.PI / 2 - dim.xMin * pulsation;
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, vOffSet + amplitude * Mathf.Sin(pulsation * x + phase));
        }

        return rst;
    }
    
    public static Vector2[] AcsSinusoide(Rect dim)
    {
        float amplitude = dim.height / 2;
        float vOffSet = amplitude + dim.yMin;
        float pulsation = Mathf.PI / dim.width;
        float phase = -Mathf.PI / 2 - dim.xMin * pulsation;
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, vOffSet + amplitude * Mathf.Sin(pulsation * x + phase));
        }

        return rst;
    }

    public static Vector2[] DescElliptique(Rect dim)
    {
        float vOffSet = dim.yMax - dim.yMin;

        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);

        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.x + i * interval;

            if(x < dim.width / 2)
                rst[i] = new Vector2(x, dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow((Mathf.Sqrt(3) * (x - dim.xMin)) / (dim.xMax - dim.xMin), 2)));
            else
                rst[i] = new Vector2(x, dim.yMax - vOffSet * Mathf.Sqrt(1 - Mathf.Pow((Mathf.Sqrt(3) * (x - dim.xMax)) / (dim.xMax - dim.xMin), 2)));
        }

        return rst;
    }

    public static Vector2[] AscElliptique(Rect dim)
    {
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);

        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.x + i * interval;

            if (x < dim.width / 2)
            {
                float vOffSet = dim.yMin - dim.yMax;

                rst[i] = new Vector2(x, dim.yMax + vOffSet * Mathf.Sqrt(1 - Mathf.Pow((Mathf.Sqrt(3) * (x - dim.xMin)) / (dim.xMax - dim.xMin), 2)));
            }
            else
            {
                float vOffSet = dim.yMax - dim.yMin;

                rst[i] = new Vector2(x, dim.yMin + vOffSet * Mathf.Sqrt(1 - Mathf.Pow((Mathf.Sqrt(3) * (x - dim.xMax)) / (dim.xMax - dim.xMin), 2)));
            }
        }

        return rst;
    }
}

