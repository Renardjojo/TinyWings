using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sinusoide p = new Sinusoide(new Rect(-2, -1, 1, 8), EInflexionType.DESCANDANTE, 1);

        float x = -2;
        float step = 4 / 10f;
        for (int i = 0; i < 10; i++)
        {
            Debug.LogFormat("x : {0}, y : {1}, d : {2}, d2 : {3}", x, p.image(x), p.derivative(x, 1), p.derivative(x, 2));
            x += step;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
