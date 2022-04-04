using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : MonoBehaviour
{

    class Wound
    {
        Mesh mesh;
        public Vector3[] vertices;
        public int[] triangles;
    }

    class Chop
    {
        List<Wound> wounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
