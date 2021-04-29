using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncitonSandBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       /* Polynome p = new Polynome(new Rect(0, 0, 830, 450), EInflexionType.ASCENDANTE);

        float x = 0;
        float step = 830 / 10f;
        for (int i = 0; i < 10; i++)
        {
            Debug.LogFormat("x : {0}, y : {1}, d : {2}, d2 : {3}", x, p.image(x), p.derivative(x, 1), p.derivative(x, 2));
            x += step;
        }*/
       //Debug.LogFormat("x : {0}, y : {1}, d : {2}, d2 : {3}",  Polynome.Factorial(0), Polynome.Factorial(1), Polynome.Factorial(2), Polynome.Factorial(3), Polynome.Factorial(4));
      
       float[] coefs = new float[3];
       coefs[0] = -3;
       coefs[1] = 2;
       coefs[2] = 8;
       
       Debug.LogFormat("x : {0}", Polynome.derivativeGen(1, 2, coefs));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
