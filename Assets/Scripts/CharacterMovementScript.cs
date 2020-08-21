using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementScript : MonoBehaviour
{
    public static CharacterMovementScript characterMovementScript;

    Rigidbody rb;
    MeshCollider coll;

    private float dynFric;

    public float speed = 6f;
    private float accelForce = 40f;

    public float jumpVel = 10f;

    float horizontalInput;
    float verticalInput;

    float turnVel;

    float jumpTimer = 0;

    public bool canJump = true;

    LayerMask groundedMask;
    // Start is called before the first frame update
    void Awake()
    {
        characterMovementScript = this;
        rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponentInChildren<MeshCollider>();
        dynFric = coll.material.dynamicFriction;
        groundedMask = LayerMask.GetMask( "Terrain","MoveableObject");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpTimer > 0)
            jumpTimer -= Time.deltaTime;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && jumpTimer <= 0)
        {
            if(IsGrounded() && canJump)
            {
                jumpTimer = 0.2f;
                rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Physics.BoxCast(transform.position, new Vector3(0.5f,0.4f,0.5f), Vector3.down, out hit,Quaternion.identity, 0.4f,groundedMask);
        if (hit.collider != null)
            return true;
        return false;
    }
    private void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput).normalized;

        float targetAngle = Camera.main.transform.rotation.eulerAngles.y - Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg + 90f;


        float radius = 10f;

        //headTarget.transform.position = transform.position + (Quaternion.Euler(Camera.main.transform.rotation.eulerAngles) * Vector3.forward).normalized * radius;

        if (inputVector.magnitude > 0)
        {

            //gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
            Vector3 dir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            GameObject target = ObjectManipulateScript.objectManipulateScript.GetTargetObject();
            if(target == null)
                gameObject.transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnVel, 0.15f), 0);
            else
            {
                Vector3 dif = target.transform.position - gameObject.transform.position;
                float newTargetAngle = Mathf.Atan2(dif.x,dif.z) * Mathf.Rad2Deg;
                gameObject.transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, newTargetAngle, ref turnVel, 0.15f), 0);
            }

            float currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

            float newSpeed = (rb.velocity + dir.normalized * accelForce * Time.fixedDeltaTime).magnitude;
            if(newSpeed < speed)
            {
                rb.AddForce(dir.normalized * accelForce);
            }
            else
            {
                rb.velocity = new Vector3(dir.normalized.x * speed, rb.velocity.y, dir.normalized.z * speed);
            }
            coll.material.dynamicFriction = 0f;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x * 0.85f,rb.velocity.y,rb.velocity.z * 0.85f);
            if(IsGrounded())
                coll.material.dynamicFriction = dynFric;
            //gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
}
