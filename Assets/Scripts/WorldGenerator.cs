using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    
    public GameObject chunkPrefab;
    
    private List<Chunk> chunks; // queue

    [SerializeField]
    private Vector2 hScale = Vector2.one;

    [SerializeField]
    private Vector2 vScale = Vector2.one;

    public float cameraGenerationAnticipationFactor = 2f;
    public float cameraDestructionAnticipationFactor = 2f;
    [Range(0f, 1f)] public float m_frictionCoef = 0.2f;

    void Awake()
    {
        chunks = new List<Chunk>();
    }

    void Start()
    {
        GenerateChunks(EInflexionType.DESCANDANTE, EType.SINUSOIDE, EChunkType.NONE);
        
        if (Application.isMobilePlatform)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }
    }
    
    private void GenerateChunks(EInflexionType inflexionType, EType functionType, EChunkType chunkType)
    {
        float offsetX = chunks.Count > 0 ? chunks.Last().m_dimension.xMax : 0f;
        float offsetY = chunks.Count > 0 ? (chunks.Last().m_inflexionType == EInflexionType.DESCANDANTE ? chunks.Last().m_dimension.yMin : chunks.Last().m_dimension.yMax) : 0f;
        
        float height = Random.Range(vScale.x, vScale.y);
        float width = Random.Range(hScale.x, hScale.y);

        if (inflexionType == EInflexionType.DESCANDANTE)
        {
            offsetY -= height;
        }

        Rect dimension = new Rect( offsetX, offsetY, width, height);
        
        GameObject chunkGO = Instantiate(chunkPrefab);
        Chunk chunk = chunkGO.GetComponent<Chunk>();

        Assert.IsNotNull(chunk, "chunk component not found");

        chunk.Apply(functionType, inflexionType, dimension, chunkType);
        chunks.Add(chunk);
    }

    public Chunk getChunkOnX(float x)
    {
        foreach (var chunk in chunks)
        {
            if (chunk.m_dimension.xMin < x && x < chunk.m_dimension.xMax)
                return chunk;
        }

        return null;
    }

    void Update()
    {
        //Add chunk
        while (camera.transform.position.x + camera.m_Lens.OrthographicSize * cameraGenerationAnticipationFactor > chunks.Last().m_dimension.xMax)
        {
            GenerateChunks((EInflexionType)Random.Range(0, (int)EInflexionType.COUNT),(EType)Random.Range(0, (int)EType.COUNT), (EChunkType)Random.Range(0, (int)EChunkType.COUNT));
        }
        
        //Remove chunk
        while (chunks.First().m_dimension.xMax < camera.transform.position.x - camera.m_Lens.OrthographicSize * cameraDestructionAnticipationFactor)
        {
            Destroy(chunks.First().gameObject);
            chunks.Remove(chunks.First());
        }
    }
}

