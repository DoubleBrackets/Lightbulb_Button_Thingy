using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public LayerMask ground;
    private GameObject Obj;
    private Vector3 objdirection;
    private bool grabbed;
    public Transform holder;
    private float cooldown;
    private float cooldowntime;
    // Start is called before the first frame update
    void Start()
    {
        cooldowntime = .4f;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2, ground))
            {
                print(hit.collider.gameObject.name);
                Obj = hit.collider.gameObject;
                objdirection = transform.position - hit.point;
                grabbed = true;
                cooldown = cooldowntime;
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
                Obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                grabbed = false;
                Obj = null;

            }
            else if (Obj.GetComponent<ObjectBehavior>().Gettouching() && cooldown<=0)
            {
                Obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                grabbed = false;
                Obj = null;
            }
        }
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
