using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbMovementScript : MonoBehaviour
{
    public GameObject target;

    public LineRenderer wire;

    void Awake()
    {
        wire.positionCount = 2;

    }
    private void FixedUpdate()
    {
        wire.SetPosition(0, transform.position);
        wire.SetPosition(1, target.transform.position);

        Vector3 diff = target.transform.position - transform.position;
        float mag = diff.magnitude;

        float zRotation = 90+Mathf.Acos(diff.x / mag)*Mathf.Rad2Deg;
        float xRotation = 90-Mathf.Acos(diff.z / mag) * Mathf.Rad2Deg;
        //float yRotation = Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;
        //float yRotation = 
        transform.rotation = Quaternion.Euler(xRotation, 0, zRotation);

    }
}
