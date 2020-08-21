using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementScript : MonoBehaviour
{
    CharacterController controller;

    Rigidbody rb;

    public float speed = 6f;

    float horizontalInput;
    float verticalInput;

    float turnVel;
    // Start is called before the first frame update
    void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput).normalized;

        float targetAngle = Camera.main.transform.rotation.eulerAngles.y - Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg + 90f;


        float radius = 10f;

        //headTarget.transform.position = transform.position + (Quaternion.Euler(Camera.main.transform.rotation.eulerAngles) * Vector3.forward).normalized * radius;

        if (inputVector.magnitude > 0)
        {

            gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
            Vector3 dir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            gameObject.transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnVel, 0.1f), 0);

            rb.velocity = (dir.normalized * speed);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
}
