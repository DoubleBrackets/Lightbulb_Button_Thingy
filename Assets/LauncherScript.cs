using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherScript : MonoBehaviour
{

    public float launchVel = 20f;

    public Vector3 size;

    public void LaunchObject()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + Vector3.up*2f, size/2f, Quaternion.identity, LayerMask.GetMask("MoveableObject", "RopeEnd", "Rope"));

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
