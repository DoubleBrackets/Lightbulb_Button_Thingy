﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public static SimpleMove simpleMove;

    public LayerMask ground;
    private GameObject Obj;
    private Vector3 objdirection;
    private bool grabbed;
    public Transform holder;
    private float cooldown;
    private float cooldowntime;

    public bool isGrabbing = false;
    private void Awake()
    {
        simpleMove = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cooldowntime = .3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !CharacterMovementScript.characterMovementScript.isSitting)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2, ground))
            {
                print(hit.collider.gameObject.name);
                Obj = hit.collider.gameObject;
                objdirection = transform.position - hit.point;
                grabbed = true;
                cooldown = cooldowntime;
                CharacterMovementScript.characterMovementScript.rotationFactor *= 5;
                CharacterMovementScript.characterMovementScript.speed /= 2f;
            }
        }
        if (grabbed)
        {
            Obj.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Obj.transform.position = holder.position;
        }

       
    }

    private void LateUpdate()
    {
        if (Obj != null)
        {
            if (Input.GetMouseButtonUp(0) && grabbed)
            {
                ReleaseObject();
            }
            else if (Obj.GetComponent<ObjectBehavior>().Gettouching() && cooldown<=0)
            {
                ReleaseObject();
            }
        }
    }

    private void ReleaseObject()
    {
        Obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        CharacterMovementScript.characterMovementScript.rotationFactor /= 5;
        CharacterMovementScript.characterMovementScript.speed *= 2f;
        grabbed = false;
        Obj = null;

    }
    public GameObject GetObject()
    {
        return Obj;
    }
    public bool IsGrabbed()
    {
        return grabbed;
    }
}
