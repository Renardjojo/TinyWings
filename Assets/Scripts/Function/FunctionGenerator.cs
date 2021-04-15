﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionGenerator
{
    public static Vector2[] DescSinusoide(Rect dim, int pow)
    {
        Sinusoide sinusois = new Sinusoide(dim, EInflexionType.DESCANDANTE, pow);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, sinusois.image(x));
        }

        return rst;
    }
    
    public static Vector2[] AcsSinusoide(Rect dim, int pow)
    {
        Sinusoide sinusois = new Sinusoide(dim, EInflexionType.ASCENDANTE, pow);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, sinusois.image(x));
        }

        return rst;
    }
    
    public static Vector2[] DescPolynome(Rect dim)
    {
        Polynome polynome = new Polynome(dim, EInflexionType.DESCANDANTE);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, polynome.image(x));
        }

        return rst;
    }
    
    public static Vector2[] AcsPolynone(Rect dim)
    {
        Polynome polynome = new Polynome(dim, EInflexionType.ASCENDANTE);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, polynome.image(x));
        }

        return rst;
    }
    
    public static Vector2[] DescHTan(Rect dim)
    {
        HyperbolicTangeante funct = new HyperbolicTangeante(dim, EInflexionType.DESCANDANTE);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, funct.image(x));
        }

        return rst;
    }
    
    public static Vector2[] AcsHTan(Rect dim)
    {
        HyperbolicTangeante funct = new HyperbolicTangeante(dim, EInflexionType.ASCENDANTE);
        
        Vector2[] rst = new Vector2[50];

        float interval = dim.width / (rst.Length - 1);
        
        for (int i = 0; i < rst.Length; i++)
        {
            float x = dim.xMin + i * interval;
            rst[i] = new Vector2(x, funct.image(x));
        }

        return rst;
    }
}



