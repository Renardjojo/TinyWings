using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Chunk> chunks; // queue

    [SerializeField]
    private Vector2 hScale = Vector2.one;

    [SerializeField]
    private Vector2 vScale = Vector2.one;

    void Awake()
    {
        chunks = new List<Chunk>();
    }
    
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GenerateRandomChunks();
        }
    }
    
    private void GenerateRandomChunks()
    {
        float offsetX = chunks.Count > 0 ? chunks.Last().m_dimension.xMax : 0f;
        float offsetY = chunks.Count > 0 ? (chunks.Last().m_inflexionType == EInflexionType.DESCANDANTE ? chunks.Last().m_dimension.yMin : chunks.Last().m_dimension.yMax) : 0f;
        
        float height = Random.Range(vScale.x, vScale.y);
        float width = Random.Range(hScale.x, hScale.y);

        EInflexionType inflexionType = (EInflexionType)Random.Range(0, (int)EInflexionType.COUNT);
        EType fuctionType = (EType)Random.Range(0, (int)EType.COUNT);

        if (inflexionType == EInflexionType.DESCANDANTE)
        {
            offsetY -= height;
        }

        Rect dimension = new Rect( offsetX, offsetY, width, height);
        
        Debug.Log(dimension);
        
        GameObject chunkGO = Instantiate(chunkPrefab);
        Chunk chunk = chunkGO.GetComponent<Chunk>();

        Assert.IsNotNull(chunk, "chunk component not found");

        chunk.Apply(fuctionType, inflexionType, dimension);
        chunks.Add(chunk);
    }
}

