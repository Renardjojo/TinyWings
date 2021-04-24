using UnityEngine;

public class MyRigidbody : MonoBehaviour
{
    public float m_mass = 1f;
    public float m_pensentorAcceleration = 9.81f;
    public Vector2 m_force = Vector2.zero;
    public Vector2 m_velocity = Vector2.zero;
    public PointCollider m_collider;
    public bool m_isGrounded = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_collider = GetComponent<PointCollider>();
    }
    
    Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    
    void SetPosition(Vector2 newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    public void AddForce(Vector2 force)
    {
        m_force += force;
    }

    public Vector2 GetGravityVector()
    {
        return Vector2.down * (m_pensentorAcceleration * m_mass);
    }

    void FixedUpdate()
    {
        Vector2 position = Vector2.zero;
        Vector2 reactionForce = Vector2.zero;
        Vector2 frictionForce = Vector2.zero;
        
        AddForce(GetGravityVector());
        
        Vector2 acceleration = m_force / m_mass;
        m_velocity = acceleration * Time.fixedDeltaTime;
        SetPosition(GetPosition() + m_velocity * Time.fixedDeltaTime);

        if (m_collider.IsColliding(ref position, ref reactionForce, ref frictionForce))
        {
            SetPosition(position);
            
            AddForce(reactionForce);
            AddForce(frictionForce);
            
            Debug.DrawLine(transform.position, transform.position + new Vector3(reactionForce.x, reactionForce.y, 0f), Color.blue);
            Debug.DrawLine(transform.position, transform.position + new Vector3(frictionForce.x, frictionForce.y, 0f), Color.green);

            Vector2 normal = reactionForce.normalized;
            transform.up = new Vector3(normal.x, normal.y, 0f);
            m_isGrounded = true;
        }
        else
        {
            //Center charater to up vector
            transform.up = Vector3.Slerp(transform.up, Vector3.up, Time.fixedDeltaTime);
            m_isGrounded = false;
        }


        
        Debug.DrawLine(transform.position, transform.position + new Vector3(GetGravityVector().x, GetGravityVector().y, 0f), Color.red);
    }
}
