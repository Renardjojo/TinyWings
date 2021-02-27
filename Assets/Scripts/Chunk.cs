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
    private Vector2[] point;
    private IFunction function;
    
    public EType functionType;
    public EInflexionType inflexionType;
    public Rect dimension;

    void Apply()
    {
        
    }
}
