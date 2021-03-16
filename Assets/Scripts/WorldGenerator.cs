using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    
    [SerializeField]
    private float minScale = 8.0f;

    [SerializeField]
    private float maxScale = 16.0f;


    void Start()
    {
        switch(generationType)
        {
            case EGenerationType.SINUSOIDE:
                GenerateRandomChunks();
                break;
        }
    }
    
    private void GenerateRandomChunks()
    {
        float offsetX = 0.0f;
        float offsetY = 0.0f;
        
        for (int i = 0; i < 10; i++)
        {
            float height = Random.Range(minScale, maxScale);
            float width = Random.Range(minScale, maxScale);

            EInflexionType inflexionType = (EInflexionType)Random.Range(0, (int)EInflexionType.COUNT);
            EType fuctionType = (EType)Random.Range(0, (int)EType.COUNT);

            if (inflexionType == EInflexionType.DESCANDANTE)
            {
                offsetY -= height;
            }

            Rect dimension = new Rect( offsetX, offsetY, width, height);
            
            if (inflexionType == EInflexionType.ASCENDANTE)
            {
                offsetY += height;
            }

            offsetX += width;
            
            GameObject chunkGO = Instantiate(chunkPrefab);
            Chunk chunk = chunkGO.GetComponent<Chunk>();

            Assert.IsNotNull(chunk, "chunk component not found");

            chunk.Apply(fuctionType, inflexionType, dimension);
        }
    }
}

