using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherScript : MonoBehaviour
{

    public float launchVel = 20f;

    private BoxCollider coll;

    private void Awake()
    {
        coll = gameObject.GetComponent<BoxCollider>();
    }
    public void LaunchObject()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + Vector3.up*2f, coll.bounds.extents, Quaternion.identity, LayerMask.GetMask("MoveableObject", "RopeEnd", "Rope"));

        foreach(Collider coll in hitColliders)
        {
            Rigidbody rb = coll.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity += Vector3.up * launchVel;
            }
        }
    }
}
