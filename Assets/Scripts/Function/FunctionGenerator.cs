using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionGenerator
{
    public static Vector2[] DescSinusoide(Rect dim)
    {
        float amplitude = dim.height/ 2;
        float vOffSet = amplitude + dim.yMin;
        float pulsation = Mathf.PI / dim.width;
        float phase = Mathf.PI / 2 - dim.yMin * pulsation;
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x - dim.xMin, vOffSet + amplitude * Mathf.Sin(pulsation * x + phase) - dim.yMin );
        }

        return rst;
    }
    
    public static Vector2[] AcsSinusoide(Rect dim)
    {
        float amplitude = dim.height/ 2;
        float vOffSet = amplitude + dim.yMin;
        float pulsation = Mathf.PI / dim.width;
        float phase = -Mathf.PI / 2 - dim.yMin * pulsation;
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x - dim.xMin, vOffSet + amplitude * Mathf.Sin(pulsation * x + phase) - dim.yMin );
        }

        return rst;
    }
}

