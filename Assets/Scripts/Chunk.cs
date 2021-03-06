﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[SerializeField]
public enum EType
{
    SINUSOIDE,
    POLYNOME,
    HYPBERBOLIC_TAN,
    ELLIPTIC,
    COUNT
}

public enum EChunkType
{
    NONE,
    SUGAR,
    GIFT,
    FOREST,
    COUNT
}

[ExecuteInEditMode]
public class Chunk : MonoBehaviour
{
    private Vector2[] m_points;
    private Material m_Material;
    public EType m_functionType; // only for display in editor
    public EChunkType m_chunkType;
    public EInflexionType m_inflexionType;
    public Rect m_dimension;
    public Function m_funct;

    public Material m_TanHMat;
    public Material m_SinMat;
    public Material m_PolynomeMat;
    public Material m_EllipticMat;

    public GameObject m_BarleySugarPrefab;
    public GameObject m_GiftPrefab;
    public GameObject m_TreePrefab;

    Transform m_surface;
    Transform m_ground;

    [Header("Settings")]
    [SerializeField] protected const int m_resolution = 20; // represente the number of chunk inside width of function
    [SerializeField] protected const int m_pointCount = 50; //must be pair 
    
    void Awake()
    {
        m_surface = transform.Find("Surface");
        m_ground = transform.Find("Ground");

        m_Material = m_surface.GetComponent<MeshRenderer>().material;
    }
    
    List<Vector2> generatePoints()
    {
        List<Vector2> points = new List<Vector2>();

        //Use integral to obtain point density in portion of curve.
        float[] pointsInSubFunct = Function.IntervalRectMed(functX => { return Mathf.Abs(m_funct.derivative(functX, 2)/ Mathf.Pow(1 + Mathf.Pow(m_funct.derivative(functX, 1), 2), 3/2f));}, m_dimension.xMin, m_dimension.xMax, m_resolution);

        float totalPointDensity = 0f;
        foreach (var pointCount in pointsInSubFunct)
        {
            totalPointDensity += pointCount;
        }

        // map max point to maxPointCount than to coef
        float densityScale = (m_pointCount - 2) / totalPointDensity; // -2 because we know value of first and last points
        
        for (int i = 0; i < pointsInSubFunct.Length; ++i)
        {
            pointsInSubFunct[i] *= densityScale;
        }
        
        //Build points
        float x = m_dimension.xMin;
        float rest = 0f; // pointCount is in float but represente an integer. The rest must be save and apply for the next chunk

        points.Add(new Vector2(m_dimension.xMin, m_inflexionType == EInflexionType.ASCENDANTE ? m_dimension.yMin :  m_dimension.yMax));

        // the calcul allow use to obtain perfect number of point in function of curvation (or density poite per zone)
        // for example considere 3 zone with each own pointe density for a width of 75:
        // |__2.2___|___1.3____|___6.5___|
        // We gonna apply this formula :
        // 75/3/2.2 * (2 + 0.2) + 75/3/2.2 * (1 + 0.3) + 75/3/2.2 * (7 - 0.5)
        // the third expression use 7 and not 6 because we will use the rest of the previous expression (0.2 + 0.3 + 0.5 + 6 = 7) and report the point here
        //We need apply it because density of point is not an integer but a real and we cannot round it if we want exact number of point
        float portionSize = m_dimension.width / (float) (pointsInSubFunct.Length);
        foreach (var pointCount in pointsInSubFunct) //portion
        {
            float currentPointDensity = pointCount + rest;
            float step = portionSize / pointCount;

            float additionnalStepWithRest = 0f;
            if ((int) currentPointDensity == (int)pointCount)
            {
                additionnalStepWithRest = step * (pointCount - (int)pointCount);
            }
            else
            {
                additionnalStepWithRest = step * (pointCount - (int) currentPointDensity);
            }            
            
            for (int i = 0; i < (int) currentPointDensity; ++i) //point number in portion
            {
                points.Add(new Vector2((float) x, (float) m_funct.image(x)));
                x += step;
            }

            x += additionnalStepWithRest;
            
            rest = currentPointDensity - (int)currentPointDensity; //if currentPointDensity == 3.5 rest == 0.5
        }
        points.Add(new Vector2(m_dimension.xMax, m_inflexionType == EInflexionType.ASCENDANTE ? m_dimension.yMax :  m_dimension.yMin));

        return points;
    }

