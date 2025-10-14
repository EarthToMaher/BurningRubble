using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDuplicator : MonoBehaviour
{
    public SplineContainer container;
    public Spline startingSpline;
    public Spline innerSpline;
    public Spline outerSpline;
    public float width;
    public int sample = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    [ContextMenu("Generate Track")]
    public void GenerateSplines()
    {
        startingSpline = container.Spline;
        List<Vector3> leftPoints = new List<Vector3>();
        List<Vector3> rightPoints = new List<Vector3>();

        for (int i = 0; i <= sample; i++)
        {
            float t = i / (float)sample;
            Debug.Log(t);
            Vector3 pos = startingSpline.EvaluatePosition(t);
            Vector3 tangent = startingSpline.EvaluateTangent(t);
            //tangent = tangent.normalized;

            Vector3 up = Vector3.up;
            Vector3 right = Vector3.Cross(up, tangent).normalized;

            float halfWidth = width * 0.5f;
            leftPoints.Add(pos - right * halfWidth);
            rightPoints.Add(pos + right * halfWidth);
            
        }

        innerSpline = BuildSpline(rightPoints);
        //outerSpline = BuildSpline(leftPoints);

        container.AddSpline(innerSpline);
        //container.AddSpline(outerSpline);
    }
    
    public Spline BuildSpline(List<Vector3> points)
    {
        Spline s = new Spline();
        foreach (var p in points)
        {

            BezierKnot knot = new BezierKnot(p);
            s.Add(knot);
        }
        var allKnots = new SplineRange(0,s.Count);
        //s.Closed = true;
        s.SetTangentMode(allKnots,TangentMode.AutoSmooth);
        return s;
    }
}
