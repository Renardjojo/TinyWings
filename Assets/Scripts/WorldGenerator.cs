using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum EGenerationType
{
    SINUSOIDE,
    RANDOM
}

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    
    public EGenerationType generationType;
    private Chunk[] chunks; // queue

    // Update is called once per frame
    void Update()
    {
        
    }
}
