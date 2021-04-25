using UnityEngine;

public class TestFunction : MonoBehaviour
{
    Elliptical e;

    Vector2[] points = new Vector2[10];
    // Start is called before the first frame update
    void Start()
    {
        e = new Elliptical(new Rect(1,1,1,3), EInflexionType.DESCANDANTE);

        float x = 1;
        float step = 1 / 10f;
        for (int i = 0; i < 10; i++)
        {
            Debug.LogFormat("x : {0}, y : {1}, d : {2}, d2 : {3}", x, e.image(x), e.derivative(x, 1), e.derivative(x, 2));
            x += step;
            points[i] = new Vector2(x, e.image(x));


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 9; i++)
            Gizmos.DrawLine(new Vector3(points[i].x, points[i].y, 0), new Vector3(points[i+1].x, points[i+1].y, 0)); 
    }

}
