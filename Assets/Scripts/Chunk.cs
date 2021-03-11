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

[ExecuteInEditMode ,RequireComponent(typeof(EdgeCollider2D))]
public class Chunk : MonoBehaviour
{
    private Vector2[] points;
    [SerializeField] private Material m_Material;
    private EType m_functionType;
    private EInflexionType m_inflexionType;
    Rect m_dimension;

    public void Awake()
    {
        m_Material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }
    
    public void Apply(EType functionType, EInflexionType inflexionType, Rect dimension)
    {
        m_functionType = functionType;
        m_inflexionType = inflexionType;
        m_dimension = dimension;
        
        transform.GetChild(0).position = new Vector3(dimension.x + dimension.width / 2, dimension.y + dimension.height / 2, 0);
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

    // Update is called once per frame
    void Update()
    {
        switch (m_inflexionType)
        {
            case EInflexionType.ASCENDANTE:

                switch (m_functionType)
                {
                    case EType.SINUSOIDE:
                        
                        m_Material.SetVector("_Dim", new Vector4(m_dimension.xMin, m_dimension.yMin, m_dimension.xMax, m_dimension.yMax));
                        m_Material.SetFloat("_isDesc", 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            case EInflexionType.DESCANDANTE:
                
                switch (m_functionType)
                {
                    case EType.SINUSOIDE:
                        
                        m_Material.SetVector("_Dim", new Vector4(m_dimension.xMin, m_dimension.yMin, m_dimension.xMax, m_dimension.yMax));
                        m_Material.SetFloat("_isDesc", 1);
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
       
    }
}
