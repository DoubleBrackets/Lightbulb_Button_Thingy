using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbMovementScript : MonoBehaviour
{
    public GameObject target;

    Rigidbody rb;

    private float swingForce = 175f;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 diff = target.transform.position - transform.position;

        transform.LookAt(target.transform);

        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);

    }

    private void Update()
    {
        float horizontalInput = Input.mousePosition.x - Screen.width / 2f;
        float verticalInput = Input.mousePosition.y - Screen.height / 2f;
        rb.AddForce(new Vector3(-verticalInput, 0, horizontalInput).normalized * swingForce*Time.deltaTime*rb.mass);
    }
}