    public void Apply(EType functionType, EInflexionType inflexionType, Rect dim, EChunkType chunkType = EChunkType.NONE)
    {
        m_functionType = functionType;
        m_inflexionType = inflexionType;
        m_dimension = dim;
        
        m_surface.position = new Vector3(m_dimension.x + m_dimension.width / 2, m_dimension.y + m_dimension.height / 2, 0);
        m_surface.localScale = new Vector3(m_dimension.width, m_dimension.height, 0);
        
        m_ground.position = new Vector3(m_dimension.x + m_dimension.width / 2, m_surface.position.y - m_ground.localScale.y / 2 - m_surface.localScale.y / 2, 0);
        m_ground.localScale = new Vector3(m_dimension.width, m_ground.localScale.y, 0);
        
        //Create function and compute constantes
        switch (functionType)
        {
            case EType.SINUSOIDE:
                m_funct = new Sinusoide(m_dimension, inflexionType, Random.Range(1, 11));
                m_surface.GetComponent<MeshRenderer>().material = new Material(m_SinMat);
                break;
            
            case EType.POLYNOME:
                m_funct = new Polynome(m_dimension, inflexionType);
                m_surface.GetComponent<MeshRenderer>().material = new Material(m_PolynomeMat);
                break;
            
            case EType.HYPBERBOLIC_TAN:
                m_funct = new HyperbolicTangeante(m_dimension, inflexionType);
                m_surface.GetComponent<MeshRenderer>().material = new Material(m_TanHMat);
                break;

            case EType.ELLIPTIC:
                m_funct = new Elliptical(m_dimension, inflexionType);
                m_surface.GetComponent<MeshRenderer>().material = new Material(m_EllipticMat);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        m_Material = m_surface.GetComponent<MeshRenderer>().material;
        m_funct.sendDataToShader(m_Material);
        
        //Generate points :
        m_points = generatePoints().ToArray();

        switch (chunkType)
        {
            case EChunkType.NONE:
                break;
            case EChunkType.SUGAR:
                addGameObjectOnChunkInStep(m_BarleySugarPrefab, Random.Range(2, 10));
                break;
            case EChunkType.GIFT:
                addGameObjectOnChunkInStep(m_GiftPrefab, Random.Range(2, 4));
                break;
            case EChunkType.FOREST:
                addGameObjectOnChunkInStep(m_TreePrefab, Random.Range(6, 10));
                break;
            case EChunkType.COUNT:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(chunkType), chunkType, null);
        }
    }

    void addGameObjectOnChunkInStep(GameObject prefab, int step)
    {
        for (int i = 0; i < step; ++i)
        {
            GameObject GO = Instantiate(prefab);
            GO.transform.SetParent(transform);
            GO.transform.up = m_funct.normal(m_dimension.xMin + i * m_dimension.width / step);
            GO.transform.position = new Vector3(m_dimension.xMin + i * m_dimension.width / step, m_funct.image(m_dimension.xMin + i * m_dimension.width / step), 0f);
            GO.transform.Translate( new Vector3(0f, GO.transform.localScale.y * GO.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2f, 0f), Space.Self);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // uncomment to test normal and tangeante
        /*
        foreach (var point in m_points)
        {
            Debug.DrawLine(new Vector3(point.x, point.y), new Vector3(point.x, point.y) + new Vector3(m_funct.normal(point.x).x, m_funct.normal(point.x).y) * 10f, Color.blue);
            Debug.DrawLine(new Vector3(point.x, point.y), new Vector3(point.x, point.y) + new Vector3(m_funct.tangeante(point.x).x, m_funct.tangeante(point.x).y) * 10f, Color.red);
        }
        */
    }
}
