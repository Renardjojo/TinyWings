using UnityEngine;

[SerializeField]
public enum EInflexionType
{
    ASCENDANTE = 0,
    DESCANDANTE = 1,
    COUNT = 2
}

public abstract class Function
{
    protected Rect m_dim;
    protected EInflexionType m_type;

    protected Function(Rect dim, EInflexionType type)
    {
        m_dim = dim;
        m_type = type;
    }

    public abstract float image(float x);
    public abstract Vector2 tangeante(float x);
    public abstract Vector2 normal (float x);
    public abstract float derivative(float x, int n);
}
