using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Polynome p = new Polynome(new Rect(-2, -2, 4, 4), EInflexionType.ASCENDANTE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
