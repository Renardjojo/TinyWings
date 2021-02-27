using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum EInflexionType
{
    ASCENDANTE,
    DESCANDANTE
}

[SerializeField]
public enum EType
{
    SINUSOIDE
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
public class Chunk : MonoBehaviour
{
    private Vector2[] points;
    private IFunction function;

    public void Apply(EType functionType, EInflexionType inflexionType, Rect dimension)
    {
        switch (inflexionType)
        {
            case EInflexionType.ASCENDANTE:

                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            case EInflexionType.DESCANDANTE:
                
                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GetComponent<EdgeCollider2D>().points = points;
    }
}
