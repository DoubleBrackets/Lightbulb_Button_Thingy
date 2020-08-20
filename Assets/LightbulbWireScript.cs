using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbWireScript : MonoBehaviour
{
    public LineRenderer wireLineRenderer;

    public GameObject target;

    private ConfigurableJoint lightbulbJoint;

    public GameObject segmentPrefab; 

    private float segmentLength = 0.4f;
    private float segmentCount = 10;

    private int maxSegments = 30;
    private int minSegments = 2;

    private float lengthChangeSpeed = 1f;

    private List<ConfigurableJoint> segments;

    public GameObject wireParent;

    private void Awake()
    {
        lightbulbJoint = gameObject.GetComponent<ConfigurableJoint>();
        segments = new List<ConfigurableJoint>();
        var lim = lightbulbJoint.linearLimit;
        lim.limit = segmentLength;
        lightbulbJoint.linearLimit = lim;

        transform.position = (gameObject.transform.position - target.transform.position).normalized * segmentLength * (segmentCount + 1) + target.transform.position;
        for (int x = 0;x < segmentCount;x++)
        {
            AddSegment(segmentLength);
        }
        
    }
    private void Update()
    {
        UpdateLineRenderer();
        if (Input.GetKey(KeyCode.E))
        {
            if(segments.Count < maxSegments)
            {
                var lim = segments[segments.Count - 1].linearLimit;
                lim.limit -= lengthChangeSpeed * Time.deltaTime;
                if (lim.limit <= 0)
                    RemoveSegment();
                else
                    segments[segments.Count - 1].linearLimit = lim;
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            if (segments.Count > minSegments)
            {
                var lim = segments[segments.Count - 1].linearLimit;
                lim.limit += lengthChangeSpeed * Time.deltaTime;
                if (lim.limit >= segmentLength)
                {
                    lim.limit = segmentLength;
                    segments[segments.Count - 1].linearLimit = lim;
                    AddSegment(0);
                }
                else
                    segments[segments.Count - 1].linearLimit = lim;
            }
        }
    }

    private void AddSegment(float length)
    {
        GameObject newSegment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, wireParent.transform);
        ConfigurableJoint joint = newSegment.GetComponent<ConfigurableJoint>();
        segments.Add(joint);
        newSegment.name = "WireSegment" + segments.Count;
        var lim = joint.linearLimit;
        lim.limit = length;
        joint.linearLimit = lim;

        joint.connectedBody = target.GetComponent<Rigidbody>();

        newSegment.transform.position = (target.transform.position - gameObject.transform.position).normalized * (segmentLength * (segments.Count-1)+length) + transform.position;
        if (segments.Count > 1)
        {
            segments[segments.Count - 2].connectedBody = newSegment.GetComponent<Rigidbody>();
        }
        else
        {
            lightbulbJoint.connectedBody = newSegment.GetComponent<Rigidbody>();
        }
    }

    private void RemoveSegment()
    {
        int index = segments.Count - 1;
        segments.RemoveAt(index);
        segments[index - 1].connectedBody = target.GetComponent<Rigidbody>();
    }
    private void UpdateLineRenderer()
    {
        int count = segments.Count;
        wireLineRenderer.positionCount = count+2;
        wireLineRenderer.SetPosition(0,gameObject.transform.position);
        for (int x = 1;x <= count;x++)
        {
            wireLineRenderer.SetPosition(x, segments[x-1].transform.position);
        }
        wireLineRenderer.SetPosition(count+1, target.transform.position);
    }
}
