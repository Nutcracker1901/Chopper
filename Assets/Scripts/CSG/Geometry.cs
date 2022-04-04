using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{

    public class Quadrangle
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
            //////compare.x = min x,    compare.y = max x, compare.z = step  - Костыль 
            foreach (Vector3 p in points)
            {
                minmaxX.x = minmaxX.x > p.x ? p.x : minmaxX.x;
                minmaxX.y = minmaxX.y < p.x ? p.x : minmaxX.y;

                minmaxY.x = minmaxY.x > p.y ? p.y : minmaxY.x;
                minmaxY.y = minmaxY.y < p.y ? p.y : minmaxY.y;

                minmaxZ.x = minmaxZ.x > p.z ? p.z : minmaxZ.x;
                minmaxZ.y = minmaxZ.y < p.z ? p.z : minmaxZ.y;

            }

            plane = new Vector3[4];
            plane[0].x = (p2.y * p3.z - p2.y * p1.z - p1.y * p3.z - p3.y * p2.z + p3.y * p1.z + p1.y * p2.z);

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

        //найти готовое решение которое выдает уравнение прямой как пересечение двух плоскостей И ТОЧКИ задающие отрезок.
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
