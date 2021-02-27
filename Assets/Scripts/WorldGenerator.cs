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

    [SerializeField]
    private float maxHeight = 0.0f;

    void Start()
    {
        switch(generationType)
        {
            case EGenerationType.RANDOM:
                GenerateRandomChunks();
                break;
            case 0:
                break;
        }
    }
    
    private void GenerateRandomChunks()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            Vector3 pos = Vector3.zero;
            
            Instantiate(chunkPrefab, pos, Quaternion.identity);
        }
    }
}

