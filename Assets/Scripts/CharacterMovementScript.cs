using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementScript : MonoBehaviour
{
    public static CharacterMovementScript characterMovementScript;

    Rigidbody rb;
    CapsuleCollider coll;
    BoxCollider feetColl;

    Animator anim;

    private float dynFric;

    public float speed = 6f;
    private float accelForce = 40f;

    public float rotationFactor = 0.15f;

    public float jumpVel = 10f;

    float horizontalInput;
    float verticalInput;

    float turnVel;

    float jumpTimer = 0;

    public bool canJump = true;

    private float movementDisabledSitting = 0f;
    private float feetCollHeight;

    private float slowdownFactor = 0.8f;

    public bool isGrounded = false;
    public bool isSitting = false;
    public bool isChangingPosition = false;
    public bool isStunned = false;
    public bool isInvuln = false;

    LayerMask groundedMask;
    // Start is called before the first frame update
    void Awake()
    {
        characterMovementScript = this;
        rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponentInChildren<CapsuleCollider>();
        feetColl = gameObject.GetComponentInChildren<BoxCollider>();
        anim = gameObject.GetComponentInChildren<Animator>();
        feetCollHeight = feetColl.size.y;
        dynFric = coll.material.dynamicFriction;
        groundedMask = LayerMask.GetMask( "Terrain","MoveableObject");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();
        anim.SetBool("IsInAir", !isGrounded);

        if (movementDisabledSitting > 0)
            movementDisabledSitting -= Time.deltaTime;
        if (jumpTimer > 0)
            jumpTimer -= Time.deltaTime;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space) && jumpTimer <= 0 && movementDisabledSitting <= 0 && !isStunned)
        {
            if(isGrounded && canJump)
            {
                if(isSitting)
                    StopSitting();
                jumpTimer = 0.2f;
                rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isSitting && !SimpleMove.simpleMove.IsGrabbed() && !isStunned)
        {
            //Sit down
            movementDisabledSitting = 0.5f;
            isSitting = true;
            anim.SetBool("IsSitting", true);
            StartCoroutine(SitDown());
        }
    }


    private void StopSitting()
    {
        isSitting = false;
        StartCoroutine(SitUp());
        anim.SetBool("IsSitting", false);
    }

    IEnumerator SitUp()//Smoothens sitting up and readding feet collider
    {
        isChangingPosition = true;
        feetColl.enabled = true;
        for (int x = 0;x <= 15;x++)
        {
            feetColl.size = new Vector3(feetColl.size.x, x / 15f * feetCollHeight, feetColl.size.z);
            yield return new WaitForFixedUpdate();
        }
        isChangingPosition = false;
    }

    IEnumerator SitDown()
    {
        for (int x = 0; x <= 20; x++)
        {
            feetColl.size = new Vector3(feetColl.size.x,  (1- x / 20f) * feetCollHeight, feetColl.size.z);
            yield return new WaitForFixedUpdate();
        }
        feetColl.enabled = false;
    }
    private bool IsGrounded()
    {
        RaycastHit hit;
        /*
        Vector3 p1 = transform.position + coll.center - (coll.bounds.extents.y -coll.radius-0.1f) * Vector3.up;
        Vector3 p2 = p1 + (2 * coll.bounds.extents.y-0.1f - coll.radius) * Vector3.up;
        Physics.CapsuleCast(p1, p2,coll.radius, Vector3.down, out hit, 0.5f,groundedMask);
        */
        Vector3 center = feetColl.center;
        if(isSitting)
        {
            Vector3 p1 = transform.position + coll.center - (coll.bounds.extents.y - coll.radius - 0.1f) * Vector3.up;
            Vector3 p2 = p1 + (2 * coll.bounds.extents.y - 0.1f - coll.radius) * Vector3.up;
            Physics.CapsuleCast(p1, p2, coll.radius, Vector3.down, out hit, 0.2f, groundedMask);
        }
        else
        {
            Vector3 size = new Vector3(feetColl.size.x, feetColl.size.y - 0.1f, feetColl.size.z);
            Physics.BoxCast(center + transform.position, size / 2f, Vector3.down, out hit, Quaternion.identity, 0.2f, groundedMask);
        }
        if (hit.collider != null)
            return true;
        return false;
    }
    private void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput).normalized;

        float inputAngle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

        float targetAngle = Camera.main.transform.rotation.eulerAngles.y - inputAngle + 90f;


        float radius = 10f;

        //headTarget.transform.position = transform.position + (Quaternion.Euler(Camera.main.transform.rotation.eulerAngles) * Vector3.forward).normalized * radius;

        if (inputVector.magnitude > 0 && movementDisabledSitting <= 0 && !isStunned)
        {
            anim.SetBool("IsMoving", true);
            if (isSitting)
                StopSitting();
            Vector3 dir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            if (SimpleMove.simpleMove.IsGrabbed())
            {
                targetAngle = Camera.main.transform.rotation.eulerAngles.y;
            }
            gameObject.transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnVel, rotationFactor), 0);

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
            feetColl.material.dynamicFriction = 0f;
            coll.material.staticFriction = 0f;
            feetColl.material.staticFriction = 0f;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x * slowdownFactor, rb.velocity.y,rb.velocity.z * slowdownFactor);
            rb.angularVelocity = rb.angularVelocity * slowdownFactor;
            if (isGrounded)
            {
                coll.material.dynamicFriction = dynFric;
                feetColl.material.dynamicFriction = dynFric;
            }
            anim.SetBool("IsMoving", false);
        }
    }

    public void Stunned(bool val)
    {
        isStunned = val;
        if(val)
        {
            PlayerParticleManager.playerParticleManager.PlayParticle("StunnedParticles");
            slowdownFactor = 0.97f;
        }
        else
        {
            PlayerParticleManager.playerParticleManager.StopParticle("StunnedParticles");
            slowdownFactor = 0.8f;
        }
    }
}
