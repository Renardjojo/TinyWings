using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum EInflexionType
{
    ASCENDANTE,
    DESCANDANTE,
    COUNT
}

[SerializeField]
public enum EType
{
    SINUSOIDE,
    COUNT
}

[RequireComponent(typeof(EdgeCollider2D))]
public class Chunk : MonoBehaviour
{
    private Vector2[] points;
    
    public void Apply(EType functionType, EInflexionType inflexionType, Rect dimension)
    {
        transform.GetChild(0).position = new Vector3(dimension.x, dimension.y, 0);
        transform.GetChild(0).localScale = new Vector3(dimension.width, dimension.height, 0);
        
        switch (inflexionType)
        {
            case EInflexionType.ASCENDANTE:

                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        
                        points = FunctionGenerator.AcsSinusoide(dimension);
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            case EInflexionType.DESCANDANTE:
                
                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        
                        points = FunctionGenerator.DescSinusoide(dimension);
                        
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
