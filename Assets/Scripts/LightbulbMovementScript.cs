using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbMovementScript : MonoBehaviour
{
    public GameObject target;

    public LineRenderer wire;

    ConfigurableJoint joint;

    Rigidbody rb;

    private float swingForce = 75f;

    private float cinchSpeed = 0.7f;

    private float wireLimit = 10f;

    void Awake()
    {
        wire.positionCount = 2;
        rb = gameObject.GetComponent<Rigidbody>();
        joint = gameObject.GetComponent<ConfigurableJoint>();
    }
    private void FixedUpdate()
    {
        wire.SetPosition(0, transform.position);
        wire.SetPosition(1, target.transform.position);

        Vector3 diff = target.transform.position - transform.position;

        transform.LookAt(target.transform);

        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);

    }

    private void Update()
    {
        float horizontalInput = Input.mousePosition.x - Screen.width / 2f;
        float verticalInput = Input.mousePosition.y - Screen.height / 2f;
        rb.AddForce(new Vector3(-verticalInput, 0, horizontalInput).normalized * swingForce*Time.deltaTime);

        if(Input.GetKey(KeyCode.E))
        {
            var lim = joint.linearLimit;
            if (lim.limit > cinchSpeed * Time.deltaTime+0.2f)
            {
                lim.limit -= cinchSpeed * Time.deltaTime;
                joint.linearLimit = lim;
            }
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            var lim = joint.linearLimit;
            if (lim.limit <= wireLimit -  cinchSpeed * Time.deltaTime)
            {
                lim.limit += cinchSpeed * Time.deltaTime;
                joint.linearLimit = lim;
            }
        }

    }
}
