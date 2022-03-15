using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Parabox.CSG;

public class ButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// œÂ‰ÒÚ‡‚ÎÂÌËÂ ÔÎÓÒÍÓÒÚË(¬–≈Ã≈ÕÕŒ≈)
    /// Õ¿◊¿ÀŒ
    /// </summary>
    struct Point
    {
        public float x;
        public float y;
        public float z;
    }

    Vector3 P1, P2;
    List<Vector3> pRes;
    class Quadrangle
    {

        public Vector3[] points;
        public Vector3[] plane;
        Vector3 minmaxX, minmaxY, minmaxZ;

        public Vector3 calc = new Vector3();

        Vector3 res = new Vector3(0.01f, 0, 0);
        public Quadrangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            points = new Vector3[4]; points[0] = p1; points[1] = p2; points[2] = p3; points[3] = p4;

            minmaxX.x = p1.x; minmaxX.y = p1.x;
            minmaxY.x = p1.y; minmaxY.y = p1.y;
            minmaxZ.x = p1.z; minmaxZ.y = p1.z;
            //////compare.x = min x,    compare.y = max x, compare.z = step  -  ÓÒÚ˚Î¸ 
            foreach (Vector3 p in points)
            {
                minmaxX.x = minmaxX.x > p.x ? p.x : minmaxX.x;
                minmaxX.y = minmaxX.y < p.x ? p.x : minmaxX.y;

                minmaxY.x = minmaxY.x > p.y ? p.y : minmaxY.x;
                minmaxY.y = minmaxY.y < p.y ? p.y : minmaxY.y;

                minmaxZ.x = minmaxZ.x > p.z ? p.z : minmaxZ.x;
                minmaxZ.y = minmaxZ.y < p.z ? p.z : minmaxZ.y;

            }

            plane = new Vector3[4]; plane[0].x = (p2.y * p3.z - p2.y * p1.z - p1.y * p3.z - p3.y * p2.z + p3.y * p1.z + p1.y * p2.z);

            plane[1].x = p2.z * p3.x - p2.z * p1.x - p1.z * p3.x - p3.z * p2.x + p3.z * p1.x + p1.z * p2.x;

            plane[2].x = p2.x * p3.y - p2.x * p1.y - p1.x * p3.y - p3.x * p2.y + p3.x * p1.y + p1.x * p2.y;

            plane[3].x = -p1.x * plane[0].x - p1.y * plane[1].x - p1.z * plane[2].x;
        }

        bool InBetween(Vector3 p)
        {
            if ((minmaxX.x <= p.x) && (p.x <= minmaxX.y) && (minmaxY.x <= p.y) && (p.y <= minmaxY.y) && (minmaxZ.x <= p.z) && (p.z <= minmaxZ.y))
            {
                //Debug.Log("check");
                return true;
            }
            else return false;
        }

        public Vector3 Intersection(Quadrangle q2, bool direct)
        {
            bool flag = true;
            minmaxX.z = (minmaxX.y - minmaxX.x) / 1000;

            if (direct)
            {
                calc.x = minmaxX.y;
                calc.z = ((q2.plane[1].x / plane[1].x) * (plane[0].x * calc.x + plane[3].x) - q2.plane[0].x * calc.x - q2.plane[3].x) / (q2.plane[2].x - plane[2].x * q2.plane[1].x / plane[1].x);
                calc.y = (-plane[2].x * calc.z - plane[0].x * calc.x - plane[3].x) / plane[1].x;

                res.y = plane[0].x * calc.x + plane[1].x * calc.y + plane[2].x * calc.z + plane[3].x;
                res.z = (q2.plane[0].x * calc.x + q2.plane[1].x * calc.y + q2.plane[2].x * calc.z + q2.plane[3].x);
                Debug.Log("cz");

                Debug.Log(calc);

                while (InBetween(calc) && flag)
                {
                    calc.x -= minmaxX.z;
                    calc.z = ((q2.plane[1].x / plane[1].x) * (plane[0].x * calc.x + plane[3].x) - q2.plane[0].x * calc.x - q2.plane[3].x) / (q2.plane[2].x - plane[2].x * q2.plane[1].x / plane[1].x);
                    calc.y = (-plane[2].x * calc.z - plane[0].x * calc.x - plane[3].x) / plane[1].x;

                    res.y = plane[0].x * calc.x + plane[1].x * calc.y + plane[2].x * calc.z + plane[3].x - (q2.plane[0].x * calc.x + q2.plane[1].x * calc.y + q2.plane[2].x * calc.z + q2.plane[3].x);
                    if ((res.y < res.x) && (q2.InBetween(calc))) flag = false;
                }
            }
            else
            {
                calc.x = minmaxX.x;
                calc.z = ((q2.plane[1].x / plane[1].x) * (plane[0].x * calc.x + plane[3].x) - q2.plane[0].x * calc.x - q2.plane[3].x) / (q2.plane[2].x - plane[2].x * q2.plane[1].x / plane[1].x);
                calc.y = (-plane[2].x * calc.z - plane[0].x * calc.x - plane[3].x) / plane[1].x;

                res.y = plane[0].x * calc.x + plane[1].x * calc.y + plane[2].x * calc.z + plane[3].x;
                res.z = (q2.plane[0].x * calc.x + q2.plane[1].x * calc.y + q2.plane[2].x * calc.z + q2.plane[3].x);

                Debug.Log("cz");

                Debug.Log(calc);

                while (InBetween(calc) && flag)
                {
                    calc.x += minmaxX.z;
                    calc.z = ((q2.plane[1].x / plane[1].x) * (plane[0].x * calc.x + plane[3].x) - q2.plane[0].x * calc.x - q2.plane[3].x) / (q2.plane[2].x - plane[2].x * q2.plane[1].x / plane[1].x);
                    calc.y = (-plane[2].x * calc.z - plane[0].x * calc.x - plane[3].x) / plane[1].x;

                    res.y = plane[0].x * calc.x + plane[1].x * calc.y + plane[2].x * calc.z + plane[3].x - (q2.plane[0].x * calc.x + q2.plane[1].x * calc.y + q2.plane[2].x * calc.z + q2.plane[3].x);
                    if ((res.y < res.x) && (q2.InBetween(calc))) flag = false;
                }
            }

            Debug.Log("Results are");
            if (InBetween(calc) && (q2.InBetween(calc)))
            {
                Debug.Log(calc); Debug.Log("They do");
                return calc;
            }
            else
            {
                Debug.Log("No Intersection");
                return new Vector3();
            }
        }

        /*public Vector3 CylinderCheck(GameObject Cylind, Vector3 point)
        {
            foreach (Vector3 p in points)
            {
                if (Cylind.GetComponent<CapsuleCollider>().bounds.Contains(p))
                {
                    Debug.Log("Yes"); Debug.Log(p);
                    return  
                }
            }

            

        }*/

        public bool CylinderCheck(GameObject Cylind, Vector3 point)
        {
            if (Cylind.GetComponent<CapsuleCollider>().bounds.Contains(point))
            {
                Debug.Log("Yes"); Debug.Log(point);
                return true;
            }
            return false;
        }
    }
    ///////////////////////////////////////////////

    void PointToVector3(Point p, Vector3 v)
    {
        v.x = p.x;
        v.y = p.y;
        v.z = p.z;
    }
    void Vector3ToPoint(Vector3 v, Point p)
    {
        p.x = v.x;
        p.y = v.y;
        p.z = v.z;
    }

    /// <summary>
    /// ÷≈ÕŒ 
    /// </summary>
    public GameObject Quadre1;
    public GameObject Quadre2;
    public GameObject Cylind;
    public GameObject Copystuff;


    public Vector3[] Vertices1, Vertices2, resVertices;
    public Vector2[] UV;
    public int[] Triangles1, Triangles2, resTriangles;

    public Mesh mesh1, mesh2, resMesh;

    private void Start()
    {
    }





    public void OnButtonPress()
    {
        mesh1 = Quadre1.GetComponent<MeshFilter>().mesh;
        mesh2 = Quadre2.GetComponent<MeshFilter>().mesh;

        Vertices1 = mesh1.vertices;
        Vertices2 = mesh2.vertices;

        Quadrangle quadrangle1 = new Quadrangle(Quadre1.transform.TransformPoint(Vertices1[0]), Quadre1.transform.TransformPoint(Vertices1[1]), Quadre1.transform.TransformPoint(Vertices1[2]), Quadre1.transform.TransformPoint(Vertices1[3]));
        Quadrangle quadrangle2 = new Quadrangle(Quadre2.transform.TransformPoint(Vertices2[0]), Quadre2.transform.TransformPoint(Vertices2[1]), Quadre2.transform.TransformPoint(Vertices2[3]), Quadre2.transform.TransformPoint(Vertices2[3]));


        Debug.Log(Quadre1.transform.TransformPoint(Vertices1[0]));
        Debug.Log(Quadre1.transform.TransformPoint(Vertices1[1]));
        Debug.Log(Quadre1.transform.TransformPoint(Vertices1[2]));
        Debug.Log(Quadre1.transform.TransformPoint(Vertices1[3]));

        Debug.Log("messssss");

        Debug.Log(Quadre2.transform.TransformPoint(Vertices2[0]));
        Debug.Log(Quadre2.transform.TransformPoint(Vertices2[1]));
        Debug.Log(Quadre2.transform.TransformPoint(Vertices2[2]));
        Debug.Log(Quadre2.transform.TransformPoint(Vertices2[3]));
        //Debug.Log(Vertices1[0]);

        P1 = new Vector3(); P2 = new Vector3();
        P1=quadrangle1.Intersection(quadrangle2, true);
        P2=quadrangle1.Intersection(quadrangle2, false);
        Debug.Log("PP"); Debug.Log(P1); Debug.Log(P2);

        //pRes = new Vector3[6];

        resVertices = new Vector3[6]
        {
            Quadre1.transform.TransformPoint(Vertices1[0]), Quadre1.transform.TransformPoint(Vertices1[2]), Quadre2.transform.TransformPoint(Vertices2[0]), Quadre2.transform.TransformPoint(Vertices2[2]), P1, P2
        };
        Debug.Log("PP AFTER");
        resMesh = new Mesh();
        resMesh.vertices = resVertices;
        Debug.Log("PP double after");
        resTriangles = new int[24]
        {
            0, 1, 4,
            1, 4, 5,
            0, 4, 3,
            1, 5, 2,
            3, 4, 5,
            2, 3, 5,
            0, 1, 3,
            1, 3, 2
        };
        Debug.Log("WHY");
        resMesh.triangles = resTriangles;

        var cutter = new GameObject();
        cutter.AddComponent<MeshFilter>().sharedMesh = resMesh;
        cutter.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        Debug.Log("not");

        Model result1 = CSG.Subtract(Cylind, cutter);
        var subtract = new GameObject();
        subtract.AddComponent<MeshFilter>().sharedMesh = result1.mesh;
        subtract.AddComponent<MeshRenderer>().sharedMaterials = result1.materials.ToArray();
        subtract.AddComponent<Rigidbody>();
        subtract.AddComponent<MeshCollider>().convex = true;

        Model result2 = CSG.Subtract(Cylind, subtract);
        var subtracted = new GameObject();
        subtracted.AddComponent<MeshFilter>().sharedMesh = result2.mesh;
        subtracted.AddComponent<MeshRenderer>().sharedMaterials = result2.materials.ToArray();
        subtracted.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
        subtracted.AddComponent<Rigidbody>();
        subtracted.AddComponent<MeshCollider>().convex = true;
        
        
        /*Model result1 = CSG.Union(Quadre1, Quadre2);
        var subtract = new GameObject();
        subtract.AddComponent<MeshFilter>().sharedMesh = result1.mesh;
        subtract.AddComponent<MeshRenderer>().sharedMaterials = result1.materials.ToArray();
        subtract.AddComponent<Rigidbody>();
        subtract.AddComponent<MeshCollider>().convex = true;

        Model result2 = CSG.Subtract(Cylind, subtract);
        var subtracted = new GameObject();
        subtracted.AddComponent<MeshFilter>().sharedMesh = result2.mesh;
        subtracted.AddComponent<MeshRenderer>().sharedMaterials = result2.materials.ToArray();
        //subtracted.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
        subtracted.AddComponent<Rigidbody>();
        subtracted.AddComponent<MeshCollider>().convex = true;

        Model result3 = CSG.Subtract(Cylind, subtracted);
        var subtractedd = new GameObject();
        subtractedd.AddComponent<MeshFilter>().sharedMesh = result3.mesh;
        subtractedd.AddComponent<MeshRenderer>().sharedMaterials = result3.materials.ToArray();
        subtractedd.GetComponent<Transform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
        subtractedd.AddComponent<Rigidbody>();
        subtractedd.AddComponent<MeshCollider>().convex = true;

        */
        Destroy(cutter); Destroy(Cylind);
        Cylind = subtracted;
        //quadrangle1.CylinderCheck(Cylind);

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(P1, P2);
        //Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(10, 5, 23));
        //Gizmos.DrawCube(new Vector3(0, 0, 0), new Vector3(5, 5, 5));
    }
}

