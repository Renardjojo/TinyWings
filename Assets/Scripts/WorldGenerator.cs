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
            float height = Random.Range(2.0f, 8.0f);
            float width = Random.Range(2.0f, 8.0f);

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

