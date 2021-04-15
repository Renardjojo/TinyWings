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
    CIRCLE,
    COUNT
}

[ExecuteInEditMode ,RequireComponent(typeof(EdgeCollider2D))]
public class Chunk : MonoBehaviour
{
    private Vector2[] points;
    [SerializeField] private Material m_Material;
    public EType m_functionType;
    public EInflexionType m_inflexionType;
    public Rect m_dimension;

    public void Awake()
    {
        m_Material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }
    
    public void Apply(EType functionType, EInflexionType inflexionType, Rect dim)
    {
        m_functionType = functionType;
        m_inflexionType = inflexionType;
        m_dimension = dim;
        
        transform.GetChild(0).position = new Vector3(m_dimension.x + m_dimension.width / 2, m_dimension.y + m_dimension.height / 2, 0);
        transform.GetChild(0).localScale = new Vector3(m_dimension.width, m_dimension.height, 0);
        
        switch (inflexionType)
        {
            case EInflexionType.ASCENDANTE:

                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        
                        points = FunctionGenerator.AcsSinusoide(m_dimension);
                        
                        break;
                    case EType.CIRCLE:

                        //points = FunctionGenerator.AscElliptique(m_dimension);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                break;
            case EInflexionType.DESCANDANTE:
                
                switch (functionType)
                {
                    case EType.SINUSOIDE:
                        
                        points = FunctionGenerator.DescSinusoide(m_dimension);
                        
                        break;

                    case EType.CIRCLE:

                        points = FunctionGenerator.DescElliptique(m_dimension);

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

                    case EType.CIRCLE:

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
