using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    // Start is called before the first frame update

    private List<Transform> ropeSegments;
    private LineRenderer lineRen;
    void Awake()
    {
        ropeSegments = new List<Transform>();
        lineRen = gameObject.GetComponent<LineRenderer>();
        foreach(Transform t in transform)
        {
            ropeSegments.Add(t);
        }
        ropeSegments[0].gameObject.layer = 14;
        ropeSegments[ropeSegments.Count-1].gameObject.layer = 14;
        lineRen.positionCount = ropeSegments.Count;
    }

    private void Update()
    {
        int iterator = 0;
        foreach(Transform t in ropeSegments)
        {
            lineRen.SetPosition(iterator, t.position);
            iterator++;
        }
    }

}
