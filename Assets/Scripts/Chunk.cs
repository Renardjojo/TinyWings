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

[RequireComponent(typeof(EdgeCollider2D))]
public class Chunk : MonoBehaviour
{
    private Vector2[] points;

    private void Start()
    {
        Apply(EType.SINUSOIDE, EInflexionType.ASCENDANTE, new Rect(5, 5, 10, 10));
    }

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
