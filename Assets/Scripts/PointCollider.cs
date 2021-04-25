using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MyRigidbody))]
public class PointCollider : MonoBehaviour
{
    protected WorldGenerator m_world;
    protected MyRigidbody m_rigid;
    
    // Start is called before the first frame update
    void Start()
    {
        m_world = GameObject.Find("World").GetComponent<WorldGenerator>();
        Assert.IsNotNull(m_world);

        m_rigid = transform.GetComponent<MyRigidbody>();
    }

    public bool IsColliding(ref Vector2 position, ref Vector2 reactionForce, ref Vector2 frictionForce)
    {
        Chunk currentChunk = m_world.getChunkOnX(transform.position.x);

        if (currentChunk.m_funct.IsPointBellow(transform.position))
        {
            position.x = transform.position.x;
            position.y = currentChunk.m_funct.image(transform.position.x);
            
            Vector2 normal = currentChunk.m_funct.normal(transform.position.x);
            Vector2 tangeante = new Vector2(normal.y, -normal.x);

            reactionForce = Mathf.Abs(Vector2.Dot(normal, m_rigid.m_force)) * normal;
            frictionForce = -Vector2.Dot(tangeante, m_rigid.m_force) * tangeante;
            frictionForce = Vector2.ClampMagnitude(frictionForce, m_rigid.m_mass * m_rigid.m_pensentorAcceleration * m_world.m_frictionCoef);

            return true;
        }
        return false;
    }
}
