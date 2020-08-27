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
        int count = ropeSegments.Count;
        for(int x = 0;x < count-1;x++)
        {
            ConfigurableJoint joint = ropeSegments[x].GetComponent<ConfigurableJoint>();
            joint.connectedBody = ropeSegments[x + 1].GetComponent<Rigidbody>();
        }
        ropeSegments[0].gameObject.layer = 14;
        ropeSegments[ropeSegments.Count-1].gameObject.layer = 14;
        lineRen.positionCount = ropeSegments.Count;
    }

    private void Update()
    {
        int iterator = 0;
        foreach(Transform t in ropeSegments)//linerenderer draws rope
        {
            lineRen.SetPosition(iterator, t.position);
            iterator++;
        }
    }

}
